// <copyright file="PartSpecificationTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PartSpecificationTypes
    {
        private static readonly Guid ConstraintSpecificationId = new Guid("0B3247BF-5E17-458F-BE52-9255EC0E5A4D");
        private static readonly Guid OperatingConditionId = new Guid("6D49DEFC-27DD-40AB-B0DA-AFFC257FFB1C");
        private static readonly Guid PartSpecificationId = new Guid("E9F306C0-3CE2-48BD-82E8-E341181A36A9");
        private static readonly Guid PerformanceSpecificationId = new Guid("96A0ABA2-6CEE-4CB9-A0DA-076E46DD2312");
        private static readonly Guid TestingRequirementId = new Guid("EC007D79-D657-44BC-B7C5-CB4B7893582E");
        private static readonly Guid ToleranceId = new Guid("E6425782-ACED-47A1-AB5A-4EC97A2C80EA");

        private UniquelyIdentifiableSticky<PartSpecificationType> cache;

        public PartSpecificationType ConstraintSpecification => this.Cache[ConstraintSpecificationId];

        public PartSpecificationType OperatingCondition => this.Cache[OperatingConditionId];

        public PartSpecificationType PartSpecification => this.Cache[PartSpecificationId];

        public PartSpecificationType PerformanceSpecification => this.Cache[PerformanceSpecificationId];

        public PartSpecificationType TestingRequirement => this.Cache[TestingRequirementId];

        public PartSpecificationType Tolerance => this.Cache[ToleranceId];

        private UniquelyIdentifiableSticky<PartSpecificationType> Cache => this.cache ??= new UniquelyIdentifiableSticky<PartSpecificationType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(ConstraintSpecificationId, v =>
            {
                v.Name = "Constraint Specification";
                localisedName.Set(v, dutchLocale, "Constraint Specification");
                v.IsActive = true;
            });

            merge(OperatingConditionId, v =>
            {
                v.Name = "Operating Condition";
                localisedName.Set(v, dutchLocale, "Operating Condition");
                v.IsActive = true;
            });

            merge(PartSpecificationId, v =>
            {
                v.Name = "Part Specification";
                localisedName.Set(v, dutchLocale, "Part Specification");
                v.IsActive = true;
            });

            merge(PerformanceSpecificationId, v =>
            {
                v.Name = "Performance Specification";
                localisedName.Set(v, dutchLocale, "Performance Specification");
                v.IsActive = true;
            });

            merge(TestingRequirementId, v =>
            {
                v.Name = "Testing Requirement";
                localisedName.Set(v, dutchLocale, "Testing Requirement");
                v.IsActive = true;
            });

            merge(ToleranceId, v =>
            {
                v.Name = "Tolerance";
                localisedName.Set(v, dutchLocale, "Tolerance");
                v.IsActive = true;
            });
        }
    }
}
