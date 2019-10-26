// <copyright file="People.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Allors.Meta;

    public partial class AutomatedAgents
    {
        public static readonly Guid GuestId = new Guid("1261CB56-67F2-4725-AF7D-604A117ABBEC");
        public static readonly Guid SchedulerId = new Guid("037C4B36-5950-4D32-BA95-85CCED5668DD");

        protected override void CorePrepare(Setup setup) => setup.AddDependency(this.Meta.ObjectType, M.Singleton.ObjectType);

        protected override void CoreSetup(Setup setup)
        {
            var singleton = this.Session.GetSingleton();

            var guest = new AutomatedAgentBuilder(this.Session).WithUniqueId(GuestId).WithUserName("Guest").Build();
            singleton.Guest = guest;

            var scheduler = new AutomatedAgentBuilder(this.Session).WithUniqueId(SchedulerId).WithUserName("Scheduler").Build();
            singleton.Scheduler = scheduler;

            if (setup.Config.SetupSecurity)
            {
                singleton.InitialSecurityToken = new SecurityTokenBuilder(this.Session).Build();
                singleton.DefaultSecurityToken = new SecurityTokenBuilder(this.Session).Build();

                // Initial: Guest Creator
                var guestCreatorsAccessControl = new AccessControlBuilder(this.Session)
                    .WithRole(new Roles(this.Session).GuestCreator)
                    .WithSubject(singleton.Guest)
                    .Build();

                singleton.InitialSecurityToken.AddAccessControl(guestCreatorsAccessControl);

                // Default: Guest
                var guestAccessControl = new AccessControlBuilder(this.Session)
                    .WithRole(new Roles(this.Session).Guest)
                    .WithSubject(singleton.Guest)
                    .Build();

                singleton.DefaultSecurityToken.AddAccessControl(guestAccessControl);
            }

        }
    }
}
