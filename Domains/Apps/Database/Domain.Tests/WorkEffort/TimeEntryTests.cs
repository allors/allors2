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
    using System;
    using System.Linq;
    using Should;
    using Xunit;
    
    public class TimeEntryTests : DomainTest
    {
        [Fact]
        public void GivenTimeEntry_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            // Arrange
            var timeEntry = new TimeEntryBuilder(this.Session).Build();

            // Act
            var derivation = this.Session.Derive(false);
            var originalCount = derivation.Errors.Count();

            // Assert
            derivation.HasErrors.ShouldBeTrue();

            //// Re-arrange
            var tomorrow = DateTime.UtcNow.AddDays(1);
            timeEntry.ThroughDate = tomorrow;

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeTrue();
            derivation.Errors.Count().ShouldEqual(originalCount - 1);

            //// Re-arrange
            var workOrder = new WorkTaskBuilder(this.Session).WithName("Work").Build();
            timeEntry.WorkEffort = workOrder;

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeTrue();
            derivation.Errors.Count().ShouldEqual(originalCount - 2);

            //// Re-arrange
            var worker = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            var organisation = new InternalOrganisations(this.Session).Extent().First;
            var employment = new EmploymentBuilder(this.Session).WithEmployee(worker).WithEmployer(organisation).Build();
            
            derivation = this.Session.Derive(false);

            worker.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeFalse();
        }

        [Fact]
        public void GivenTimeEntryWithFromAndThroughDates_WhenDeriving_ThenAmountOfTimeDerived()
        {
            // Arrange
            var units = new UnitsOfMeasure(this.Session);
            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").Build();
            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            var employment = new EmploymentBuilder(this.Session).WithEmployee(employee).Build();

            this.Session.Derive(true);

            var now = DateTimeFactory.CreateDateTime(this.Session.Now());
            var later = DateTimeFactory.CreateDateTime(now.AddHours(4));

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithFromDate(now)
                .WithThroughDate(later)
                .WithUnitOfMeasure(units.Hour)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            // Act
            this.Session.Derive(true);

            // Assert
            timeEntry.AmountOfTime.ShouldEqual(4.0M);
            timeEntry.ActualHours.ShouldEqual(4.0M);

            //// Re-arrange
            timeEntry.UnitOfMeasure = units.Day;

            // Act
            this.Session.Derive(true);

            // Assert
            timeEntry.AmountOfTime.ShouldEqual(4.0M / 24.0M);
            timeEntry.ActualHours.ShouldEqual(4.0M);

            //// Re-arrange
            timeEntry.UnitOfMeasure = units.Minute;

            // Act
            this.Session.Derive(true);

            // Assert
            timeEntry.AmountOfTime.ShouldEqual(4.0M * 60.0M);
            timeEntry.ActualHours.ShouldEqual(4.0M);
        }

        [Fact]
        public void GivenTimeEntryWithFromDateAndAmountOfTime_WhenDeriving_ThenThroughDateDerived()
        {
            // Arrange
            var units = new UnitsOfMeasure(this.Session);
            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").Build();
            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            var employment = new EmploymentBuilder(this.Session).WithEmployee(employee).Build();

            this.Session.Derive(true);

            var now = DateTimeFactory.CreateDateTime(this.Session.Now());
            var hour = units.Hour;

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithFromDate(now)
                .WithAmountOfTime(4.0M)
                .WithUnitOfMeasure(hour)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            // Act
            this.Session.Derive(true);

            // Assert
            var timeSpan = timeEntry.ThroughDate - timeEntry.FromDate;
            timeSpan.Value.TotalHours.ShouldEqual(4.0, 17);
            timeEntry.ActualHours.ShouldEqual(4.0M);

            //// Re-arrange
            timeEntry.RemoveThroughDate();
            timeEntry.UnitOfMeasure = units.Minute;

            // Act
            this.Session.Derive(true);

            // Assert
            timeSpan = timeEntry.ThroughDate - timeEntry.FromDate;
            timeSpan.Value.TotalMinutes.ShouldEqual(4.0, 17);
            timeEntry.ActualHours.ShouldEqual(Math.Round(4.0M / 60.0M, 17));

            //// Re-arrange
            timeEntry.RemoveThroughDate();
            timeEntry.UnitOfMeasure = units.Day;

            // Act
            this.Session.Derive(true);

            // Assert
            timeSpan = timeEntry.ThroughDate - timeEntry.FromDate;
            timeSpan.Value.TotalDays.ShouldEqual(4.0, 17);
            timeEntry.ActualHours.ShouldEqual(Math.Round(4.0M * 24.0M));
        }
    }
}