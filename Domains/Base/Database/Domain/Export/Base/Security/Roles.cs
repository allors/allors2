//-------------------------------------------------------------------------------------------------
// <copyright file="roles.cs" company="Allors bvba">
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

    public partial class Roles
    {
        // Horizontal roles
        public static readonly Guid AdministratorId = new Guid("9D162C26-15B2-428e-AB80-DB4B3EBDBB7A");
        public static readonly Guid GuestId = new Guid("86445257-3F62-41e0-8B4A-2DF9FB18A8AA");
        public static readonly Guid ProductQuoteApproverId = new Guid("07D39583-C82C-4EA0-92F1-288FB8E17FA3");
        public static readonly Guid BlueCollarWorkerId = new Guid("3C2D223E-6056-447A-A3F9-AED2413D717D");

        // DAC
        public static readonly Guid CreatorId = new Guid("3A3D1E25-4A91-4D07-8203-A9F3EA691598");
        public static readonly Guid OwnerId = new Guid("E22EA50F-E616-4429-92D5-B91684AD3C2A");

        private UniquelyIdentifiableSticky<Role> sticky;

        public Role Administrator => this.Sticky[AdministratorId];

        public Role Guest => this.Sticky[GuestId];

        public Role ProductQuoteApprover => this.Sticky[ProductQuoteApproverId];

        public Role BlueCollarWorker => this.Sticky[BlueCollarWorkerId];

        public Role Creator => this.Sticky[CreatorId];

        public Role Owner => this.Sticky[OwnerId];

        private UniquelyIdentifiableSticky<Role> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<Role>(this.Session));

        protected override void BaseSetup(Setup config)
        {
            base.BaseSetup(config);

            // Horizontal Roles
            new RoleBuilder(this.Session).WithName("Administrator").WithUniqueId(AdministratorId).Build();
            new RoleBuilder(this.Session).WithName("Guest").WithUniqueId(GuestId).Build();
            new RoleBuilder(this.Session).WithName("ProductQuote approver").WithUniqueId(ProductQuoteApproverId).Build();
            new RoleBuilder(this.Session).WithName("Blue-collar worker").WithUniqueId(BlueCollarWorkerId).Build();

            // DAC emulation
            new RoleBuilder(this.Session).WithName("Creator").WithUniqueId(CreatorId).Build();
            new RoleBuilder(this.Session).WithName("Owner").WithUniqueId(OwnerId).Build();
        }

        protected override void BaseSecure(Security config)
        {
            var full = new[] { Domain.Operations.Read, Domain.Operations.Write, Domain.Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}