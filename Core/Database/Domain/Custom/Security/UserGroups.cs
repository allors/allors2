// <copyright file="UserGroups.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Domain
{
    using System;

    public partial class UserGroups
    {
        public static readonly Guid OperationsId = new Guid("4EA028A4-57C6-46A1-AC4B-E18204F9B498");
        public static readonly Guid SalesId = new Guid("1511E4E2-829F-4133-8824-B94ED46E6BED");
        public static readonly Guid ProcurementId = new Guid("FF887B58-CDA3-4C76-8308-0F005E362E0E");

        public UserGroup Operations => this.Cache[OperationsId];

        public UserGroup Sales => this.Cache[SalesId];

        public UserGroup Procurement => this.Cache[ProcurementId];

        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            new UserGroupBuilder(this.Session).WithName("operations").WithUniqueId(OperationsId).Build();
            new UserGroupBuilder(this.Session).WithName("sales").WithUniqueId(SalesId).Build();
            new UserGroupBuilder(this.Session).WithName("procurement").WithUniqueId(ProcurementId).Build();
        }
    }
}
