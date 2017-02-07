//------------------------------------------------------------------------------------------------- 
// <copyright file="AssociationTest.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the AssociationTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta.Static
{
    using System;

    using Allors.Meta;

    using NUnit.Framework;

    [TestFixture]
    public class AssociationTest : AbstractTest
    {
        [Test]
        public void Id()
        {
            this.Populate();

            var associationId = Guid.NewGuid();
            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), associationId, Guid.NewGuid()).Build();

            var associationType = relationType.AssociationType;

            Assert.AreEqual(associationId, associationType.Id);
        }

        [Test]
        public void DefaultName()
        {
            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();

            var associationType = relationType.AssociationType;

            Assert.AreEqual(associationType.Id.ToString().ToLower(), associationType.SingularName);
            Assert.AreEqual(associationType.Id.ToString().ToLower(), associationType.PluralName);

            relationType.AssignedMultiplicity = Multiplicity.OneToOne;
            Assert.AreEqual(associationType.Id.ToString(), associationType.Name);
            relationType.AssignedMultiplicity = Multiplicity.OneToMany;
            Assert.AreEqual(associationType.Id.ToString(), associationType.Name);
        }

        [Test]
        public void SingularName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .Build();

            var associationType = companyPerson.AssociationType;

            Assert.AreEqual("Company", associationType.SingularName);
        }

        [Test]
        public void SingularFullName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .Build();

            var associationType = companyPerson.AssociationType;

            Assert.AreEqual("PersonCompany", associationType.SingularFullName);
        }

        [Test]
        public void SingularPropertyName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .Build();

            var associationType = companyPerson.AssociationType;

            Assert.AreEqual("CompanyWherePerson", associationType.SingularPropertyName);
        }
        
        [Test]
        public void PluralName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .WithMultiplicity(Multiplicity.ManyToOne)
                .Build();

            Assert.AreEqual("Companies", companyPerson.AssociationType.PluralName);
        }

        [Test]
        public void PluralFullName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .WithMultiplicity(Multiplicity.ManyToOne)
                .Build();

            Assert.AreEqual("PersonCompanies", companyPerson.AssociationType.PluralFullName);
        }

        [Test]
        public void PluralPropertyName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .WithMultiplicity(Multiplicity.ManyToOne)
                .Build();

            Assert.AreEqual("CompaniesWherePerson", companyPerson.AssociationType.PluralPropertyName);
        }
    }

    public class AssociationTestWithSuperDomains : AssociationTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}