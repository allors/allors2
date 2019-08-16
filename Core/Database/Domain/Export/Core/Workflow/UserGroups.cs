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
        public static readonly Guid EmployeesId = new Guid("ED2D31E3-E18E-4C08-9AF3-F9D849D0F6B2");
        public static readonly Guid SalesAccountManagersId = new Guid("449EA7CE-124B-4E19-AFDF-46CAFB8D7B20");

        private UniquelyIdentifiableSticky<UserGroup> sticky;

        public UserGroup Administrators => this.Sticky[AdministratorsId];

        public UserGroup Guests => this.Sticky[GuestsId];

        public UserGroup Creators => this.Sticky[CreatorsId];

        public UserGroup Employees => this.Sticky[EmployeesId];

        public UserGroup SalesAccountManagers => this.Sticky[SalesAccountManagersId];

        private UniquelyIdentifiableSticky<UserGroup> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<UserGroup>(this.Session));

        protected override void CoreSetup(Setup setup)
        {
            // Default Groups
            new UserGroupBuilder(this.Session).WithName("Guests").WithUniqueId(GuestsId).Build();
            new UserGroupBuilder(this.Session).WithName("Administrators").WithUniqueId(AdministratorsId).Build();
            new UserGroupBuilder(this.Session).WithName("Creators").WithUniqueId(CreatorsId).Build();
            new UserGroupBuilder(this.Session).WithName("Employees").WithUniqueId(EmployeesId).Build();
            new UserGroupBuilder(this.Session).WithName("Sales AccountManagers").WithUniqueId(SalesAccountManagersId).Build();
        }

        protected override void CoreSecure(Security config)
        {
            var full = new[] { Domain.Operations.Read, Domain.Operations.Write, Domain.Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
