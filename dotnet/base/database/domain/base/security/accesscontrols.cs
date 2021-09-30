// <copyright file="Roles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class AccessControls
    {
        public static readonly Guid EmployeesId = new Guid("C1D5C7A3-673E-41FD-BF5D-94438307A7E3");

        public AccessControl Employees => this.Cache[EmployeesId];

        protected override void BaseSetup(Setup setup)
        {
            if (setup.Config.SetupSecurity)
            {
                var merge = this.Cache.Merger().Action();

                var roles = new Roles(this.Session);
                var userGroups = new UserGroups(this.Session);

                merge(EmployeesId, v =>
                {
                    v.Role = roles.Employee;
                    v.AddSubjectGroup(userGroups.Employees);
                });
            }
        }
    }
}
