// <copyright file="PartSpecificationStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PartSpecificationStates
    {
        public const string ApproveIdString = "7E08EA40-5A3B-4a18-A675-97212BA896DE";
        private static readonly Guid CreatedId = new Guid("89D6029D-2F90-43c7-989A-CF9EF7FE5DCE");
        private static readonly Guid DesignedId = new Guid("0B9F3E57-D3DE-43c7-AD71-0558D8CDF114");
        private static readonly Guid TestedId = new Guid("E87C46E1-D357-4cb8-B2DA-2BB7ADAF4970");
        private static readonly Guid ApprovedId = new Guid("B1FE3774-BD98-4230-9371-717F97DCD25B");
        private static readonly Guid RequirementSpecifiedId = new Guid("3604C076-EB3E-44c8-B855-D4F20918EC70");

        private UniquelyIdentifiableSticky<PartSpecificationState> cache;

        public PartSpecificationState Created => this.Cache[CreatedId];

        public PartSpecificationState Designed => this.Cache[DesignedId];

        public PartSpecificationState Tested => this.Cache[TestedId];

        public PartSpecificationState Approved => this.Cache[ApprovedId];

        public PartSpecificationState RequirementSpecified => this.Cache[RequirementSpecifiedId];

        private UniquelyIdentifiableSticky<PartSpecificationState> Cache => this.cache ??= new UniquelyIdentifiableSticky<PartSpecificationState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(CreatedId, v => v.Name = "Created");
            merge(DesignedId, v => v.Name = "Designed");
            merge(TestedId, v => v.Name = "Tested");
            merge(ApprovedId, v => v.Name = "Approved");
            merge(RequirementSpecifiedId, v => v.Name = "Requirement Specified");
        }
    }
}
