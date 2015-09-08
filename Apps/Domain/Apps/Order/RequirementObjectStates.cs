// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequirementObjectStates.cs" company="Allors bvba">
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

    public partial class RequirementObjectStates
    {
        public static readonly Guid ActiveId = new Guid("FD324E47-69A2-4a7e-B953-E4749C67A2B0");
        public static readonly Guid InactiveId = new Guid("8B3422D1-67DD-4de7-A153-6ACF7B07F551");
        public static readonly Guid OnHoldId = new Guid("4CDBCF13-FC01-4671-A717-DDBEC2B6E8CF");
        public static readonly Guid CancelledId = new Guid("E5FA825B-C774-4627-BB91-9A441C6DAE5B");
        public static readonly Guid ClosedId = new Guid("8C800781-5371-4072-A281-4F1455573AA0");
        public static readonly Guid PendingApprovalFromClientId = new Guid("A6216522-44DA-404d-92A3-61160F814A15");
        public static readonly Guid FullfilledByOtherEnterpriseId = new Guid("10E9F384-541D-4fb3-ABDC-539EC291EFC6");

        private UniquelyIdentifiableCache<RequirementObjectState> stateCache;

        public RequirementObjectState Active
        {
            get { return this.StateCache.Get(ActiveId); }
        }

        public RequirementObjectState Inactive
        {
            get { return this.StateCache.Get(InactiveId); }
        }

        public RequirementObjectState OnHold
        {
            get { return this.StateCache.Get(OnHoldId); }
        }

        public RequirementObjectState Cancelled
        {
            get { return this.StateCache.Get(CancelledId); }
        }

        public RequirementObjectState Closed
        {
            get { return this.StateCache.Get(ClosedId); }
        }

        public RequirementObjectState PendingApprovalFromClient
        {
            get { return this.StateCache.Get(PendingApprovalFromClientId); }
        }

        public RequirementObjectState FullfilledByOtherEnterprise
        {
            get { return this.StateCache.Get(FullfilledByOtherEnterpriseId); }
        }

        private UniquelyIdentifiableCache<RequirementObjectState> StateCache
        {
            get
            {
                return this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<RequirementObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;
            
            new RequirementObjectStateBuilder(Session)
                .WithUniqueId(ActiveId)
                .WithName("Active")
                .Build();

            new RequirementObjectStateBuilder(Session)
                .WithUniqueId(InactiveId)
                .WithName("Inactive")
                .Build();

            new RequirementObjectStateBuilder(Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new RequirementObjectStateBuilder(Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new RequirementObjectStateBuilder(Session)
                .WithUniqueId(ClosedId)
                .WithName("Closed")
                .Build();

            new RequirementObjectStateBuilder(Session)
                .WithUniqueId(PendingApprovalFromClientId)
                .WithName("Pending Approval From Client")
                .Build();

            new RequirementObjectStateBuilder(Session)
                .WithUniqueId(FullfilledByOtherEnterpriseId)
                .WithName("Fullfilled By Other Enterprise")
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
