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
    using System;
    using System.Collections.Generic;

    using Allors.Meta;

    public partial class People
    {
        public static readonly IEnumerable<Person> EmptyList = new Person[0];

        public static readonly Guid AdministratorId = new Guid("FF791BA1-6E02-4F64-83A3-E6BEE1208C11");
        public static readonly Guid GuestId = new Guid("1261CB56-67F2-4725-AF7D-604A117ABBEC");

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

            var administrator = new PersonBuilder(this.Session).WithUniqueId(People.AdministratorId).WithUserName(Users.AdministratorUserName).Build();
            userGroups.Administrators.AddMember(administrator);
            userGroups.Creators.AddMember(administrator);

            var guest = new PersonBuilder(this.Session).WithUniqueId(People.GuestId).WithUserName(Users.GuestUserName).Build();
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