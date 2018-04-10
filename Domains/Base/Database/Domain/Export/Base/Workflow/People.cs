// --------------------------------------------------------------------------------------------------------------------
// <copyright file="People.cs" company="Allors bvba">
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

    public partial class People
    {
        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.Meta.ObjectType, M.Singleton.ObjectType);
            setup.AddDependency(this.Meta.ObjectType, M.UserGroup.ObjectType);
            setup.AddDependency(this.Meta.ObjectType, M.Locale.ObjectType);
        }

        protected override void BaseSetup(Setup config)
        {
            base.BaseSetup(config);

            var userGroups = new UserGroups(this.Session);

            var locale = new Locales(this.Session).DutchBelgium;
            var administrator = new PersonBuilder(this.Session).WithUserName(Users.AdministratorUserName).Build();
            userGroups.Administrators.AddMember(administrator);
            userGroups.Creators.AddMember(administrator);

            var guest = new PersonBuilder(this.Session).WithUserName(Users.GuestUserName).Build();
            userGroups.Guests.AddMember(guest);

            var singleton = this.Session.GetSingleton();
            singleton.Guest = guest;
        }

        protected override void BaseSecure(Security config)
        {
            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);

            config.GrantCreator(this.ObjectType, full);
        }
    }
}