
using System.Linq;
using Allors.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using Meta;
    using Resources;

    public partial class PurchaseOrder
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.PurchaseOrder, M.PurchaseOrder.PurchaseOrderState),
                new TransitionalConfiguration(M.PurchaseOrder, M.PurchaseOrder.PurchaseOrderShipmentState),
                new TransitionalConfiguration(M.PurchaseOrder, M.PurchaseOrder.PurchaseOrderPaymentState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public bool NeedsApprovalLevel1
        {
            get
            {
                if (this.ExistTakenViaSupplier && this.ExistOrderedBy)
                {
                    var supplierRelationship = this.TakenViaSupplier.SupplierRelationshipsWhereSupplier.FirstOrDefault(v => v.InternalOrganisation.Equals(this.OrderedBy));
                    if (supplierRelationship != null &&
                        supplierRelationship.NeedsApproval &&
                        (!supplierRelationship.ExistApprovalThresholdLevel1 || this.TotalExVat >= supplierRelationship.ApprovalThresholdLevel1))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool NeedsApprovalLevel2
        {
            get
            {
                if (this.ExistTakenViaSupplier && this.ExistOrderedBy)
                {
                    var supplierRelationship = this.TakenViaSupplier.SupplierRelationshipsWhereSupplier.FirstOrDefault(v => v.InternalOrganisation.Equals(this.OrderedBy));
                    if (supplierRelationship != null &&
                        supplierRelationship.NeedsApproval && this.TotalExVat >= supplierRelationship.ApprovalThresholdLevel2)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool CanInvoice
        {
            get
            {
                foreach (PurchaseOrderItem purchaseOrderItem in this.ValidOrderItems)
                {
                    if (!purchaseOrderItem.ExistOrderItemBillingsWhereOrderItem &&
                        (purchaseOrderItem.PurchaseOrderItemShipmentState.IsReceived || purchaseOrderItem.PurchaseOrderItemShipmentState.IsPartiallyReceived || !purchaseOrderItem.ExistPart && purchaseOrderItem.QuantityReceived == 1))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void BaseOnInit(ObjectOnInit method)
        {
            if (!this.ExistPurchaseOrderState)
            {
                this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Created;
            }

            if (!this.ExistPurchaseOrderShipmentState)
            {
                this.PurchaseOrderShipmentState = new PurchaseOrderShipmentStates(this.Strategy.Session).NotReceived;
            }

            if (!this.ExistPurchaseOrderPaymentState)
            {
                this.PurchaseOrderPaymentState = new PurchaseOrderPaymentStates(this.Strategy.Session).NotPaid;
            }

            if (!this.ExistOrderDate)
            {
                this.OrderDate = this.strategy.Session.Now();
            }

            if (!this.ExistEntryDate)
            {
                this.EntryDate = this.strategy.Session.Now();
            }

            if (!this.ExistOrderedBy)
            {
                var internalOrganisations = new Organisations(this.Strategy.Session).InternalOrganisations();
                if (internalOrganisations.Count() == 1)
                {
                    this.OrderedBy = internalOrganisations.First();
                }
            }

            if (!this.ExistOrderNumber)
            {
                this.OrderNumber = this.OrderedBy.NextPurchaseOrderNumber(this.OrderDate.Year);
            }

            if (!this.ExistCurrency)
            {
                this.Currency = this.OrderedBy.PreferredCurrency;
            }

            if (!this.ExistFacility && this.OrderedBy.StoresWhereInternalOrganisation.Count == 1)
            {
                this.Facility = this.OrderedBy.StoresWhereInternalOrganisation.Single().DefaultFacility;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.HasChangedRoles(this))
            {
                derivation.AddDependency(this, this.Strategy.Session.GetSingleton());
                derivation.AddDependency(this, this.Strategy.Session.GetSingleton());
                derivation.AddDependency(this, this.TakenViaSupplier);
            }

            foreach (PurchaseOrderItem orderItem in this.PurchaseOrderItems)
            {
                derivation.AddDependency(this, orderItem);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            #region Derivations and Validations

            this.SecurityTokens = new[]
            {
                this.strategy.Session.GetSingleton().DefaultSecurityToken,
                this.OrderedBy.PurchaseOrderApproverLevel1SecurityToken,
                this.OrderedBy.PurchaseOrderApproverLevel2SecurityToken
            };

            Organisation supplier = this.TakenViaSupplier as Organisation;
            if (supplier != null)
            {
                if (!this.OrderedBy.ActiveSuppliers.Contains(supplier))
                {
                    derivation.Validation.AddError(this, this.Meta.TakenViaSupplier, ErrorMessages.PartyIsNotASupplier);
                }
            }

            if (!this.ExistShipToAddress)
            {
                this.ShipToAddress = this.OrderedBy.ShippingAddress;
            }

            if (!this.ExistBillToContactMechanism)
            {
                this.BillToContactMechanism = this.OrderedBy.ExistBillingAddress ? this.OrderedBy.BillingAddress : this.OrderedBy.GeneralCorrespondence;
            }

            if (!this.ExistTakenViaContactMechanism && this.ExistTakenViaSupplier)
            {
                this.TakenViaContactMechanism = this.TakenViaSupplier.OrderAddress;
            }

            this.VatRegime = this.VatRegime ?? this.TakenViaSupplier?.VatRegime;

            this.Locale = this.Strategy.Session.GetSingleton().DefaultLocale;

            var validOrderItems = this.PurchaseOrderItems.Where(v => v.IsValid).ToArray();
            this.ValidOrderItems = validOrderItems;
            #endregion

            #region States
            var purchaseOrderShipmentStates = new PurchaseOrderShipmentStates(this.Strategy.Session);
            var purchaseOrderPaymentStates = new PurchaseOrderPaymentStates(this.Strategy.Session);

            var purchaseOrderItemStates = new PurchaseOrderItemStates(derivation.Session);

            // PurchaseOrder Shipment State
            if (validOrderItems.Any())
            {
                //                var receivable = validOrderItems.Where(v => this.PurchaseOrderState.IsSent && v.PurchaseOrderItemState.IsInProcess && !v.PurchaseOrderItemShipmentState.IsReceived);

                if (validOrderItems.Any(v => v.ExistPart) && validOrderItems.Where(v => v.ExistPart).All(v => v.PurchaseOrderItemShipmentState.IsReceived) ||
                    validOrderItems.Any(v => !v.ExistPart) && validOrderItems.Where(v => !v.ExistPart).All(v => v.PurchaseOrderItemShipmentState.IsReceived))
                {
                    this.PurchaseOrderShipmentState = purchaseOrderShipmentStates.Received;
                }
                else if (validOrderItems.All(v => v.PurchaseOrderItemShipmentState.IsNotReceived))
                {
                    this.PurchaseOrderShipmentState = purchaseOrderShipmentStates.NotReceived;
                }
                else
                {
                    this.PurchaseOrderShipmentState = purchaseOrderShipmentStates.PartiallyReceived;
                }

                // PurchaseOrder Payment State
                if (validOrderItems.All(v => v.PurchaseOrderItemPaymentState.IsPaid))
                {
                    this.PurchaseOrderPaymentState = purchaseOrderPaymentStates.Paid;
                }
                else if (validOrderItems.All(v => v.PurchaseOrderItemPaymentState.IsNotPaid))
                {
                    this.PurchaseOrderPaymentState = purchaseOrderPaymentStates.NotPaid;
                }
                else
                {
                    this.PurchaseOrderPaymentState = purchaseOrderPaymentStates.PartiallyPaid;
                }

                // PurchaseOrder OrderState
                if (this.PurchaseOrderShipmentState.IsReceived)
                {
                    this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Completed;
                }

                if (this.PurchaseOrderState.IsCompleted && this.PurchaseOrderPaymentState.IsPaid)
                {
                    this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Finished;
                }
            }

            // PurchaseOrderItem States
            foreach (var purchaseOrderItem in validOrderItems)
            {
                if (this.PurchaseOrderState.IsCreated)
                {
                    if (purchaseOrderItem.PurchaseOrderItemState.IsCancelledByOrder)
                    {
                        purchaseOrderItem.PurchaseOrderItemState = purchaseOrderItemStates.Created;
                    }
                }

                if (this.PurchaseOrderState.IsInProcess &&
                    (purchaseOrderItem.PurchaseOrderItemState.IsCreated || purchaseOrderItem.PurchaseOrderItemState.IsOnHold))
                {
                    purchaseOrderItem.PurchaseOrderItemState = purchaseOrderItemStates.InProcess;
                }

                if (this.PurchaseOrderState.IsOnHold && purchaseOrderItem.PurchaseOrderItemState.IsInProcess)
                {
                    purchaseOrderItem.PurchaseOrderItemState = purchaseOrderItemStates.OnHold;
                }

                if (this.PurchaseOrderState.IsSent && purchaseOrderItem.PurchaseOrderItemState.IsInProcess)
                {
                    purchaseOrderItem.PurchaseOrderItemState = purchaseOrderItemStates.Sent;
                }

                if (this.PurchaseOrderState.IsFinished)
                {
                    purchaseOrderItem.PurchaseOrderItemState = purchaseOrderItemStates.Finished;
                }

                if (this.PurchaseOrderState.IsCancelled)
                {
                    purchaseOrderItem.PurchaseOrderItemState = purchaseOrderItemStates.Cancelled;
                }

                if (this.PurchaseOrderState.IsRejected)
                {
                    purchaseOrderItem.PurchaseOrderItemState = purchaseOrderItemStates.Rejected;
                }
            }
            #endregion

            this.BaseOnDeriveOrderItems(derivation);
            this.BaseOnDeriveOrderTotals(derivation);

            this.PreviousTakenViaSupplier = this.TakenViaSupplier;

            this.DeriveWorkflow();

            var singleton = this.strategy.Session.GetSingleton();

            this.SecurityTokens = new[]
            {
                singleton.DefaultSecurityToken
            };

            if (this.ExistOrderedBy)
            {
                this.AddSecurityToken(this.OrderedBy.LocalAdministratorSecurityToken);
                this.AddSecurityToken(this.OrderedBy.PurchaseOrderApproverLevel1SecurityToken);
                this.AddSecurityToken(this.OrderedBy.PurchaseOrderApproverLevel2SecurityToken);
            }

            this.Sync(this.Strategy.Session);

            this.ResetPrintDocument();
        }

        private void Sync(ISession session)
        {
            //session.Prefetch(this.SyncPrefetch, this);

            foreach (PurchaseOrderItem orderItem in this.PurchaseOrderItems)
            {
                orderItem.Sync(this);
            }
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            if (!this.CanInvoice)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Invoice, Operations.Execute));
            }

            if (this.PurchaseOrderShipmentState.IsReceived)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.QuickReceive, Operations.Execute));
            }
        }

        private void DeriveWorkflow()
        {
            this.WorkItemDescription = $"PurchaseOrder: {this.OrderNumber} [{this.TakenViaSupplier?.PartyName}]";

            var openTasks = this.TasksWhereWorkItem.Where(v => !v.ExistDateClosed).ToArray();

            if (this.PurchaseOrderState.IsAwaitingApprovalLevel1)
            {
                if (!openTasks.OfType<PurchaseOrderApprovalLevel1>().Any())
                {
                    new PurchaseOrderApprovalLevel1Builder(this.strategy.Session).WithPurchaseOrder(this).Build();
                }
            }

            if (this.PurchaseOrderState.IsAwaitingApprovalLevel2)
            {
                if (!openTasks.OfType<PurchaseOrderApprovalLevel2>().Any())
                {
                    new PurchaseOrderApprovalLevel2Builder(this.strategy.Session).WithPurchaseOrder(this).Build();
                }
            }
        }

        public void BasePrint(PrintablePrint method)
        {
            if (!method.IsPrinted)
            {
                var singleton = this.Strategy.Session.GetSingleton();
                var logo = this.OrderedBy?.ExistLogoImage == true ?
                    this.OrderedBy.LogoImage.MediaContent.Data :
                    singleton.LogoImage.MediaContent.Data;

                var images = new Dictionary<string, byte[]>
                {
                    { "Logo", logo },
                };

                if (this.ExistOrderNumber)
                {
                    var session = this.Strategy.Session;
                    var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                    var barcode = barcodeService.Generate(this.OrderNumber, BarcodeType.CODE_128, 320, 80);
                    images.Add("Barcode", barcode);
                }

                var model = new Print.PurchaseOrderModel.Model(this);
                this.RenderPrintDocument(this.OrderedBy?.PurchaseOrderTemplate, model, images);

                this.PrintDocument.Media.FileName = $"{this.OrderNumber}.odt";
            }
        }

        public void BaseCancel(OrderCancel method)
        {
            this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Cancelled;
            foreach (PurchaseOrderItem purchaseOrderItem in this.ValidOrderItems)
            {
                purchaseOrderItem.CancelFromOrder();
            }
        }

        public void BaseConfirm(OrderConfirm method) => this.PurchaseOrderState = this.NeedsApprovalLevel1 ? new PurchaseOrderStates(this.Strategy.Session).AwaitingApprovalLevel1 : new PurchaseOrderStates(this.Strategy.Session).InProcess;

        public void BaseReject(OrderReject method)
        {
            this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Rejected;
            foreach (PurchaseOrderItem purchaseOrderItem in this.ValidOrderItems)
            {
                purchaseOrderItem.Reject();
            }
        }

        public void BaseHold(OrderHold method) => this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).OnHold;

        public void BaseApprove(OrderApprove method)
        {
            this.PurchaseOrderState = this.NeedsApprovalLevel2 && this.PurchaseOrderState.IsAwaitingApprovalLevel1 ? new PurchaseOrderStates(this.Strategy.Session).AwaitingApprovalLevel2 : new PurchaseOrderStates(this.Strategy.Session).InProcess;

            var openTasks = this.TasksWhereWorkItem.Where(v => !v.ExistDateClosed).ToArray();

            if (openTasks.OfType<PurchaseOrderApprovalLevel1>().Any())
            {
                openTasks.First().DateClosed = this.strategy.Session.Now();
            }

            if (openTasks.OfType<PurchaseOrderApprovalLevel2>().Any())
            {
                openTasks.First().DateClosed = this.strategy.Session.Now();
            }
        }

        public void BaseReopen(OrderReopen method) => this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Created;

        public void BaseContinue(OrderContinue method) => this.PurchaseOrderState = this.PreviousPurchaseOrderState;

        public void BaseSend(PurchaseOrderSend method) => this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Sent;

        public void BaseQuickReceive(PurchaseOrderQuickReceive method)
        {
            var session = this.strategy.Session;

            if (this.ValidOrderItems.Any(v => ((PurchaseOrderItem)v).ExistPart))
            {
                var shipment = new PurchaseShipmentBuilder(session)
                    .WithShipmentMethod(new ShipmentMethods(session).Ground)
                    .WithShipToParty(this.OrderedBy)
                    .WithShipFromParty(this.TakenViaSupplier)
                    .WithShipToFacility(this.Facility)
                    .Build();

                foreach (PurchaseOrderItem orderItem in this.ValidOrderItems)
                {
                    if (orderItem.PurchaseOrderItemShipmentState.IsNotReceived && orderItem.ExistPart)
                    {
                        var shipmentItem = new ShipmentItemBuilder(session)
                            .WithPart(orderItem.Part)
                            .WithQuantity(orderItem.QuantityOrdered)
                            .WithContentsDescription($"{orderItem.QuantityOrdered} * {orderItem.Part.Name}")
                            .Build();

                        shipment.AddShipmentItem(shipmentItem);

                        new OrderShipmentBuilder(session)
                            .WithOrderItem(orderItem)
                            .WithShipmentItem(shipmentItem)
                            .WithQuantity(orderItem.QuantityOrdered)
                            .Build();

                        new ShipmentReceiptBuilder(session)
                            .WithQuantityAccepted(orderItem.QuantityOrdered)
                            .WithShipmentItem(shipmentItem)
                            .WithOrderItem(orderItem)
                            .Build();

                        if (orderItem.Part.InventoryItemKind.Serialised)
                        {
                            SerialisedItem serialisedItem = orderItem.SerialisedItem;
                            if (!orderItem.ExistSerialisedItem)
                            {
                                serialisedItem = new SerialisedItemBuilder(session)
                                    .WithSerialNumber(orderItem.SerialNumber)
                                    .Build();

                                orderItem.Part.AddSerialisedItem(serialisedItem);
                            }

                            serialisedItem.PurchaseOrder = this;
                            serialisedItem.OwnedBy = this.OrderedBy;
                            serialisedItem.SuppliedBy = this.TakenViaSupplier;
                            serialisedItem.PurchasePrice = orderItem.UnitPrice;

                            new InventoryItemTransactionBuilder(this.strategy.Session)
                                .WithSerialisedItem(serialisedItem)
                                .WithUnitOfMeasure(orderItem.Part.UnitOfMeasure)
                                .WithFacility(this.Facility)
                                .WithReason(new InventoryTransactionReasons(this.Strategy.Session).IncomingShipment)
                                .WithSerialisedInventoryItemState(new SerialisedInventoryItemStates(session).Available)
                                .WithQuantity(1)
                                .Build();
                        }
                    }
                }
            }

            foreach (PurchaseOrderItem orderItem in this.ValidOrderItems.Where(v => !((PurchaseOrderItem)v).ExistPart))
            {
                orderItem.QuantityReceived = 1;
            }
        }

        public void BaseInvoice(PurchaseOrderInvoice method)
        {
            if (this.CanInvoice)
            {
                var purchaseInvoice = new PurchaseInvoiceBuilder(this.Strategy.Session)
                    .WithBilledFrom(this.TakenViaSupplier)
                    .WithBilledFromContactMechanism(this.TakenViaContactMechanism)
                    .WithBilledFromContactPerson(this.TakenViaContactPerson)
                    .WithBilledTo(this.OrderedBy)
                    .WithBilledToContactPerson(this.BillToContactPerson)
                    .WithDescription(this.Description)
                    .WithInvoiceDate(this.strategy.Session.Now())
                    .WithVatRegime(this.VatRegime)
                    .WithDiscountAdjustment(this.DiscountAdjustment)
                    .WithSurchargeAdjustment(this.SurchargeAdjustment)
                    .WithShippingAndHandlingCharge(this.ShippingAndHandlingCharge)
                    .WithFee(this.Fee)
                    .WithCustomerReference(this.CustomerReference)
                    .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.strategy.Session).PurchaseInvoice)
                    .Build();

                foreach (PurchaseOrderItem orderItem in this.ValidOrderItems)
                {
                    if (orderItem.CanInvoice)
                    {
                        var invoiceItem = new PurchaseInvoiceItemBuilder(this.Strategy.Session)
                            .WithAssignedUnitPrice(orderItem.UnitPrice)
                            .WithPart(orderItem.Part)
                            .WithQuantity(orderItem.QuantityOrdered)
                            .WithDescription(orderItem.Description)
                            .WithInternalComment(orderItem.InternalComment)
                            .WithMessage(orderItem.Message)
                            .Build();

                        if (invoiceItem.ExistPart)
                        {
                            invoiceItem.InvoiceItemType = new InvoiceItemTypes(this.Strategy.Session).PartItem;
                        }
                        else
                        {
                            invoiceItem.InvoiceItemType = new InvoiceItemTypes(this.Strategy.Session).WorkDone;
                        }

                        purchaseInvoice.AddPurchaseInvoiceItem(invoiceItem);

                        new OrderItemBillingBuilder(this.Strategy.Session)
                            .WithQuantity(orderItem.QuantityOrdered)
                            .WithAmount(orderItem.TotalBasePrice)
                            .WithOrderItem(orderItem)
                            .WithInvoiceItem(invoiceItem)
                            .Build();
                    }
                }
            }
        }

        public void BaseOnDeriveOrderTotals(IDerivation derivation)
        {
            if (this.ExistValidOrderItems)
            {
                this.TotalBasePrice = 0;
                this.TotalDiscount = 0;
                this.TotalSurcharge = 0;
                this.TotalVat = 0;
                this.TotalExVat = 0;
                this.TotalIncVat = 0;

                foreach (PurchaseOrderItem orderItem in this.ValidOrderItems)
                {
                    this.TotalBasePrice += orderItem.TotalBasePrice;
                    this.TotalDiscount += orderItem.TotalDiscount;
                    this.TotalSurcharge += orderItem.TotalSurcharge;
                    this.TotalVat += orderItem.TotalVat;
                    this.TotalExVat += orderItem.TotalExVat;
                    this.TotalIncVat += orderItem.TotalIncVat;
                }
            }
        }

        public void BaseOnDeriveOrderItems(IDerivation derivation)
        {
            var quantityOrderedByProduct = new Dictionary<Product, decimal>();
            var totalBasePriceByProduct = new Dictionary<Product, decimal>();
            var quantityOrderedByPart = new Dictionary<Part, decimal>();
            var totalBasePriceByPart = new Dictionary<Part, decimal>();

            foreach (PurchaseOrderItem purchaseOrderItem in this.ValidOrderItems)
            {
                purchaseOrderItem.OnDerive(x => x.WithDerivation(derivation));
                purchaseOrderItem.BaseOnDeriveDeliveryDate(derivation);
                purchaseOrderItem.BaseDeriveVatRegime(derivation);
                purchaseOrderItem.BaseOnDerivePrices();

                if (purchaseOrderItem.ExistPart)
                {
                    if (!quantityOrderedByPart.ContainsKey(purchaseOrderItem.Part))
                    {
                        quantityOrderedByPart.Add(purchaseOrderItem.Part, purchaseOrderItem.QuantityOrdered);
                        totalBasePriceByPart.Add(purchaseOrderItem.Part, purchaseOrderItem.TotalBasePrice);
                    }
                    else
                    {
                        quantityOrderedByPart[purchaseOrderItem.Part] += purchaseOrderItem.QuantityOrdered;
                        totalBasePriceByPart[purchaseOrderItem.Part] += purchaseOrderItem.TotalBasePrice;
                    }
                }
            }
        }
    }
}
