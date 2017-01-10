//------------------------------------------------------------------------------------------------- 
// <copyright file="RelationTypeTest.cs" company="Allors bvba">
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
// <summary>Defines the RelationTypeTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta.Static
{
    using System;

    using Allors.Meta;

    using NUnit.Framework;

    [TestFixture]
    public class RelationTypeTest : AbstractTest
    {
        [Test]
        public void Minimal()
        {
            this.Populate();

            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.RoleType.ObjectType = this.Population.C2;

            Assert.IsTrue(this.MetaPopulation.IsValid);
        }

        [Test]
        public void HasExclusiveLeafClasses()
        {
            this.Populate();

            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();

            Assert.IsFalse(relationType.ExistExclusiveLeafClasses);

            var association = relationType.AssociationType;

            Assert.IsFalse(relationType.ExistExclusiveLeafClasses);

            var role = relationType.RoleType;

            Assert.IsFalse(relationType.ExistExclusiveLeafClasses);

            association.ObjectType = this.Population.I1;
            role.ObjectType = this.Population.I2;

            Assert.IsTrue(relationType.ExistExclusiveLeafClasses);

            association.ObjectType = this.Population.I1;
            role.ObjectType = null;

            Assert.IsFalse(relationType.ExistExclusiveLeafClasses);

            association.ObjectType = null;
            role.ObjectType = this.Population.I2;

            Assert.IsFalse(relationType.ExistExclusiveLeafClasses);

            association.ObjectType = this.Population.I1;
            role.ObjectType = this.Population.I12;

            Assert.IsFalse(relationType.ExistExclusiveLeafClasses);
        }

        [Test]
        public void Multiplicity()
        {
            this.Populate();

            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            var associationType = relationType.AssociationType;
            associationType.ObjectType = this.Population.C1;

            var roleType = relationType.RoleType;
            roleType.ObjectType = this.Population.C2;

            Assert.IsTrue(this.MetaPopulation.IsValid);

            Assert.IsTrue(relationType.IsOneToOne);

            relationType.AssignedMultiplicity = Meta.Multiplicity.OneToMany;

            Assert.IsTrue(relationType.IsOneToMany);

            relationType.AssignedMultiplicity = Meta.Multiplicity.ManyToMany;

            Assert.IsTrue(relationType.IsManyToMany);

            relationType.AssignedMultiplicity = Meta.Multiplicity.ManyToOne;

            Assert.IsTrue(relationType.IsManyToOne);
        }

        [Test]
        public void UnitCardinality()
        {
            this.Populate();

            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.RoleType.ObjectType = this.Population.IntegerType;

            relationType.AssignedMultiplicity = Meta.Multiplicity.OneToOne;

            Assert.IsTrue(this.MetaPopulation.IsValid);

            relationType.AssignedMultiplicity = Meta.Multiplicity.ManyToOne;
            Assert.IsFalse(relationType.AssociationType.IsMany);

            relationType.AssignedMultiplicity = Meta.Multiplicity.ManyToMany;
            Assert.IsFalse(relationType.RoleType.IsMany);
        }

        [Test]
        public void CompositeCardinality()
        {
            this.Populate();

            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.RoleType.ObjectType = this.Population.C2;

            relationType.AssignedMultiplicity = Meta.Multiplicity.OneToOne;

            Assert.IsTrue(this.MetaPopulation.IsValid);

            relationType.AssignedMultiplicity = Meta.Multiplicity.ManyToOne;

            Assert.IsTrue(this.MetaPopulation.IsValid);

            relationType.AssignedMultiplicity = Meta.Multiplicity.OneToMany;

            Assert.IsTrue(this.MetaPopulation.IsValid);

            relationType.AssignedMultiplicity = Meta.Multiplicity.ManyToMany;

            Assert.IsTrue(this.MetaPopulation.IsValid);

            relationType.AssignedMultiplicity = Meta.Multiplicity.OneToOne;

            Assert.IsTrue(this.MetaPopulation.IsValid);
        }

        [Test]
        public void ValidateDuplicateRelation()
        {
            this.Populate();

            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.RoleType.ObjectType = this.Population.C2;
            relationType.RoleType.AssignedSingularName = "bb";
            relationType.RoleType.AssignedPluralName = "bbs";

            Assert.IsTrue(this.MetaPopulation.IsValid);

            var otherRelationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            otherRelationType.AssociationType.ObjectType = this.Population.C1;
            otherRelationType.RoleType.ObjectType = this.Population.C4;
            otherRelationType.RoleType.AssignedSingularName = "bb";
            otherRelationType.RoleType.AssignedPluralName = "bbs";

            Assert.IsFalse(this.MetaPopulation.IsValid);
        }

        [Test]
        public void ValidateDuplicateReverseRelation()
        {
            this.Populate();

            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.RoleType.ObjectType = this.Population.C2;
            Assert.IsTrue(this.MetaPopulation.IsValid);

            var otherRelationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            otherRelationType.AssociationType.ObjectType = this.Population.C2;
            otherRelationType.RoleType.ObjectType = this.Population.C1;

            Assert.IsFalse(this.MetaPopulation.IsValid);
        }

        [Test]
        public void ValidateNameMinimumLength()
        {
            this.Populate();

            var relationType = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            var associationType = relationType.AssociationType;
            var role = relationType.RoleType;
            associationType.ObjectType = this.Population.C1;
            role.ObjectType = this.Population.C2;

            Assert.IsTrue(this.MetaPopulation.IsValid);

            role.AssignedSingularName = "E";
            role.AssignedPluralName = "GH";

            Assert.IsFalse(this.MetaPopulation.IsValid);

            role.AssignedSingularName = "EF";

            Assert.IsTrue(this.MetaPopulation.IsValid);

            role.AssignedPluralName = "G";

            Assert.IsFalse(this.MetaPopulation.IsValid);

            role.AssignedPluralName = "GH";

            Assert.IsTrue(this.MetaPopulation.IsValid);
        }
    }

    public class RelationTypeTestWithSuperDomains : RelationTypeTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}