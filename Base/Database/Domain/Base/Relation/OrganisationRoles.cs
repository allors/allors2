// <copyright file="OrganisationRoles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class OrganisationRoles
    {
        private static readonly Guid CustomerId = new Guid("8B5E0CEE-4C98-42F1-8F18-3638FBA943A0");
        private static readonly Guid SupplierId = new Guid("8C6D629B-1E27-4520-AA8C-E8ADF93A5095");
        private static readonly Guid ManufacturerId = new Guid("32E74BEF-2D79-4427-8902-B093AFA81661");

        private UniquelyIdentifiableSticky<OrganisationRole> cache;

        public OrganisationRole Customer => this.Cache[CustomerId];

        public OrganisationRole Supplier => this.Cache[SupplierId];

        public OrganisationRole Manufacturer => this.Cache[ManufacturerId];

        private UniquelyIdentifiableSticky<OrganisationRole> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<OrganisationRole>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new OrganisationRoleBuilder(this.Session)
                .WithName("Customer")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Klant").WithLocale(dutchLocale).Build())
                .WithUniqueId(CustomerId)
                .WithIsActive(true)
                .Build();

            new OrganisationRoleBuilder(this.Session)
                .WithName("Supplier")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Leverancier").WithLocale(dutchLocale).Build())
                .WithUniqueId(SupplierId)
                .WithIsActive(true)
                .Build();

            new OrganisationRoleBuilder(this.Session)
                .WithName("Manufacturer")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fabrikant").WithLocale(dutchLocale).Build())
                .WithUniqueId(ManufacturerId)
                .WithIsActive(true)
                .Build();
        }
    }
}
