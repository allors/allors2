// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectTypeTest.cs" company="Allors bvba">
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
    public class ObjectTypeTest : AbstractTest
    {
        [Test]
        public void Associations()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var i1 = this.Population.I1;
            var i2 = this.Population.I2;

            Assert.AreEqual(0, c2.AssociationTypes.Count());

            var c1_c2 = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;

            Assert.AreEqual(1, c2.AssociationTypes.Count());

            var i1_i2 = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            i1_i2.AssociationType.ObjectType = i1;
            i1_i2.RoleType.ObjectType = i2;

            Assert.AreEqual(2, c2.AssociationTypes.Count());

            var ix = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("ix").WithPluralName("ixs").Build();

            var c1_ix = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            c1_ix.AssociationType.ObjectType = c1;
            c1_ix.RoleType.ObjectType = ix;

            Assert.AreEqual(2, c2.AssociationTypes.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c2).WithSupertype(ix).Build();

            Assert.AreEqual(3, c2.AssociationTypes.Count());
        }

        [Test]
        public void Roles()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var i1 = this.Population.I1;
            var i2 = this.Population.I2;

            Assert.AreEqual(0, c1.RoleTypes.Count());

            var c1_c2 = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;

            Assert.AreEqual(1, c1.RoleTypes.Count());

            var i1_i2 = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            i1_i2.AssociationType.ObjectType = i1;
            i1_i2.RoleType.ObjectType = i2;

            Assert.AreEqual(2, c1.RoleTypes.Count());

            var ix = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("ix").WithPluralName("ixs").Build();

            var ix_c2 = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            ix_c2.AssociationType.ObjectType = ix;
            ix_c2.RoleType.ObjectType = c2;

            Assert.AreEqual(2, c1.RoleTypes.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(ix).Build();

            Assert.AreEqual(3, c1.RoleTypes.Count());
        }

        [Test]
        public void ContainsAssociation()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var c3 = this.Population.C3;
            var c4 = this.Population.C4;

            var c1_c2 = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;

            Assert.IsFalse(c1.ExistAssociationType(c1_c2.AssociationType));
            Assert.IsTrue(c2.ExistAssociationType(c1_c2.AssociationType));
            Assert.IsFalse(c3.ExistAssociationType(c1_c2.AssociationType));
            Assert.IsFalse(c4.ExistAssociationType(c1_c2.AssociationType));

            var c1_c3 = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            c1_c3.AssociationType.ObjectType = c1;
            c1_c3.RoleType.ObjectType = c3;

            Assert.IsFalse(c1.ExistAssociationType(c1_c2.AssociationType));
            Assert.IsTrue(c2.ExistAssociationType(c1_c2.AssociationType));
            Assert.IsFalse(c3.ExistAssociationType(c1_c2.AssociationType));
            Assert.IsFalse(c4.ExistAssociationType(c1_c2.AssociationType));

            Assert.IsFalse(c1.ExistAssociationType(c1_c3.AssociationType));
            Assert.IsFalse(c2.ExistAssociationType(c1_c3.AssociationType));
            Assert.IsTrue(c3.ExistAssociationType(c1_c3.AssociationType));
            Assert.IsFalse(c4.ExistAssociationType(c1_c3.AssociationType));
        }

        [Test]
        public void ContainsRole()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var c3 = this.Population.C3;
            var c4 = this.Population.C4;

            var c1_c2 = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;

            Assert.IsTrue(c1.ExistRoleType(c1_c2.RoleType));
            Assert.IsFalse(c2.ExistRoleType(c1_c2.RoleType));
            Assert.IsFalse(c3.ExistRoleType(c1_c2.RoleType));
            Assert.IsFalse(c4.ExistRoleType(c1_c2.RoleType));

            var c1_c3 = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            c1_c3.AssociationType.ObjectType = c1;
            c1_c3.RoleType.ObjectType = c3;

            Assert.IsTrue(c1.ExistRoleType(c1_c2.RoleType));
            Assert.IsFalse(c2.ExistRoleType(c1_c2.RoleType));
            Assert.IsFalse(c3.ExistRoleType(c1_c2.RoleType));
            Assert.IsFalse(c4.ExistRoleType(c1_c2.RoleType));

            Assert.IsTrue(c1.ExistRoleType(c1_c3.RoleType));
            Assert.IsFalse(c2.ExistRoleType(c1_c3.RoleType));
            Assert.IsFalse(c3.ExistRoleType(c1_c3.RoleType));
            Assert.IsFalse(c4.ExistRoleType(c1_c3.RoleType));
        }

        [Test]
        public void Defaults()
        {
            this.Populate();

            var thisType = new ClassBuilder(this.Domain, Guid.NewGuid()).Build();
            thisType.SingularName = "ThisType";
            thisType.PluralName = "TheseTypes";

            var thatType = new ClassBuilder(this.Domain, Guid.NewGuid()).Build();
            thatType.SingularName = "ThatType";
            thatType.PluralName = "ThatTypes";

            var supertype = new InterfaceBuilder(this.Domain, Guid.NewGuid()).Build();
            supertype.SingularName = "Supertype";
            supertype.PluralName = "Supertypes";

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(thisType).WithSupertype(supertype).Build();

            var relationTypeWhereAssociation = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            relationTypeWhereAssociation.AssociationType.ObjectType = thisType;
            relationTypeWhereAssociation.RoleType.ObjectType = thatType;

            var relationTypeWhereRole = new RelationTypeBuilder(this.Domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
            relationTypeWhereRole.AssociationType.ObjectType = thatType;
            relationTypeWhereRole.RoleType.ObjectType = thisType;
            relationTypeWhereRole.RoleType.AssignedSingularName = "to";
            relationTypeWhereRole.RoleType.AssignedPluralName = "tos";

            Assert.IsTrue(this.MetaPopulation.IsValid);
        }

        [Test]
        public void DirectSupertypes()
        {
            var c1 = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("c1").WithPluralName("c1s").Build();
            var c2 = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("c2").WithPluralName("c2s").Build();

            var i1 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();
            var i2 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i2").WithPluralName("i2s").Build();

            Assert.AreEqual(0, c1.DirectSupertypes.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i1).Build();

            Assert.AreEqual(1, c1.DirectSupertypes.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i2).Build();

            Assert.AreEqual(2, c1.DirectSupertypes.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c2).WithSupertype(i1).Build();

            Assert.AreEqual(2, c1.DirectSupertypes.Count());
        }

        [Test]
        public void LeafClasses()
        {
            var c1 = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("c1").WithPluralName("c1s").Build();
            var c2 = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("c2").WithPluralName("c2s").Build();

            var i1 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();
            var i2 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i2").WithPluralName("i2s").Build();

            Assert.AreEqual(1, c1.LeafClasses.Count());
            Assert.AreEqual(c1, c1.LeafClasses.First());
            Assert.AreEqual(1, c2.LeafClasses.Count());
            Assert.AreEqual(c2, c2.LeafClasses.First());
            Assert.AreEqual(0, i1.LeafClasses.Count());
            Assert.AreEqual(0, i2.LeafClasses.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i1).Build();

            Assert.AreEqual(1, c1.LeafClasses.Count());
            Assert.AreEqual(c1, c1.LeafClasses.First());
            Assert.AreEqual(1, c2.LeafClasses.Count());
            Assert.AreEqual(c2, c2.LeafClasses.First());
            Assert.AreEqual(1, i1.LeafClasses.Count());
            Assert.AreEqual(c1, i1.LeafClasses.First());
            Assert.AreEqual(0, i2.LeafClasses.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c2).WithSupertype(i2).Build();

            Assert.AreEqual(1, c1.LeafClasses.Count());
            Assert.AreEqual(c1, c1.LeafClasses.First());
            Assert.AreEqual(1, c2.LeafClasses.Count());
            Assert.AreEqual(c2, c2.LeafClasses.First());
            Assert.AreEqual(1, i1.LeafClasses.Count());
            Assert.AreEqual(c1, i1.LeafClasses.First());
            Assert.AreEqual(1, i2.LeafClasses.Count());
            Assert.AreEqual(c2, i2.LeafClasses.First());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i2).Build();

            Assert.AreEqual(1, c1.LeafClasses.Count());
            Assert.AreEqual(c1, c1.LeafClasses.First());
            Assert.AreEqual(1, c2.LeafClasses.Count());
            Assert.AreEqual(c2, c2.LeafClasses.First());
            Assert.AreEqual(1, i1.LeafClasses.Count());
            Assert.AreEqual(c1, i1.LeafClasses.First());
            Assert.AreEqual(2, i2.LeafClasses.Count());
            Assert.Contains(c1, i2.LeafClasses.ToList());
            Assert.Contains(c2, i2.LeafClasses.ToList());
        }

        [Test]
        public void ExclusiveLeafClass()
        {
            var c1 = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("c1").WithPluralName("c1s").Build();
            var c2 = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("c2").WithPluralName("c2s").Build();

            var i1 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();
            var i2 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i2").WithPluralName("i2s").Build();

            Assert.AreEqual(c1, c1.ExclusiveLeafClass);
            Assert.AreEqual(c2, c2.ExclusiveLeafClass);
            Assert.IsNull(i1.ExclusiveLeafClass);
            Assert.IsNull(i2.ExclusiveLeafClass);

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i1).Build();

            Assert.AreEqual(c1, c1.ExclusiveLeafClass);
            Assert.AreEqual(c2, c2.ExclusiveLeafClass);
            Assert.AreEqual(c1, i1.ExclusiveLeafClass);
            Assert.IsNull(i2.ExclusiveLeafClass);

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c2).WithSupertype(i2).Build();

            Assert.AreEqual(c1, c1.ExclusiveLeafClass);
            Assert.AreEqual(c2, c2.ExclusiveLeafClass);
            Assert.AreEqual(c1, i1.ExclusiveLeafClass);
            Assert.AreEqual(c2, i2.ExclusiveLeafClass);

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i2).Build();

            Assert.AreEqual(c1, c1.ExclusiveLeafClass);
            Assert.AreEqual(c2, c2.ExclusiveLeafClass);
            Assert.AreEqual(c1, i1.ExclusiveLeafClass);
            Assert.IsNull(i2.ExclusiveLeafClass);
        }

        [Test]
        public void Id()
        {
            this.Populate();

            var thisType = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("c1").WithPluralName("c1s").Build();
            thisType.SingularName = "ThisType";
            thisType.PluralName = "TheseTypes";

            Assert.IsTrue(this.MetaPopulation.IsValid);

            foreach (var objectType in this.MetaPopulation.Classes)
            {
                Assert.IsTrue(objectType.Id != Guid.Empty);
            }
        }

        [Test]
        public void IsClass()
        {
            this.Populate();

            foreach (var composite in this.MetaPopulation.Composites)
            {
                Assert.AreEqual(composite.IsClass, !composite.IsInterface);
            }

            foreach (var objectType in this.MetaPopulation.Classes)
            {
                Assert.IsTrue(objectType.IsClass);
            }

            foreach (var objectType in this.MetaPopulation.Interfaces)
            {
                Assert.IsFalse(objectType.IsClass);
            }
        }

        [Test]
        public void Methods()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var i1 = this.Population.I1;
            var i2 = this.Population.I2;

            Assert.AreEqual(0, c1.MethodTypes.Count());

            var methodC1 = new MethodTypeBuilder(this.Domain, Guid.NewGuid()).Build();
            methodC1.ObjectType = this.Population.C1;
            methodC1.Name = "MethodC1";

            Assert.AreEqual(1, c1.MethodTypes.Count());

            var methodC2 = new MethodTypeBuilder(this.Domain, Guid.NewGuid()).Build();
            methodC2.ObjectType = this.Population.C2;
            methodC2.Name = "MethodC2";

            Assert.AreEqual(1, c1.MethodTypes.Count());

            var methodI1 = new MethodTypeBuilder(this.Domain, Guid.NewGuid()).Build();
            methodI1.ObjectType = this.Population.I1;
            methodI1.Name = "MethodI1";

            Assert.AreEqual(2, c1.MethodTypes.Count());
        }

        [Test]
        public void SubTypes()
        {
            var c1 = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("c1").WithPluralName("c1s").Build();

            var i1 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();
            var i2 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i2").WithPluralName("i2s").Build();

            var i12 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i1).Build();

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(i1).WithSupertype(i12).Build();
            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(i2).WithSupertype(i12).Build();

            Assert.AreEqual(1, i1.Subtypes.Count());
            Assert.AreEqual(0, i2.Subtypes.Count());
            Assert.AreEqual(3, i12.Subtypes.Count());
        }

        [Test]
        public void Supertypes()
        {
            var c1 = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("c1").WithPluralName("c1s").Build();
            var c2 = new ClassBuilder(this.Domain, Guid.NewGuid()).WithSingularName("c1").WithPluralName("c1s").Build();

            var i1 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();
            var i2 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i2").WithPluralName("i2s").Build();

            var i12 = new InterfaceBuilder(this.Domain, Guid.NewGuid()).WithSingularName("i1").WithPluralName("i1s").Build();

            Assert.AreEqual(0, c1.Supertypes.Count());
            Assert.AreEqual(0, c2.Supertypes.Count());
            Assert.AreEqual(0, i1.Supertypes.Count());
            Assert.AreEqual(0, i2.Supertypes.Count());
            Assert.AreEqual(0, i12.Supertypes.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i1).Build();

            Assert.AreEqual(1, c1.Supertypes.Count());
            Assert.Contains(i1, c1.Supertypes.ToList());
            Assert.AreEqual(0, c2.Supertypes.Count());
            Assert.AreEqual(0, i1.Supertypes.Count());
            Assert.AreEqual(0, i2.Supertypes.Count());
            Assert.AreEqual(0, i12.Supertypes.Count());

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(i1).WithSupertype(i12).Build();

            Assert.AreEqual(2, c1.Supertypes.Count());
            Assert.Contains(i1, c1.Supertypes.ToList());
            Assert.Contains(i12, c1.Supertypes.ToList());
            Assert.AreEqual(0, c2.Supertypes.Count());
            Assert.AreEqual(1, i1.Supertypes.Count());
            Assert.Contains(i12, i1.Supertypes.ToList());
            Assert.AreEqual(0, i2.Supertypes.Count());
            Assert.AreEqual(0, i12.Supertypes.Count());
            
            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(i2).WithSupertype(i12).Build();

            Assert.AreEqual(2, c1.Supertypes.Count());
            Assert.Contains(i1, c1.Supertypes.ToList());
            Assert.Contains(i12, c1.Supertypes.ToList());
            Assert.AreEqual(0, c2.Supertypes.Count());
            Assert.AreEqual(1, i1.Supertypes.Count());
            Assert.Contains(i12, i1.Supertypes.ToList());
            Assert.AreEqual(1, i2.Supertypes.Count());
            Assert.Contains(i12, i2.Supertypes.ToList());
            Assert.AreEqual(0, i12.Supertypes.Count());
        }

        [Test]
        public void Units()
        {
            this.Populate();

            Assert.IsTrue(((Unit)this.MetaPopulation.Find(new Guid("ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9"))).IsString);
            Assert.IsTrue(((Unit)this.MetaPopulation.Find(new Guid("ccd6f134-26de-4103-bff9-a37ec3e997a3"))).IsInteger);
            Assert.IsTrue(((Unit)this.MetaPopulation.Find(new Guid("da866d8e-2c40-41a8-ae5b-5f6dae0b89c8"))).IsDecimal);
            Assert.IsTrue(((Unit)this.MetaPopulation.Find(new Guid("ffcabd07-f35f-4083-bef6-f6c47970ca5d"))).IsFloat);
            Assert.IsTrue(((Unit)this.MetaPopulation.Find(new Guid("b5ee6cea-4e2b-498e-a5dd-24671d896477"))).IsBoolean);
            Assert.IsTrue(((Unit)this.MetaPopulation.Find(new Guid("6DC0A1A8-88A4-4614-ADB4-92DD3D017C0E"))).IsUnique);
            Assert.IsTrue(((Unit)this.MetaPopulation.Find(new Guid("c28e515b-cae8-4d6b-95bf-062aec8042fc"))).IsBinary);

            Assert.AreEqual(7, this.MetaPopulation.Units.Count());
        }

        [Test]
        public void Validate()
        {
            var type = new ClassBuilder(this.Domain, Guid.NewGuid()).Build();

            var validationReport = this.MetaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            var errors = validationReport.Errors;
            Assert.AreEqual(2, errors.Length);

            foreach (var error in errors)
            {
                Assert.AreEqual(1, error.Members.Length);
                var member = error.Members[0];

                Assert.IsTrue(member.StartsWith("IObjectType."));

                if (member.Equals("IObjectType.SingularName"))
                {
                    Assert.AreEqual(ValidationKind.Required, error.Kind);
                }
                else if (member.Equals("IObjectType.PluralName"))
                {
                    Assert.AreEqual(ValidationKind.Required, error.Kind);
                }
            }

            // SingularName
            type.SingularName = string.Empty;
            type.PluralName = "Plural";

            validationReport = this.MetaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("IObjectType.SingularName", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Required, validationReport.Errors[0].Kind);

            type.SingularName = "_a";

            validationReport = this.MetaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("IObjectType.SingularName", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.SingularName = "a_";

            validationReport = this.MetaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("IObjectType.SingularName", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.SingularName = "11";

            validationReport = this.MetaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("IObjectType.SingularName", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.SingularName = "a1";

            validationReport = this.MetaPopulation.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);

            type.SingularName = "aa";

            validationReport = this.MetaPopulation.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);

            // PluralName
            type.SingularName = "SingularName";
            type.PluralName = null;

            validationReport = this.MetaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("IObjectType.PluralName", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Required, validationReport.Errors[0].Kind);

            type.PluralName = "_a";

            validationReport = this.MetaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("IObjectType.PluralName", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.PluralName = "a_";

            validationReport = this.MetaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("IObjectType.PluralName", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.PluralName = "11";

            validationReport = this.MetaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("IObjectType.PluralName", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.PluralName = "a1";

            validationReport = this.MetaPopulation.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);

            type.PluralName = "aa";

            validationReport = this.MetaPopulation.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);
        }

        [Test]
        public void ValidateDuplicateSingularName()
        {
            this.Populate();

            var type = new ClassBuilder(this.Domain,Guid.NewGuid()).Build();
            type.SingularName = "Type1";
            type.PluralName = "XXX";

            var anotherType = new ClassBuilder(this.Domain, Guid.NewGuid()).Build();
            anotherType.SingularName = "Type1";
            anotherType.PluralName = "YYY";

            Assert.IsFalse(this.MetaPopulation.IsValid);
        }


        [Test]
        public void ValidateDuplicatePluralName()
        {
            this.Populate();

            var type = new ClassBuilder(this.Domain, Guid.NewGuid()).Build();
            type.SingularName = "XXX";
            type.PluralName = "Type1s";

            var anotherType = new ClassBuilder(this.Domain, Guid.NewGuid()).Build();
            anotherType.SingularName = "YYY";
            anotherType.PluralName = "Type1s";

            Assert.IsFalse(this.MetaPopulation.IsValid);
        }


        [Test]
        public void ValidateNameMinimumLength()
        {
            var type = new ClassBuilder(this.Domain, Guid.NewGuid()).Build();
            type.SingularName = "A";
            type.PluralName = "CD";

            Assert.IsFalse(this.MetaPopulation.IsValid);

            type.SingularName = "AB";

            Assert.IsTrue(this.MetaPopulation.IsValid);

            type.PluralName = "C";

            Assert.IsFalse(this.MetaPopulation.IsValid);

            type.PluralName = "CD";

            Assert.IsTrue(this.MetaPopulation.IsValid);
        }
    }

    public class ObjectTypeTestWithSuperDomains : ObjectTypeTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}