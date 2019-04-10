namespace Domain
{
    using Allors;
    using Allors.Domain;
    using Xunit;
   
    public class BlueCollarWorkerTests : DomainTest
    {
        [Fact]
        public void UserGroup()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistBlueCollarWorkerUserGroup);

            organisation.RemoveBlueCollarWorkerUserGroup();
            this.Session.Derive();

            Assert.True(organisation.ExistBlueCollarWorkerUserGroup);
        }

        [Fact]
        public void AccessControl()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistBlueCollarWorkerAccessControl);
            Assert.Equal(new Roles(this.Session).BlueCollarWorker, organisation.BlueCollarWorkerAccessControl.Role);
            Assert.Contains(organisation.BlueCollarWorkerUserGroup, organisation.BlueCollarWorkerAccessControl.SubjectGroups);

            organisation.RemoveBlueCollarWorkerAccessControl();

            this.Session.Derive(true);

            Assert.True(organisation.ExistBlueCollarWorkerAccessControl);
            Assert.Equal(new Roles(this.Session).BlueCollarWorker, organisation.BlueCollarWorkerAccessControl.Role);
            Assert.Contains(organisation.BlueCollarWorkerUserGroup, organisation.BlueCollarWorkerAccessControl.SubjectGroups);
        }

        [Fact]
        public void SecurityToken()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistBlueCollarWorkerSecurityToken);
            Assert.Contains(organisation.BlueCollarWorkerAccessControl, organisation.BlueCollarWorkerSecurityToken.AccessControls);
        }

        [Fact]
        public void BlueCollarWorkers()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            var worker = new PersonBuilder(this.Session)
                .WithUserName("worker")
                .WithFirstName("blue-collar")
                .WithLastName("worker")
                .Build();

            organisation.AddBlueCollarWorker(worker);

            this.Session.Derive(true);

            Assert.Contains(worker, organisation.BlueCollarWorkerUserGroup.Members);
        }
    }
}