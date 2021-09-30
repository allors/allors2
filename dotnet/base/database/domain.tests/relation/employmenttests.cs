// <copyright file="EmploymentTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class EmploymentTests : DomainTest
    {
        private Person employee;
        private InternalOrganisation internalOrganisation;
        private Employment employment;

        public EmploymentTests()
        {
            this.employee = new PersonBuilder(this.Session).WithLastName("slave").Build();

            this.employment = new EmploymentBuilder(this.Session)
                .WithEmployee(this.employee)
                .WithFromDate(this.Session.Now())
                .Build();

            this.Session.Derive();
            this.Session.Commit();
        }

        [Fact]
        public void GivenActiveEmployment_WhenDeriving_ThenInternalOrganisationEmployeesContainsEmployee()
        {
            var employee = new PersonBuilder(this.Session).WithLastName("customer").Build();
            var employer = this.InternalOrganisation;

            new EmploymentBuilder(this.Session)
                .WithEmployee(employee)
                .Build();

            this.Session.Derive();

            Assert.Contains(employee, employer.ActiveEmployees);
        }

        [Fact]
        public void GivenEmploymentToCome_WhenDeriving_ThenInternalOrganisationEmployeesDosNotContainEmployee()
        {
            var employee = new PersonBuilder(this.Session).WithLastName("customer").Build();
            var employer = this.InternalOrganisation;

            new EmploymentBuilder(this.Session)
                .WithEmployee(employee)
                .WithFromDate(this.Session.Now().AddDays(1))
                .Build();

            this.Session.Derive();

            Assert.False(employer.ActiveEmployees.Contains(employee));
        }

        [Fact]
        public void GivenEmploymentThatHasEnded_WhenDeriving_ThenInternalOrganisationEmployeesDosNotContainEmployee()
        {
            var employee = new PersonBuilder(this.Session).WithLastName("customer").Build();
            var employer = this.InternalOrganisation;

            new EmploymentBuilder(this.Session)
                .WithEmployee(employee)
                .WithFromDate(this.Session.Now().AddDays(-10))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            this.Session.Derive();

            Assert.False(employer.ActiveEmployees.Contains(employee));
        }

        private void InstantiateObjects(ISession session)
        {
            this.employee = (Person)session.Instantiate(this.employee);
            this.internalOrganisation = (InternalOrganisation)session.Instantiate(this.internalOrganisation);
            this.employment = (Employment)session.Instantiate(this.employment);
        }
    }
}
