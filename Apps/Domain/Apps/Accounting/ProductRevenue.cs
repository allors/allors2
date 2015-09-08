// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductRevenue.cs" company="Allors bvba">
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

    public partial class ProductRevenue
    {
        public string RevenueAsCurrencyString()
        {
            return DecimalExtensions.AsCurrencyString(this.Revenue, this.InternalOrganisation.CurrencyFormat);
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.ProductName = this.Product.Description;

            this.AppsOnDeriveRevenue(derivation);
        }

        public void AppsOnDeriveRevenue(IDerivation derivation)
        {
            this.Revenue = 0;

            var partyProductRevenues = this.Product.PartyProductRevenuesWhereProduct;
            partyProductRevenues.Filter.AddEquals(PartyProductRevenues.Meta.InternalOrganisation, this.InternalOrganisation);
            partyProductRevenues.Filter.AddEquals(PartyProductRevenues.Meta.Year, this.Year);
            partyProductRevenues.Filter.AddEquals(PartyProductRevenues.Meta.Month, this.Month);

            foreach (PartyProductRevenue partyProductRevenue in partyProductRevenues)
            {
                this.Revenue += partyProductRevenue.Revenue;
            }

            var months = ((DateTime.UtcNow.Year - this.Year) * 12) + DateTime.UtcNow.Month - this.Month;
            if (months <= 12)
            {
                var histories = this.Product.ProductRevenueHistoriesWhereProduct;
                histories.Filter.AddEquals(ProductRevenueHistories.Meta.InternalOrganisation, this.InternalOrganisation);
                var history = histories.First ?? new ProductRevenueHistoryBuilder(this.Strategy.Session)
                                                     .WithCurrency(this.Currency)
                                                     .WithInternalOrganisation(this.InternalOrganisation)
                                                     .WithProduct(this.Product)
                                                     .Build();
            }

            foreach (ProductCategory productCategory in this.Product.ProductCategories)
            {
                productCategory.OnDerive(x => x.WithDerivation(derivation));
            }
        }
    }
}
