// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Good.cs" company="Allors bvba">
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
namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Meta;

    public partial class Good
    {
        private bool IsDeletable => !this.ExistDeploymentsWhereProductOffering 
            && !this.ExistEngagementItemsWhereProduct
            && !this.ExistGeneralLedgerAccountsWhereCostUnitsAllowed
            && !this.ExistGeneralLedgerAccountsWhereDefaultCostUnit
            && !this.ExistQuoteItemsWhereProduct
            && !this.ExistShipmentItemsWhereGood 
            && !this.ExistWorkEffortGoodStandardsWhereGood
            && !this.ExistMarketingPackageWhereProductsUsedIn
            && !this.ExistMarketingPackagesWhereProduct
            && !this.ExistOrganisationGlAccountsWhereProduct
            && !this.ExistProductConfigurationsWhereProductsUsedIn
            && !this.ExistProductConfigurationsWhereProduct
            && !this.ExistPurchaseOrderItemsWhereProduct 
            && !this.ExistRequestItemsWhereProduct
            && !this.ExistInvoiceItemsWhereProduct
            && !this.ExistSalesOrderItemsWhereProduct
            && !this.ExistSupplierOfferingsWhereProduct
            && !this.ExistWorkEffortTypesWhereProductToProduce
            && !this.ExistEngagementItemsWhereProduct
            && !this.ExistProductWhereVariant;

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (this.ExistInventoryItemsWhereGood)
            {
                foreach (InventoryItem inventoryItem in this.InventoryItemsWhereGood)
                {
                    derivation.AddDependency(inventoryItem, this);
                }
            }

            if (derivation.HasChangedAssociation(this, this.Meta.SupplierOfferingsWhereProduct))
            {
                derivation.AddDerivable(this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var defaultLocale = this.strategy.Session.GetSingleton().DefaultLocale;

            derivation.Validation.AssertExistsAtMostOne(this, M.Good.FinishedGood, M.Good.InventoryItemKind);

            if (this.LocalisedNames.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Name = this.LocalisedNames.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (this.LocalisedDescriptions.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Description = this.LocalisedDescriptions.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (this.ProductCategories.Count == 1 && !this.ExistPrimaryProductCategory)
            {
                this.PrimaryProductCategory = this.ProductCategories.First;
            }

            if (this.ExistPrimaryProductCategory && !this.ExistProductCategories)
            {
                this.AddProductCategory(this.PrimaryProductCategory);
            }

            foreach (SupplierOffering supplierOffering in this.SupplierOfferingsWhereProduct)
            {
                if (supplierOffering.FromDate <= DateTime.UtcNow && (!supplierOffering.ExistThroughDate || supplierOffering.ThroughDate >= DateTime.UtcNow))
                {
                    this.AddSuppliedBy(supplierOffering.Supplier);
                }

                if (supplierOffering.FromDate > DateTime.UtcNow || (supplierOffering.ExistThroughDate && supplierOffering.ThroughDate < DateTime.UtcNow))
                {
                    this.RemoveSuppliedBy(supplierOffering.Supplier);
                }
            }

            this.DeriveVirtualProductPriceComponent();
            this.DeriveProductCategoriesExpanded(derivation);
            this.DeriveQuantityOnHand();
            this.DeriveAvailableToPromise();
            this.DeriveQuantityCommittedOut();
            this.DeriveQuantityExpectedIn();
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
                        product.AddVirtualProductPriceComponent(priceComponent);

                        var basePrice = priceComponent as BasePrice;
                        if (basePrice != null && !priceComponent.ExistProductFeature)
                        {
                            product.AddToBasePrice(basePrice);
                        }
                    }
                }
            }
        }

        public void DeriveProductCategoriesExpanded(IDerivation derivation)
        {
            this.RemoveProductCategoriesExpanded();

            if (this.ExistPrimaryProductCategory)
            {
                this.AddProductCategoriesExpanded(this.PrimaryProductCategory);
                foreach (ProductCategory superJacent in this.PrimaryProductCategory.SuperJacent)
                {
                    this.AddProductCategoriesExpanded(superJacent);
                    superJacent.AppsOnDeriveAllProducts(derivation);
                }
            }

            foreach (ProductCategory productCategory in this.ProductCategories)
            {
                this.AddProductCategoriesExpanded(productCategory);
                foreach (ProductCategory superJacent in productCategory.SuperJacent)
                {
                    this.AddProductCategoriesExpanded(superJacent);
                    superJacent.AppsOnDeriveAllProducts(derivation);
                }
            }
        }

        public void DeriveQuantityOnHand()
        {
            this.QuantityOnHand = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWhereGood)
            {
                if (inventoryItem is NonSerialisedInventoryItem)
                {
                    var nonSerialised = (NonSerialisedInventoryItem)inventoryItem;
                    this.QuantityOnHand += nonSerialised.QuantityOnHand;
                }
            }
        }

        public void DeriveAvailableToPromise()
        {
            this.AvailableToPromise = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWhereGood)
            {
                if (inventoryItem is NonSerialisedInventoryItem)
                {
                    var nonSerialised = (NonSerialisedInventoryItem)inventoryItem;
                    this.AvailableToPromise += nonSerialised.AvailableToPromise;
                }
            }
        }

        public void DeriveQuantityCommittedOut()
        {
            this.QuantityCommittedOut = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWhereGood)
            {
                if (inventoryItem is NonSerialisedInventoryItem)
                {
                    var nonSerialised = (NonSerialisedInventoryItem)inventoryItem;
                    this.QuantityCommittedOut += nonSerialised.QuantityCommittedOut;
                }
            }
        }

        public void DeriveQuantityExpectedIn()
        {
            this.QuantityExpectedIn = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWhereGood)
            {
                if (inventoryItem is NonSerialisedInventoryItem)
                {
                    var nonSerialised = (NonSerialisedInventoryItem)inventoryItem;
                    this.QuantityExpectedIn += nonSerialised.QuantityExpectedIn;
                }
            }
        }

        public void AppsDelete(DeletableDelete method)
        {
            if (this.IsDeletable)
            {
                foreach (VendorProduct vendorProduct in this.VendorProductsWhereProduct)
                {
                    vendorProduct.Delete();
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

                foreach (PartyProductRevenue revenue in this.PartyProductRevenuesWhereProduct)
                {
                    revenue.Delete();
                }

                foreach (ProductRevenue revenue in this.ProductRevenuesWhereProduct)
                {
                    revenue.Delete();
                }

                foreach (InventoryItem inventoryItem in this.InventoryItemsWhereGood)
                {
                    inventoryItem.Delete();
                }
            }
        }

        public string DeriveDetails()
        {
            var builder = new StringBuilder();

            if (this.ExistManufacturedBy)
            {
                builder.Append($", Manufacturer: {this.ManufacturedBy.PartyName}");
            }

            foreach (ProductFeature feature in this.ProductFeatureApplicabilitiesWhereAvailableFor)
            {
                if (feature is Brand)
                {
                    var brand = (Brand)feature;
                    builder.Append($", Brand: {brand.Name}");
                }
                if (feature is Model)
                {
                    var model = (Model)feature;
                    builder.Append($", Model: {model.Name}");
                }
            }

            if (this.InventoryItemsWhereGood.First is SerialisedInventoryItem serialisedInventoryItem)
            {
                builder.Append($", SN: {serialisedInventoryItem.SerialNumber}");

                if (serialisedInventoryItem.ExistManufacturingYear)
                {
                    builder.Append($", YOM: {serialisedInventoryItem.ManufacturingYear}");
                }

                foreach (SerialisedInventoryItemCharacteristic characteristic in serialisedInventoryItem.SerialisedInventoryItemCharacteristics)
                {
                    if (characteristic.ExistValue)
                    {
                        var characteristicType = characteristic.SerialisedInventoryItemCharacteristicType;
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
            }

            var details = builder.ToString();

            if (details.StartsWith(","))
            {
                details = details.Substring(2);
            }

            return details;
        }
    }
}