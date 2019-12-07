// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Allors.Meta;

    public partial class AccessControls
    {
        public static readonly Guid CreatorsId = new Guid("1ED18B97-44F1-4AE7-A8DD-C7DA0BAE21E4");
        public static readonly Guid GuestCreatorsId = new Guid("16AC80CE-FC54-408D-AD33-DDAD249B82E4");

        public static readonly Guid AdministratorId = new Guid("282C4874-10EC-437B-9B0D-FAADFDFEC63E");
        public static readonly Guid GuestId = new Guid("07AED92A-84E7-4DA6-96A3-C764093D2A58");

        private UniquelyIdentifiableSticky<AccessControl> cache;

        public UniquelyIdentifiableSticky<AccessControl> Cache => this.cache ??= new UniquelyIdentifiableSticky<AccessControl>(this.Session);

        public AccessControl Creators => this.Cache[CreatorsId];

        public AccessControl GuestCreator => this.Cache[GuestCreatorsId];

        public AccessControl Administrator => this.Cache[AdministratorId];

        public AccessControl Guest => this.Cache[GuestId];

        protected override void CorePrepare(Setup setup)
        {
            base.CorePrepare(setup);

            setup.AddDependency(this.ObjectType, M.Role.ObjectType);
            setup.AddDependency(this.ObjectType, M.UserGroup.ObjectType);
        }

        protected override void CoreSetup(Setup setup)
        {
            if (setup.Config.SetupSecurity)
            {
                var merge = this.Cache.Merger().Action();

                var roles = new Roles(this.Session);
                var userGroups = new UserGroups(this.Session);

                merge(CreatorsId, v =>
                {
                    v.Role = roles.Creator;
                    v.AddSubjectGroup(userGroups.Creators);
                });

                merge(GuestCreatorsId, v =>
                {
                    v.Role = roles.GuestCreator;
                    v.AddSubjectGroup(userGroups.Guests);
                });

                merge(AdministratorId, v =>
                {
                    v.Role = roles.Administrator;
                    v.AddSubjectGroup(userGroups.Administrators);
                });

                merge(GuestId, v =>
                {
                    v.Role = roles.Guest;
                    v.AddSubjectGroup(userGroups.Guests);
                });
            }
        }
    }
}
