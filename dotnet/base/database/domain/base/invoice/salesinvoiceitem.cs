// <copyright file="SalesInvoiceItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using System.Text;
    using Allors.Meta;
    using Resources;

    public partial class SalesInvoiceItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.SalesInvoiceItem, M.SalesInvoiceItem.SalesInvoiceItemState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public bool IsValid => !(this.SalesInvoiceItemState.IsCancelledByInvoice || this.SalesInvoiceItemState.IsWrittenOff);

        public decimal PriceAdjustment => this.TotalSurcharge - this.TotalDiscount;

        public decimal PriceAdjustmentAsPercentage => Rounder.RoundDecimal((this.TotalSurcharge - this.TotalDiscount) / this.TotalBasePrice * 100, 2);

        public Part DerivedPart
        {
            get
            {
                if (this.ExistPart)
                {
                    return this.Part;
                }
                else
                {
                    if (this.ExistProduct)
                    {
                        var nonUnifiedGood = this.Product as NonUnifiedGood;
                        var unifiedGood = this.Product as UnifiedGood;
                        return unifiedGood ?? nonUnifiedGood?.Part;
                    }
                }

                return null;
            }
        }

        internal bool IsDeletable =>
            this.SalesInvoiceItemState.Equals(new SalesInvoiceItemStates(this.Strategy.Session).ReadyForPosting);

        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            if (method.SecurityTokens == null)
            {
                method.SecurityTokens = this.SyncedInvoice?.SecurityTokens.ToArray();
            }

            if (method.Restrictions == null)
            {
                method.Restrictions = this.SyncedInvoice?.Restrictions.ToArray();
            }
        }

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSalesInvoiceItemState)
            {
                this.SalesInvoiceItemState = new SalesInvoiceItemStates(this.Strategy.Session).ReadyForPosting;
            }

            if (this.ExistProduct && !this.ExistInvoiceItemType)
            {
                this.InvoiceItemType = new InvoiceItemTypes(this.Strategy.Session).ProductItem;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {

                if (this.ExistSalesInvoiceWhereSalesInvoiceItem)
                {
                    iteration.AddDependency(this.SalesInvoiceWhereSalesInvoiceItem, this);
                    iteration.Mark(this.SalesInvoiceWhereSalesInvoiceItem);
                }

                if (this.ExistPaymentApplicationsWhereInvoiceItem)
                {
                    foreach (PaymentApplication paymentApplication in this.PaymentApplicationsWhereInvoiceItem)
                    {
                        iteration.AddDependency(this, paymentApplication);
                        iteration.Mark(paymentApplication);
                    }
                }

                foreach (OrderItemBilling orderItemBilling in this.OrderItemBillingsWhereInvoiceItem)
                {
                    iteration.AddDependency(orderItemBilling.OrderItem, this);
                    iteration.Mark(orderItemBilling.OrderItem);
                }

                foreach (ShipmentItemBilling shipmentItemBilling in this.ShipmentItemBillingsWhereInvoiceItem)
                {
                    foreach (OrderShipment orderShipment in shipmentItemBilling.ShipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        iteration.AddDependency(orderShipment.OrderItem, this);
                        iteration.Mark(orderShipment.OrderItem);
                    }
                }
            }
        }

        public void BaseOnInit(ObjectOnInit method)
        {
            if (this.ExistProduct && !this.ExistInvoiceItemType)
            {
                this.InvoiceItemType = new InvoiceItemTypes(this.Strategy.Session).ProductItem;
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var salesInvoice = this.SalesInvoiceWhereSalesInvoiceItem;
            var salesInvoiceItemStates = new SalesInvoiceItemStates(derivation.Session);

            derivation.Validation.AssertExistsAtMostOne(this, M.SalesInvoiceItem.Product, M.SalesInvoiceItem.ProductFeatures, M.SalesInvoiceItem.Part);
            derivation.Validation.AssertExistsAtMostOne(this, M.SalesInvoiceItem.SerialisedItem, M.SalesInvoiceItem.ProductFeatures, M.SalesInvoiceItem.Part);

            if (!this.ExistDerivationTrigger)
            {
                this.DerivationTrigger = Guid.NewGuid();
            }

            if (this.ExistSerialisedItem && !this.ExistNextSerialisedItemAvailability && salesInvoice.SalesInvoiceType.Equals(new SalesInvoiceTypes(this.Session()).SalesInvoice))
            {
                derivation.Validation.AssertExists(this, this.Meta.NextSerialisedItemAvailability);
            }

            if (this.Part != null && this.Part.InventoryItemKind.IsSerialised && this.Quantity != 1)
            {
                derivation.Validation.AddError(this, M.SalesInvoiceItem.Quantity, ErrorMessages.InvalidQuantity);
            }

            if (this.Part != null && this.Part.InventoryItemKind.IsNonSerialised && this.Quantity == 0)
            {
                derivation.Validation.AddError(this, M.SalesInvoiceItem.Quantity, ErrorMessages.InvalidQuantity);
            }

            if (this.ExistInvoiceItemType && this.InvoiceItemType.MaxQuantity.HasValue && this.Quantity > this.InvoiceItemType.MaxQuantity.Value)
            {
                derivation.Validation.AddError(this, M.SalesInvoiceItem.Quantity, ErrorMessages.InvalidQuantity);
            }

            this.DerivedVatRegime = this.ExistAssignedVatRegime ? this.AssignedVatRegime : this.SalesInvoiceWhereSalesInvoiceItem?.DerivedVatRegime;
            this.VatRate = this.DerivedVatRegime?.VatRates.First(v => v.FromDate <= salesInvoice.InvoiceDate && (!v.ExistThroughDate || v.ThroughDate >= salesInvoice.InvoiceDate));

            this.DerivedIrpfRegime = this.ExistAssignedIrpfRegime ? this.AssignedIrpfRegime : this.SalesInvoiceWhereSalesInvoiceItem?.DerivedIrpfRegime;
            this.IrpfRate = this.DerivedIrpfRegime?.IrpfRates.First(v => v.FromDate <= salesInvoice.InvoiceDate && (!v.ExistThroughDate || v.ThroughDate >= salesInvoice.InvoiceDate));

            if (this.ExistInvoiceItemType && this.IsSubTotalItem().Result == true && this.Quantity <= 0)
            {
                derivation.Validation.AssertExists(this, this.Meta.Quantity);
            }

            this.AmountPaid = 0;
            foreach (PaymentApplication paymentApplication in this.PaymentApplicationsWhereInvoiceItem)
            {
                this.AmountPaid += paymentApplication.AmountApplied;
            }

            if (salesInvoice != null && salesInvoice.SalesInvoiceState.IsReadyForPosting && this.SalesInvoiceItemState.IsCancelledByInvoice)
            {
                this.SalesInvoiceItemState = salesInvoiceItemStates.ReadyForPosting;
            }

            // SalesInvoiceItem States
            if (salesInvoice != null && this.IsValid)
            {
                if (salesInvoice.SalesInvoiceState.IsWrittenOff)
                {
                    this.SalesInvoiceItemState = salesInvoiceItemStates.WrittenOff;
                }

                if (salesInvoice.SalesInvoiceState.IsCancelled)
                {
                    this.SalesInvoiceItemState = salesInvoiceItemStates.CancelledByInvoice;
                }
            }
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var deletePermission = new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.Delete, Operations.Execute);
            if (this.IsDeletable)
            {
                this.RemoveDeniedPermission(deletePermission);
            }
            else
            {
                this.AddDeniedPermission(deletePermission);
            }
        }

        public void BaseWriteOff() => this.SalesInvoiceItemState = new SalesInvoiceItemStates(this.Strategy.Session).WrittenOff;

        public void CancelFromInvoice() => this.SalesInvoiceItemState = new SalesInvoiceItemStates(this.Strategy.Session).CancelledByInvoice;

        public void BaseDelete(DeletableDelete method)
        {
            foreach (SalesTerm salesTerm in this.SalesTerms)
            {
                salesTerm.Delete();
            }

            foreach (InvoiceVatRateItem invoiceVatRateItem in this.InvoiceVatRateItems)
            {
                invoiceVatRateItem.Delete();
            }

            foreach (WorkEffortBilling billing in this.WorkEffortBillingsWhereInvoiceItem)
            {
                billing.WorkEffort.DerivationTrigger = Guid.NewGuid();
                billing.Delete();
            }

            foreach (OrderItemBilling billing in this.OrderItemBillingsWhereInvoiceItem)
            {
                billing.OrderItem.DerivationTrigger = Guid.NewGuid();
                billing.Delete();
            }

            foreach (ShipmentItemBilling billing in this.ShipmentItemBillingsWhereInvoiceItem)
            {
                billing.ShipmentItem.DerivationTrigger = Guid.NewGuid();
                billing.Delete();
            }

            foreach (TimeEntryBilling billing in this.TimeEntryBillingsWhereInvoiceItem)
            {
                billing.TimeEntry.WorkEffort.DerivationTrigger = Guid.NewGuid();
                billing.Delete();
            }

            foreach (ServiceEntryBilling billing in this.ServiceEntryBillingsWhereInvoiceItem)
            {
                billing.ServiceEntry.DerivationTrigger = Guid.NewGuid();
                billing.Delete();
            }
        }

        public void BaseIsSubTotalItem(SalesInvoiceItemIsSubTotalItem method)
        {
            if (!method.Result.HasValue)
            {
                method.Result = this.InvoiceItemType.Equals(new InvoiceItemTypes(this.Strategy.Session).ProductItem)
                    || this.InvoiceItemType.Equals(new InvoiceItemTypes(this.Strategy.Session).PartItem);
            }
        }

        public void Sync(Invoice invoice) => this.SyncedInvoice = invoice;
    }
}
