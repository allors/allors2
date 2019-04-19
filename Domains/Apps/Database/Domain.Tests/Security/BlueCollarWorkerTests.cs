using System.Linq;
using Allors.Meta;

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

        [Fact]
        public void WorkEffortInventoryAssignment()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var worker = new PersonBuilder(this.Session)
                .WithUserName("worker")
                .WithFirstName("blue-collar")
                .WithLastName("worker")
                .Build();

            internalOrganisation.AddBlueCollarWorker(worker);

            this.Session.Derive(true);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            this.Session.Derive();

            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workTask)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            Assert.Equal(new WorkEffortStates(this.Session).Created, workTask.WorkEffortState);

            this.SetIdentity(worker.UserName);

            var acl = new AccessControlList(inventoryAssignment, worker);
            Assert.True(acl.CanRead(M.WorkEffortInventoryAssignment.InventoryItem));
            Assert.True(acl.CanWrite(M.WorkEffortInventoryAssignment.InventoryItem));
            Assert.True(acl.CanRead(M.WorkEffortInventoryAssignment.Quantity));
            Assert.True(acl.CanWrite(M.WorkEffortInventoryAssignment.Quantity));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.BillableQuantity));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.BillableQuantity));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.UnitSellingPrice));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.UnitSellingPrice));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.AssignedUnitSellingPrice));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.AssignedUnitSellingPrice));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.UnitPurchasePrice));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.UnitPurchasePrice));
        }
    }
}