//------------------------------------------------------------------------------------------------- 
// <copyright file="EmploymentTests.cs" company="Allors bvba">
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

    

    using NUnit.Framework;

    [TestFixture]
    public class EmploymentTests : DomainTest
    {
        private Person employee;
        private InternalOrganisation internalOrganisation;
        private Employment employment;

        [SetUp]
        public override void Init()
        {
            base.Init();

            this.employee = new PersonBuilder(this.DatabaseSession).WithLastName("slave").Build();
            this.internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            this.employment = new EmploymentBuilder(this.DatabaseSession)
                .WithEmployee(this.employee)
                .WithEmployer(this.internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();
        }

        [Test]
        public void GivenEmployment_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);
            var employee = new PersonBuilder(this.DatabaseSession).WithLastName("employee").Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new EmploymentBuilder(this.DatabaseSession);
            var employment = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithEmployee(employee);
            employment = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithEmployer(this.internalOrganisation);
            employment = builder.Build();
            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenPerson_WhenEmploymentIsCreated_ThenNoOtherActiveEmploymentMayExist()
        {
            var secondEmployment = new EmploymentBuilder(this.DatabaseSession)
                .WithEmployee(this.employee)
                .WithEmployer(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            this.employment.ThroughDate = DateTime.UtcNow;
        
            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        private void InstantiateObjects(ISession session)
        {
            this.employee = (Person)session.Instantiate(this.employee);
            this.internalOrganisation = (InternalOrganisation)session.Instantiate(this.internalOrganisation);
            this.employment = (Employment)session.Instantiate(this.employment);
        }
    }
}
