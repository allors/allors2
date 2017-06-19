// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationRoles.cs" company="Allors bvba">
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

    public partial class OrganisationRoles
    {
        private static readonly Guid CustomerId = new Guid("8B5E0CEE-4C98-42F1-8F18-3638FBA943A0");
        private static readonly Guid SupplierId = new Guid("8C6D629B-1E27-4520-AA8C-E8ADF93A5095");
        private static readonly Guid ManufacturerId = new Guid("32E74BEF-2D79-4427-8902-B093AFA81661");

        private UniquelyIdentifiableCache<OrganisationRole> cache;

        public OrganisationRole Customer => this.Cache.Get(CustomerId);

        public OrganisationRole Supplier => this.Cache.Get(SupplierId);

        public OrganisationRole Manufacturer => this.Cache.Get(ManufacturerId);

        private UniquelyIdentifiableCache<OrganisationRole> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableCache<OrganisationRole>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new OrganisationRoleBuilder(this.Session)
                .WithName("Customer")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Customer").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Klant").WithLocale(dutchLocale).Build())
                .WithUniqueId(CustomerId)
                .Build();

            new OrganisationRoleBuilder(this.Session)
                .WithName("Supplier")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Supplier").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Leverancier").WithLocale(dutchLocale).Build())
                .WithUniqueId(SupplierId)
                .Build();

            new OrganisationRoleBuilder(this.Session)
                .WithName("Manufacturer")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Manufacturer").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fabrikant").WithLocale(dutchLocale).Build())
                .WithUniqueId(ManufacturerId)
                .Build();
        }
    }
}
