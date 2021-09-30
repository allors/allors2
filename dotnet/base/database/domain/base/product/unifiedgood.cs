// <copyright file="UnifiedGood.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Text;

namespace Allors.Domain
{
    using System;
    using System.Linq;

    using Allors.Meta;

    public partial class UnifiedGood
    {
        private bool IsDeletable => !this.ExistDeploymentsWhereProductOffering &&
                                    !this.ExistEngagementItemsWhereProduct &&
                                    !this.ExistGeneralLedgerAccountsWhereCostUnitsAllowed &&
                                    !this.ExistGeneralLedgerAccountsWhereDefaultCostUnit &&
                                    !this.ExistQuoteItemsWhereProduct &&
                                    !this.ExistShipmentItemsWhereGood &&
                                    !this.ExistWorkEffortPartStandardsWherePart &&
                                    !this.ExistMarketingPackageWhereProductsUsedIn &&
                                    !this.ExistMarketingPackagesWhereProduct &&
                                    !this.ExistOrganisationGlAccountsWhereProduct &&
                                    !this.ExistProductConfigurationsWhereProductsUsedIn &&
                                    !this.ExistProductConfigurationsWhereProduct &&
                                    !this.ExistRequestItemsWhereProduct &&
                                    !this.ExistSalesInvoiceItemsWhereProduct &&
                                    !this.ExistSalesOrderItemsWhereProduct &&
                                    !this.ExistWorkEffortTypesWhereProductToProduce &&
                                    !this.ExistWorkEffortInventoryProducedsWherePart &&
                                    !this.ExistWorkEffortPartStandardsWherePart &&
                                    !this.ExistPartBillOfMaterialsWherePart &&
                                    !this.ExistPartBillOfMaterialsWhereComponentPart &&
                                    !this.ExistInventoryItemTransactionsWherePart &&
                                    !this.ExistSerialisedItems;
                                    

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistInventoryItemKind)
            {
                this.InventoryItemKind = new InventoryItemKinds(this.Strategy.Session).NonSerialised;
            }

            if (!this.ExistUnitOfMeasure)
            {
                this.UnitOfMeasure = new UnitsOfMeasure(this.Strategy.Session).Piece;
            }

            if (!this.ExistDefaultFacility)
            {
                this.DefaultFacility = this.Strategy.Session.GetSingleton().Settings.DefaultFacility;
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var defaultLocale = this.Strategy.Session.GetSingleton().DefaultLocale;
            var settings = this.Strategy.Session.GetSingleton().Settings;

            if (!this.ExistDerivationTrigger)
            {
                this.DerivationTrigger = Guid.NewGuid();
            }

            if (derivation.ChangeSet.HasChangedRoles(this, new RoleType[] { this.Meta.UnitOfMeasure, this.Meta.DefaultFacility }))
            {
                this.SyncDefaultInventoryItem();
            }

            var identifications = this.ProductIdentifications;
            identifications.Filter.AddEquals(M.ProductIdentification.ProductIdentificationType, new ProductIdentificationTypes(this.Strategy.Session).Good);
            var goodIdentification = identifications.FirstOrDefault();

            if (goodIdentification == null && settings.UseProductNumberCounter)
            {
                goodIdentification = new ProductNumberBuilder(this.Strategy.Session)
                    .WithIdentification(settings.NextProductNumber())
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Strategy.Session).Good).Build();

