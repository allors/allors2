// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Singleton.cs" company="Allors bvba">
//   Copyright 2002-2011 Allors bvba.
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
    /// <summary>
    /// The Application object serves as the singleton for
    /// your population.
    /// It is the ideal place to hold application settings
    /// (e.g. the domain, the guest user, ...).
    /// </summary>
    public partial class Singleton
    {
        public void AppsOnDeriveRevenues()
        {
            PartyPackageRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();

            PartyProductCategoryRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();

            PartyProductRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();

            PartyRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();

            var derivation = new Derivation(this.Strategy.Session);

            foreach (PartyPackageRevenue revenue in this.Strategy.Session.Extent<PartyPackageRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (PartyProductCategoryRevenue revenue in this.Strategy.Session.Extent<PartyProductCategoryRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (PartyProductRevenue revenue in this.Strategy.Session.Extent<PartyProductRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (PartyRevenue revenue in this.Strategy.Session.Extent<PartyRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            CustomerRelationships.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();

            Parties.DeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();

            InternalOrganisationRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();
            PackageRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();
            ProductCategoryRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();
            ProductRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();
            SalesRepPartyProductCategoryRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();
            SalesRepPartyRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();
            SalesRepProductCategoryRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();
            SalesRepRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();
            StoreRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();
            SalesChannelRevenues.AppsOnDeriveRevenues(this.Strategy.Session);
            this.Strategy.Session.Commit();

            foreach (InternalOrganisationRevenue revenue in this.Strategy.Session.Extent<InternalOrganisationRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (PackageRevenue revenue in this.Strategy.Session.Extent<PackageRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (ProductCategoryRevenue revenue in this.Strategy.Session.Extent<ProductCategoryRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (ProductRevenue revenue in this.Strategy.Session.Extent<ProductRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (SalesRepPartyProductCategoryRevenue revenue in this.Strategy.Session.Extent<SalesRepPartyProductCategoryRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (SalesRepPartyRevenue revenue in this.Strategy.Session.Extent<SalesRepPartyRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (SalesRepProductCategoryRevenue revenue in this.Strategy.Session.Extent<SalesRepProductCategoryRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (SalesRepRevenue revenue in this.Strategy.Session.Extent<SalesRepRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (StoreRevenue revenue in this.Strategy.Session.Extent<StoreRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (SalesChannelRevenue revenue in this.Strategy.Session.Extent<SalesChannelRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            SalesRepRelationships.DeriveCommissions(this.Strategy.Session);
            this.Strategy.Session.Commit();
            Persons.AppsOnDeriveCommissions(this.Strategy.Session);
            this.Strategy.Session.Commit();

            this.AppsOnDeriveHistories();
        }

        public void AppsOnDeriveHistories()
        {
            PartyPackageRevenueHistories.AppsOnDeriveHistory(this.Strategy.Session);
            this.Strategy.Session.Commit();

            PartyProductCategoryRevenueHistories.AppsOnDeriveHistory(this.Strategy.Session);
            this.Strategy.Session.Commit();

            PartyProductRevenueHistories.AppsOnDeriveHistory(this.Strategy.Session);
            this.Strategy.Session.Commit();

            PartyRevenueHistories.AppsOnDeriveHistory(this.Strategy.Session);
            this.Strategy.Session.Commit();

            var derivation = new Derivation(this.Strategy.Session);

            var revenues = this.Strategy.Session.Extent<PartyPackageRevenueHistory>();
            foreach (PartyPackageRevenueHistory revenue in revenues)
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (PartyProductCategoryRevenueHistory revenue in this.Strategy.Session.Extent<PartyProductCategoryRevenueHistory>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (PartyProductRevenueHistory revenue in this.Strategy.Session.Extent<PartyProductRevenueHistory>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (PartyRevenueHistory revenue in this.Strategy.Session.Extent<PartyRevenueHistory>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            InternalOrganisationRevenueHistories.AppsOnDeriveHistory(this.Strategy.Session);
            this.Strategy.Session.Commit();
            PackageRevenueHistories.AppsOnDeriveHistory(this.Strategy.Session);
            this.Strategy.Session.Commit();
            ProductCategoryRevenueHistories.AppsOnDeriveHistory(this.Strategy.Session);
            this.Strategy.Session.Commit();
            ProductRevenueHistories.AppsOnDeriveHistory(this.Strategy.Session);
            this.Strategy.Session.Commit();
            SalesChannelRevenueHistories.AppsOnDeriveHistory(this.Strategy.Session);
            this.Strategy.Session.Commit();
            SalesRepRevenueHistories.AppsOnDeriveHistory(this.Strategy.Session);
            this.Strategy.Session.Commit();
            StoreRevenueHistories.AppsOnDeriveHistory(this.Strategy.Session);
            this.Strategy.Session.Commit();

            foreach (InternalOrganisationRevenueHistory revenueHistory in this.Strategy.Session.Extent<InternalOrganisationRevenueHistory>())
            {
                revenueHistory.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (PackageRevenueHistory revenueHistory in this.Strategy.Session.Extent<PackageRevenueHistory>())
            {
                revenueHistory.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (ProductCategoryRevenueHistory revenueHistory in this.Strategy.Session.Extent<ProductCategoryRevenueHistory>())
            {
                revenueHistory.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (ProductRevenueHistory revenueHistory in this.Strategy.Session.Extent<ProductRevenueHistory>())
            {
                revenueHistory.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (SalesChannelRevenueHistory revenueHistory in this.Strategy.Session.Extent<SalesChannelRevenueHistory>())
            {
                revenueHistory.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (SalesRepRevenueHistory revenueHistory in this.Strategy.Session.Extent<SalesRepRevenueHistory>())
            {
                revenueHistory.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();

            foreach (StoreRevenueHistory revenueHistory in this.Strategy.Session.Extent<StoreRevenueHistory>())
            {
                revenueHistory.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Strategy.Session.Commit();
        }
    }
}
