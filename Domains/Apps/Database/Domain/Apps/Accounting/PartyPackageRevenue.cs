// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyPackageRevenue.cs" company="Allors bvba">
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
    using Meta;

    public partial class PartyPackageRevenue
    {
        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.AppsOnDeriveRevenue(derivation);
        }

        public void AppsOnDeriveRevenue(IDerivation derivation)
        {
            this.Revenue = 0;

            var partyProductCategoryRevenues = this.Party.PartyProductCategoryRevenuesWhereParty;
            partyProductCategoryRevenues.Filter.AddEquals(M.PartyProductCategoryRevenue.Year, this.Year);
            partyProductCategoryRevenues.Filter.AddEquals(M.PartyProductCategoryRevenue.Month, this.Month);

            foreach (PartyProductCategoryRevenue productCategoryRevenue in partyProductCategoryRevenues)
            {
                if (productCategoryRevenue.ProductCategory.ExistPackage && productCategoryRevenue.ProductCategory.Package.Equals(this.Package))
                {
                    this.Revenue += productCategoryRevenue.Revenue;
                }
            }
            
            if (this.ExistPackage)
            {
                var packageRevenue = PackageRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
                packageRevenue.OnDerive(x => x.WithDerivation(derivation));
            }
        }
    }
}
