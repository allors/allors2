//------------------------------------------------------------------------------------------------- 
// <copyright file="roles.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
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
    using global::System;

    public partial class Roles
    {
        public static readonly Guid OperationsId = new Guid("387E5E5A-727F-4098-9FDC-3431C258E1AA");
        public static readonly Guid ProcurementId = new Guid("ACB4E8EE-61CC-48AA-BB0A-75B279A03049");
        public static readonly Guid SalesId = new Guid("052F86E8-D40D-43CC-9555-9C3107500116");

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

        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            new RoleBuilder(Session).WithName("Operations").WithUniqueId(OperationsId).Build();
            new RoleBuilder(Session).WithName("Procurement").WithUniqueId(ProcurementId).Build();
            new RoleBuilder(Session).WithName("Sales").WithUniqueId(SalesId).Build();
        }
    }
}
