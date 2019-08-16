//------------------------------------------------------------------------------------------------- 
// <copyright file="WorkTaskTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Allors.Meta;
    using System;
    using System.Linq;
    using Xunit;

    public class TimeEntryTests : DomainTest
    {
        [Fact]
        public void GivenTimeEntry_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            // Arrange
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .Build();

            // Act
            var derivation = this.Session.Derive(false);
            var originalCount = derivation.Errors.Count();

            // Assert
            Assert.True(derivation.HasErrors);

            //// Re-arrange
            var tomorrow = this.Session.Now().AddDays(1);
            timeEntry.ThroughDate = tomorrow;

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            Assert.True(derivation.HasErrors);
            Assert.Equal(originalCount, derivation.Errors.Count());

            //// Re-arrange
            var workOrder = new WorkTaskBuilder(this.Session).WithName("Work").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            timeEntry.WorkEffort = workOrder;

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            Assert.True(derivation.HasErrors);
            Assert.Equal(originalCount - 1, derivation.Errors.Count());

            //// Re-arrange
            var worker = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(worker).WithEmployer(internalOrganisation).Build();

            derivation = this.Session.Derive(false);

            worker.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            Assert.False(derivation.HasErrors);
        }

        [Fact]
        public void GivenTimeEntryWithFromAndThroughDates_WhenDeriving_ThenAmountOfTimeDerived()
        {
            // Arrange
            var frequencies = new TimeFrequencies(this.Session);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(internalOrganisation).Build();

            this.Session.Derive(true);

            var now = DateTimeFactory.CreateDateTime(this.Session.Now());
            var later = DateTimeFactory.CreateDateTime(now.AddHours(4));

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(now)
                .WithThroughDate(later)
                .WithTimeFrequency(frequencies.Hour)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            // Act
            this.Session.Derive(true);

            // Assert
            Assert.Equal(4.00M, timeEntry.AmountOfTime);
            Assert.Equal(4.00M, timeEntry.ActualHours);

            //// Re-arrange
            timeEntry.RemoveAmountOfTime();
            timeEntry.TimeFrequency = frequencies.Day;

            // Act
            this.Session.Derive(true);

            // Assert
            Assert.Equal(Math.Round(4.0M / 24.0M, M.TimeEntry.AmountOfTime.Scale ?? 2), timeEntry.AmountOfTime);
            Assert.Equal(4.00M, timeEntry.ActualHours);

            //// Re-arrange
            timeEntry.RemoveAmountOfTime();
            timeEntry.TimeFrequency = frequencies.Minute;

            // Act
            this.Session.Derive(true);

            // Assert
            Assert.Equal(4.0M * 60.0M, timeEntry.AmountOfTime);
            Assert.Equal(4.00M, timeEntry.ActualHours);
        }

        [Fact]
        public void GivenTimeEntryWithFromDateAndAmountOfTime_WhenDeriving_ThenThroughDateDerived()
        {
            // Arrange
            var frequencies = new TimeFrequencies(this.Session);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(internalOrganisation).Build();

            this.Session.Derive(true);

            var now = DateTimeFactory.CreateDateTime(this.Session.Now());
            var hour = frequencies.Hour;

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(now)
                .WithAssignedAmountOfTime(4.0M)
                .WithTimeFrequency(hour)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            // Act
            this.Session.Derive(true);

            // Assert
            var timeSpan = timeEntry.ThroughDate - timeEntry.FromDate;
            Assert.Equal(4.00, timeSpan.Value.TotalHours);
            Assert.Equal(4.0M, timeEntry.ActualHours);

            //// Re-arrange
            timeEntry.RemoveThroughDate();
            timeEntry.TimeFrequency = frequencies.Minute;

            // Act
            this.Session.Derive(true);

            // Assert
            timeSpan = timeEntry.ThroughDate - timeEntry.FromDate;
            Assert.Equal(4.00, timeSpan.Value.TotalMinutes);
            Assert.Equal(Math.Round(4.0M / 60.0M, M.TimeEntry.AmountOfTime.Scale ?? 2), timeEntry.ActualHours);

            //// Re-arrange
            timeEntry.RemoveThroughDate();
            timeEntry.TimeFrequency = frequencies.Day;

            // Act
            this.Session.Derive(true);

            // Assert
            timeSpan = timeEntry.ThroughDate - timeEntry.FromDate;
            Assert.Equal(4.00, timeSpan.Value.TotalDays);
            Assert.Equal(4.00M * 24.00M, timeEntry.ActualHours);
        }
    }
}