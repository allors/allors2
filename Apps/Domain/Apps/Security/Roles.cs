//------------------------------------------------------------------------------------------------- 
// <copyright file="roles.cs" company="Allors bvba">
// Copyright 2002-2012 Allors bvba.
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

    public partial class Roles
    {
        public static readonly Guid CustomerRoleId = new Guid("C2B2AC0D-CF9F-4fc8-A845-D78203346663");
        public static readonly Guid SupplierRoleId = new Guid("E6A319B9-3E48-488a-9DEC-AD54FAA364D7");
        public static readonly Guid PartnerRoleId = new Guid("F3F6E8F6-94FB-42db-9F4A-CA33D8A88C97");

        public static readonly Guid OperationsId = new Guid("387E5E5A-727F-4098-9FDC-3431C258E1AA");
        public static readonly Guid ProcurementId = new Guid("ACB4E8EE-61CC-48AA-BB0A-75B279A03049");
        public static readonly Guid SalesId = new Guid("052F86E8-D40D-43CC-9555-9C3107500116");

        public Role Customer
        {
            get { return this.RoleCache.Get(CustomerRoleId); }
        }

        public Role Supplier
        {
            get { return this.RoleCache.Get(SupplierRoleId); }
        }

        public Role Partner
        {
            get { return this.RoleCache.Get(PartnerRoleId); }
        }

        public Role Operations
        {
            get { return this.RoleCache.Get(OperationsId); }
        }

        public Role Procurement
        {
            get { return this.RoleCache.Get(ProcurementId); }
        }

        public Role Sales
        {
            get { return this.RoleCache.Get(SalesId); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new RoleBuilder(Session).WithName("Customer").WithUniqueId(CustomerRoleId).Build();
            new RoleBuilder(Session).WithName("Supplier").WithUniqueId(SupplierRoleId).Build();
            new RoleBuilder(Session).WithName("Partner").WithUniqueId(PartnerRoleId).Build();
            
            new RoleBuilder(Session).WithName("Operations").WithUniqueId(OperationsId).Build();
            new RoleBuilder(Session).WithName("Procurement").WithUniqueId(ProcurementId).Build();
            new RoleBuilder(Session).WithName("Sales").WithUniqueId(SalesId).Build();

            new SecurityCache(this.Session).Invalidate();
        }
    }
}
