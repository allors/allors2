// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyPackageRevenue.cs" company="Allors bvba">
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

    public partial class PartyPackageRevenue
    {
        public string RevenueAsCurrencyString()
        {
            return DecimalExtensions.AsCurrencyString(this.Revenue, this.InternalOrganisation.CurrencyFormat);
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.AppsOnDeriveRevenue(derivation);
        }

        public void AppsOnDeriveRevenue(IDerivation derivation)
        {
            this.Revenue = 0;

            var partyProductCategoryRevenues = this.Party.PartyProductCategoryRevenuesWhereParty;
            partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.InternalOrganisation, this.InternalOrganisation);
            partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.Year, this.Year);
            partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.Month, this.Month);

            foreach (PartyProductCategoryRevenue productCategoryRevenue in partyProductCategoryRevenues)
            {
                if (productCategoryRevenue.ProductCategory.ExistPackage && productCategoryRevenue.ProductCategory.Package.Equals(this.Package))
                {
                    this.Revenue += productCategoryRevenue.Revenue;
                }
            }

            var months = ((DateTime.UtcNow.Year - this.Year) * 12) + DateTime.UtcNow.Month - this.Month;
            if (months <= 12)
            {
                var histories = this.Party.PartyPackageRevenueHistoriesWhereParty;
                histories.Filter.AddEquals(PartyPackageRevenueHistories.Meta.InternalOrganisation, this.InternalOrganisation);
                histories.Filter.AddEquals(PartyPackageRevenueHistories.Meta.Package, this.Package);
                var history = histories.First ?? new PartyPackageRevenueHistoryBuilder(this.Strategy.Session)
                                                     .WithCurrency(this.Currency)
                                                     .WithInternalOrganisation(this.InternalOrganisation)
                                                     .WithParty(this.Party)
                                                     .WithPackage(this.Package)
                                                     .WithRevenue(0)
                                                     .Build();

                history.AppsOnDeriveHistory();
            }

            if (this.ExistPackage)
            {
                var packageRevenue = PackageRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
                packageRevenue.OnDerive(x => x.WithDerivation(derivation));
            }
        }
    }
}
