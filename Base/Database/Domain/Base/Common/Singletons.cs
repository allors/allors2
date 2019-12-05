// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Singletons
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.Locale.ObjectType);

        protected override void BaseSetup(Setup setup)
        {
            var singleton = this.Instance;
            singleton.DefaultLocale = new Locales(this.Session).EnglishGreatBritain;

            singleton.EmployeeUserGroup = new UserGroups(this.Session).Employees;

            if (setup.Config.SetupSecurity)
            {
                var employeeRole = new Roles(this.Session).Employee;

                singleton.EmployeeAccessControl = new AccessControlBuilder(this.Session)
                    .WithSubjectGroup(singleton.EmployeeUserGroup)
                    .WithRole(employeeRole)
                    .Build();

                new SecurityTokens(this.Session).DefaultSecurityToken.AddAccessControl(singleton.EmployeeAccessControl);
            }
        }
    }
}
