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
        // Horizontal roles
        public static readonly Guid AdministratorId = new Guid("9D162C26-15B2-428e-AB80-DB4B3EBDBB7A");
        public static readonly Guid GuestId = new Guid("86445257-3F62-41e0-8B4A-2DF9FB18A8AA");

        // DAC
        public static readonly Guid CreatorId = new Guid("3A3D1E25-4A91-4D07-8203-A9F3EA691598");
        public static readonly Guid OwnerId = new Guid("E22EA50F-E616-4429-92D5-B91684AD3C2A");

        private UniquelyIdentifiableSticky<Role> sticky;

        public Role Administrator => this.Sticky[AdministratorId];

        public Role Guest => this.Sticky[GuestId];

        public Role Creator => this.Sticky[CreatorId];

        public Role Owner => this.Sticky[OwnerId];

        private UniquelyIdentifiableSticky<Role> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<Role>(this.Session));

        protected override void CoreSetup(Setup setup)
        {
            // Horizontal Roles
            new RoleBuilder(this.Session).WithName("Administrator").WithUniqueId(AdministratorId).Build();
            new RoleBuilder(this.Session).WithName("Guest").WithUniqueId(GuestId).Build();

            // DAC emulation
            new RoleBuilder(this.Session).WithName("Creator").WithUniqueId(CreatorId).Build();
            new RoleBuilder(this.Session).WithName("Owner").WithUniqueId(OwnerId).Build();
        }

        protected override void CoreSecure(Security config)
        {
            var full = new[] { Domain.Operations.Read, Domain.Operations.Write, Domain.Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
