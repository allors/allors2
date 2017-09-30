// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequirementStates.cs" company="Allors bvba">
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

    public partial class RequirementStates
    {
        private static readonly Guid ActiveId = new Guid("FD324E47-69A2-4a7e-B953-E4749C67A2B0");
        private static readonly Guid InactiveId = new Guid("8B3422D1-67DD-4de7-A153-6ACF7B07F551");
        private static readonly Guid OnHoldId = new Guid("4CDBCF13-FC01-4671-A717-DDBEC2B6E8CF");
        private static readonly Guid CancelledId = new Guid("E5FA825B-C774-4627-BB91-9A441C6DAE5B");
        private static readonly Guid ClosedId = new Guid("8C800781-5371-4072-A281-4F1455573AA0");
        private static readonly Guid PendingApprovalFromClientId = new Guid("A6216522-44DA-404d-92A3-61160F814A15");
        private static readonly Guid FullfilledByOtherEnterpriseId = new Guid("10E9F384-541D-4fb3-ABDC-539EC291EFC6");

        private UniquelyIdentifiableCache<RequirementState> stateCache;

        public RequirementState Active => this.StateCache.Get(ActiveId);

        public RequirementState Inactive => this.StateCache.Get(InactiveId);

        public RequirementState OnHold => this.StateCache.Get(OnHoldId);

        public RequirementState Cancelled => this.StateCache.Get(CancelledId);

        public RequirementState Closed => this.StateCache.Get(ClosedId);

        public RequirementState PendingApprovalFromClient => this.StateCache.Get(PendingApprovalFromClientId);

        public RequirementState FullfilledByOtherEnterprise => this.StateCache.Get(FullfilledByOtherEnterpriseId);

        private UniquelyIdentifiableCache<RequirementState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<RequirementState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;
            
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
