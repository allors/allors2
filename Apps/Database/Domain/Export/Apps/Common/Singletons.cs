namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Singletons
    {
        protected override void AppsPrepare(Setup config)
        {
            config.AddDependency(this.ObjectType, M.Locale.ObjectType);
        }

        protected override void AppsSetup(Setup setup)
        {
            var singleton = this.Instance;
            singleton.DefaultLocale = new Locales(this.Session).EnglishGreatBritain;

            var employeeRole = new Roles(this.Session).Employee;

            singleton.EmployeeUserGroup = new UserGroups(this.Session).Employees;
            singleton.EmployeeAccessControl = new AccessControlBuilder(this.Session)
                .WithSubjectGroup(singleton.EmployeeUserGroup)
                .WithRole(employeeRole)
                .Build();

            singleton.DefaultSecurityToken.AddAccessControl(singleton.EmployeeAccessControl);

            var salesAccountManagerRole = new Roles(this.Session).SalesAccountManager;

            singleton.SalesAccountManagerUserGroup = new UserGroups(this.Session).SalesAccountManagers;
            singleton.SalesAccountManagerAccessControl = new AccessControlBuilder(this.Session)
                .WithSubjectGroup(singleton.SalesAccountManagerUserGroup)
                .WithRole(salesAccountManagerRole)
                .Build();

            singleton.DefaultSecurityToken.AddAccessControl(singleton.SalesAccountManagerAccessControl);
        }
    }
}
