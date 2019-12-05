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
        public static readonly Guid EmployeesId = new Guid("ED2D31E3-E18E-4C08-9AF3-F9D849D0F6B2");
        public static readonly Guid ManagersId = new Guid("02011FEF-D342-44AE-A001-6AC1D3670A6E");
        public static readonly Guid SalesAccountManagersId = new Guid("449EA7CE-124B-4E19-AFDF-46CAFB8D7B20");

        public UserGroup Employees => this.Cache[EmployeesId];

        public UserGroup Managers => this.Cache[ManagersId];

        public UserGroup SalesAccountManagers => this.Cache[SalesAccountManagersId];
        
        protected override void BaseSetup(Setup setup)
        {
            var merge = this.cache.Merger().Action();

            merge(EmployeesId, v => v.Name = "Employees");
            merge(ManagersId, v => v.Name = "Managers");
            merge(SalesAccountManagersId, v => v.Name = "Sales AccountManagers");
        }
    }
}
