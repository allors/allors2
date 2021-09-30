// <copyright file="OrganisationUnits.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class OrganisationUnits
    {
        private static readonly Guid DepartmentId = new Guid("1894DE01-6CAD-4788-96FB-EB977B4B20A8");
        private static readonly Guid DivisionId = new Guid("C2C4FA98-B123-4dce-BFFD-D18CCA9984E3");
        private static readonly Guid SubsidiaryId = new Guid("EC515EC8-7CE8-49ee-B23B-BEA4B46AF540");

        private UniquelyIdentifiableSticky<OrganisationUnit> cache;

        public OrganisationUnit Department => this.Cache[DepartmentId];

        public OrganisationUnit Division => this.Cache[DivisionId];

        public OrganisationUnit Subsidiary => this.Cache[SubsidiaryId];

        private UniquelyIdentifiableSticky<OrganisationUnit> Cache => this.cache ??= new UniquelyIdentifiableSticky<OrganisationUnit>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;


            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(DepartmentId, v =>
            {
                v.Name = "Department";
                localisedName.Set(v, dutchLocale, "Departement");
                v.IsActive = true;
            });

            merge(DivisionId, v =>
            {
                v.Name = "Division";
                localisedName.Set(v, dutchLocale, "Divisie");
                v.IsActive = true;
            });

            merge(SubsidiaryId, v =>
            {
                v.Name = "Subsidiary";
                localisedName.Set(v, dutchLocale, "Dochtermaatschappij");
                v.IsActive = true;
            });
        }
    }
}
