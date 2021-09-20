// <copyright file="QuoteItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Resources;

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using System.Text;
    using Allors.Meta;

    public partial class QuoteItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.QuoteItem, M.QuoteItem.QuoteItemState),
            };

        public decimal LineTotal => this.Quantity * this.UnitPrice;

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public bool IsValid => !(this.QuoteItemState.IsCancelled || this.QuoteItemState.IsRejected);

        public bool WasValid => this.ExistLastObjectStates && !(this.LastQuoteItemState.IsCancelled || this.LastQuoteItemState.IsRejected);

        internal bool IsDeletable =>
            (this.QuoteItemState.Equals(new QuoteItemStates(this.Strategy.Session).Draft)
                || this.QuoteItemState.Equals(new QuoteItemStates(this.Strategy.Session).Submitted)
                || this.QuoteItemState.Equals(new QuoteItemStates(this.Strategy.Session).Cancelled))
            && !this.ExistEngagementItemsWhereQuoteItem
            && !this.ExistOrderItemsWhereQuoteItem;

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistQuoteItemState)
            {
                this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Draft;
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
                if (this.ExistQuoteWhereQuoteItem)
                {
                    iteration.AddDependency(this.QuoteWhereQuoteItem, this);
                    iteration.Mark(this.QuoteWhereQuoteItem);
                }

                if (this.ExistSerialisedItem)
                {
                    iteration.AddDependency(this.SerialisedItem, this);
                    iteration.Mark(this.SerialisedItem);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var quote = this.QuoteWhereQuoteItem;

            if (this.InvoiceItemType.IsPartItem
                || this.InvoiceItemType.IsProductFeatureItem
                || this.InvoiceItemType.IsProductItem)
            {
                derivation.Validation.AssertAtLeastOne(this, M.QuoteItem.Product, M.QuoteItem.ProductFeature, M.QuoteItem.SerialisedItem, M.QuoteItem.Deliverable, M.QuoteItem.WorkEffort);
                derivation.Validation.AssertExistsAtMostOne(this, M.QuoteItem.Product, M.QuoteItem.ProductFeature, M.QuoteItem.Deliverable, M.QuoteItem.WorkEffort);
                derivation.Validation.AssertExistsAtMostOne(this, M.QuoteItem.SerialisedItem, M.QuoteItem.ProductFeature, M.QuoteItem.Deliverable, M.QuoteItem.WorkEffort);
            }
            else
            {
                this.Quantity = 1;
            }

            if (this.ExistSerialisedItem && this.Quantity != 1)
            {
                derivation.Validation.AddError(this, this.Meta.Quantity, ErrorMessages.SerializedItemQuantity);
            }

            this.DerivedVatRegime = this.ExistAssignedVatRegime ? this.AssignedVatRegime : quote?.DerivedVatRegime;
            this.VatRate = this.DerivedVatRegime?.VatRates.First(v => v.FromDate <= quote.IssueDate && (!v.ExistThroughDate || v.ThroughDate >= quote.IssueDate));

            this.DerivedIrpfRegime = this.ExistAssignedIrpfRegime ? this.AssignedIrpfRegime : quote?.DerivedIrpfRegime;
            this.IrpfRate = this.DerivedIrpfRegime?.IrpfRates.First(v => v.FromDate <= quote.IssueDate && (!v.ExistThroughDate || v.ThroughDate >= quote.IssueDate));


            if (derivation.ChangeSet.IsCreated(this) && !this.ExistDetails)
            {
                this.DeriveDetails();
            }

            if (this.ExistRequestItem)
            {
                this.RequiredByDate = this.RequestItem.RequiredByDate;
            }

            if (!this.ExistUnitOfMeasure)
            {
                this.UnitOfMeasure = new UnitsOfMeasure(this.Strategy.Session).Piece;
            }

            this.UnitVat = this.ExistVatRate ? this.UnitPrice * this.VatRate.Rate / 100 : 0;

            // Calculate Totals
            this.TotalBasePrice = this.UnitBasePrice * this.Quantity;
            this.TotalDiscount = this.UnitDiscount * this.Quantity;
            this.TotalSurcharge = this.UnitSurcharge * this.Quantity;

            if (this.TotalBasePrice > 0)
            {
                this.TotalDiscountAsPercentage = Rounder.RoundDecimal(this.TotalDiscount / this.TotalBasePrice * 100, 2);
                this.TotalSurchargeAsPercentage = Rounder.RoundDecimal(this.TotalSurcharge / this.TotalBasePrice * 100, 2);
            }
            else
            {
                this.TotalDiscountAsPercentage = 0;
                this.TotalSurchargeAsPercentage = 0;
            }

            this.TotalExVat = this.UnitPrice * this.Quantity;
            this.TotalVat = this.UnitVat * this.Quantity;
            this.TotalIncVat = this.TotalExVat + this.TotalVat;

            // CurrentVersion is Previous Version until PostDerive
            var previousSerialisedItem = this.CurrentVersion?.SerialisedItem;
            if (previousSerialisedItem != null && previousSerialisedItem != this.SerialisedItem)
            {
                previousSerialisedItem.DerivationTrigger = Guid.NewGuid();
            }
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistUnitPrice)
            {
                derivation.Validation.AddError(this, this.Meta.UnitPrice, ErrorMessages.UnitPriceRequired);
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
        }

        public void BaseDeriveDetails(QuoteItemDeriveDetails method)
        {
            if (!method.Result.HasValue)
            {
                if (this.ExistSerialisedItem)
                {
                    var builder = new StringBuilder();
                    var part = this.SerialisedItem.PartWhereSerialisedItem;

                    if (part != null && part.ExistManufacturedBy)
                    {
                        builder.Append($", Manufacturer: {part.ManufacturedBy.PartyName}");
                    }

                    if (part != null && part.ExistBrand)
                    {
                        builder.Append($", Brand: {part.Brand.Name}");
                    }

                    if (part != null && part.ExistModel)
                    {
                        builder.Append($", Model: {part.Model.Name}");
                    }

                    builder.Append($", SN: {this.SerialisedItem.SerialNumber}");

                    if (this.SerialisedItem.ExistManufacturingYear)
                    {
                        builder.Append($", YOM: {this.SerialisedItem.ManufacturingYear}");
                    }

                    foreach (SerialisedItemCharacteristic characteristic in this.SerialisedItem.SerialisedItemCharacteristics)
                    {
                        if (characteristic.ExistValue)
                        {
                            var characteristicType = characteristic.SerialisedItemCharacteristicType;
                            if (characteristicType.ExistUnitOfMeasure)
                            {
                                var uom = characteristicType.UnitOfMeasure.ExistAbbreviation
                                                ? characteristicType.UnitOfMeasure.Abbreviation
                                                : characteristicType.UnitOfMeasure.Name;
                                builder.Append(
                                    $", {characteristicType.Name}: {characteristic.Value} {uom}");
                            }
                            else
                            {
                                builder.Append($", {characteristicType.Name}: {characteristic.Value}");
                            }
                        }
                    }

                    var details = builder.ToString();

                    if (details.StartsWith(","))
                    {
                        details = details.Substring(2);
                    }

                    this.Details = details;

                }
                else if (this.ExistProduct && this.Product is UnifiedGood unifiedGood)
                {
                    var builder = new StringBuilder();

                    if (unifiedGood != null && unifiedGood.ExistManufacturedBy)
                    {
                        builder.Append($", Manufacturer: {unifiedGood.ManufacturedBy.PartyName}");
                    }

                    if (unifiedGood != null && unifiedGood.ExistBrand)
                    {
                        builder.Append($", Brand: {unifiedGood.Brand.Name}");
                    }

                    if (unifiedGood != null && unifiedGood.ExistModel)
                    {
                        builder.Append($", Model: {unifiedGood.Model.Name}");
                    }

                    foreach (SerialisedItemCharacteristic characteristic in unifiedGood.SerialisedItemCharacteristics)
                    {
                        if (characteristic.ExistValue)
                        {
                            var characteristicType = characteristic.SerialisedItemCharacteristicType;
                            if (characteristicType.ExistUnitOfMeasure)
                            {
                                var uom = characteristicType.UnitOfMeasure.ExistAbbreviation
                                                ? characteristicType.UnitOfMeasure.Abbreviation
                                                : characteristicType.UnitOfMeasure.Name;
                                builder.Append($", {characteristicType.Name}: {characteristic.Value} {uom}");
                            }
                            else
                            {
                                builder.Append($", {characteristicType.Name}: {characteristic.Value}");
                            }
                        }
                    }

                    var details = builder.ToString();

                    if (details.StartsWith(","))
                    {
                        details = details.Substring(2);
                    }

                    this.Details = details;
                }

                method.Result = true;
            }
        }

        public void BaseDelete(QuoteItemDelete method)
        {
            if (this.ExistSerialisedItem)
            {
                this.SerialisedItem.DerivationTrigger = Guid.NewGuid();
            }
        }

        public void BaseSend(QuoteItemSend method) => this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).AwaitingAcceptance;

        public void BaseCancel(QuoteItemCancel method) => this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Cancelled;

        public void BaseReject(QuoteItemReject method) => this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Rejected;

        public void BaseOrder(QuoteItemOrder method) => this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Ordered;

        public void BaseSubmit(QuoteItemSubmit method) => this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Submitted;

        public void Sync(Quote quote) => this.SyncedQuote = quote;
    }
}
