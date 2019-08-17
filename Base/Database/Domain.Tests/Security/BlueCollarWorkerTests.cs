// <copyright file="BlueCollarWorkerTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using Allors;
    using Allors.Meta;
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

    public class BlueCollarWorkerSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void WorkEffortInventoryAssignmentOwnInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var worker = new PersonBuilder(this.Session)
                .WithUserName("worker")
                .WithFirstName("blue-collar")
                .WithLastName("worker")
                .Build();

            internalOrganisation.AddBlueCollarWorker(worker);
            new EmploymentBuilder(this.Session).WithEmployee(worker).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(worker);

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
            this.Session.Commit();

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

        [Fact]
        public void WorkEffortInventoryAssignmentOtherInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var worker = new PersonBuilder(this.Session)
                .WithUserName("worker")
                .WithFirstName("blue-collar")
                .WithLastName("worker")
                .Build();

            internalOrganisation.AddBlueCollarWorker(worker);
            new EmploymentBuilder(this.Session).WithEmployee(worker).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(worker);

            this.Session.Derive(true);

            var otherInternalOrganisation = new OrganisationBuilder(this.Session).WithIsInternalOrganisation(true).WithName("other internalOrganisation").Build();

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(otherInternalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(otherInternalOrganisation).Build();

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
            this.Session.Commit();

            Assert.Equal(new WorkEffortStates(this.Session).Created, workTask.WorkEffortState);

            this.SetIdentity(worker.UserName);

            var acl = new AccessControlList(inventoryAssignment, worker);
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.InventoryItem));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.InventoryItem));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.Quantity));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.Quantity));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.BillableQuantity));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.BillableQuantity));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.UnitSellingPrice));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.UnitSellingPrice));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.AssignedUnitSellingPrice));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.AssignedUnitSellingPrice));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.UnitPurchasePrice));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.UnitPurchasePrice));
        }

        [Fact]
        public void WorkTaskOwnInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var worker = new PersonBuilder(this.Session)
                .WithUserName("worker")
                .WithFirstName("blue-collar")
                .WithLastName("worker")
                .Build();

            internalOrganisation.AddBlueCollarWorker(worker);
            new EmploymentBuilder(this.Session).WithEmployee(worker).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(worker);

            this.Session.Derive(true);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity(worker.UserName);

            var acl = new AccessControlList(workTask, worker);
            Assert.True(acl.CanRead(M.WorkTask.WorkDone));
            Assert.True(acl.CanWrite(M.WorkTask.WorkDone));
        }

        [Fact]
        public void WorkTaskOtherInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var worker = new PersonBuilder(this.Session)
                .WithUserName("worker")
                .WithFirstName("blue-collar")
                .WithLastName("worker")
                .Build();

            internalOrganisation.AddBlueCollarWorker(worker);
            new EmploymentBuilder(this.Session).WithEmployee(worker).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(worker);

            this.Session.Derive(true);

            var otherInternalOrganisation = new OrganisationBuilder(this.Session).WithIsInternalOrganisation(true).WithName("other internalOrganisation").Build();

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(otherInternalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(otherInternalOrganisation).Build();

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity(worker.UserName);

            var acl = new AccessControlList(workTask, worker);
            Assert.False(acl.CanRead(M.WorkTask.WorkDone));
            Assert.False(acl.CanWrite(M.WorkTask.WorkDone));
        }

        [Fact]
        public void TimeEntryOwnInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var worker = new PersonBuilder(this.Session)
                .WithUserName("worker")
                .WithFirstName("blue-collar")
                .WithLastName("worker")
                .Build();

            internalOrganisation.AddBlueCollarWorker(worker);
            new EmploymentBuilder(this.Session).WithEmployee(worker).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(worker);

            this.Session.Derive(true);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(this.Session.Now())
                .WithWorkEffort(workTask)
                .Build();

            worker.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity(worker.UserName);

            var acl = new AccessControlList(timeEntry, worker);
            Assert.True(acl.CanRead(M.TimeEntry.ThroughDate));
            Assert.True(acl.CanWrite(M.TimeEntry.ThroughDate));
        }

        [Fact]
        public void TimeEntryOtherInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var worker = new PersonBuilder(this.Session)
                .WithUserName("worker")
                .WithFirstName("blue-collar")
                .WithLastName("worker")
                .Build();

            internalOrganisation.AddBlueCollarWorker(worker);
            new EmploymentBuilder(this.Session).WithEmployee(worker).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(worker);

            this.Session.Derive(true);

            var otherInternalOrganisation = new OrganisationBuilder(this.Session).WithIsInternalOrganisation(true).WithName("other internalOrganisation").Build();

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(otherInternalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(otherInternalOrganisation).Build();

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(this.Session.Now())
                .WithWorkEffort(workTask)
                .Build();

            worker.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity(worker.UserName);

            var acl = new AccessControlList(timeEntry, worker);
            Assert.False(acl.CanRead(M.TimeEntry.ThroughDate));
            Assert.False(acl.CanWrite(M.TimeEntry.ThroughDate));
        }

        [Fact]
        public void Organisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var worker = new PersonBuilder(this.Session)
                .WithUserName("worker")
                .WithFirstName("blue-collar")
                .WithLastName("worker")
                .Build();

            internalOrganisation.AddBlueCollarWorker(worker);
            new EmploymentBuilder(this.Session).WithEmployee(worker).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(worker);

            this.Session.Derive(true);
            this.Session.Commit();

            this.SetIdentity(worker.UserName);

            var acl = new AccessControlList(internalOrganisation, worker);
            Assert.True(acl.CanRead(M.Organisation.Name));
            Assert.False(acl.CanWrite(M.Organisation.Name));
        }
    }
}
