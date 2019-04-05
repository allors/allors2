//------------------------------------------------------------------------------------------------- 
// <copyright file="WorkEffortPartyAssignmentTests.cs" company="Allors bvba">
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
    using Xunit;

    using Allors.Meta;

    public class WorkEffortPartyAssignmentTests : DomainTest
    {
        [Fact]
        public void GivenWorkEffortAndTimeEntry_WhenDeriving_ThenWorkEffortPartyAssignmentSynced()
        {
            // Arrange
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(internalOrganisation).Build();

            this.Session.Derive(true);

            var today = DateTimeFactory.CreateDateTime(DateTime.UtcNow);
            var tomorrow = DateTimeFactory.CreateDateTime(DateTime.UtcNow.AddDays(1));
            var hour = new TimeFrequencies(this.Session).Hour;

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(today)
                .WithThroughDate(tomorrow)
                .WithTimeFrequency(hour)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            // Act
            this.Session.Derive(true);

            // Assert
            var partyAssignment = workOrder.WorkEffortPartyAssignmentsWhereAssignment.First;

            Assert.Equal(workOrder, partyAssignment.Assignment);
            Assert.Equal(employee, partyAssignment.Party);
            Assert.False(partyAssignment.ExistFromDate);
            Assert.False(partyAssignment.ExistThroughDate);
        }

        [Fact]
        public void GivenTimeEntryWithRequiredAssignmentOrganisation_WhenDeriving_ThenWorkEffortPartyAssignmentSynced()
        {
            // Arrange
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(internalOrganisation).Build();

            internalOrganisation.RequireExistingWorkEffortPartyAssignment = true;
            this.Session.Derive(true);

            var today = DateTimeFactory.CreateDateTime(DateTime.UtcNow);
            var tomorrow = DateTimeFactory.CreateDateTime(DateTime.UtcNow.AddDays(1));
            var hour = new TimeFrequencies(this.Session).Hour;

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(today)
                .WithThroughDate(tomorrow)
                .WithTimeFrequency(hour)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            // Act
            var derivation = this.Session.Derive(false);

            // Assert
            Assert.True(derivation.HasErrors);
            Assert.Contains(derivation.Errors.SelectMany(e => e.Relations), r => r.AssociationType.Equals(M.WorkEffort.WorkEffortPartyAssignmentsWhereAssignment));

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
            Assert.False(derivation.HasErrors);
        }

        [Fact]
        public void GivenWorkEffort_WhenAddingRates_ThenRateForPartyIsNotAllowed()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            // Calculating rates per party is not implemented yet
            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(internalOrganisation).Build();

            var assignedParty = new WorkEffortPartyAssignmentBuilder(this.Session).WithAssignment(workOrder).WithParty(employee).Build();

            this.Session.Derive(true);

            var assignedRate = new WorkEffortAssignmentRateBuilder(this.Session)
                .WithWorkEffort(workOrder)
                .WithRate(1)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            assignedParty.AddAssignmentRate(assignedRate);

            var derivation = this.Session.Derive(false);

            Assert.True(derivation.HasErrors);
            Assert.Contains(derivation.Errors.SelectMany(e => e.Relations), r => r.RoleType.Equals(M.WorkEffortPartyAssignment.AssignmentRates));
        }
    }
}