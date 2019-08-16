//-------------------------------------------------------------------------------------------------
// <copyright file="Roles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class Roles
    {
        public static readonly Guid OperationsId = new Guid("387E5E5A-727F-4098-9FDC-3431C258E1AA");
        public static readonly Guid ProcurementId = new Guid("ACB4E8EE-61CC-48AA-BB0A-75B279A03049");
        public static readonly Guid SalesId = new Guid("052F86E8-D40D-43CC-9555-9C3107500116");

        public Role Operations => this.Sticky[OperationsId];

        public Role Procurement => this.Sticky[ProcurementId];

        public Role Sales => this.Sticky[SalesId];

        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            new RoleBuilder(this.Session).WithName("Operations").WithUniqueId(OperationsId).Build();
            new RoleBuilder(this.Session).WithName("Procurement").WithUniqueId(ProcurementId).Build();
            new RoleBuilder(this.Session).WithName("Sales").WithUniqueId(SalesId).Build();
        }
    }
}
