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
        private UniquelyIdentifiableSticky<PartSpecificationState> stateCache;

        public PartSpecificationState Created => this.StateCache[CreatedId];

        public PartSpecificationState Designed => this.StateCache[DesignedId];

        public PartSpecificationState Tested => this.StateCache[TestedId];

        public PartSpecificationState Approved => this.StateCache[ApprovedId];

        public PartSpecificationState RequirementSpecified => this.StateCache[RequirementSpecifiedId];

        private UniquelyIdentifiableSticky<PartSpecificationState> StateCache => this.stateCache ??= new UniquelyIdentifiableSticky<PartSpecificationState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            new PartSpecificationStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PartSpecificationStateBuilder(this.Session)
                .WithUniqueId(DesignedId)
                .WithName("Designed")
                .Build();

            new PartSpecificationStateBuilder(this.Session)
                .WithUniqueId(TestedId)
                .WithName("Tested")
                .Build();

            new PartSpecificationStateBuilder(this.Session)
                .WithUniqueId(ApprovedId)
                .WithName("Approved")
                .Build();

            new PartSpecificationStateBuilder(this.Session)
                .WithUniqueId(RequirementSpecifiedId)
                .WithName("Requirement Specified")
                .Build();
        }
    }
}
