// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationUnits.cs" company="Allors bvba">
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

    public partial class OrganisationUnits
    {
        public static readonly Guid DepartmentId = new Guid("1894DE01-6CAD-4788-96FB-EB977B4B20A8");
        public static readonly Guid DivisionId = new Guid("C2C4FA98-B123-4dce-BFFD-D18CCA9984E3");
        public static readonly Guid SubsidiaryId = new Guid("EC515EC8-7CE8-49ee-B23B-BEA4B46AF540");

        private UniquelyIdentifiableCache<OrganisationUnit> cache;

        public OrganisationUnit Department
        {
            get { return this.Cache.Get(DepartmentId); }
        }

        public OrganisationUnit Division
        {
            get { return this.Cache.Get(DivisionId); }
        }

        public OrganisationUnit Subsidiary
        {
            get { return this.Cache.Get(SubsidiaryId); }
        }

        private UniquelyIdentifiableCache<OrganisationUnit> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<OrganisationUnit>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new OrganisationUnitBuilder(this.Session)
                .WithName("Department")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Department").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Departement").WithLocale(dutchLocale).Build())
                .WithUniqueId(DepartmentId)
                .Build();
            
            new OrganisationUnitBuilder(this.Session)
                .WithName("Division")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Division").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Divisie").WithLocale(dutchLocale).Build())
                .WithUniqueId(DivisionId)
                .Build();
            
            new OrganisationUnitBuilder(this.Session)
                .WithName("Subsidiary")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Subsidiary").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Dochtermaatschappij").WithLocale(dutchLocale).Build())
                .WithUniqueId(SubsidiaryId)
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
