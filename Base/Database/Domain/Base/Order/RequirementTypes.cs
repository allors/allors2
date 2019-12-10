// <copyright file="RequirementTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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

        private UniquelyIdentifiableSticky<RequirementType> cache;

        public RequirementType CustomerRequirement => this.Cache[CustomerRequirementId];

        public RequirementType InternalRequirement => this.Cache[InternalRequirementId];

        public RequirementType ProductRequirement => this.Cache[ProductRequirementId];

        public RequirementType ProjectRequirement => this.Cache[ProjectRequirementId];

        public RequirementType ResourceRequirement => this.Cache[ResourceRequirementId];

        public RequirementType WorkRequirement => this.Cache[WorkRequirementId];

        private UniquelyIdentifiableSticky<RequirementType> Cache => this.cache ??= new UniquelyIdentifiableSticky<RequirementType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(CustomerRequirementId, v =>
            {
                v.Name = "Customer Requirement";
                localisedName.Set(v, dutchLocale, "Klant vereiste");
                v.IsActive = true;
            });

            merge(InternalRequirementId, v =>
            {
                v.Name = "Internal Requirement";
                localisedName.Set(v, dutchLocale, "Interne vereiste");
                v.IsActive = true;
            });

            merge(ProductRequirementId, v =>
            {
                v.Name = "Product Requirement";
                localisedName.Set(v, dutchLocale, "Product vereiste");
                v.IsActive = true;
            });

            merge(ProjectRequirementId, v =>
            {
                v.Name = "Project Requirement";
                localisedName.Set(v, dutchLocale, "Project vereiste");
                v.IsActive = true;
            });

            merge(ResourceRequirementId, v =>
            {
                v.Name = "Resource Requirement";
                localisedName.Set(v, dutchLocale, "Resource vereiste");
                v.IsActive = true;
            });

            merge(WorkRequirementId, v =>
            {
                v.Name = "Work Requirement";
                localisedName.Set(v, dutchLocale, "Werk vereiste");
                v.IsActive = true;
            });
        }
    }
}
