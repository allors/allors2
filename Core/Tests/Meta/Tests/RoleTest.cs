//------------------------------------------------------------------------------------------------- 
// <copyright file="RoleTest.cs" company="Allors bvba">
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
// <summary>Defines the RoleTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta.Static
{
    using System;

    using Allors.Meta;

    using NUnit.Framework;

    [TestFixture]
    public class RoleTest : AbstractTest
    {
        [Test]
        public void Id()
        {
            this.Populate();

            var roleId = Guid.NewGuid();
            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), roleId).Build();

            var roleType = relationType.RoleType;

            Assert.AreEqual(roleId, roleType.Id);
        }
        
        [Test]
        public void DefaultName()
        {
            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();

            var roleType = relationType.RoleType;

            Assert.AreEqual(roleType.Id.ToString(), roleType.SingularName);
            Assert.AreEqual(roleType.Id.ToString(), roleType.PluralName);

            relationType.AssignedMultiplicity = Multiplicity.OneToOne;
            Assert.AreEqual(roleType.Id.ToString(), roleType.Name);
            relationType.AssignedMultiplicity = Multiplicity.OneToMany;
            Assert.AreEqual(roleType.Id.ToString(), roleType.Name);
        }

        [Test]
        public void SingularName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .Build();

            Assert.AreEqual("Person", companyPerson.RoleType.SingularName);

            companyPerson.RoleType.AssignedPluralName = "Personen";
            Assert.AreEqual("Person", companyPerson.RoleType.SingularName);

            companyPerson.RoleType.AssignedSingularName = "Persoon";
            Assert.AreEqual("Persoon", companyPerson.RoleType.SingularName);
        }

        [Test]
        public void SingularFullName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .Build();

            Assert.AreEqual("CompanyPerson", companyPerson.RoleType.SingularFullName);

            companyPerson.RoleType.AssignedPluralName = "Personen";
            Assert.AreEqual("CompanyPerson", companyPerson.RoleType.SingularFullName);

            companyPerson.RoleType.AssignedSingularName = "Persoon";
            Assert.AreEqual("CompanyPersoon", companyPerson.RoleType.SingularFullName);
        }

        [Test]
        public void SingularPropertyName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .Build();

            Assert.AreEqual("Person", companyPerson.RoleType.SingularPropertyName);

            companyPerson.RoleType.AssignedPluralName = "Personen";
            Assert.AreEqual("Person", companyPerson.RoleType.SingularPropertyName);

            companyPerson.RoleType.AssignedSingularName = "Persoon";
            Assert.AreEqual("Persoon", companyPerson.RoleType.SingularPropertyName);


            var @interfaceWithoutLeafClass = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Interface").WithPluralName("Interfaces").Build();

            var interfaceWithoutLeafClassPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(@interfaceWithoutLeafClass, person)
                .Build();

            Assert.AreEqual("Person", interfaceWithoutLeafClassPerson.RoleType.SingularPropertyName);

            interfaceWithoutLeafClassPerson.RoleType.AssignedPluralName = "Personen";
            Assert.AreEqual("Person", interfaceWithoutLeafClassPerson.RoleType.SingularPropertyName);

            interfaceWithoutLeafClassPerson.RoleType.AssignedSingularName = "Persoon";
            Assert.AreEqual("Persoon", interfaceWithoutLeafClassPerson.RoleType.SingularPropertyName);
        }

        [Test]
        public void SingularPropertyNameWithInheritance()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();
            
            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .Build();

            var super = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(company).WithSupertype(super).Build();

            var superPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(super, person)
                .Build();
            
            Assert.AreEqual("CompanyPerson", companyPerson.RoleType.SingularPropertyName);
            Assert.AreEqual("CompanyPerson", superPerson.RoleType.SingularPropertyName);

            companyPerson.RoleType.AssignedPluralName = "Personen";
            Assert.AreEqual("CompanyPerson", companyPerson.RoleType.SingularPropertyName);
            Assert.AreEqual("CompanyPerson", superPerson.RoleType.SingularPropertyName);

            companyPerson.RoleType.AssignedSingularName = "Persoon";
            Assert.AreEqual("Persoon", companyPerson.RoleType.SingularPropertyName);
            Assert.AreEqual("Person", superPerson.RoleType.SingularPropertyName);
        }

        [Test]
        public void PluralName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .WithMultiplicity(Multiplicity.OneToMany)
                .Build();

            Assert.AreEqual("Persons", companyPerson.RoleType.PluralName);

            companyPerson.RoleType.AssignedSingularName = "Persoon";

            Assert.AreEqual("Persons", companyPerson.RoleType.PluralName);

            companyPerson.RoleType.AssignedPluralName = "Personen";

            Assert.AreEqual("Personen", companyPerson.RoleType.PluralName);
        }

        [Test]
        public void PluralFullName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .WithMultiplicity(Multiplicity.OneToMany)
                .Build();

            Assert.AreEqual("CompanyPersons", companyPerson.RoleType.PluralFullName);

            companyPerson.RoleType.AssignedSingularName = "Persoon";

            Assert.AreEqual("CompanyPersons", companyPerson.RoleType.PluralFullName);

            companyPerson.RoleType.AssignedPluralName = "Personen";

            Assert.AreEqual("CompanyPersonen", companyPerson.RoleType.PluralFullName);
        }

        [Test]
        public void PluralPropertyName()
        {
            var company = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            var person = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Person").WithPluralName("Persons").Build();

            var companyPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(company, person)
                .WithMultiplicity(Multiplicity.OneToMany)
                .Build();

            var super = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Company").WithPluralName("Companies").Build();
            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(company).WithSupertype(super).Build();

            var superPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(super, person)
                .Build();
            
            Assert.AreEqual("CompanyPersons", companyPerson.RoleType.PluralPropertyName);
            Assert.AreEqual("CompanyPersons", superPerson.RoleType.PluralPropertyName);

            companyPerson.RoleType.AssignedSingularName = "Persoon";

            Assert.AreEqual("CompanyPersons", companyPerson.RoleType.PluralPropertyName);
            Assert.AreEqual("CompanyPersons", superPerson.RoleType.PluralPropertyName);

            companyPerson.RoleType.AssignedPluralName = "Personen";

            Assert.AreEqual("Personen", companyPerson.RoleType.PluralPropertyName);
            Assert.AreEqual("Persons", superPerson.RoleType.PluralPropertyName);



            var @interfaceWithoutLeafClass = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("Interface").WithPluralName("Interfaces").Build();

            var interfaceWithoutLeafClassPerson = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(@interfaceWithoutLeafClass, person)
                .WithMultiplicity(Multiplicity.OneToMany)
                .Build();

            Assert.AreEqual("Persons", interfaceWithoutLeafClassPerson.RoleType.PluralPropertyName);

            interfaceWithoutLeafClassPerson.RoleType.AssignedSingularName = "Persoon";
            Assert.AreEqual("Persons", interfaceWithoutLeafClassPerson.RoleType.PluralPropertyName);

            interfaceWithoutLeafClassPerson.RoleType.AssignedPluralName = "Personen";
            Assert.AreEqual("Personen", interfaceWithoutLeafClassPerson.RoleType.PluralPropertyName);

        }

        [Test]
        public void DeriveSize()
        {
            this.Populate();

            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                .WithObjectTypes(this.Population.C1, this.Population.IntegerType)
                .Build();

            Assert.IsFalse(relationType.RoleType.Size.HasValue);

            relationType.RoleType.ObjectType = this.Population.StringType;

            Assert.IsTrue(relationType.RoleType.Size.HasValue);
            Assert.AreEqual(256, relationType.RoleType.Size);

            relationType.RoleType.ObjectType = this.Population.IntegerType;

            Assert.IsFalse(relationType.RoleType.Size.HasValue);
        }
        
        [Test]
        public void DerivePrecisionScale()
        {
            this.Populate();

            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
             .WithObjectTypes(this.Population.C1, this.Population.IntegerType)
             .Build();

            Assert.IsFalse(relationType.RoleType.Precision.HasValue);
            Assert.IsFalse(relationType.RoleType.Scale.HasValue);

            relationType.RoleType.ObjectType = this.Population.DecimalType;

            Assert.IsTrue(relationType.RoleType.Precision.HasValue);
            Assert.AreEqual(19, relationType.RoleType.Precision);
            Assert.IsTrue(relationType.RoleType.Scale.HasValue);
            Assert.AreEqual(2, relationType.RoleType.Scale);

            relationType.RoleType.ObjectType = this.Population.IntegerType;

            Assert.IsFalse(relationType.RoleType.Precision.HasValue);
            Assert.IsFalse(relationType.RoleType.Scale.HasValue);
        }
    }

    public class RoleTestWithSuperDomains : RoleTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}