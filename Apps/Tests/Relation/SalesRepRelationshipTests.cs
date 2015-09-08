//------------------------------------------------------------------------------------------------- 
// <copyright file="SalesRepRelationshipTests.cs" company="Allors bvba">
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
    public class SalesRepRelationshipTests : DomainTest
    {
        [Test]
        public void GivenSalesRepRelationship_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain).Build();
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new SalesRepRelationshipBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCustomer(customer);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithSalesRepresentative(new PersonBuilder(this.DatabaseSession).WithLastName("salesrep.").Build());
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenNextSalesRep_WhenEmploymentAndSalesRepRelationshipAreCreated_ThenSalesRepIsAddedToUserGroup()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();

            var usergroups = internalOrganisation.UserGroupsWhereParty;
            usergroups.Filter.AddEquals(UserGroups.Meta.Parent, new Roles(this.DatabaseSession).Sales.UserGroupWhereRole);
            var salesRepUserGroup = usergroups.First;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, salesRepUserGroup.Members.Count);
            Assert.Contains(new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "salesRep"), salesRepUserGroup.Members);

            var salesrep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep2").WithUserName("salesRep2").Build();

            new EmploymentBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithEmployee(salesrep2)
                .WithEmployer(internalOrganisation)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithCustomer(customer)
                .WithSalesRepresentative(salesrep2)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, salesRepUserGroup.Members.Count);
            Assert.IsTrue(salesRepUserGroup.Members.Contains(salesrep2));
        }

        [Test]
        public void GivenEmployment_WhenEmploymentPeriodIsNotValid_ThenEmployeeIsNotInSalesRepUserGroup()
        {
            var salesRep = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "salesRep");
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var usergroups = internalOrganisation.UserGroupsWhereParty;
            usergroups.Filter.AddEquals(UserGroups.Meta.Parent, new Roles(this.DatabaseSession).Sales.UserGroupWhereRole);
            var salesRepUserGroup = usergroups.First;

            Assert.AreEqual(1, salesRepUserGroup.Members.Count);
            Assert.Contains(salesRep, salesRepUserGroup.Members);

            salesRep.EmploymentsWhereEmployee.First.FromDate = DateTime.UtcNow.AddDays(+1);
            salesRep.EmploymentsWhereEmployee.First.RemoveThroughDate();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, salesRepUserGroup.Members.Count);

            salesRep.EmploymentsWhereEmployee.First.FromDate = DateTime.UtcNow;
            salesRep.EmploymentsWhereEmployee.First.RemoveThroughDate();
            
            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, salesRepUserGroup.Members.Count);
            Assert.Contains(salesRep, salesRepUserGroup.Members);

            salesRep.EmploymentsWhereEmployee.First.FromDate = DateTime.UtcNow.AddDays(-2);
            salesRep.EmploymentsWhereEmployee.First.ThroughDate = DateTime.UtcNow.AddDays(-1);
            
            this.DatabaseSession.Derive(true);
            
            Assert.AreEqual(0, salesRepUserGroup.Members.Count);
        }
    }
}
