// <copyright file="PurchaseOrder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;
    using Allors.Services;
    using Microsoft.Extensions.DependencyInjection;
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

        private bool NeedsApprovalLevel1
        {
            get
            {
                if (this.ExistTakenViaSupplier && this.ExistOrderedBy)
                {
                    var supplierRelationship = ((Organisation)this.TakenViaSupplier).SupplierRelationshipsWhereSupplier.FirstOrDefault(v => v.InternalOrganisation.Equals(this.OrderedBy));
                    if (supplierRelationship != null
                        && supplierRelationship.NeedsApproval
                        && supplierRelationship.ApprovalThresholdLevel1.HasValue
                        && this.TotalExVat >= supplierRelationship.ApprovalThresholdLevel1.Value)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool NeedsApprovalLevel2
        {
            get
            {
                if (this.ExistTakenViaSupplier && this.ExistOrderedBy)
                {
                    var supplierRelationship = ((Organisation)this.TakenViaSupplier).SupplierRelationshipsWhereSupplier.FirstOrDefault(v => v.InternalOrganisation.Equals(this.OrderedBy));
                    if (supplierRelationship != null
                        && supplierRelationship.NeedsApproval
                        && supplierRelationship.ApprovalThresholdLevel2.HasValue
                        && this.TotalExVat >= supplierRelationship.ApprovalThresholdLevel2.Value)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool CanInvoice
        {
            get
            {
                if (this.PurchaseOrderState.IsSent || this.PurchaseOrderState.IsCompleted)
                {
                    foreach (PurchaseOrderItem purchaseOrderItem in this.ValidOrderItems)
                    {
                        if (!purchaseOrderItem.ExistOrderItemBillingsWhereOrderItem)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private bool CanRevise
        {
            get
            {
                if ((this.PurchaseOrderState.IsInProcess || this.PurchaseOrderState.IsSent || this.PurchaseOrderState.IsCompleted)
                    && (this.PurchaseOrderShipmentState.IsNotReceived || this.PurchaseOrderShipmentState.IsNa))
                {
                    if (!this.ExistPurchaseInvoicesWherePurchaseOrder)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool IsReceivable
        {
            get
            {
                if (this.PurchaseOrderState.IsSent
                    && this.ValidOrderItems.Any(v => ((PurchaseOrderItem)v).IsReceivable))
                {
                    return true;
                }

                return false;
            }
        }

        private bool IsDeletable =>
            (this.PurchaseOrderState.Equals(new PurchaseOrderStates(this.Strategy.Session).Created)
                || this.PurchaseOrderState.Equals(new PurchaseOrderStates(this.Strategy.Session).Cancelled)
                || this.PurchaseOrderState.Equals(new PurchaseOrderStates(this.Strategy.Session).Rejected))
            && !this.ExistPurchaseInvoicesWherePurchaseOrder
            && !this.ExistSerialisedItemsWherePurchaseOrder
            && !this.ExistWorkEffortPurchaseOrderItemAssignmentsWherePurchaseOrder
            && this.PurchaseOrderItems.All(v => v.IsDeletable);

        public void BaseOnInit(ObjectOnInit method)
        {
            this.OrderDate = this.Session().Now();

            this.PurchaseOrderState ??= new PurchaseOrderStates(this.Strategy.Session).Created;
            this.PurchaseOrderShipmentState ??= new PurchaseOrderShipmentStates(this.Strategy.Session).NotReceived;
            this.PurchaseOrderPaymentState ??= new PurchaseOrderPaymentStates(this.Strategy.Session).NotPaid;

            if (!this.ExistEntryDate)
            {
                this.EntryDate = this.Session().Now();
            }

            if (!this.ExistOrderedBy)
            {
                var internalOrganisations = new Organisations(this.Strategy.Session).InternalOrganisations();
                if (internalOrganisations.Count() == 1)
                {
                    this.OrderedBy = internalOrganisations.First();
                }
            }

            this.DerivedCurrency = this.OrderedBy?.PreferredCurrency;
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;
            var singleton = this.Strategy.Session.GetSingleton();

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                iteration.AddDependency(this, singleton);
                iteration.Mark(singleton);

                if (this.ExistTakenViaSupplier)
                {
                    iteration.AddDependency(this, this.TakenViaSupplier);
                    iteration.Mark(this.TakenViaSupplier);
                }

                if (this.ExistTakenViaSubcontractor)
                {
                    iteration.AddDependency(this, this.TakenViaSubcontractor);
                    iteration.Mark(this.TakenViaSubcontractor);
                }

                foreach (PurchaseOrderItem orderItem in this.PurchaseOrderItems)
                {
                    iteration.AddDependency(this, orderItem);
                    iteration.Mark(orderItem);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistOrderNumber)
            {
                var year = this.OrderDate.Year;
                this.OrderNumber = this.OrderedBy.NextPurchaseOrderNumber(year);

                var fiscalYearInternalOrganisationSequenceNumbers = this.OrderedBy?.FiscalYearsInternalOrganisationSequenceNumbers.FirstOrDefault(v => v.FiscalYear == year);
                var prefix = this.OrderedBy.InvoiceSequence.IsEnforcedSequence ? this.OrderedBy?.PurchaseOrderNumberPrefix : fiscalYearInternalOrganisationSequenceNumbers.PurchaseOrderNumberPrefix;
                this.SortableOrderNumber = this.Session().GetSingleton().SortableNumber(prefix, this.OrderNumber, year.ToString());
            }

            if (this.TakenViaSupplier is Organisation supplier)
            {
                if (!this.OrderedBy.ActiveSuppliers.Contains(supplier))
                {
                    derivation.Validation.AddError(this, this.Meta.TakenViaSupplier, ErrorMessages.PartyIsNotASupplier);
                }
            }

            if (this.TakenViaSubcontractor is Organisation subcontractor)
            {
                if (!this.OrderedBy.ActiveSubContractors.Contains(subcontractor))
                {
                    derivation.Validation.AddError(this, this.Meta.TakenViaSubcontractor, ErrorMessages.PartyIsNotASubcontractor);
                }
            }

            if (this.PurchaseOrderState.IsCreated)
            {
                this.DerivedLocale = this.Locale ?? this.OrderedBy?.Locale;
                this.DerivedCurrency = this.AssignedCurrency ?? this.OrderedBy?.PreferredCurrency;
                this.DerivedVatRegime = this.AssignedVatRegime;
                this.DerivedIrpfRegime = this.AssignedIrpfRegime;
                this.DerivedShipToAddress = this.AssignedShipToAddress ?? this.OrderedBy?.ShippingAddress;
                this.DerivedBillToContactMechanism = this.AssignedBillToContactMechanism ?? this.OrderedBy?.BillingAddress ?? this.OrderedBy?.GeneralCorrespondence;
                this.DerivedTakenViaContactMechanism = this.AssignedTakenViaContactMechanism ?? this.TakenViaSupplier?.OrderAddress;
            }
    
            derivation.Validation.AssertExistsAtMostOne(this, this.Meta.TakenViaSupplier, this.Meta.TakenViaSubcontractor);
            derivation.Validation.AssertAtLeastOne(this, this.Meta.TakenViaSupplier, this.Meta.TakenViaSubcontractor);

            if (this.ExistOrderDate
                && this.ExistOrderedBy
                && this.DerivedCurrency != this.OrderedBy.PreferredCurrency)
            {
                var exchangeRate = this.DerivedCurrency.ExchangeRatesWhereFromCurrency.Where(v => v.ValidFrom.Date <= this.OrderDate.Date && v.ToCurrency.Equals(this.OrderedBy.PreferredCurrency)).OrderByDescending(v => v.ValidFrom).FirstOrDefault();

                if (exchangeRate == null)
                {
                    exchangeRate = this.OrderedBy.PreferredCurrency.ExchangeRatesWhereFromCurrency.Where(v => v.ValidFrom.Date <= this.OrderDate.Date && v.ToCurrency.Equals(this.DerivedCurrency)).OrderByDescending(v => v.ValidFrom).FirstOrDefault();
                }

                if (exchangeRate == null)
                {
                    derivation.Validation.AddError(this, M.Quote.AssignedCurrency, ErrorMessages.CurrencyNotAllowed);
                }
            }

            var validOrderItems = this.PurchaseOrderItems.Where(v => v.IsValid).ToArray();
            this.ValidOrderItems = validOrderItems;

            var purchaseOrderShipmentStates = new PurchaseOrderShipmentStates(this.Strategy.Session);
            var purchaseOrderPaymentStates = new PurchaseOrderPaymentStates(this.Strategy.Session);
            var purchaseOrderItemStates = new PurchaseOrderItemStates(derivation.Session);

            // PurchaseOrder Shipment State
            if (validOrderItems.Any())
            {
                if (validOrderItems.Any(v => v.IsReceivable))
                {
                    if (validOrderItems.Where(v => v.IsReceivable).All(v => v.PurchaseOrderItemShipmentState.IsReceived))
                    {
                        this.PurchaseOrderShipmentState = purchaseOrderShipmentStates.Received;
                    }
                    else if (validOrderItems.Where(v => v.IsReceivable).All(v => v.PurchaseOrderItemShipmentState.IsNotReceived))
                    {
                        this.PurchaseOrderShipmentState = purchaseOrderShipmentStates.NotReceived;
                    }
                    else
                    {
                        this.PurchaseOrderShipmentState = purchaseOrderShipmentStates.PartiallyReceived;
                    }
                }
                else
                {
                    this.PurchaseOrderShipmentState = purchaseOrderShipmentStates.Na;
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
                if (this.PurchaseOrderState.IsSent
                    && (this.PurchaseOrderShipmentState.IsReceived || this.PurchaseOrderShipmentState.IsNa))
                {
                    this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Completed;
                }

                if (this.PurchaseOrderState.IsCompleted && this.PurchaseOrderPaymentState.IsPaid)
                {
                    this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Finished;
                }
            }

            // Derive Totals
            var quantityOrderedByPart = new Dictionary<Part, decimal>();
            var totalBasePriceByPart = new Dictionary<Part, decimal>();
            foreach (PurchaseOrderItem item in this.ValidOrderItems)
            {
                if (item.ExistPart)
                {
                    if (!quantityOrderedByPart.ContainsKey(item.Part))
                    {
                        quantityOrderedByPart.Add(item.Part, item.QuantityOrdered);
                        totalBasePriceByPart.Add(item.Part, item.TotalBasePrice);
                    }
                    else
                    {
                        quantityOrderedByPart[item.Part] += item.QuantityOrdered;
                        totalBasePriceByPart[item.Part] += item.TotalBasePrice;
                    }
                }
            }

            this.TotalBasePrice = 0;
            this.TotalDiscount = 0;
            this.TotalSurcharge = 0;
            this.TotalVat = 0;
            this.TotalIrpf = 0;
            this.TotalExVat = 0;
            this.TotalExtraCharge = 0;
            this.TotalIncVat = 0;
            this.GrandTotal = 0;
            this.TotalShippingAndHandlingInPreferredCurrency = 0;
            this.TotalFeeInPreferredCurrency = 0;
            this.TotalExtraChargeInPreferredCurrency = 0;
            this.TotalListPriceInPreferredCurrency = 0;

            foreach (PurchaseOrderItem orderItem in this.ValidOrderItems)
            {
                this.TotalBasePrice += orderItem.TotalBasePrice;
                this.TotalDiscount += orderItem.TotalDiscount;
                this.TotalSurcharge += orderItem.TotalSurcharge;
                this.TotalVat += orderItem.TotalVat;
                this.TotalIrpf += orderItem.TotalIrpf;
                this.TotalExVat += orderItem.TotalExVat;
                this.TotalIncVat += orderItem.TotalIncVat;
                this.GrandTotal += orderItem.GrandTotal;
            }

            if (this.ExistOrderDate && this.ExistDerivedCurrency && this.ExistOrderedBy)
            {
                this.TotalBasePriceInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalBasePrice, this.OrderDate, this.DerivedCurrency, this.OrderedBy.PreferredCurrency), 2);
                this.TotalDiscountInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalDiscount, this.OrderDate, this.DerivedCurrency, this.OrderedBy.PreferredCurrency), 2);
                this.TotalSurchargeInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalSurcharge, this.OrderDate, this.DerivedCurrency, this.OrderedBy.PreferredCurrency), 2);
                this.TotalExVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalExVat, this.OrderDate, this.DerivedCurrency, this.OrderedBy.PreferredCurrency), 2);
                this.TotalVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalVat, this.OrderDate, this.DerivedCurrency, this.OrderedBy.PreferredCurrency), 2);
                this.TotalIncVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalIncVat, this.OrderDate, this.DerivedCurrency, this.OrderedBy.PreferredCurrency), 2);
                this.TotalIrpfInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalIrpf, this.OrderDate, this.DerivedCurrency, this.OrderedBy.PreferredCurrency), 2);
                this.GrandTotalInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.GrandTotal, this.OrderDate, this.DerivedCurrency, this.OrderedBy.PreferredCurrency), 2);
            }

            this.TotalBasePrice = Rounder.RoundDecimal(this.TotalBasePrice, 2);
            this.TotalDiscount = Rounder.RoundDecimal(this.TotalDiscount, 2);
            this.TotalSurcharge = Rounder.RoundDecimal(this.TotalSurcharge, 2);
            this.TotalVat = Rounder.RoundDecimal(this.TotalVat, 2);
            this.TotalIrpf = Rounder.RoundDecimal(this.TotalIrpf, 2);
            this.TotalExVat = Rounder.RoundDecimal(this.TotalExVat, 2);
            this.TotalIncVat = Rounder.RoundDecimal(this.TotalIncVat, 2);
            this.GrandTotal = Rounder.RoundDecimal(this.GrandTotal, 2);

            this.PreviousTakenViaSupplier = this.TakenViaSupplier;

            // Derive Workflow
            this.WorkItemDescription = $"PurchaseOrder: {this.OrderNumber} [{this.TakenViaSupplier?.PartyName}]";
            var openTasks = this.TasksWhereWorkItem.Where(v => !v.ExistDateClosed).ToArray();
            if (this.PurchaseOrderState.IsAwaitingApprovalLevel1)
            {
                if (!openTasks.OfType<PurchaseOrderApprovalLevel1>().Any())
                {
                    new PurchaseOrderApprovalLevel1Builder(this.Session()).WithPurchaseOrder(this).Build();
                }
            }

            if (this.PurchaseOrderState.IsAwaitingApprovalLevel2)
            {
                if (!openTasks.OfType<PurchaseOrderApprovalLevel2>().Any())
                {
                    new PurchaseOrderApprovalLevel2Builder(this.Session()).WithPurchaseOrder(this).Build();
                }
            }

            this.Sync(derivation, validOrderItems);

            this.ResetPrintDocument();
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            if (this.CanInvoice)
            {
                this.RemoveDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Invoice, Operations.Execute));
            }
            else
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Invoice, Operations.Execute));
            }

            if (this.CanRevise)
            {
                this.RemoveDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Revise, Operations.Execute));
            }
            else
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Revise, Operations.Execute));
            }

            if (this.IsReceivable)
            {
                this.RemoveDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.QuickReceive, Operations.Execute));
            }
            else
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.QuickReceive, Operations.Execute));
            }

            var deletePermission = new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.Delete, Operations.Execute);
            if (this.IsDeletable)
            {
                this.RemoveDeniedPermission(deletePermission);
            }
            else
            {
                this.AddDeniedPermission(deletePermission);
            }

            if (!this.PurchaseOrderShipmentState.IsNotReceived && !this.PurchaseOrderShipmentState.IsNa)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Reject, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Cancel, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.QuickReceive, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Revise, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.SetReadyForProcessing, Operations.Execute));

                var deniablePermissionByOperandTypeId = new Dictionary<Guid, Permission>();

                foreach (Permission permission in this.Session().Extent<Permission>())
                {
                    if (permission.ConcreteClassPointer == this.strategy.Class.Id && permission.Operation == Operations.Write)
                    {
                        deniablePermissionByOperandTypeId.Add(permission.OperandTypePointer, permission);
                    }
                }

                foreach (var keyValuePair in deniablePermissionByOperandTypeId)
                {
                    this.AddDeniedPermission(keyValuePair.Value);
                }
            }

            if (this.PurchaseOrderState.IsCompleted && this.PurchaseOrderPaymentState.IsNotPaid && this.PurchaseOrderShipmentState.IsNa)
            {
                this.RemoveDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Reopen, Operations.Execute));
            }
        }

        public void BaseDelete(PurchaseOrderDelete method)
        {
            if (this.IsDeletable)
            {
                foreach (OrderAdjustment orderAdjustment in this.OrderAdjustments)
                {
                    orderAdjustment.Delete();
                }

                foreach (PurchaseOrderItem item in this.PurchaseOrderItems)
                {
                    item.Delete();
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
                    var barcode = barcodeService.Generate(this.OrderNumber, BarcodeType.CODE_128, 320, 80, pure: true);
                    images.Add("Barcode", barcode);
                }

                var model = new Print.PurchaseOrderModel.Model(this);
                this.RenderPrintDocument(this.OrderedBy?.PurchaseOrderTemplate, model, images);

                this.PrintDocument.Media.InFileName = $"{this.OrderNumber}.odt";
            }
        }

        public void BaseCancel(OrderCancel method) => this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Cancelled;

        public void BaseSetReadyForProcessing(PurchaseOrderSetReadyForProcessing method) => this.PurchaseOrderState = this.NeedsApprovalLevel1
                ? new PurchaseOrderStates(this.Strategy.Session).AwaitingApprovalLevel1
                : this.PurchaseOrderState = this.NeedsApprovalLevel2
                    ? new PurchaseOrderStates(this.Strategy.Session).AwaitingApprovalLevel2
                    : new PurchaseOrderStates(this.Strategy.Session).InProcess;

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
                openTasks.First().DateClosed = this.Session().Now();
            }

            if (openTasks.OfType<PurchaseOrderApprovalLevel2>().Any())
            {
                openTasks.First().DateClosed = this.Session().Now();
            }
        }

        public void BaseRevise(PurchaseOrderRevise method) => this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Created;

        public void BaseReopen(OrderReopen method) => this.PurchaseOrderState = this.PreviousPurchaseOrderState;

        public void BaseContinue(OrderContinue method) => this.PurchaseOrderState = this.PreviousPurchaseOrderState;

        public void BaseSend(PurchaseOrderSend method) => this.PurchaseOrderState = new PurchaseOrderStates(this.Strategy.Session).Sent;

        public void BaseQuickReceive(PurchaseOrderQuickReceive method)
        {
            var session = this.Session();

            if (this.ValidOrderItems.Any(v => ((PurchaseOrderItem)v).IsReceivable))
            {
                var shipment = new PurchaseShipmentBuilder(session)
                    .WithShipmentMethod(new ShipmentMethods(session).Ground)
                    .WithShipToParty(this.OrderedBy)
                    .WithShipToAddress(this.DerivedShipToAddress)
                    .WithShipFromParty(this.TakenViaSupplier)
                    .WithShipToFacility(this.StoredInFacility)
                    .Build();

                foreach (PurchaseOrderItem orderItem in this.ValidOrderItems)
                {
                    ShipmentItem shipmentItem = null;
                    if (orderItem.PurchaseOrderItemShipmentState.IsNotReceived && orderItem.ExistPart)
                    {
                        shipmentItem = new ShipmentItemBuilder(session)
                            .WithPart(orderItem.Part)
                            .WithStoredInFacility(orderItem.StoredInFacility)
                            .WithQuantity(orderItem.QuantityOrdered)
                            .WithUnitPurchasePrice(orderItem.UnitPrice)
                            .WithContentsDescription($"{orderItem.QuantityOrdered} * {orderItem.Part.Name}")
                            .Build();

                        shipment.AddShipmentItem(shipmentItem);

                        new OrderShipmentBuilder(session)
                            .WithOrderItem(orderItem)
                            .WithShipmentItem(shipmentItem)
                            .WithQuantity(orderItem.QuantityOrdered)
                            .Build();

                        if (orderItem.Part.InventoryItemKind.IsSerialised)
                        {
                            var serialisedItem = orderItem.SerialisedItem;
                            if (!orderItem.ExistSerialisedItem)
                            {
                                serialisedItem = new SerialisedItemBuilder(session)
                                    .WithSerialNumber(orderItem.SerialNumber)
                                    .Build();

                                orderItem.Part.AddSerialisedItem(serialisedItem);
                            }

                            shipmentItem.SerialisedItem = serialisedItem;

                            serialisedItem.AcquiredDate = orderItem.PurchaseOrderWherePurchaseOrderItem.OrderDate;

                            if (serialisedItem.ExistAcquiredDate && serialisedItem.ExistAcquisitionYear)
                            {
                                serialisedItem.RemoveAcquisitionYear();
                            }

                            if (this.OrderedBy.SerialisedItemSoldOns.Contains(new SerialisedItemSoldOns(this.Session()).PurchaseshipmentReceive))
                            {
                                serialisedItem.Buyer = this.OrderedBy;
                                serialisedItem.OwnedBy = this.OrderedBy;
                                serialisedItem.Ownership = new Ownerships(this.Session()).Own;
                            }
                        }
                    }
                }

                if (shipment.ShipToParty is InternalOrganisation internalOrganisation)
                {
                    if (internalOrganisation.IsAutomaticallyReceived)
                    {
                        shipment.Receive();
                    }
                }
            }
        }

        public void BaseInvoice(PurchaseOrderInvoice method)
        {
            if (this.CanInvoice)
            {
                var purchaseInvoice = new PurchaseInvoiceBuilder(this.Strategy.Session)
                    .WithBilledFrom(this.TakenViaSupplier)
                    .WithAssignedBilledFromContactMechanism(this.DerivedTakenViaContactMechanism)
                    .WithBilledFromContactPerson(this.TakenViaContactPerson)
                    .WithBilledTo(this.OrderedBy)
                    .WithBilledToContactPerson(this.BillToContactPerson)
                    .WithDescription(this.Description)
                    .WithInvoiceDate(this.Session().Now())
                    .WithAssignedVatRegime(this.DerivedVatRegime)
                    .WithAssignedIrpfRegime(this.DerivedIrpfRegime)
                    .WithCustomerReference(this.CustomerReference)
                    .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session()).PurchaseInvoice)
                    .Build();

                foreach (OrderAdjustment orderAdjustment in this.OrderAdjustments)
                {
                    OrderAdjustment newAdjustment = null;
                    if (orderAdjustment.GetType().Name.Equals(typeof(DiscountAdjustment).Name))
                    {
                        newAdjustment = new DiscountAdjustmentBuilder(this.Session()).Build();
                    }

                    if (orderAdjustment.GetType().Name.Equals(typeof(SurchargeAdjustment).Name))
                    {
                        newAdjustment = new SurchargeAdjustmentBuilder(this.Session()).Build();
                    }

                    if (orderAdjustment.GetType().Name.Equals(typeof(Fee).Name))
                    {
                        newAdjustment = new FeeBuilder(this.Session()).Build();
                    }

                    if (orderAdjustment.GetType().Name.Equals(typeof(ShippingAndHandlingCharge).Name))
                    {
                        newAdjustment = new ShippingAndHandlingChargeBuilder(this.Session()).Build();
                    }

                    if (orderAdjustment.GetType().Name.Equals(typeof(MiscellaneousCharge).Name))
                    {
                        newAdjustment = new MiscellaneousChargeBuilder(this.Session()).Build();
                    }

                    newAdjustment.Amount ??= orderAdjustment.Amount;
                    newAdjustment.Percentage ??= orderAdjustment.Percentage;
                    purchaseInvoice.AddOrderAdjustment(newAdjustment);
                }

                foreach (PurchaseOrderItem orderItem in this.ValidOrderItems)
                {
                    if (orderItem.CanInvoice)
                    {
                        var invoiceItem = new PurchaseInvoiceItemBuilder(this.Strategy.Session)
                            .WithAssignedUnitPrice(orderItem.UnitPrice)
                            .WithInvoiceItemType(orderItem.InvoiceItemType)
                            .WithPart(orderItem.Part)
                            .WithQuantity(orderItem.QuantityOrdered)
                            .WithAssignedVatRegime(orderItem.AssignedVatRegime)
                            .WithAssignedIrpfRegime(orderItem.AssignedIrpfRegime)
                            .WithDescription(orderItem.Description)
                            .WithInternalComment(orderItem.InternalComment)
                            .WithMessage(orderItem.Message)
                            .Build();

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

        private void Sync(IDerivation derivation, PurchaseOrderItem[] validOrderItems)
        {
            foreach (var orderItem in validOrderItems)
            {
                orderItem.Sync(this);
            }
        }
    }
}
