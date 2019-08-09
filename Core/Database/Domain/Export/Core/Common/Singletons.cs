// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Singletons.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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