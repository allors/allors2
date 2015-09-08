// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyProductCategoryRevenue.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class PartyProductCategoryRevenue
    {
        public string RevenueAsCurrencyString()
        {
            return DecimalExtensions.AsCurrencyString(this.Revenue, this.InternalOrganisation.CurrencyFormat);
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.PartyProductCategoryName = string.Concat(this.Party.PartyName, "/", this.ProductCategory.Description);

            this.AppsOnDeriveRevenue(derivation);
        }

        public void AppsOnDeriveRevenue(IDerivation derivation)
        {
            this.Revenue = 0;
            this.Quantity = 0;

            var partyProductRevenues = this.Party.PartyProductRevenuesWhereParty;
            partyProductRevenues.Filter.AddEquals(PartyProductRevenues.Meta.InternalOrganisation, this.InternalOrganisation);
            partyProductRevenues.Filter.AddEquals(PartyProductRevenues.Meta.Year, this.Year);
            partyProductRevenues.Filter.AddEquals(PartyProductRevenues.Meta.Month, this.Month);

            foreach (PartyProductRevenue partyProductRevenue in partyProductRevenues)
            {
                if (this.ProductCategory.ProductsWhereProductCategory.Contains(partyProductRevenue.Product))
                {
                    this.Revenue += partyProductRevenue.Revenue;
                    this.Quantity += partyProductRevenue.Quantity;
                }
                else
                {
                    if (partyProductRevenue.Product.ProductCategoriesExpanded.Contains(this.ProductCategory))
                    {
                        this.Revenue += partyProductRevenue.Revenue;
                        this.Quantity += partyProductRevenue.Quantity;
                    }
                }
            }

            if (this.ProductCategory.ExistParents)
            {
                foreach (ProductCategory parentCategory in this.ProductCategory.Parents)
                {
                    var partyProductCategoryRevenues = this.Party.PartyProductCategoryRevenuesWhereParty;
                    partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.InternalOrganisation, this.InternalOrganisation);
                    partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.Year, this.Year);
                    partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.Month, this.Month);
                    partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.ProductCategory, parentCategory);
                    var partyProductCategoryRevenue = partyProductCategoryRevenues.First
                                                      ?? new PartyProductCategoryRevenueBuilder(this.Strategy.Session)
                                                                .WithInternalOrganisation(this.InternalOrganisation)
                                                                .WithParty(this.Party)
                                                                .WithProductCategory(parentCategory)
                                                                .WithYear(this.Year)
                                                                .WithMonth(this.Month)
                                                                .WithCurrency(this.Currency)
                                                                .WithRevenue(0M)
                                                                .Build();

                    partyProductCategoryRevenue.OnDerive(x => x.WithDerivation(derivation));
                }
            }

            var months = ((DateTime.UtcNow.Year - this.Year) * 12) + DateTime.UtcNow.Month - this.Month;
            if (months <= 12)
            {
                var histories = this.Party.PartyProductCategoryRevenueHistoriesWhereParty;
                histories.Filter.AddEquals(
                    PartyProductCategoryRevenueHistories.Meta.InternalOrganisation, this.InternalOrganisation);
                histories.Filter.AddEquals(PartyProductCategoryRevenueHistories.Meta.ProductCategory, this.ProductCategory);
                var history = histories.First
                              ??
                              new PartyProductCategoryRevenueHistoryBuilder(this.Strategy.Session).WithCurrency(this.Currency).
                                  WithInternalOrganisation(this.InternalOrganisation).WithParty(this.Party).WithProductCategory(
                                      this.ProductCategory).WithRevenue(0).WithQuantity(0).Build();

                history.AppsOnDeriveHistory();
            }

            if (this.ExistProductCategory)
            {
                var productCategoryRevenue = ProductCategoryRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
                productCategoryRevenue.OnDerive(x => x.WithDerivation(derivation));
            }

            if (this.ExistProductCategory)
            {
                var productCategoryRevenue = ProductCategoryRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
                productCategoryRevenue.OnDerive(x => x.WithDerivation(derivation));
            }

            if (this.ExistProductCategory && this.ProductCategory.ExistPackage)
            {
                var partyPackageRevenue = PartyPackageRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
                partyPackageRevenue.OnDerive(x => x.WithDerivation(derivation));
            }
        }
    }
}
