// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmploymentTerminations.cs" company="Allors bvba">
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

    public partial class EmploymentTerminations
    {
        public static readonly Guid ResignationId = new Guid("93A901E4-5BB6-456c-886E-463D9F60B4F2");
        public static readonly Guid FiredId = new Guid("C1EFC297-20C2-469d-BC93-2AB4A452C512");
        public static readonly Guid RetirenmentId = new Guid("1D567408-2630-4625-A676-D7CB8B19D04B");
        public static readonly Guid DeceasedId = new Guid("BE60EFE5-9790-49f2-886C-1C8DE5DB046C");

        private UniquelyIdentifiableCache<EmploymentTermination> cache;

        public EmploymentTermination Resignation
        {
            get { return this.Cache.Get(ResignationId); }
        }

        public EmploymentTermination Fired
        {
            get { return this.Cache.Get(FiredId); }
        }

        public EmploymentTermination Retirenment
        {
            get { return this.Cache.Get(RetirenmentId); }
        }

        public EmploymentTermination Deceased
        {
            get { return this.Cache.Get(DeceasedId); }
        }

        private UniquelyIdentifiableCache<EmploymentTermination> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<EmploymentTermination>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new EmploymentTerminationBuilder(this.Session)
                .WithName("Resignation")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Resignation").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ontslag genomen").WithLocale(dutchLocale).Build())
                .WithUniqueId(ResignationId)
                .Build();
            
            new EmploymentTerminationBuilder(this.Session)
                .WithName("Fired")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fired").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ontslagen").WithLocale(dutchLocale).Build())
                .WithUniqueId(FiredId)
                .Build();
            
            new EmploymentTerminationBuilder(this.Session)
                .WithName("Retirenment")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Retirenment").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pensioen").WithLocale(dutchLocale).Build())
                .WithUniqueId(RetirenmentId)
                .Build();
            
            new EmploymentTerminationBuilder(this.Session)
                .WithName("Deceased")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Deceased").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overleden").WithLocale(dutchLocale).Build())
                .WithUniqueId(DeceasedId)
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
