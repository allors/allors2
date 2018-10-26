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
    using Should;
    using System;
    using System.Linq;
    using Xunit;

    using Allors.Meta;
    
    public class WorkEffortPartyAssignmentTests : DomainTest
    {
        [Fact]
        public void GivenWorkEffortAndTimeEntry_WhenDeriving_ThenWorkEffortPartyAssignmentSynced()
        {
            // Arrange
            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").Build();
            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            var employment = new EmploymentBuilder(this.Session).WithEmployee(employee).Build();

            this.Session.Derive(true);

            var today = DateTimeFactory.CreateDateTime(DateTime.UtcNow);
            var tomorrow = DateTimeFactory.CreateDateTime(DateTime.UtcNow.AddDays(1));
            var hour = new UnitsOfMeasure(this.Session).Hour;

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithFromDate(today)
                .WithThroughDate(tomorrow)
                .WithUnitOfMeasure(hour)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            // Act
            this.Session.Derive(true);

            // Assert
            var partyAssignment = workOrder.WorkEffortPartyAssignmentsWhereAssignment.First;

            partyAssignment.Assignment.ShouldEqual(workOrder);
            partyAssignment.Party.ShouldEqual(employee);
            partyAssignment.ExistFromDate.ShouldBeFalse();
            partyAssignment.ExistThroughDate.ShouldBeFalse();
        }

        [Fact]
        public void GivenTimeEntryWithRequiredAssignmentOrganisation_WhenDeriving_ThenWorkEffortPartyAssignmentSynced()
        {
            // Arrange
            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").Build();
            var organisation = new InternalOrganisations(this.Session).Extent().First;
            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            var employment = new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(organisation).Build();

            organisation.RequireExistingWorkEffortPartyAssignment = true;
            this.Session.Derive(true);

            var today = DateTimeFactory.CreateDateTime(DateTime.UtcNow);
            var tomorrow = DateTimeFactory.CreateDateTime(DateTime.UtcNow.AddDays(1));
            var hour = new UnitsOfMeasure(this.Session).Hour;

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithFromDate(today)
                .WithThroughDate(tomorrow)
                .WithUnitOfMeasure(hour)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            // Act
            var derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeTrue();
            derivation.Errors.SelectMany(e => e.Relations).Any(r => r.AssociationType.Equals(M.WorkEffort.WorkEffortPartyAssignmentsWhereAssignment)).ShouldBeTrue();

            //// Re-Arrange
            employee.TimeSheetWhereWorker.RemoveTimeEntries();

            var assignment = new WorkEffortPartyAssignmentBuilder(this.Session)
                .WithAssignment(workOrder)
                .WithParty(employee)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeTrue();
        }
    }
}