// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrder.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public bool NeedsApproval
        {
            get
            {
                if (this.ExistTakenViaSupplier && this.ExistOrderedBy)
                {
                    var supplierRelationship = this.TakenViaSupplier.SupplierRelationshipsWhereSupplier.FirstOrDefault(v => v.InternalOrganisation.Equals(this.OrderedBy));
                    if (supplierRelationship != null && 
                        supplierRelationship.NeedsApproval &&
                        (!supplierRelationship.ExistApprovalThreshold || this.TotalExVat >= supplierRelationship.ApprovalThreshold))
                    {
                        return true;
                    }
                    else
                    {
                        if (OrderedBy.PurchaseOrderNeedsApproval &&
                            (!OrderedBy.ExistPurchaseOrderApprovalThreshold || this.TotalExVat >= OrderedBy.PurchaseOrderApprovalThreshold))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistPurchaseOrderState)
            {
                this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Created;
            }

            if (this.ExistTakenViaSupplier)
            {
                this.PreviousTakenViaSupplier = this.TakenViaSupplier;
            }

            if (!this.ExistOrderDate)
            {
                this.OrderDate = DateTime.UtcNow;
            }

            if (!this.ExistEntryDate)
            {
                this.EntryDate = DateTime.UtcNow;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
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

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistOrderedBy && internalOrganisations.Count() == 1)
            {
                this.OrderedBy = internalOrganisations.First();
            }

            if (!this.ExistOrderNumber)
            {
                this.OrderNumber = this.OrderedBy.NextPurchaseOrderNumber();
            }

            if (!this.ExistCurrency)
            {
                this.Currency = this.OrderedBy.PreferredCurrency;
            }

            if (!this.ExistFacility && this.OrderedBy.StoresWhereInternalOrganisation.Count == 1)
            {
                this.Facility = this.OrderedBy.StoresWhereInternalOrganisation.Single().DefaultFacility;
            }

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
                this.BillToContactMechanism = this.OrderedBy.ExistBillingAddress? this.OrderedBy.BillingAddress : this.OrderedBy.GeneralCorrespondence;
            }

            if (!this.ExistTakenViaContactMechanism && this.ExistTakenViaSupplier)
            {
                this.TakenViaContactMechanism = this.TakenViaSupplier.OrderAddress;
            }

            this.VatRegime = this.VatRegime ?? this.TakenViaSupplier?.VatRegime;

            this.Locale = this.Strategy.Session.GetSingleton().DefaultLocale;

            this.AppsOnDeriveOrderItems(derivation);
            this.AppsOnDerivePurchaseOrderState(derivation);
            this.AppsOnDeriveOrderTotals(derivation);

            this.PreviousTakenViaSupplier = this.TakenViaSupplier;

            this.DeriveWorkflow();
            this.ResetPrintDocument();
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            if (this.PurchaseInvoicesWherePurchaseOrder.Count > 0)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Invoice, Operations.Execute));
            }
        }

        private void DeriveWorkflow()
        {
            this.WorkItemDescription = $"PurchaseOrder: {this.OrderNumber} [{this.TakenViaSupplier?.PartyName}]";

            var openTasks = this.TasksWhereWorkItem.Where(v => !v.ExistDateClosed).ToArray();

            if (this.PurchaseOrderState.IsAwaitingApproval)
            {
                if (!openTasks.OfType<PurchaseOrderApproval>().Any())
                {
                    new PurchaseOrderApprovalBuilder(this.strategy.Session).WithPurchaseOrder(this).Build();
                }
            }
        }

        public void AppsPrint(PrintablePrint method)
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

        public void AppsCancel(OrderCancel method)
        {
            this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Cancelled;
        }

        public void AppsConfirm(OrderConfirm method)
        {
            this.PurchaseOrderState = this.NeedsApproval ? new PurchaseOrderStates(this.Strategy.Session).AwaitingApproval : new PurchaseOrderStates(this.Strategy.Session).InProcess;
        }

        public void AppsReject(OrderReject method)
        {
            this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Rejected;
        }

        public void AppsHold(OrderHold method)
        {
            this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).OnHold;
        }

        public void AppsApprove(OrderApprove method)
        {
            this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).InProcess;
        }

        public void AppsReopen(OrderReopen method)
        {
            this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Created;
        }

        public void AppsContinue(OrderContinue method)
        {
            this.PurchaseOrderState = this.PreviousPurchaseOrderState;
        }

        public void AppsSend(PurchaseOrderSend method)
        {
            this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Sent;
        }

        public void AppsQuickReceive(PurchaseOrderQuickReceive method)
        {
            var session = this.strategy.Session;

            //Shipment receipt
            var shipment = new PurchaseShipmentBuilder(session)
                .WithShipmentMethod(new ShipmentMethods(session).Ground)
                .WithReceiver(this.OrderedBy)
                .WithShipFromParty(this.TakenViaSupplier)
                .WithFacility(this.Facility)
                .Build();

            foreach (PurchaseOrderItem orderItem in this.PurchaseOrderItems)
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
            }
        }

        public void AppsInvoice(PurchaseOrderInvoice method)
        {
            if (this.PurchaseInvoicesWherePurchaseOrder.Count == 0)
            {
                var purchaseInvoice = new PurchaseInvoiceBuilder(this.Strategy.Session)
                    .WithPurchaseOrder(this)
                    .WithBilledFrom(this.TakenViaSupplier)
                    .WithBilledFromContactMechanism(this.TakenViaContactMechanism)
                    .WithBilledFromContactPerson(this.TakenViaContactPerson)
                    .WithBilledTo(this.OrderedBy)
                    .WithBilledToContactPerson(this.BillToContactPerson)
                    .WithDescription(this.Description)
                    .WithInvoiceDate(DateTime.UtcNow)
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
                    var invoiceItem = new PurchaseInvoiceItemBuilder(this.Strategy.Session)
                        .WithAssignedUnitPrice(orderItem.UnitPrice)
                        .WithPart(orderItem.Part)
                        .WithQuantity(orderItem.QuantityOrdered)
                        .WithDescription(orderItem.Description)
                        .WithInternalComment(orderItem.InternalComment)
                        .WithMessage(orderItem.Message)
                        .Build();

                    purchaseInvoice.AddPurchaseInvoiceItem(invoiceItem);
                }
            }
        }

        public void AppsOnDerivePurchaseOrderState(IDerivation derivation)
        {
            if (this.ExistPurchaseOrderShipmentState && this.PurchaseOrderShipmentState.Equals(new PurchaseOrderShipmentStates(this.Strategy.Session).Received))
            {
                this.Complete();
            }

            if (this.ExistPurchaseOrderPaymentState && this.PurchaseOrderPaymentState.Equals(new PurchaseOrderPaymentStates(this.Strategy.Session).Paid))
            {
                this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Finished;
            }
        }

        public void AppsOnDeriveOrderTotals(IDerivation derivation)
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

        public void AppsOnDeriveOrderItems(IDerivation derivation)
        {
            var quantityOrderedByProduct = new Dictionary<Product, decimal>();
            var totalBasePriceByProduct = new Dictionary<Product, decimal>();
            var quantityOrderedByPart = new Dictionary<Part, decimal>();
            var totalBasePriceByPart = new Dictionary<Part, decimal>();

            foreach (PurchaseOrderItem purchaseOrderItem in this.ValidOrderItems)
            {
                purchaseOrderItem.OnDerive(x => x.WithDerivation(derivation));
                purchaseOrderItem.AppsOnDeriveDeliveryDate(derivation);
                purchaseOrderItem.AppsOnDeriveCurrentShipmentStatus(derivation);
                purchaseOrderItem.AppsDeriveVatRegime(derivation);
                purchaseOrderItem.AppsOnDerivePrices();

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