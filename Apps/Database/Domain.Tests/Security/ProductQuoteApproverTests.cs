namespace Allors.Domain
{
    using Allors;
    using Allors.Domain;
    using Xunit;
   
    public class ProductQuoteApproverTests : DomainTest
    {
        [Fact]
        public void UserGroup()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistProductQuoteApproverUserGroup);

            organisation.RemoveProductQuoteApproverUserGroup();
            this.Session.Derive();

            Assert.True(organisation.ExistProductQuoteApproverUserGroup);
        }

        [Fact]
        public void AccessControl()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistProductQuoteApproverAccessControl);
            Assert.Equal(new Roles(this.Session).ProductQuoteApprover, organisation.ProductQuoteApproverAccessControl.Role);
            Assert.Contains(organisation.ProductQuoteApproverUserGroup, organisation.ProductQuoteApproverAccessControl.SubjectGroups);

            organisation.RemoveProductQuoteApproverAccessControl();

            this.Session.Derive(true);

            Assert.True(organisation.ExistProductQuoteApproverAccessControl);
            Assert.Equal(new Roles(this.Session).ProductQuoteApprover, organisation.ProductQuoteApproverAccessControl.Role);
            Assert.Contains(organisation.ProductQuoteApproverUserGroup, organisation.ProductQuoteApproverAccessControl.SubjectGroups);
        }

        [Fact]
        public void SecurityToken()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistProductQuoteApproverSecurityToken);
            Assert.Contains(organisation.ProductQuoteApproverAccessControl, organisation.ProductQuoteApproverSecurityToken.AccessControls);
        }

        [Fact]
        public void ProductQuoteApprovers()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            var approver = new PersonBuilder(this.Session)
                .WithUserName("approver")
                .WithFirstName("productquote")
                .WithLastName("approver")
                .Build();

            organisation.AddProductQuoteApprover(approver);

            this.Session.Derive(true);

            Assert.Contains(approver, organisation.ProductQuoteApproverUserGroup.Members);
        }
    }
}