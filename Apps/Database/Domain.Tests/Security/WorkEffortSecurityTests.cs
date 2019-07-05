using System.Linq;

namespace Allors.Domain
{
    using Meta;
    using Xunit;

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

            var administrator = new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName);
            this.SetIdentity(Users.AdministratorUserName);

            var acl = new AccessControlList(workTask, administrator);
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

            var administrator = new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName);
            this.SetIdentity(Users.AdministratorUserName);

            var acl = new AccessControlList(workTask, administrator);
            Assert.True(acl.CanExecute(M.WorkEffort.Invoice));
            Assert.True(acl.CanExecute(M.WorkEffort.Cancel));
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

            var administrator = new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName);
            this.SetIdentity(Users.AdministratorUserName);

            var acl = new AccessControlList(timeEntry, administrator);
            Assert.False(acl.CanWrite(M.TimeEntry.AmountOfTime));
        }
    }
}