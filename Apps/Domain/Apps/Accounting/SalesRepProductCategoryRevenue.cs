// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepProductCategoryRevenue.cs" company="Allors bvba">
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
    public partial class SalesRepProductCategoryRevenue
    {
        public string RevenueAsCurrencyString()
        {
            return DecimalExtensions.AsCurrencyString(this.Revenue, this.InternalOrganisation.CurrencyFormat);
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            
            this.SalesRepName = this.SalesRep.PartyName;

            this.AppsOnDeriveRevenue();
        }

        public void AppsOnDeriveRevenue()
        {
            this.Revenue = 0;

            var salesRepPartyProductCategoryRevenues = this.ProductCategory.SalesRepPartyProductCategoryRevenuesWhereProductCategory;
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.InternalOrganisation, this.InternalOrganisation);
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.SalesRep, this.SalesRep);
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Year, this.Year);
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Month, this.Month);

            foreach (SalesRepPartyProductCategoryRevenue salesRepPartyProductCategoryRevenue in salesRepPartyProductCategoryRevenues)
            {
                this.Revenue += salesRepPartyProductCategoryRevenue.Revenue;
            }

            if (this.ProductCategory.ExistParents)
            {
                SalesRepProductCategoryRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
            }
        }
    }
}