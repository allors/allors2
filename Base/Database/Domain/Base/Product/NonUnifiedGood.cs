// <copyright file="NonUnifiedGood.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Text;

namespace Allors.Domain
{
    using System.Linq;

    using Allors.Meta;

    public partial class NonUnifiedGood
    {
        private bool IsDeletable => !this.ExistPart &&
                                    !this.ExistDeploymentsWhereProductOffering &&
                                    !this.ExistEngagementItemsWhereProduct &&
                                    !this.ExistGeneralLedgerAccountsWhereCostUnitsAllowed &&
                                    !this.ExistGeneralLedgerAccountsWhereDefaultCostUnit &&
                                    !this.ExistQuoteItemsWhereProduct &&
                                    !this.ExistShipmentItemsWhereGood &&
                                    !this.ExistWorkEffortTypesWhereProductToProduce &&
                                    !this.ExistMarketingPackageWhereProductsUsedIn &&
                                    !this.ExistMarketingPackagesWhereProduct &&
                                    !this.ExistOrganisationGlAccountsWhereProduct &&
                                    !this.ExistProductConfigurationsWhereProductsUsedIn &&
                                    !this.ExistProductConfigurationsWhereProduct &&
                                    !this.ExistRequestItemsWhereProduct &&
                                    !this.ExistSalesInvoiceItemsWhereProduct &&
                                    !this.ExistSalesOrderItemsWhereProduct &&
                                    !this.ExistWorkEffortTypesWhereProductToProduce;

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var defaultLocale = this.Strategy.Session.GetSingleton().DefaultLocale;
            var settings = this.Strategy.Session.GetSingleton().Settings;

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

            if (!this.ExistVariants)
            {
                derivation.Validation.AssertExists(this, M.NonUnifiedGood.Part);
            }

            if (this.LocalisedNames.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Name = this.LocalisedNames.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (this.LocalisedDescriptions.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Description = this.LocalisedDescriptions.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            this.DeriveVirtualProductPriceComponent();
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var builder = new StringBuilder();
            if (this.ExistProductIdentifications)
            {
                builder.Append(string.Join(" ", this.ProductIdentifications.Select(v => v.Identification)));
            }

            if (this.ExistProductCategoriesWhereAllProduct)
            {
                builder.Append(string.Join(" ", this.ProductCategoriesWhereAllProduct.Select(v => v.Name)));
            }

            builder.Append(string.Join(" ", this.Keywords));

            this.SearchString = builder.ToString();

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
            }
        }
    }
}
