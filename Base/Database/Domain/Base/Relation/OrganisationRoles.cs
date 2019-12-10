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

        private UniquelyIdentifiableSticky<OrganisationRole> Cache => this.cache ??= new UniquelyIdentifiableSticky<OrganisationRole>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(CustomerId, v =>
            {
                v.Name = "Customer";
                localisedName.Set(v, dutchLocale, "Klant");
                v.IsActive = true;
            });

            merge(SupplierId, v =>
            {
                v.Name = "Supplier";
                localisedName.Set(v, dutchLocale, "Leverancier");
                v.IsActive = true;
            });

            merge(ManufacturerId, v =>
            {
                v.Name = "Manufacturer";
                localisedName.Set(v, dutchLocale, "Fabrikant");
                v.IsActive = true;
            });
        }
    }
}
