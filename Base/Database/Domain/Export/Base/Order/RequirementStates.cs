// <copyright file="RequirementStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class RequirementStates
    {
        private static readonly Guid ActiveId = new Guid("FD324E47-69A2-4a7e-B953-E4749C67A2B0");
        private static readonly Guid InactiveId = new Guid("8B3422D1-67DD-4de7-A153-6ACF7B07F551");
        private static readonly Guid OnHoldId = new Guid("4CDBCF13-FC01-4671-A717-DDBEC2B6E8CF");
        private static readonly Guid CancelledId = new Guid("E5FA825B-C774-4627-BB91-9A441C6DAE5B");
        private static readonly Guid ClosedId = new Guid("8C800781-5371-4072-A281-4F1455573AA0");
        private static readonly Guid PendingApprovalFromClientId = new Guid("A6216522-44DA-404d-92A3-61160F814A15");
        private static readonly Guid FullfilledByOtherEnterpriseId = new Guid("10E9F384-541D-4fb3-ABDC-539EC291EFC6");

        private UniquelyIdentifiableSticky<RequirementState> stateCache;

        public RequirementState Active => this.StateCache[ActiveId];

        public RequirementState Inactive => this.StateCache[InactiveId];

        public RequirementState OnHold => this.StateCache[OnHoldId];

        public RequirementState Cancelled => this.StateCache[CancelledId];

        public RequirementState Closed => this.StateCache[ClosedId];

        public RequirementState PendingApprovalFromClient => this.StateCache[PendingApprovalFromClientId];

        public RequirementState FullfilledByOtherEnterprise => this.StateCache[FullfilledByOtherEnterpriseId];

        private UniquelyIdentifiableSticky<RequirementState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<RequirementState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new RequirementStateBuilder(this.Session)
                .WithUniqueId(ActiveId)
                .WithName("Active")
                .Build();

            new RequirementStateBuilder(this.Session)
                .WithUniqueId(InactiveId)
                .WithName("Inactive")
                .Build();

            new RequirementStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new RequirementStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new RequirementStateBuilder(this.Session)
                .WithUniqueId(ClosedId)
                .WithName("Closed")
                .Build();

            new RequirementStateBuilder(this.Session)
                .WithUniqueId(PendingApprovalFromClientId)
                .WithName("Pending Approval From Client")
                .Build();

            new RequirementStateBuilder(this.Session)
                .WithUniqueId(FullfilledByOtherEnterpriseId)
                .WithName("Fullfilled By Other Enterprise")
                .Build();
        }
    }
}
