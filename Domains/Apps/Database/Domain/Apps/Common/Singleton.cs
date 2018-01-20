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
    using Allors.Meta;

    /// <summary>
    /// The Application object serves as the singleton for
    /// your population.
    /// It is the ideal place to hold application settings
    /// (e.g. the domain, the guest user, ...).
    /// </summary>
    public partial class Singleton
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.HasChangedRole(this, this.Meta.AdditionalLocales))
            {
                foreach (Good product in new Goods(this.strategy.Session).Extent())
                {
                    derivation.AddDerivable(product);
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistPreviousCurrency)
            {
                derivation.Validation.AssertAreEqual(
                    this,
                    M.Singleton.PreferredCurrency,
                    M.Singleton.PreviousCurrency);
            }
            else
            {
                this.PreviousCurrency = this.PreferredCurrency;
            }
        }

        public void CalculateRevenues(IDerivation derivation)
        {
            var session = this.Strategy.Session;

            PartyPackageRevenues.AppsOnDeriveRevenues(session);
            session.Commit();

            PartyProductCategoryRevenues.AppsOnDeriveRevenues(session);
            session.Commit();

            PartyProductRevenues.AppsOnDeriveRevenues(session);
            session.Commit();

            PartyRevenues.AppsOnDeriveRevenues(session);
            session.Commit();

            foreach (PartyPackageRevenue revenue in session.Extent<PartyPackageRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (PartyProductCategoryRevenue revenue in session.Extent<PartyProductCategoryRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (PartyProductRevenue revenue in session.Extent<PartyProductRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (PartyRevenue revenue in session.Extent<PartyRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            Parties.AppsOnDeriveRevenues(session);
            session.Commit();

            InternalOrganisationRevenues.AppsOnDeriveRevenues(session);
            session.Commit();
            PackageRevenues.AppsOnDeriveRevenues(session);
            session.Commit();
            ProductCategoryRevenues.AppsOnDeriveRevenues(session);
            session.Commit();
            ProductRevenues.AppsOnDeriveRevenues(session);
            session.Commit();
            SalesRepPartyProductCategoryRevenues.AppsOnDeriveRevenues(session);
            session.Commit();
            SalesRepPartyRevenues.AppsOnDeriveRevenues(session);
            session.Commit();
            SalesRepProductCategoryRevenues.AppsOnDeriveRevenues(session);
            session.Commit();
            SalesRepRevenues.AppsOnDeriveRevenues(session);
            session.Commit();
            StoreRevenues.AppsOnDeriveRevenues(session);
            session.Commit();
            SalesChannelRevenues.AppsOnDeriveRevenues(session);
            session.Commit();

            foreach (InternalOrganisationRevenue revenue in session.Extent<InternalOrganisationRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (PackageRevenue revenue in session.Extent<PackageRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (ProductCategoryRevenue revenue in session.Extent<ProductCategoryRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (ProductRevenue revenue in session.Extent<ProductRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (SalesRepPartyProductCategoryRevenue revenue in session.Extent<SalesRepPartyProductCategoryRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (SalesRepPartyRevenue revenue in session.Extent<SalesRepPartyRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (SalesRepProductCategoryRevenue revenue in session.Extent<SalesRepProductCategoryRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (SalesRepRevenue revenue in session.Extent<SalesRepRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (StoreRevenue revenue in session.Extent<StoreRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            foreach (SalesChannelRevenue revenue in session.Extent<SalesChannelRevenue>())
            {
                revenue.OnDerive(x => x.WithDerivation(derivation));
            }

            session.Commit();

            SalesRepRelationships.DeriveCommissions(session);
            session.Commit();
            People.AppsOnDeriveCommissions(session);
            session.Commit();
        }
    }
}
