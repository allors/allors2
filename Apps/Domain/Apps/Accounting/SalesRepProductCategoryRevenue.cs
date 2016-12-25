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
    using Meta;

    public partial class SalesRepProductCategoryRevenue
    {
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
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.InternalOrganisation, this.InternalOrganisation);
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.SalesRep, this.SalesRep);
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Year, this.Year);
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Month, this.Month);

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