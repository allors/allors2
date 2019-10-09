// <copyright file="UserGroups.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Domain
{
    using System;
    using Allors;

    public partial class UserGroups
    {
        public static readonly Guid ManagersId = new Guid("02011FEF-D342-44AE-A001-6AC1D3670A6E");

        public UserGroup Managers => this.Sticky[ManagersId];

        protected override void BaseSetup(Setup setup)
        {
            new UserGroupBuilder(this.Session).WithName("Managers").WithUniqueId(ManagersId).Build();
        }
    }
}
