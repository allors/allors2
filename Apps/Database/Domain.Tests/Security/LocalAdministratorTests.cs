using System.Linq;
using Allors.Meta;

namespace Allors.Domain
{
    using Allors;
    using Allors.Domain;
    using Xunit;
   
    public class LocalAdministratorTests : DomainTest
    {
        [Fact]
        public void UserGroup()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistLocalAdministratorUserGroup);

            organisation.RemoveLocalAdministratorUserGroup();
            this.Session.Derive();

            Assert.True(organisation.ExistLocalAdministratorUserGroup);
        }

        [Fact]
        public void AccessControl()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistLocalAdministratorAccessControl);
            Assert.Equal(new Roles(this.Session).LocalAdministrator, organisation.LocalAdministratorAccessControl.Role);
            Assert.Contains(organisation.LocalAdministratorUserGroup, organisation.LocalAdministratorAccessControl.SubjectGroups);

            organisation.RemoveLocalAdministratorAccessControl();

            this.Session.Derive(true);

            Assert.True(organisation.ExistLocalAdministratorAccessControl);
            Assert.Equal(new Roles(this.Session).LocalAdministrator, organisation.LocalAdministratorAccessControl.Role);
            Assert.Contains(organisation.LocalAdministratorUserGroup, organisation.LocalAdministratorAccessControl.SubjectGroups);
        }

        [Fact]
        public void SecurityToken()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistLocalAdministratorSecurityToken);
            Assert.Contains(organisation.LocalAdministratorAccessControl, organisation.LocalAdministratorSecurityToken.AccessControls);
        }

        [Fact]
        public void LocalAdministrators()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            var worker = new PersonBuilder(this.Session)
                .WithUserName("worker")
                .WithFirstName("blue-collar")
                .WithLastName("worker")
                .Build();

            organisation.AddLocalAdministrator(worker);

            this.Session.Derive(true);

            Assert.Contains(worker, organisation.LocalAdministratorUserGroup.Members);
        }
    }
}