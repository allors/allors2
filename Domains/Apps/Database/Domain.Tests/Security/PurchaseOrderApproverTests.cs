namespace Domain
{
    using Allors;
    using Allors.Domain;
    using Xunit;
   
    public class PurchaseOrderApproverTests : DomainTest
    {
        [Fact]
        public void UserGroup()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistPurchaseOrderApproverUserGroup);

            organisation.RemovePurchaseOrderApproverUserGroup();
            this.Session.Derive();

            Assert.True(organisation.ExistPurchaseOrderApproverUserGroup);
        }

        [Fact]
        public void AccessControl()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistPurchaseOrderApproverAccessControl);
            Assert.Equal(new Roles(this.Session).PurchaseOrderApprover, organisation.PurchaseOrderApproverAccessControl.Role);
            Assert.Contains(organisation.PurchaseOrderApproverUserGroup, organisation.PurchaseOrderApproverAccessControl.SubjectGroups);

            organisation.RemovePurchaseOrderApproverAccessControl();

            this.Session.Derive(true);

            Assert.True(organisation.ExistPurchaseOrderApproverAccessControl);
            Assert.Equal(new Roles(this.Session).PurchaseOrderApprover, organisation.PurchaseOrderApproverAccessControl.Role);
            Assert.Contains(organisation.PurchaseOrderApproverUserGroup, organisation.PurchaseOrderApproverAccessControl.SubjectGroups);
        }

        [Fact]
        public void SecurityToken()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistPurchaseOrderApproverSecurityToken);
            Assert.Contains(organisation.PurchaseOrderApproverAccessControl, organisation.PurchaseOrderApproverSecurityToken.AccessControls);
        }

        [Fact]
        public void PurchaseOrderApprovers()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            var approver = new PersonBuilder(this.Session)
                .WithUserName("approver")
                .WithFirstName("productquote")
                .WithLastName("approver")
                .Build();

            organisation.AddPurchaseOrderApprover(approver);

            this.Session.Derive(true);

            Assert.Contains(approver, organisation.PurchaseOrderApproverUserGroup.Members);
        }
    }
}