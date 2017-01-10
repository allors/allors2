// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaPopulationTest.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Meta.Static
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class MetaPopulationTest : AbstractTest
    {
        [Test]
        public void Inheritances()
        {
            var domain = this.Domain;
            var superdomain = new Domain(this.MetaPopulation, Guid.NewGuid());
            domain.AddDirectSuperdomain(superdomain);

            var c1 = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("c1").WithPluralName("c1s").Build();
            var c2 = new ClassBuilder(superdomain, Guid.NewGuid()).WithSingularName("c2").WithPluralName("c2s").Build();

            var i1 = new InterfaceBuilder(domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();
            var i2 = new InterfaceBuilder(superdomain, Guid.NewGuid()).WithSingularName("i2").WithPluralName("i2s").Build();

            Assert.AreEqual(0, this.MetaPopulation.Inheritances.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i1).Build();

            Assert.AreEqual(1, this.MetaPopulation.Inheritances.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c2).WithSupertype(i2).Build();

            Assert.AreEqual(2, this.MetaPopulation.Inheritances.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i2).Build();

            Assert.AreEqual(3, this.MetaPopulation.Inheritances.Count());
        }

        [Test]
        public void Composites()
        {
            var metaPopulation = new MetaPopulation();
            var domain = new Domain(metaPopulation, Guid.NewGuid());
            var superdomain = new Domain(metaPopulation, Guid.NewGuid());
            domain.AddDirectSuperdomain(superdomain);

            Assert.AreEqual(0, metaPopulation.Composites.Count());

            var @class = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("Class").WithPluralName("Classes").Build();

            Assert.AreEqual(1, metaPopulation.Composites.Count());

            var superclass = new ClassBuilder(superdomain, Guid.NewGuid()).WithSingularName("Superclass").WithPluralName("Superclasses").Build();

            Assert.AreEqual(2, metaPopulation.Composites.Count());

            var @interface = new InterfaceBuilder(domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();

            Assert.AreEqual(3, metaPopulation.Composites.Count());

            var superinterface = new InterfaceBuilder(superdomain, Guid.NewGuid()).WithSingularName("i2").WithPluralName("i2s").Build();

            Assert.AreEqual(4, metaPopulation.Composites.Count());

            var unit = new UnitBuilder(domain, UnitIds.StringId).WithSingularName("AllorsString").WithPluralName("AllorsStrings").WithUnitTag(UnitTags.AllorsString).Build();

            Assert.AreEqual(4, metaPopulation.Composites.Count());

            var superunit = new UnitBuilder(domain, UnitIds.IntegerId).WithSingularName("AllorsInteger").WithPluralName("AllorsIntegers").WithUnitTag(UnitTags.AllorsString).Build();

            Assert.AreEqual(4, metaPopulation.Composites.Count());
        }

        [Test]
        public void Units()
        {
            var metaPopulation = new MetaPopulation();
            var domain = new Domain(metaPopulation, Guid.NewGuid());
            var superdomain = new Domain(metaPopulation, Guid.NewGuid());
            domain.AddDirectSuperdomain(superdomain);

            Assert.AreEqual(0, metaPopulation.Units.Count());

            var unit = new UnitBuilder(domain, UnitIds.StringId).WithSingularName("AllorsString").WithPluralName("AllorsStrings").WithUnitTag(UnitTags.AllorsString).Build();

            Assert.AreEqual(1, metaPopulation.Units.Count());

            var superunit = new UnitBuilder(domain, UnitIds.IntegerId).WithSingularName("AllorsInteger").WithPluralName("AllorsIntegers").WithUnitTag(UnitTags.AllorsInteger).Build();

            Assert.AreEqual(2, metaPopulation.Units.Count());

            var @class = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("Class").WithPluralName("Classes").Build();

            Assert.AreEqual(2, metaPopulation.Units.Count());

            var superclass = new ClassBuilder(superdomain, Guid.NewGuid()).WithSingularName("Superclass").WithPluralName("Superclasses").Build();

            Assert.AreEqual(2, metaPopulation.Units.Count());

            var @interface = new InterfaceBuilder(domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();

            Assert.AreEqual(2, metaPopulation.Units.Count());

            var superinterface = new InterfaceBuilder(superdomain, Guid.NewGuid()).WithSingularName("i2").WithPluralName("i2s").Build();

            Assert.AreEqual(2, metaPopulation.Units.Count());
        }

        [Test]
        public void RelationTypes()
        {
            var metaPopulation = new MetaPopulation();
            var domain = new Domain(metaPopulation, Guid.NewGuid());
            var superdomain = new Domain(metaPopulation, Guid.NewGuid());
            domain.AddDirectSuperdomain(superdomain);

            var c1 = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("c1").WithPluralName("c1s").Build();
            var c2 = new ClassBuilder(superdomain, Guid.NewGuid()).WithSingularName("c2").WithPluralName("c2s").Build();

            var i1 = new InterfaceBuilder(domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();
            var i2 = new InterfaceBuilder(superdomain, Guid.NewGuid()).WithSingularName("i2").WithPluralName("i2s").Build();

            Assert.AreEqual(0, metaPopulation.RelationTypes.Count());

            new RelationTypeBuilder(domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).WithObjectTypes(c1, i1).Build();

            Assert.AreEqual(1, metaPopulation.RelationTypes.Count());

            new RelationTypeBuilder(superdomain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).WithObjectTypes(c1, i2).Build();

            Assert.AreEqual(2, metaPopulation.RelationTypes.Count());

            new RelationTypeBuilder(domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).WithObjectTypes(c1, c2).Build();

            Assert.AreEqual(3, metaPopulation.RelationTypes.Count());

            new MethodTypeBuilder(domain, Guid.NewGuid()).WithName("Method1").Build();

            Assert.AreEqual(3, metaPopulation.RelationTypes.Count());

            new MethodTypeBuilder(superdomain, Guid.NewGuid()).WithName("Method2").Build();

            Assert.AreEqual(3, metaPopulation.RelationTypes.Count());
        }

        [Test]
        public void MethodTypes()
        {
            var metaPopulation = new MetaPopulation();
            var domain = new Domain(metaPopulation, Guid.NewGuid());
            var superdomain = new Domain(metaPopulation, Guid.NewGuid());
            domain.AddDirectSuperdomain(superdomain);

            var c1 = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("c1").WithPluralName("c1s").Build();
            var c2 = new ClassBuilder(superdomain, Guid.NewGuid()).WithSingularName("c2").WithPluralName("c2s").Build();

            var i1 = new InterfaceBuilder(domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();
            var i2 = new InterfaceBuilder(superdomain, Guid.NewGuid()).WithSingularName("i2").WithPluralName("i2s").Build();

            Assert.AreEqual(0, metaPopulation.MethodTypes.Count());

            new MethodTypeBuilder(domain, Guid.NewGuid()).WithName("Method1").Build();

            Assert.AreEqual(1, metaPopulation.MethodTypes.Count());

            new MethodTypeBuilder(superdomain, Guid.NewGuid()).WithName("Method2").Build();

            Assert.AreEqual(2, metaPopulation.MethodTypes.Count());

            new RelationTypeBuilder(domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).WithObjectTypes(c1, i1).Build();

            Assert.AreEqual(2, metaPopulation.MethodTypes.Count());

            new RelationTypeBuilder(superdomain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).WithObjectTypes(c2, i2).Build();

            Assert.AreEqual(2, metaPopulation.MethodTypes.Count());
        }


        [Test]
        public void Superdomains()
        {
            var metaPopulation = new MetaPopulation();
            var anotherMetaPopulation = new MetaPopulation();

            Assert.AreEqual(0, metaPopulation.Domains.Count());

            var domain = new Domain(metaPopulation, Guid.NewGuid());

            Assert.AreEqual(1, metaPopulation.Domains.Count());

            var superdomain = new Domain(metaPopulation, Guid.NewGuid());
            domain.AddDirectSuperdomain(superdomain);

            Assert.AreEqual(2, metaPopulation.Domains.Count());

            var anotherdomain = new Domain(anotherMetaPopulation, Guid.NewGuid());

            Assert.AreEqual(2, metaPopulation.Domains.Count());
        }

        [Test]
        public void Find()
        {
            var metapopulation = new MetaPopulation();

            var domain = new Domain(metapopulation, Guid.NewGuid());
            var superdomain = new Domain(metapopulation, Guid.NewGuid());
            domain.AddDirectSuperdomain(superdomain);

            var unitId = UnitIds.StringId;
            var interfaceId = Guid.NewGuid();
            var classId = Guid.NewGuid();
            var inheritanceId = Guid.NewGuid();
            var relationTypeId = Guid.NewGuid();
            var associationTypeId = Guid.NewGuid();
            var roleTypeId = Guid.NewGuid();

            var superunitId = UnitIds.IntegerId;
            var superinterfaceId = Guid.NewGuid();
            var superclassId = Guid.NewGuid();
            var superinheritanceId = Guid.NewGuid();
            var superrelationTypeId = Guid.NewGuid();
            var superassociationTypeId = Guid.NewGuid();
            var superroleTypeId = Guid.NewGuid();

            Assert.IsNull(metapopulation.Find(unitId));
            Assert.IsNull(metapopulation.Find(interfaceId));
            Assert.IsNull(metapopulation.Find(classId));
            Assert.IsNull(metapopulation.Find(inheritanceId));
            Assert.IsNull(metapopulation.Find(relationTypeId));
            Assert.IsNull(metapopulation.Find(associationTypeId));
            Assert.IsNull(metapopulation.Find(roleTypeId));

            Assert.IsNull(metapopulation.Find(superunitId));
            Assert.IsNull(metapopulation.Find(superinterfaceId));
            Assert.IsNull(metapopulation.Find(superclassId));
            Assert.IsNull(metapopulation.Find(superinheritanceId));
            Assert.IsNull(metapopulation.Find(superrelationTypeId));
            Assert.IsNull(metapopulation.Find(superassociationTypeId));
            Assert.IsNull(metapopulation.Find(superroleTypeId));

            new UnitBuilder(domain, unitId).WithSingularName("AllorsString").WithPluralName("AllorsStrings").WithUnitTag(UnitTags.AllorsString).Build();

            Assert.IsNotNull(metapopulation.Find(unitId) as Unit);
            Assert.IsNull(metapopulation.Find(interfaceId));
            Assert.IsNull(metapopulation.Find(classId));
            Assert.IsNull(metapopulation.Find(inheritanceId));
            Assert.IsNull(metapopulation.Find(relationTypeId));
            Assert.IsNull(metapopulation.Find(associationTypeId));
            Assert.IsNull(metapopulation.Find(roleTypeId));

            Assert.IsNull(metapopulation.Find(superunitId));
            Assert.IsNull(metapopulation.Find(superinterfaceId));
            Assert.IsNull(metapopulation.Find(superclassId));
            Assert.IsNull(metapopulation.Find(superinheritanceId));
            Assert.IsNull(metapopulation.Find(superrelationTypeId));
            Assert.IsNull(metapopulation.Find(superassociationTypeId));
            Assert.IsNull(metapopulation.Find(superroleTypeId));

            var @interface = new InterfaceBuilder(domain, interfaceId).WithSingularName("Interface").WithPluralName("Interfaces").Build();
            
            Assert.IsNotNull(metapopulation.Find(unitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(interfaceId) as Interface);
            Assert.IsNull(metapopulation.Find(classId));
            Assert.IsNull(metapopulation.Find(inheritanceId));
            Assert.IsNull(metapopulation.Find(relationTypeId));
            Assert.IsNull(metapopulation.Find(associationTypeId));
            Assert.IsNull(metapopulation.Find(roleTypeId));

            Assert.IsNull(metapopulation.Find(superunitId));
            Assert.IsNull(metapopulation.Find(superinterfaceId));
            Assert.IsNull(metapopulation.Find(superclassId));
            Assert.IsNull(metapopulation.Find(superinheritanceId));
            Assert.IsNull(metapopulation.Find(superrelationTypeId));
            Assert.IsNull(metapopulation.Find(superassociationTypeId));
            Assert.IsNull(metapopulation.Find(superroleTypeId));

            var @class = new ClassBuilder(domain, classId).WithSingularName("Class").WithPluralName("Classes").Build();

            Assert.IsNotNull(metapopulation.Find(unitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(interfaceId) as Interface);
            Assert.IsNotNull(metapopulation.Find(classId) as Class);
            Assert.IsNull(metapopulation.Find(inheritanceId));
            Assert.IsNull(metapopulation.Find(relationTypeId));
            Assert.IsNull(metapopulation.Find(associationTypeId));
            Assert.IsNull(metapopulation.Find(roleTypeId));

            Assert.IsNull(metapopulation.Find(superunitId));
            Assert.IsNull(metapopulation.Find(superinterfaceId));
            Assert.IsNull(metapopulation.Find(superclassId));
            Assert.IsNull(metapopulation.Find(superinheritanceId));
            Assert.IsNull(metapopulation.Find(superrelationTypeId));
            Assert.IsNull(metapopulation.Find(superassociationTypeId));
            Assert.IsNull(metapopulation.Find(superroleTypeId));

            new InheritanceBuilder(domain, inheritanceId).WithSubtype(@class).WithSupertype(@interface).Build();

            Assert.IsNotNull(metapopulation.Find(unitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(interfaceId) as Interface);
            Assert.IsNotNull(metapopulation.Find(classId) as Class);
            Assert.IsNotNull(metapopulation.Find(inheritanceId) as Inheritance);
            Assert.IsNull(metapopulation.Find(relationTypeId));
            Assert.IsNull(metapopulation.Find(associationTypeId));
            Assert.IsNull(metapopulation.Find(roleTypeId));

            Assert.IsNull(metapopulation.Find(superunitId));
            Assert.IsNull(metapopulation.Find(superinterfaceId));
            Assert.IsNull(metapopulation.Find(superclassId));
            Assert.IsNull(metapopulation.Find(superinheritanceId));
            Assert.IsNull(metapopulation.Find(superrelationTypeId));
            Assert.IsNull(metapopulation.Find(superassociationTypeId));
            Assert.IsNull(metapopulation.Find(superroleTypeId));

            new RelationTypeBuilder(domain, relationTypeId, associationTypeId, roleTypeId).WithObjectTypes(@class, @interface).Build();

            Assert.IsNotNull(metapopulation.Find(unitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(interfaceId) as Interface);
            Assert.IsNotNull(metapopulation.Find(classId) as Class);
            Assert.IsNotNull(metapopulation.Find(inheritanceId) as Inheritance);
            Assert.IsNotNull(metapopulation.Find(relationTypeId) as RelationType);
            Assert.IsNotNull(metapopulation.Find(associationTypeId) as AssociationType);
            Assert.IsNotNull(metapopulation.Find(roleTypeId) as RoleType);

            Assert.IsNull(metapopulation.Find(superunitId));
            Assert.IsNull(metapopulation.Find(superinterfaceId));
            Assert.IsNull(metapopulation.Find(superclassId));
            Assert.IsNull(metapopulation.Find(superinheritanceId));
            Assert.IsNull(metapopulation.Find(superrelationTypeId));
            Assert.IsNull(metapopulation.Find(superassociationTypeId));
            Assert.IsNull(metapopulation.Find(superroleTypeId));

            new UnitBuilder(superdomain, superunitId).WithSingularName("AllorsInteger").WithPluralName("AllorsIntegers").WithUnitTag(UnitTags.AllorsInteger).Build();

            Assert.IsNotNull(metapopulation.Find(unitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(interfaceId) as Interface);
            Assert.IsNotNull(metapopulation.Find(classId) as Class);
            Assert.IsNotNull(metapopulation.Find(inheritanceId) as Inheritance);
            Assert.IsNotNull(metapopulation.Find(relationTypeId) as RelationType);
            Assert.IsNotNull(metapopulation.Find(associationTypeId) as AssociationType);
            Assert.IsNotNull(metapopulation.Find(roleTypeId) as RoleType);

            Assert.IsNotNull(metapopulation.Find(superunitId) as Unit);
            Assert.IsNull(metapopulation.Find(superinterfaceId));
            Assert.IsNull(metapopulation.Find(superclassId));
            Assert.IsNull(metapopulation.Find(superinheritanceId));
            Assert.IsNull(metapopulation.Find(superrelationTypeId));
            Assert.IsNull(metapopulation.Find(superassociationTypeId));
            Assert.IsNull(metapopulation.Find(superroleTypeId));

            var @superinterface = new InterfaceBuilder(superdomain, superinterfaceId).WithSingularName("SuperInterface").WithPluralName("SuperInterfaces").Build();

            Assert.IsNotNull(metapopulation.Find(unitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(interfaceId) as Interface);
            Assert.IsNotNull(metapopulation.Find(classId) as Class);
            Assert.IsNotNull(metapopulation.Find(inheritanceId) as Inheritance);
            Assert.IsNotNull(metapopulation.Find(relationTypeId) as RelationType);
            Assert.IsNotNull(metapopulation.Find(associationTypeId) as AssociationType);
            Assert.IsNotNull(metapopulation.Find(roleTypeId) as RoleType);

            Assert.IsNotNull(metapopulation.Find(superunitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(superinterfaceId) as Interface);
            Assert.IsNull(metapopulation.Find(superclassId));
            Assert.IsNull(metapopulation.Find(superinheritanceId));
            Assert.IsNull(metapopulation.Find(superrelationTypeId));
            Assert.IsNull(metapopulation.Find(superassociationTypeId));
            Assert.IsNull(metapopulation.Find(superroleTypeId));

            var @superclass = new ClassBuilder(superdomain, superclassId).WithSingularName("SuperClass").WithPluralName("SuperClasses").Build();

            Assert.IsNotNull(metapopulation.Find(unitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(interfaceId) as Interface);
            Assert.IsNotNull(metapopulation.Find(classId) as Class);
            Assert.IsNotNull(metapopulation.Find(inheritanceId) as Inheritance);
            Assert.IsNotNull(metapopulation.Find(relationTypeId) as RelationType);
            Assert.IsNotNull(metapopulation.Find(associationTypeId) as AssociationType);
            Assert.IsNotNull(metapopulation.Find(roleTypeId) as RoleType);

            Assert.IsNotNull(metapopulation.Find(superunitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(superinterfaceId) as Interface);
            Assert.IsNotNull(metapopulation.Find(superclassId) as Class);
            Assert.IsNull(metapopulation.Find(superinheritanceId));
            Assert.IsNull(metapopulation.Find(superrelationTypeId));
            Assert.IsNull(metapopulation.Find(superassociationTypeId));
            Assert.IsNull(metapopulation.Find(superroleTypeId));

            new InheritanceBuilder(superdomain, superinheritanceId).WithSubtype(@superclass).WithSupertype(@superinterface).Build();

            Assert.IsNotNull(metapopulation.Find(unitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(interfaceId) as Interface);
            Assert.IsNotNull(metapopulation.Find(classId) as Class);
            Assert.IsNotNull(metapopulation.Find(inheritanceId) as Inheritance);
            Assert.IsNotNull(metapopulation.Find(relationTypeId) as RelationType);
            Assert.IsNotNull(metapopulation.Find(associationTypeId) as AssociationType);
            Assert.IsNotNull(metapopulation.Find(roleTypeId) as RoleType);

            Assert.IsNotNull(metapopulation.Find(superunitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(superinterfaceId) as Interface);
            Assert.IsNotNull(metapopulation.Find(superclassId) as Class);
            Assert.IsNotNull(metapopulation.Find(superinheritanceId) as Inheritance);
            Assert.IsNull(metapopulation.Find(superrelationTypeId));
            Assert.IsNull(metapopulation.Find(superassociationTypeId));
            Assert.IsNull(metapopulation.Find(superroleTypeId));

            new RelationTypeBuilder(superdomain, superrelationTypeId, superassociationTypeId, superroleTypeId).WithObjectTypes(@superclass, @superinterface).Build();

            Assert.IsNotNull(metapopulation.Find(unitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(interfaceId) as Interface);
            Assert.IsNotNull(metapopulation.Find(classId) as Class);
            Assert.IsNotNull(metapopulation.Find(inheritanceId) as Inheritance);
            Assert.IsNotNull(metapopulation.Find(relationTypeId) as RelationType);
            Assert.IsNotNull(metapopulation.Find(associationTypeId) as AssociationType);
            Assert.IsNotNull(metapopulation.Find(roleTypeId) as RoleType);

            Assert.IsNotNull(metapopulation.Find(superunitId) as Unit);
            Assert.IsNotNull(metapopulation.Find(superinterfaceId) as Interface);
            Assert.IsNotNull(metapopulation.Find(superclassId) as Class);
            Assert.IsNotNull(metapopulation.Find(superinheritanceId) as Inheritance);
            Assert.IsNotNull(metapopulation.Find(superrelationTypeId) as RelationType);
            Assert.IsNotNull(metapopulation.Find(superassociationTypeId) as AssociationType);
            Assert.IsNotNull(metapopulation.Find(superroleTypeId) as RoleType);
        }

        [Test]
        public void ValidateDuplicateRelationAndType()
        {
            var metaPopulation = new MetaPopulation();
            var domain = new Domain(metaPopulation, Guid.NewGuid()) { Name = "Domain" };

            var c1 = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("C1").WithPluralName("C1s").Build();
            var c2 = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("C2").WithPluralName("C2s").Build();

            var relationType = new RelationTypeBuilder(domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            relationType.AssociationType.ObjectType = c1;
            relationType.RoleType.ObjectType = c2;
            relationType.RoleType.AssignedSingularName = "bb";
            relationType.RoleType.AssignedPluralName = "bbs";

            Assert.IsTrue(metaPopulation.IsValid);

            var c1bb = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("C1bb").WithPluralName("YYY").Build();

            Assert.IsFalse(metaPopulation.IsValid);

            c1bb.SingularName = "XXX";

            Assert.IsTrue(metaPopulation.IsValid);

            c1bb.PluralName = "C1bbs";
        }

        [Test]
        public void ValidateDuplicateReverseRelationAndType()
        {
            var metaPopulation = new MetaPopulation();
            var domain = new Domain(metaPopulation, Guid.NewGuid()) { Name = "Domain" };

            var c1 = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("C1").WithPluralName("C1s").Build();
            var c2 = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("C2").WithPluralName("C2s").Build();

            var relationType = new RelationTypeBuilder(domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            relationType.AssociationType.ObjectType = c1;
            relationType.RoleType.ObjectType = c2;
            relationType.RoleType.AssignedSingularName = "bb";
            relationType.RoleType.AssignedPluralName = "bbs";

            Assert.IsTrue(metaPopulation.IsValid);

            var c1bb = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("bbC1").WithPluralName("YYY").Build();

            Assert.IsFalse(metaPopulation.IsValid);

            c1bb.SingularName = "XXX";

            Assert.IsTrue(metaPopulation.IsValid);

            c1bb.PluralName = "bbsC1";
        }
    }

    public class MetaPopulationTestWithSuperDomains : MetaPopulationTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}