//-------------------------------------------------------------------------------------------------
// <copyright file="UserGroups.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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
// <summary>Defines the role type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    using Allors;

    public partial class UserGroups
    {
        public static readonly Guid GuestsId = new Guid("1B022AA5-1B73-486A-9386-81D6EBFF2A4B");
        public static readonly Guid AdministratorsId = new Guid("CDC04209-683B-429C-BED2-440851F430DF");

        public static readonly Guid CreatorsId = new Guid("F0D8132B-79D6-4A30-A866-EF6E5C952761");

        private UniquelyIdentifiableCache<UserGroup> cache;

        public UserGroup Administrators => this.Cache.Get(AdministratorsId);

        public UserGroup Guests => this.Cache.Get(GuestsId);

        public UserGroup Creators => this.Cache.Get(CreatorsId);

        private UniquelyIdentifiableCache<UserGroup> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableCache<UserGroup>(this.Session));

        protected override void BaseSetup(Setup config)
        {
            base.BaseSetup(config);

            // Default Groups
            new UserGroupBuilder(this.Session).WithName("Guests").WithUniqueId(GuestsId).Build();
            new UserGroupBuilder(this.Session).WithName("Administrators").WithUniqueId(AdministratorsId).Build();

            new UserGroupBuilder(this.Session).WithName("Creators").WithUniqueId(CreatorsId).Build();
        }

        protected override void BaseSecure(Security config)
        {
            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}