namespace Allors.Domain
{
    using Meta;
    using Xunit;

    public class WorkEffortSecurityTests : DomainTest
    {
        [Fact]
        public void WorkTask_StateCreated()
        {
            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").Build();

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
        public void WorkTask_StateCancelled_TimeEntry()
        {
            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").Build();

            this.Session.Derive();

            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).Build();

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