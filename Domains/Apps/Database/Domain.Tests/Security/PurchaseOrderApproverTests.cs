using Allors.Meta;

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

        [Fact]
        public void PurchaseOrder_Approve()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive();

            var approver = new PersonBuilder(this.Session)
                .WithUserName("approver")
                .WithFirstName("productquote")
                .WithLastName("approver")
                .Build();

            organisation.AddPurchaseOrderApprover(approver);

            this.Session.Derive();

            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithInternalOrganisation(organisation).WithSupplier(supplier).WithNeedsApproval(true).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            ContactMechanism takenViaContactMechanism = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var supplierContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(takenViaContactMechanism)
                .WithUseAsDefault(true)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .Build();
            supplier.AddPartyContactMechanism(supplierContactMechanism);

            this.Session.Derive();

            var order = new PurchaseOrderBuilder(this.Session).WithOrderedBy(organisation).WithTakenViaSupplier(supplier).WithBillToContactMechanism(takenViaContactMechanism).Build();

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            this.SetIdentity("approver");

            var acl = new AccessControlList(order, approver);
            Assert.True(acl.CanExecute(M.PurchaseOrder.Approve));
        }

        [Fact]
        public void PurchaseOrderApproval()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive();

            var approver = new PersonBuilder(this.Session)
                .WithUserName("approver")
                .WithFirstName("productquote")
                .WithLastName("approver")
                .Build();

            organisation.AddPurchaseOrderApprover(approver);

            this.Session.Derive();

            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithInternalOrganisation(organisation).WithSupplier(supplier).WithNeedsApproval(true).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            ContactMechanism takenViaContactMechanism = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var supplierContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(takenViaContactMechanism)
                .WithUseAsDefault(true)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .Build();
            supplier.AddPartyContactMechanism(supplierContactMechanism);

            this.Session.Derive();

            var order = new PurchaseOrderBuilder(this.Session).WithOrderedBy(organisation).WithTakenViaSupplier(supplier).WithBillToContactMechanism(takenViaContactMechanism).Build();

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            this.SetIdentity("approver");

            var acl = new AccessControlList(order.PurchaseOrderApprovalsWherePurchaseOrder.First, approver);
            Assert.True(acl.CanWrite(M.ApproveTask.Comment));
            Assert.True(acl.CanExecute(M.ApproveTask.Approve));
        }
    }
}