// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequirementTypes.cs" company="Allors bvba">
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

    public partial class RequirementTypes
    {
        private static readonly Guid CustomerRequirementId = new Guid("2597C2E9-3E75-4448-A1B2-014A03D650A5");
        private static readonly Guid InternalRequirementId = new Guid("921DFF06-0488-49EA-991F-4DCB4D6D2C98");
        private static readonly Guid ProductRequirementId = new Guid("515CDEDF-3D42-4B27-871B-FE2A6B43932E");
        private static readonly Guid ProjectRequirementId = new Guid("957B9D21-016F-40F2-8CBF-219EE7DC1877");
        private static readonly Guid ResourceRequirementId = new Guid("809298A8-9C2A-4E21-9B03-BB5B9F681B2E");
        private static readonly Guid WorkRequirementId = new Guid("3E43DE6A-C68F-4422-A557-C8DE877C2CF6");

        private UniquelyIdentifiableCache<RequirementType> cache;

        public RequirementType CustomerRequirement => this.Cache.Get(CustomerRequirementId);

        public RequirementType InternalRequirement => this.Cache.Get(InternalRequirementId);

        public RequirementType ProductRequirement => this.Cache.Get(ProductRequirementId);

        public RequirementType ProjectRequirement => this.Cache.Get(ProjectRequirementId);

        public RequirementType ResourceRequirement => this.Cache.Get(ResourceRequirementId);

        public RequirementType WorkRequirement => this.Cache.Get(WorkRequirementId);

        private UniquelyIdentifiableCache<RequirementType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableCache<RequirementType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new RequirementTypeBuilder(this.Session)
                .WithName("Production Run")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Customer Requirement").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Klant vereiste").WithLocale(dutchLocale).Build())
                .WithUniqueId(CustomerRequirementId)
                .Build();
            
            new RequirementTypeBuilder(this.Session)
                .WithName("Process")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Internal Requirement").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Interne vereiste").WithLocale(dutchLocale).Build())
                .WithUniqueId(InternalRequirementId)
                .Build();
            
            new RequirementTypeBuilder(this.Session)
                .WithName("Process Step")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product Requirement").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product vereiste").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProductRequirementId)
                .Build();

            new RequirementTypeBuilder(this.Session)
                .WithName("Process Step")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Project Requirement").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Project vereiste").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProjectRequirementId)
                .Build();

            new RequirementTypeBuilder(this.Session)
                .WithName("Process Step")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Resource Requirement").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Resource vereiste").WithLocale(dutchLocale).Build())
                .WithUniqueId(ResourceRequirementId)
                .Build();

            new RequirementTypeBuilder(this.Session)
                .WithName("Process Step")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Work Requirement").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Werk vereiste").WithLocale(dutchLocale).Build())
                .WithUniqueId(WorkRequirementId)
                .Build();
        }
    }
}