                this.AddProductIdentification(goodIdentification);
            }

            this.ProductNumber = goodIdentification.Identification;

            if (!this.ExistProductIdentifications)
            {
                derivation.Validation.AssertExists(this, M.Good.ProductIdentifications);
            }

            if (this.LocalisedNames.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Name = this.LocalisedNames.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (this.LocalisedDescriptions.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Description = this.LocalisedDescriptions.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            foreach (SupplierOffering supplierOffering in this.SupplierOfferingsWherePart)
            {
                if (supplierOffering.FromDate <= this.Session().Now()
                    && (!supplierOffering.ExistThroughDate || supplierOffering.ThroughDate >= this.Session().Now()))
                {
                    this.AddSuppliedBy(supplierOffering.Supplier);
                }

                if (supplierOffering.FromDate > this.Session().Now()
                    || (supplierOffering.ExistThroughDate && supplierOffering.ThroughDate < this.Session().Now()))
                {
                    this.RemoveSuppliedBy(supplierOffering.Supplier);
                }
            }

            this.DeriveVirtualProductPriceComponent();
            this.DeriveProductCharacteristics(derivation);
            this.DeriveQuantityOnHand();
            this.DeriveAvailableToPromise();
            this.DeriveQuantityCommittedOut();
            this.DeriveQuantityExpectedIn();

            var quantityOnHand = 0M;
            var totalCost = 0M;

            foreach (InventoryItemTransaction inventoryTransaction in this.InventoryItemTransactionsWherePart)
            {
                var reason = inventoryTransaction.Reason;

                if (reason.IncreasesQuantityOnHand == true)
                {
                    quantityOnHand += inventoryTransaction.Quantity;

                    var transactionCost = inventoryTransaction.Quantity * inventoryTransaction.Cost;
                    totalCost += transactionCost;

                    var averageCost = quantityOnHand > 0 ? totalCost / quantityOnHand : 0M;
                    ((PartWeightedAverageDerivedRoles)this.PartWeightedAverage).AverageCost = decimal.Round(averageCost, 2);
                }
                else if (reason.IncreasesQuantityOnHand == false)
                {
                    quantityOnHand -= inventoryTransaction.Quantity;

                    totalCost = quantityOnHand * this.PartWeightedAverage.AverageCost;
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

            var builder = new StringBuilder();
            if (this.ExistProductIdentifications)
            {
                builder.Append(string.Join(" ", this.ProductIdentifications.Select(v => v.Identification)));
            }

            if (this.ExistProductCategoriesWhereAllProduct)
            {
                builder.Append(string.Join(" ", this.ProductCategoriesWhereAllProduct.Select(v => v.Name)));
            }

            if (this.ExistProductCategoriesWhereAllPart)
            {
                builder.Append(string.Join(" ", this.ProductCategoriesWhereAllPart.Select(v => v.Name)));
            }

            if (this.ExistSupplierOfferingsWherePart)
            {
                builder.Append(string.Join(" ", this.SupplierOfferingsWherePart.Select(v => v.Supplier.PartyName)));
            }

            if (this.ExistSerialisedItems)
            {
                builder.Append(string.Join(" ", this.SerialisedItems.Select(v => v.SerialNumber)));
                builder.Append(string.Join(" ", this.SerialisedItems.Select(v => v.ItemNumber)));
            }

            if (this.ExistProductType)
            {
                builder.Append(string.Join(" ", this.ProductType.Name));
            }

            if (this.ExistBrand)
            {
                builder.Append(string.Join(" ", this.Brand.Name));
            }

            if (this.ExistModel)
            {
                builder.Append(string.Join(" ", this.Model.Name));
            }

            foreach (PartCategory partCategory in this.PartCategoriesWherePart)
            {
                builder.Append(string.Join(" ", partCategory.Name));
            }

            builder.Append(string.Join(" ", this.Keywords));

            this.SearchString = builder.ToString();
        }

        public void DeriveVirtualProductPriceComponent()
        {
            if (!this.ExistProductWhereVariant)
            {
                this.RemoveVirtualProductPriceComponents();
            }

            if (this.ExistVariants)
            {
                this.RemoveVirtualProductPriceComponents();

                var priceComponents = this.PriceComponentsWhereProduct;

                foreach (Good product in this.Variants)
                {
                    foreach (PriceComponent priceComponent in priceComponents)
                    {
                        // HACK: DerivedRoles
                        var productDerivedRoles = (ProductDerivedRoles)product;

                        productDerivedRoles.AddVirtualProductPriceComponent(priceComponent);

                        if (priceComponent is BasePrice basePrice && !priceComponent.ExistProductFeature)
                        {
                            productDerivedRoles.AddBasePrice(basePrice);
                        }
                    }
                }
            }
        }

        public void BaseDelete(DeletableDelete method)
        {
            if (this.IsDeletable)
            {
                foreach (ProductIdentification productIdentification in this.ProductIdentifications)
                {
                    productIdentification.Delete();
                }

                foreach (LocalisedText localisedText in this.LocalisedNames)
                {
                    localisedText.Delete();
                }

                foreach (LocalisedText localisedText in this.LocalisedDescriptions)
                {
                    localisedText.Delete();
                }

                foreach (PriceComponent priceComponent in this.VirtualProductPriceComponents)
                {
                    priceComponent.Delete();
                }

                foreach (EstimatedProductCost estimatedProductCosts in this.EstimatedProductCosts)
                {
                    estimatedProductCosts.Delete();
                }

                foreach (ProductFeatureApplicability productFeatureApplicability in this.ProductFeatureApplicabilitiesWhereAvailableFor)
                {
                    productFeatureApplicability.Delete();
                }

                foreach (InventoryItem inventoryItem in this.InventoryItemsWherePart)
                {
                    inventoryItem.Delete();
                }

                foreach (PartSubstitute partSubstitute in this.PartSubstitutesWherePart)
                {
                    partSubstitute.Delete();
                }

                foreach (PartSubstitute partSubstitute in this.PartSubstitutesWhereSubstitutionPart)
                {
                    partSubstitute.Delete();
                }

                foreach (SupplierOffering supplierOffering in this.SupplierOfferingsWherePart)
                {
                    supplierOffering.Delete();
                }
            }
        }

        private void SyncDefaultInventoryItem()
        {
            if (this.InventoryItemKind.IsNonSerialised)
            {
                var inventoryItems = this.InventoryItemsWherePart;

                if (!inventoryItems.Any(i => i.Facility.Equals(this.DefaultFacility) && i.UnitOfMeasure.Equals(this.UnitOfMeasure)))
                {
                    var inventoryItem = (InventoryItem)new NonSerialisedInventoryItemBuilder(this.Strategy.Session)
                        .WithFacility(this.DefaultFacility)
                        .WithUnitOfMeasure(this.UnitOfMeasure)
                        .WithPart(this)
                        .Build();
                }
            }
        }

        private void DeriveProductCharacteristics(IDerivation derivation)
        {
            var characteristicsToDelete = this.SerialisedItemCharacteristics.ToList();

            if (this.ExistProductType)
            {
                foreach (SerialisedItemCharacteristicType characteristicType in this.ProductType.SerialisedItemCharacteristicTypes)
                {
                    var characteristic = this.SerialisedItemCharacteristics.FirstOrDefault(v => Equals(v.SerialisedItemCharacteristicType, characteristicType));
                    if (characteristic == null)
                    {
                        this.AddSerialisedItemCharacteristic(
                            new SerialisedItemCharacteristicBuilder(this.Strategy.Session)
                                .WithSerialisedItemCharacteristicType(characteristicType)
                                .Build());
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

        private void DeriveQuantityOnHand()
        {
            this.QuantityOnHand = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWherePart)
            {
                if (inventoryItem is NonSerialisedInventoryItem nonSerialisedItem)
                {
                    this.QuantityOnHand += nonSerialisedItem.QuantityOnHand;
                }
                else if (inventoryItem is SerialisedInventoryItem serialisedItem)
                {
                    this.QuantityOnHand += serialisedItem.QuantityOnHand;
                }
            }
        }

        private void DeriveAvailableToPromise()
        {
            this.AvailableToPromise = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWherePart)
            {
                if (inventoryItem is NonSerialisedInventoryItem nonSerialisedItem)
                {
                    this.AvailableToPromise += nonSerialisedItem.AvailableToPromise;
                }
                else if (inventoryItem is SerialisedInventoryItem serialisedItem)
                {
                    this.AvailableToPromise += serialisedItem.AvailableToPromise;
                }
            }
        }

        private void DeriveQuantityCommittedOut()
        {
            this.QuantityCommittedOut = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWherePart)
            {
                if (inventoryItem is NonSerialisedInventoryItem nonSerialised)
                {
                    this.QuantityCommittedOut += nonSerialised.QuantityCommittedOut;
                }
            }
        }

        private void DeriveQuantityExpectedIn()
        {
            this.QuantityExpectedIn = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWherePart)
            {
                if (inventoryItem is NonSerialisedInventoryItem nonSerialised)
                {
                    this.QuantityExpectedIn += nonSerialised.QuantityExpectedIn;
                }
            }
        }
    }
}
