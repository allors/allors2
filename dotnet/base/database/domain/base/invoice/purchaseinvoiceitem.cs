// <copyright file="PurchaseInvoiceItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Meta;

    public partial class PurchaseInvoiceItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.PurchaseInvoiceItem, M.PurchaseInvoiceItem.PurchaseInvoiceItemState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public bool IsValid => !(this.PurchaseInvoiceItemState.IsCancelledByInvoice || this.PurchaseInvoiceItemState.IsRejected);

        public decimal PriceAdjustment => this.TotalSurcharge - this.TotalDiscount;

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
            if (!this.ExistPurchaseInvoiceItemState)
            {
                this.PurchaseInvoiceItemState = new PurchaseInvoiceItemStates(this.Strategy.Session).Created;
            }

            if (this.ExistPart && !this.ExistInvoiceItemType)
            {
                this.InvoiceItemType = new InvoiceItemTypes(this.Strategy.Session).PartItem;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;
            var invoice = this.PurchaseInvoiceWherePurchaseInvoiceItem;

            if (invoice != null && iteration.ChangeSet.Associations.Contains(this.Id))
            {
                iteration.AddDependency(invoice, this);
                iteration.Mark(invoice);
            }

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                iteration.AddDependency(this.SerialisedItem, this);
                iteration.Mark(this.SerialisedItem);
            }

            foreach (OrderItemBilling orderItemBilling in this.OrderItemBillingsWhereInvoiceItem)
            {
                iteration.AddDependency(orderItemBilling.OrderItem, this);
                iteration.Mark(orderItemBilling.OrderItem);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistDerivationTrigger)
            {
                this.DerivationTrigger = Guid.NewGuid();
            }
        }

        public void BaseOnDerivePrices()
        {
            this.UnitBasePrice = 0;
            this.UnitDiscount = 0;
            this.UnitSurcharge = 0;

            var invoice = this.PurchaseInvoiceWherePurchaseInvoiceItem;

            if (this.AssignedUnitPrice.HasValue)
            {
                this.UnitBasePrice = this.AssignedUnitPrice.Value;
                this.UnitPrice = this.AssignedUnitPrice.Value;
            }
            else
            {
                if (this.ExistPart)
                {
                    this.UnitBasePrice = new SupplierOfferings(this.Strategy.Session).PurchasePrice(invoice.BilledFrom, invoice.InvoiceDate, this.Part);
                }
            }

            if (this.ExistUnitBasePrice)
            {
                this.DerivedVatRegime = this.AssignedVatRegime ?? this.PurchaseInvoiceWherePurchaseInvoiceItem.DerivedVatRegime;
                this.VatRate = this.DerivedVatRegime?.VatRates.First(v => v.FromDate <= invoice.InvoiceDate && (!v.ExistThroughDate || v.ThroughDate >= invoice.InvoiceDate));

                this.DerivedIrpfRegime = this.AssignedIrpfRegime ?? this.PurchaseInvoiceWherePurchaseInvoiceItem.DerivedIrpfRegime;
                this.IrpfRate = this.DerivedIrpfRegime?.IrpfRates.First(v => v.FromDate <= invoice.InvoiceDate && (!v.ExistThroughDate || v.ThroughDate >= invoice.InvoiceDate));

                this.TotalBasePrice = this.UnitBasePrice * this.Quantity;
                this.TotalDiscount = this.UnitDiscount * this.Quantity;
                this.TotalSurcharge = this.UnitSurcharge * this.Quantity;
                this.UnitPrice = this.UnitBasePrice - this.UnitDiscount + this.UnitSurcharge;

                this.UnitVat = this.ExistVatRate ? this.UnitPrice * this.VatRate.Rate / 100 : 0;
                this.UnitIrpf = this.ExistIrpfRate ? this.UnitPrice * this.IrpfRate.Rate / 100 : 0;
                this.TotalExVat = this.UnitPrice * this.Quantity;
                this.TotalVat = this.UnitVat * this.Quantity;
                this.TotalIncVat = this.TotalExVat + this.TotalVat;
                this.TotalIrpf = this.UnitIrpf * this.Quantity;
                this.GrandTotal = this.TotalIncVat - this.TotalIrpf;
            }
        }

        public void CancelFromInvoice() => this.PurchaseInvoiceItemState = new PurchaseInvoiceItemStates(this.Strategy.Session).CancelledByinvoice;

        public void BaseDelete(DeletableDelete method)
        {
            if (this.PurchaseInvoiceWherePurchaseInvoiceItem.PurchaseInvoiceState.IsCreated)
            {
                this.PurchaseInvoiceWherePurchaseInvoiceItem.RemovePurchaseInvoiceItem(this);
                foreach (OrderItemBilling orderItemBilling in this.OrderItemBillingsWhereInvoiceItem)
                {
                    orderItemBilling.OrderItem.DerivationTrigger = Guid.NewGuid();
                    orderItemBilling.Delete();
                }
            }
        }

        public void BaseReject(PurchaseInvoiceItemReject method) => this.PurchaseInvoiceItemState = new PurchaseInvoiceItemStates(this.Strategy.Session).Rejected;

        public void BaseRevise(PurchaseInvoiceItemRevise method) => this.PurchaseInvoiceItemState = new PurchaseInvoiceItemStates(this.Strategy.Session).Revising;

        public void BaseFinishRevising(PurchaseInvoiceItemFinishRevising method) => this.PurchaseInvoiceItemState = new PurchaseInvoiceItemStates(this.Strategy.Session).Created;

        public void Sync(Invoice invoice) => this.SyncedInvoice = invoice;
    }
}
