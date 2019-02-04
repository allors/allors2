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
    using System.Linq;

    using Allors.Meta;

    public partial class Good
    {
        private bool IsDeletable => !this.ExistDeploymentsWhereProductOffering && 
                                    !this.ExistEngagementItemsWhereProduct && 
                                    !this.ExistGeneralLedgerAccountsWhereCostUnitsAllowed && 
                                    !this.ExistGeneralLedgerAccountsWhereDefaultCostUnit && 
                                    !this.ExistQuoteItemsWhereProduct && 
                                    !this.ExistShipmentItemsWhereGood && 
                                    !this.ExistWorkEffortGoodStandardsWhereGood && 
                                    !this.ExistMarketingPackageWhereProductsUsedIn && 
                                    !this.ExistMarketingPackagesWhereProduct && 
                                    !this.ExistOrganisationGlAccountsWhereProduct && 
                                    !this.ExistProductConfigurationsWhereProductsUsedIn && 
                                    !this.ExistProductConfigurationsWhereProduct && 
                                    !this.ExistRequestItemsWhereProduct && 
                                    !this.ExistSalesInvoiceItemsWhereProduct && 
                                    !this.ExistSalesOrderItemsWhereProduct && 
                                    !this.ExistWorkEffortTypesWhereProductToProduce && 
                                    !this.ExistEngagementItemsWhereProduct && 
                                    !this.ExistProductWhereVariant;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var defaultLocale = this.strategy.Session.GetSingleton().DefaultLocale;
            var settings = this.strategy.Session.GetSingleton().Settings;

            var identifications = this.GoodIdentifications;
            identifications.Filter.AddEquals(M.IGoodIdentification.GoodIdentificationType, new GoodIdentificationTypes(this.strategy.Session).Good);
            var goodNumber = identifications.FirstOrDefault();

            if (goodNumber == null && settings.UseProductNumberCounter)
            {
                this.AddGoodIdentification(new ProductNumberBuilder(this.strategy.Session)
                    .WithIdentification(settings.NextProductNumber())
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.strategy.Session).Good).Build());
            }

            if (!this.ExistGoodIdentifications)
            {
                derivation.Validation.AssertExists(this, M.Good.GoodIdentifications);
            }

            if (!this.ExistVariants)
            {
                derivation.Validation.AssertExists(this, M.Good.Part);
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

                        if (priceComponent is BasePrice basePrice && !priceComponent.ExistProductFeature)
                        {
                            product.AddToBasePrice(basePrice);
                        }
                    }
                }
            }
        }

        public void AppsDelete(DeletableDelete method)
        {
            if (this.IsDeletable)
            {
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