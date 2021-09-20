// <copyright file="SerialisedItem.cs" company="Allors bvba">
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

    public partial class SerialisedItem
    {
        private bool IsDeletable =>
            !this.ExistInventoryItemTransactionsWhereSerialisedItem
            && !this.ExistPurchaseInvoiceItemsWhereSerialisedItem
            && !this.ExistPurchaseOrderItemsWhereSerialisedItem
            && !this.ExistQuoteItemsWhereSerialisedItem
            && !this.ExistSalesInvoiceItemsWhereSerialisedItem
            && !this.ExistSalesOrderItemsWhereSerialisedItem
            && !this.ExistSerialisedInventoryItemsWhereSerialisedItem
            && !this.ExistShipmentItemsWhereSerialisedItem;

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSerialisedItemState)
            {
                this.SerialisedItemState = new SerialisedItemStates(this.Strategy.Session).NA;
            }

            if (!this.ExistItemNumber)
            {
                this.ItemNumber = this.Strategy.Session.GetSingleton().Settings.NextSerialisedItemNumber();
            }

            if (!this.ExistDerivationTrigger)
            {
                this.DerivationTrigger = Guid.NewGuid();
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                iteration.AddDependency(this.PartWhereSerialisedItem, this);
                iteration.Mark(this.PartWhereSerialisedItem);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExistsAtMostOne(this, this.Meta.AcquiredDate, this.Meta.AcquisitionYear);

            if (!this.ExistName && this.ExistPartWhereSerialisedItem)
            {
                this.Name = this.PartWhereSerialisedItem.Name;
            }

            this.DerivePurchaseOrder();
            this.DerivePurchaseInvoice();
            this.DerivePurchasePrice();

            this.SuppliedBy = this.AssignedSuppliedBy ??
                this.PurchaseOrder?.TakenViaSupplier ??
                this.PartWhereSerialisedItem?.SupplierOfferingsWherePart?.FirstOrDefault()?.Supplier;

            this.SuppliedByPartyName = this.ExistSuppliedBy ? this.SuppliedBy.PartyName : string.Empty;
            this.OwnedByPartyName = this.ExistOwnedBy ? this.OwnedBy.PartyName : string.Empty;
            this.RentedByPartyName = this.ExistRentedBy ? this.RentedBy.PartyName : string.Empty;
            this.OwnershipByOwnershipName = this.ExistOwnership ? this.Ownership.Name : string.Empty;
            this.SerialisedItemAvailabilityName = this.ExistSerialisedItemAvailability ? this.SerialisedItemAvailability.Name : string.Empty;

            var doubles = this.PartWhereSerialisedItem?.SerialisedItems.Where(v =>
                !string.IsNullOrEmpty(v.SerialNumber)
                && v.SerialNumber != "."
                && v.SerialNumber.Equals(this.SerialNumber)).ToArray();
            if (doubles?.Length > 1)
            {
                derivation.Validation.AddError(this, this.Meta.SerialNumber, ErrorMessages.SameSerialNumber);
            }

            this.OnQuote = this.QuoteItemsWhereSerialisedItem.Any(v => v.QuoteItemState.IsDraft
                        || v.QuoteItemState.IsSubmitted || v.QuoteItemState.IsApproved
                        || v.QuoteItemState.IsAwaitingAcceptance || v.QuoteItemState.IsAccepted);

            this.OnSalesOrder = this.SalesOrderItemsWhereSerialisedItem.Any(v => v.SalesOrderItemState.IsProvisional
                        || v.SalesOrderItemState.IsReadyForPosting || v.SalesOrderItemState.IsRequestsApproval
                        || v.SalesOrderItemState.IsAwaitingAcceptance || v.SalesOrderItemState.IsOnHold || v.SalesOrderItemState.IsInProcess);

            this.OnWorkEffort = this.WorkEffortFixedAssetAssignmentsWhereFixedAsset.Any(v => v.Assignment.WorkEffortState.IsCreated
                        || v.Assignment.WorkEffortState.IsInProgress);

            this.DeriveProductCharacteristics(derivation);
            this.DeriveDisplayProductCategories();
        }

        public void BaseDeriveDisplayProductCategories(SerialisedItemDeriveDisplayProductCategories method)
        {
            if (!method.Result.HasValue)
            {
                if (this.ExistPartWhereSerialisedItem && this.PartWhereSerialisedItem.GetType().Name == typeof(UnifiedGood).Name)
                {
                    var unifiedGood = this.PartWhereSerialisedItem as UnifiedGood;
                    this.DisplayProductCategories = string.Join(", ", unifiedGood.ProductCategoriesWhereProduct.Select(v => v.DisplayName));
                }

                method.Result = true;
            }
        }

        public void BaseDerivePurchaseOrder(SerialisedItemDerivePurchaseOrder method)
        {
            if (!method.Result.HasValue)
            {
                this.PurchaseOrder = this.PurchaseOrderItemsWhereSerialisedItem
                    .LastOrDefault(v => v.ExistOrderWhereValidOrderItem
                                        && (v.InvoiceItemType.Equals(new InvoiceItemTypes(this.Session()).PartItem)
                                            || v.InvoiceItemType.Equals(new InvoiceItemTypes(this.Session()).ProductItem))
                                        && (((PurchaseOrder)v.OrderWhereValidOrderItem).PurchaseOrderState.Equals(new PurchaseOrderStates(this.Session()).Sent)
                                            || ((PurchaseOrder)v.OrderWhereValidOrderItem).PurchaseOrderState.Equals(new PurchaseOrderStates(this.Session()).Completed)
                                            || ((PurchaseOrder)v.OrderWhereValidOrderItem).PurchaseOrderState.Equals(new PurchaseOrderStates(this.Session()).Finished)))?
                    .PurchaseOrderWherePurchaseOrderItem;

                method.Result = true;
            }
        }

        public void BaseDerivePurchaseInvoice(SerialisedItemDerivePurchaseInvoice method)
        {
            if (!method.Result.HasValue)
            {
                this.PurchaseInvoice = this.PurchaseInvoiceItemsWhereSerialisedItem
                    .LastOrDefault(v => v.ExistInvoiceWhereValidInvoiceItem
                                            && (v.InvoiceItemType.Equals(new InvoiceItemTypes(this.Session()).PartItem)
                                            || v.InvoiceItemType.Equals(new InvoiceItemTypes(this.Session()).ProductItem))
                                        && (((PurchaseInvoice)v.InvoiceWhereValidInvoiceItem).PurchaseInvoiceState.Equals(new PurchaseInvoiceStates(this.Session()).NotPaid)
                                            || ((PurchaseInvoice)v.InvoiceWhereValidInvoiceItem).PurchaseInvoiceState.Equals(new PurchaseInvoiceStates(this.Session()).PartiallyPaid)
                                            || ((PurchaseInvoice)v.InvoiceWhereValidInvoiceItem).PurchaseInvoiceState.Equals(new PurchaseInvoiceStates(this.Session()).Paid)))?
                    .PurchaseInvoiceWherePurchaseInvoiceItem;

                method.Result = true;
            }
        }

        public void BaseDerivePurchasePrice(SerialisedItemDerivePurchasePrice method)
        {
            if (!method.Result.HasValue && this.ExistPurchaseInvoice)
            {
                this.PurchasePrice = this.PurchaseInvoiceItemsWhereSerialisedItem
                    .LastOrDefault(v => v.ExistInvoiceWhereValidInvoiceItem
                                        && (v.InvoiceItemType.Equals(new InvoiceItemTypes(this.Session()).PartItem)
                                            || v.InvoiceItemType.Equals(new InvoiceItemTypes(this.Session()).ProductItem)))?
                    .UnitPrice ?? 0M;

                this.RemoveAssignedPurchasePrice();

                method.Result = true;
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

            var builder = new StringBuilder();

            builder.Append(this.ItemNumber);
            builder.Append(string.Join(" ", this.SerialNumber));
            builder.Append(string.Join(" ", this.Name));

            if (this.ExistOwnedBy)
            {
                builder.Append(string.Join(" ", this.OwnedBy.PartyName));
            }

            if (this.ExistBuyer)
            {
                builder.Append(string.Join(" ", this.Buyer.PartyName));
            }

            if (this.ExistSeller)
            {
                builder.Append(string.Join(" ", this.Seller.PartyName));
            }

            if (this.ExistPartWhereSerialisedItem)
            {
                builder.Append(string.Join(" ", this.PartWhereSerialisedItem?.Brand?.Name));
                builder.Append(string.Join(" ", this.PartWhereSerialisedItem?.Model?.Name));
            }

            builder.Append(string.Join(" ", this.Keywords));

            this.SearchString = builder.ToString();
        }

        public void BaseDelete(DeletableDelete method)
        {
            if (this.IsDeletable)
            {
                foreach (LocalisedText deletable in this.LocalisedComments)
                {
                    deletable.Delete();
                }

                foreach (LocalisedText deletable in this.LocalisedNames)
                {
                    deletable.Delete();
                }

                foreach (LocalisedText deletable in this.LocalisedDescriptions)
                {
                    deletable.Delete();
                }

                foreach (LocalisedText deletable in this.LocalisedKeywords)
                {
                    deletable.Delete();
                }

                foreach (Media deletable in this.PublicElectronicDocuments)
                {
                    deletable.Delete();
                }

                foreach (Media deletable in this.PublicLocalisedElectronicDocuments)
                {
                    deletable.Delete();
                }

                foreach (Media deletable in this.PrivateElectronicDocuments)
                {
                    deletable.Delete();
                }

                foreach (Media deletable in this.PrivateLocalisedElectronicDocuments)
                {
                    deletable.Delete();
                }

                foreach (SerialisedItemCharacteristic deletable in this.SerialisedItemCharacteristics)
                {
                    deletable.Delete();
                }

                foreach (SerialisedItemVersion version in this.AllVersions)
                {
                    version.Delete();
                }
            }
        }

        private void DeriveProductCharacteristics(IDerivation derivation)
        {
            var characteristicsToDelete = this.SerialisedItemCharacteristics.ToList();
            var part = this.PartWhereSerialisedItem;

            if (this.ExistPartWhereSerialisedItem && part.ExistProductType)
            {
                foreach (SerialisedItemCharacteristicType characteristicType in part.ProductType.SerialisedItemCharacteristicTypes)
                {
                    var characteristic = this.SerialisedItemCharacteristics.FirstOrDefault(v => Equals(v.SerialisedItemCharacteristicType, characteristicType));
                    if (characteristic == null)
                    {
                        var newCharacteristic = new SerialisedItemCharacteristicBuilder(this.Strategy.Session)
                            .WithSerialisedItemCharacteristicType(characteristicType).Build();

                        this.AddSerialisedItemCharacteristic(newCharacteristic);

                        var partCharacteristics = part.SerialisedItemCharacteristics;
                        partCharacteristics.Filter.AddEquals(M.SerialisedItemCharacteristic.SerialisedItemCharacteristicType, characteristicType);
                        var fromPart = partCharacteristics.FirstOrDefault();

                        if (fromPart != null)
                        {
                            newCharacteristic.Value = fromPart.Value;
                        }
                    }
                    else
                    {
                        characteristicsToDelete.Remove(characteristic);
                    }
                }
            }

            foreach (var characteristic in characteristicsToDelete)
            {
                this.RemoveSerialisedItemCharacteristic(characteristic);
            }
        }
    }
}
