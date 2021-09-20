// <copyright file="Roles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Domain
{
    using System;
    using Allors;

    public partial class Roles
    {
        public static readonly Guid AdministratorId = new Guid("9D162C26-15B2-428e-AB80-DB4B3EBDBB7A");

        public static readonly Guid GuestId = new Guid("86445257-3F62-41e0-8B4A-2DF9FB18A8AA");

        public static readonly Guid GuestCreatorId = new Guid("5281A811-81A2-4381-94BB-90B1AED0CC40");

        public static readonly Guid CreatorId = new Guid("3A3D1E25-4A91-4D07-8203-A9F3EA691598");

        public static readonly Guid OwnerId = new Guid("E22EA50F-E616-4429-92D5-B91684AD3C2A");

        private UniquelyIdentifiableSticky<Role> cache;

        public Role Administrator => this.Cache[AdministratorId];

        public Role Guest => this.Cache[GuestId];

        public Role GuestCreator => this.Cache[GuestCreatorId];

        public Role Creator => this.Cache[CreatorId];

        public Role Owner => this.Cache[OwnerId];

        private UniquelyIdentifiableSticky<Role> Cache => this.cache ??= new UniquelyIdentifiableSticky<Role>(this.Session);

        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(AdministratorId, v => v.Name = "Administrator");
            merge(GuestId, v => v.Name = "Guest");
            merge(GuestCreatorId, v => v.Name = "GuestCreator");
            merge(CreatorId, v => v.Name = "Creator");
            merge(OwnerId, v => v.Name = "Owner");
        }
    }
}
