// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmploymentTerminations.cs" company="Allors bvba">
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

    public partial class EmploymentTerminations
    {
        private static readonly Guid ResignationId = new Guid("93A901E4-5BB6-456c-886E-463D9F60B4F2");
        private static readonly Guid FiredId = new Guid("C1EFC297-20C2-469d-BC93-2AB4A452C512");
        private static readonly Guid RetirenmentId = new Guid("1D567408-2630-4625-A676-D7CB8B19D04B");
        private static readonly Guid DeceasedId = new Guid("BE60EFE5-9790-49f2-886C-1C8DE5DB046C");

        private UniquelyIdentifiableSticky<EmploymentTermination> cache;

        public EmploymentTermination Resignation => this.Cache[ResignationId];

        public EmploymentTermination Fired => this.Cache[FiredId];

        public EmploymentTermination Retirenment => this.Cache[RetirenmentId];

        public EmploymentTermination Deceased => this.Cache[DeceasedId];

        private UniquelyIdentifiableSticky<EmploymentTermination> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<EmploymentTermination>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new EmploymentTerminationBuilder(this.Session)
                .WithName("Resignation")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ontslag genomen").WithLocale(dutchLocale).Build())
                .WithUniqueId(ResignationId)
                .WithIsActive(true)
                .Build();
            
            new EmploymentTerminationBuilder(this.Session)
                .WithName("Fired")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ontslagen").WithLocale(dutchLocale).Build())
                .WithUniqueId(FiredId)
                .WithIsActive(true)
                .Build();
            
            new EmploymentTerminationBuilder(this.Session)
                .WithName("Retirement")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pensioen").WithLocale(dutchLocale).Build())
                .WithUniqueId(RetirenmentId)
                .WithIsActive(true)
                .Build();
            
            new EmploymentTerminationBuilder(this.Session)
                .WithName("Deceased")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overleden").WithLocale(dutchLocale).Build())
                .WithUniqueId(DeceasedId)
                .WithIsActive(true)
                .Build();
        }
    }
}
