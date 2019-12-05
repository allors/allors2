// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Singletons
    {
        public Singleton Instance => this.Session.GetSingleton();

        protected override void CorePrepare(Setup setup)
        {
            base.CorePrepare(setup);

            setup.AddDependency(this.ObjectType, M.SecurityToken.ObjectType);
            setup.AddDependency(this.ObjectType, M.Locale.ObjectType);
            setup.AddDependency(this.ObjectType, M.Role.ObjectType);
            setup.AddDependency(this.ObjectType, M.UserGroup.ObjectType);
            setup.AddDependency(this.ObjectType, M.AutomatedAgent.ObjectType);
        }

        protected override void CoreSetup(Setup setup)
        {
            var singleton = new SingletonBuilder(this.Session).Build();

            singleton.DefaultLocale = new Locales(this.Session).EnglishGreatBritain;

            var automatedAgents = new AutomatedAgents(this.Session);
            singleton.Guest = automatedAgents.Guest;
            singleton.Scheduler = automatedAgents.Scheduler;

            var securityTokens = new SecurityTokens(this.Session);

            if (setup.Config.SetupSecurity)
            {
                var roles = new Roles(this.Session);
                var userGroups = new UserGroups(this.Session);

                // Initial => Creator
                var creators = singleton.CreatorsAccessControl ??= new AccessControlBuilder(this.Session).Build();
                creators.Role = roles.Creator;
                creators.AddSubjectGroup(userGroups.Creators);

                securityTokens.InitialSecurityToken.AddAccessControl(creators);

                // Initial => Guest Creator
                var guestCreators = singleton.GuestCreatorsAccessControl ??= new AccessControlBuilder(this.Session).Build();
                guestCreators.Role = roles.GuestCreator;
                guestCreators.AddSubject(singleton.Guest);

                securityTokens.InitialSecurityToken.AddAccessControl(guestCreators);

                // Default => Administrator
                var administrators = singleton.AdministratorsAccessControl ??= new AccessControlBuilder(this.Session).Build();
                administrators.Role = roles.Administrator;
                administrators.AddSubjectGroup(userGroups.Administrators);

                securityTokens.DefaultSecurityToken.AddAccessControl(administrators);

                // Default => Guest
                var guest = singleton.GuestAccessControl ??= new AccessControlBuilder(this.Session).Build();
                guest.Role = roles.Guest;
                guest.AddSubject(singleton.Guest);

                securityTokens.DefaultSecurityToken.AddAccessControl(guest);
            }
        }
    }
}
