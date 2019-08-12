//------------------------------------------------------------------------------------------------- 
// <copyright file="WorkEffortAssignmentRateTests.cs" company="Allors bvba">
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

    public class WorkEffortAssignmentRateTests : DomainTest
    {
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

            this.Session.Derive(true);

            new WorkEffortAssignmentRateBuilder(this.Session)
                .WithWorkEffort(workOrder)
                .WithRate(1)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            new WorkEffortAssignmentRateBuilder(this.Session)
                .WithWorkEffort(workOrder)
                .WithRate(1)
                .WithRateType(new RateTypes(this.Session).OvertimeRate)
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            new WorkEffortAssignmentRateBuilder(this.Session)
                .WithWorkEffort(workOrder)
                .WithRate(1)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .Build();

            var derivation = this.Session.Derive(false);

            Assert.True(derivation.HasErrors);
            Assert.Contains(derivation.Errors.SelectMany(e => e.Relations), r => r.RoleType.Equals(M.WorkEffortAssignmentRate.RateType));
        }
    }
}