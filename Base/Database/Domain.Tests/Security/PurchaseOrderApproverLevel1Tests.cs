using Allors.Meta;

namespace Allors.Domain
{
    using Allors;
    using Allors.Domain;
    using Xunit;

    public class PurchaseOrderApproverLevel1Tests : DomainTest
    {
        [Fact]
        public void UserGroup()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistPurchaseOrderApproverLevel1UserGroup);

            organisation.RemovePurchaseOrderApproverLevel1UserGroup();
            this.Session.Derive();

            Assert.True(organisation.ExistPurchaseOrderApproverLevel1UserGroup);
        }

        [Fact]
        public void AccessControl()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistPurchaseOrderApproverLevel1AccessControl);
            Assert.Equal(new Roles(this.Session).PurchaseOrderApproverLevel1, organisation.PurchaseOrderApproverLevel1AccessControl.Role);
            Assert.Contains(organisation.PurchaseOrderApproverLevel1UserGroup, organisation.PurchaseOrderApproverLevel1AccessControl.SubjectGroups);

            organisation.RemovePurchaseOrderApproverLevel1AccessControl();

            this.Session.Derive(true);

            Assert.True(organisation.ExistPurchaseOrderApproverLevel1AccessControl);
            Assert.Equal(new Roles(this.Session).PurchaseOrderApproverLevel1, organisation.PurchaseOrderApproverLevel1AccessControl.Role);
            Assert.Contains(organisation.PurchaseOrderApproverLevel1UserGroup, organisation.PurchaseOrderApproverLevel1AccessControl.SubjectGroups);
        }

        [Fact]
        public void SecurityToken()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistPurchaseOrderApproverLevel1SecurityToken);
            Assert.Contains(organisation.PurchaseOrderApproverLevel1AccessControl, organisation.PurchaseOrderApproverLevel1SecurityToken.AccessControls);
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

            organisation.AddPurchaseOrderApproversLevel1(approver);

            this.Session.Derive(true);

            Assert.Contains(approver, organisation.PurchaseOrderApproverLevel1UserGroup.Members);
        }
    }

    public class PurchaseOrderApproverLevel1SecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void PurchaseOrder_Approve()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive();
            this.Session.Commit();

            var approver = new PersonBuilder(this.Session)
                .WithUserName("approver")
                .WithFirstName("purchaseorder")
                .WithLastName("approver")
                .Build();

            organisation.AddPurchaseOrderApproversLevel1(approver);

            this.Session.Derive();

            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithInternalOrganisation(organisation).WithSupplier(supplier).WithNeedsApproval(true).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            ContactMechanism takenViaContactMechanism = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var supplierContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(takenViaContactMechanism)
                .WithUseAsDefault(true)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .Build();
            supplier.AddPartyContactMechanism(supplierContactMechanism);

            this.Session.Derive();

            var order = new PurchaseOrderBuilder(this.Session)
                .WithOrderedBy(organisation)
                .WithTakenViaSupplier(supplier)
                .WithBillToContactMechanism(takenViaContactMechanism)
                .WithFacility(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse))
                .Build();

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();
            this.Session.Commit();

            Assert.True(order.PurchaseOrderState.IsAwaitingApprovalLevel1);

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
                .WithFirstName("purchaseorder")
                .WithLastName("approver")
                .Build();

            organisation.AddPurchaseOrderApproversLevel1(approver);

            this.Session.Derive();

            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithInternalOrganisation(organisation).WithSupplier(supplier).WithNeedsApproval(true).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            ContactMechanism takenViaContactMechanism = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var supplierContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(takenViaContactMechanism)
                .WithUseAsDefault(true)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .Build();
            supplier.AddPartyContactMechanism(supplierContactMechanism);

            this.Session.Derive();

            var order = new PurchaseOrderBuilder(this.Session)
                .WithOrderedBy(organisation)
                .WithTakenViaSupplier(supplier)
                .WithBillToContactMechanism(takenViaContactMechanism)
                .WithFacility(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse))
                .Build();

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            this.SetIdentity(approver.UserName);

            var purchaseOrderApproval = order.PurchaseOrderApprovalsLevel1WherePurchaseOrder.First;
            var acl = new AccessControlList(purchaseOrderApproval, approver);
            Assert.True(acl.CanWrite(M.ApproveTask.Comment));
            Assert.True(acl.CanExecute(M.ApproveTask.Approve));
        }
    }
}
