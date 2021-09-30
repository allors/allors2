// <copyright file="WorkEffortSecurityTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;
    using Xunit;

    [Trait("Category", "Security")]
    public class WorkEffortSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void WorkTask_StateCreated()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            this.Session.Derive();

            Assert.Equal(new WorkEffortStates(this.Session).Created, workTask.WorkEffortState);

            User user = this.Administrator;
            this.Session.SetUser(user);

            var acl = new DatabaseAccessControlLists(this.Administrator)[workTask];
            Assert.True(acl.CanExecute(M.WorkEffort.Cancel));
            Assert.False(acl.CanExecute(M.WorkEffort.Reopen));
            Assert.False(acl.CanExecute(M.WorkEffort.Complete));
            Assert.False(acl.CanExecute(M.WorkEffort.Invoice));
        }

        [Fact]
        public void WorkTask_StateCompleted()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            this.Session.Derive();

            workTask.Complete();

            this.Session.Derive();

            Assert.Equal(new WorkEffortStates(this.Session).Completed, workTask.WorkEffortState);

            User user = this.Administrator;
            this.Session.SetUser(user);

            var acl = new DatabaseAccessControlLists(this.Administrator)[workTask];
            Assert.True(acl.CanExecute(M.WorkEffort.Invoice));
            Assert.False(acl.CanExecute(M.WorkEffort.Cancel));
            Assert.False(acl.CanExecute(M.WorkEffort.Reopen));
            Assert.False(acl.CanExecute(M.WorkEffort.Complete));
        }

        [Fact]
        public void WorkTask_StateFinished()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var billToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").WithPartyContactMechanism(billToMechelen).Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(internalOrganisation).Build();

            this.Session.Derive();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(DateTimeFactory.CreateDateTime(this.Session.Now()))
                .WithThroughDate(DateTimeFactory.CreateDateTime(this.Session.Now().AddHours(1)))
                .WithTimeFrequency(new TimeFrequencies(this.Session).Hour)
                .WithWorkEffort(workTask)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            this.Session.Derive();

            workTask.Complete();

            this.Session.Derive();

            workTask.Invoice();

            this.Session.Derive();

            Assert.Equal(new WorkEffortStates(this.Session).Finished, workTask.WorkEffortState);

            User user = this.Administrator;
            this.Session.SetUser(user);

            var acl = new DatabaseAccessControlLists(this.Administrator)[workTask];
            Assert.False(acl.CanExecute(M.WorkEffort.Invoice));
            Assert.False(acl.CanExecute(M.WorkEffort.Cancel));
            Assert.False(acl.CanExecute(M.WorkEffort.Reopen));
            Assert.False(acl.CanExecute(M.WorkEffort.Complete));
        }

        [Fact]
        public void WorkTask_StateCancelled_TimeEntry()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            this.Session.Derive();

            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(internalOrganisation).Build();

            this.Session.Derive();

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(DateTimeFactory.CreateDateTime(this.Session.Now()))
                .WithTimeFrequency(new TimeFrequencies(this.Session).Hour)
                .WithWorkEffort(workTask)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            this.Session.Derive();

            workTask.Cancel();

            this.Session.Derive();

            Assert.Equal(new WorkEffortStates(this.Session).Cancelled, workTask.WorkEffortState);

            User user = this.Administrator;
            this.Session.SetUser(user);

            var acl = new DatabaseAccessControlLists(this.Administrator)[timeEntry];
            Assert.False(acl.CanWrite(M.TimeEntry.AmountOfTime));
        }
    }
}
