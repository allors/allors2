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

            setup.AddDependency(this.ObjectType, M.Locale.ObjectType);
            setup.AddDependency(this.ObjectType, M.Role.ObjectType);
            setup.AddDependency(this.ObjectType, M.UserGroup.ObjectType);
        }

        protected override void CoreSetup(Setup setup)
        {
            var singleton = new SingletonBuilder(this.Session).Build();

            singleton.DefaultLocale = new Locales(this.Session).EnglishGreatBritain;

            if (setup.Config.SetupSecurity)
            {
                singleton.InitialSecurityToken = new SecurityTokenBuilder(this.Session).Build();
                singleton.DefaultSecurityToken = new SecurityTokenBuilder(this.Session).Build();

                // Initial
                singleton.CreatorsAccessControl = new AccessControlBuilder(this.Session)
                    .WithRole(new Roles(this.Session).Creator)
                    .WithSubjectGroup(new UserGroups(this.Session).Creators)
                    .Build();

                singleton.InitialSecurityToken.AddAccessControl(singleton.CreatorsAccessControl);

                // Administrator
                singleton.AdministratorsAccessControl = new AccessControlBuilder(this.Session)
                    .WithRole(new Roles(this.Session).Administrator)
                    .WithSubjectGroup(new UserGroups(this.Session).Administrators)
                    .Build();

                singleton.DefaultSecurityToken.AddAccessControl(singleton.AdministratorsAccessControl);

                // Guest
                singleton.GuestAccessControl = new AccessControlBuilder(this.Session)
                    .WithRole(new Roles(this.Session).Guest)
                    .WithSubjectGroup(new UserGroups(this.Session).Guests)
                    .Build();

                singleton.DefaultSecurityToken.AddAccessControl(singleton.GuestAccessControl);
            }
        }
    }
}
