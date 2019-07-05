namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Singletons
    {
        protected override void AppsPrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.Locale.ObjectType);
        }

        protected override void AppsSetup(Setup setup)
        {
            var singleton = this.Instance;
            singleton.DefaultLocale = new Locales(this.Session).EnglishGreatBritain;

            singleton.EmployeeUserGroup = new UserGroups(this.Session).Employees;
            singleton.SalesAccountManagerUserGroup = new UserGroups(this.Session).SalesAccountManagers;

            if (setup.Config.SetupSecurity)
            {
                var employeeRole = new Roles(this.Session).Employee;

                singleton.EmployeeAccessControl = new AccessControlBuilder(this.Session)
                    .WithSubjectGroup(singleton.EmployeeUserGroup)
                    .WithRole(employeeRole)
                    .Build();

                singleton.DefaultSecurityToken.AddAccessControl(singleton.EmployeeAccessControl);

                var salesAccountManagerRole = new Roles(this.Session).SalesAccountManager;

                singleton.SalesAccountManagerAccessControl = new AccessControlBuilder(this.Session)
                    .WithSubjectGroup(singleton.SalesAccountManagerUserGroup)
                    .WithRole(salesAccountManagerRole)
                    .Build();

                singleton.DefaultSecurityToken.AddAccessControl(singleton.SalesAccountManagerAccessControl);
            }
        }
    }
}