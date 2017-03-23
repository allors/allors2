// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiffTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Adapters.General
{
    using System;

    using Allors.Extra;
    using Allors.Meta;

    using Xunit;

    public abstract class DiffTest : Test
    {
        [Fact]
        [Trait("Category", "Dynamic")]
        public void Default()
        {
            // Unit RoleTypes
            var concreteClasses = this.GetTestTypes();
            foreach (var concreteClass in concreteClasses)
            {
                var roles = this.GetUnitRoles(concreteClass);
                foreach (var role in roles)
                {
                    var sourceAssociation = this.GetSession().Create(concreteClass);
                    var destinationAssociation = this.GetSession().Create(concreteClass);

                    // Different
                    var unitTypeTag = (UnitTypeTags)role.ObjectType.UnitTag;
                    switch (unitTypeTag)
                    {
                        case UnitTypeTags.AllorsString:
                            sourceAssociation.Strategy.SetUnitRole(role, "0");
                            destinationAssociation.Strategy.SetUnitRole(role, "1");
                            break;
                        case UnitTypeTags.AllorsInteger:
                            sourceAssociation.Strategy.SetUnitRole(role, 0);
                            destinationAssociation.Strategy.SetUnitRole(role, 1);
                            break;
                        case UnitTypeTags.AllorsLong:
                            sourceAssociation.Strategy.SetUnitRole(role, 0L);
                            destinationAssociation.Strategy.SetUnitRole(role, 1L);
                            break;
                        case UnitTypeTags.AllorsDecimal:
                            sourceAssociation.Strategy.SetUnitRole(role, 0m);
                            destinationAssociation.Strategy.SetUnitRole(role, 1m);
                            break;
                        case UnitTypeTags.AllorsDouble:
                            sourceAssociation.Strategy.SetUnitRole(role, 0d);
                            destinationAssociation.Strategy.SetUnitRole(role, 1d);
                            break;
                        case UnitTypeTags.AllorsBoolean:
                            sourceAssociation.Strategy.SetUnitRole(role, false);
                            destinationAssociation.Strategy.SetUnitRole(role, true);
                            break;
                        case UnitTypeTags.AllorsDate:
                            sourceAssociation.Strategy.SetUnitRole(role, DateTime.Now);
                            destinationAssociation.Strategy.SetUnitRole(role, DateTime.Now.AddDays(1d));
                            break;
                        case UnitTypeTags.AllorsDateTime:
                            sourceAssociation.Strategy.SetUnitRole(role, DateTime.Now);
                            destinationAssociation.Strategy.SetUnitRole(role, DateTime.Now.AddMilliseconds(1d));
                            break;
                        case UnitTypeTags.AllorsUnique:
                            sourceAssociation.Strategy.SetUnitRole(role, Guid.NewGuid());
                            destinationAssociation.Strategy.SetUnitRole(role, Guid.NewGuid());
                            break;
                        case UnitTypeTags.AllorsBinary:
                            byte[] sourceBinary = { 0xFF, 0x00 };
                            byte[] destinationBinary = { 0x00, 0xFF };
                            sourceAssociation.Strategy.SetUnitRole(role, sourceBinary);
                            destinationAssociation.Strategy.SetUnitRole(role, destinationBinary);
                            break;
                        default:
                            throw new ArgumentException("Unknown Unit ObjectType: " + unitTypeTag);
                    }

                    var differingRoles = Diff.Execute(sourceAssociation, destinationAssociation);
                    Assert.Equal(1, differingRoles.Length);
                    Assert.Equal(role, differingRoles[0]);

                    sourceAssociation = this.GetSession().Create(concreteClass);
                    destinationAssociation = this.GetSession().Create(concreteClass);

                    // Same
                    switch (unitTypeTag)
                    {
                        case UnitTypeTags.AllorsString:
                            const string Str = "0";
                            sourceAssociation.Strategy.SetUnitRole(role, Str.Clone());
                            destinationAssociation.Strategy.SetUnitRole(role, Str.Clone());
                            break;
                        case UnitTypeTags.AllorsInteger:
                            sourceAssociation.Strategy.SetUnitRole(role, 0);
                            destinationAssociation.Strategy.SetUnitRole(role, 0);
                            break;
                        case UnitTypeTags.AllorsLong:
                            sourceAssociation.Strategy.SetUnitRole(role, 0L);
                            destinationAssociation.Strategy.SetUnitRole(role, 0L);
                            break;
                        case UnitTypeTags.AllorsDecimal:
                            sourceAssociation.Strategy.SetUnitRole(role, 0m);
                            destinationAssociation.Strategy.SetUnitRole(role, 0m);
                            break;
                        case UnitTypeTags.AllorsDouble:
                            sourceAssociation.Strategy.SetUnitRole(role, 0d);
                            destinationAssociation.Strategy.SetUnitRole(role, 0d);
                            break;
                        case UnitTypeTags.AllorsBoolean:
                            sourceAssociation.Strategy.SetUnitRole(role, false);
                            destinationAssociation.Strategy.SetUnitRole(role, false);
                            break;
                        case UnitTypeTags.AllorsDate:
                            var nowDate = DateTime.Now;
                            sourceAssociation.Strategy.SetUnitRole(role, nowDate);
                            destinationAssociation.Strategy.SetUnitRole(role, nowDate);
                            break;
                        case UnitTypeTags.AllorsDateTime:
                            var nowDateTime = DateTime.Now;
                            sourceAssociation.Strategy.SetUnitRole(role, nowDateTime);
                            destinationAssociation.Strategy.SetUnitRole(role, nowDateTime);
                            break;
                        case UnitTypeTags.AllorsUnique:
                            var guid = Guid.NewGuid();
                            sourceAssociation.Strategy.SetUnitRole(role, guid);
                            destinationAssociation.Strategy.SetUnitRole(role, guid);
                            break;
                        case UnitTypeTags.AllorsBinary:
                            byte[] sourceBinary = { 0xFF, 0x00 };
                            byte[] destinationBinary = { 0xFF, 0x00 };
                            sourceAssociation.Strategy.SetUnitRole(role, sourceBinary);
                            destinationAssociation.Strategy.SetUnitRole(role, destinationBinary);
                            break;
                        default:
                            throw new ArgumentException("Unknown Unit ObjectType: " + unitTypeTag);
                    }

                    differingRoles = Diff.Execute(sourceAssociation, destinationAssociation);
                    Assert.Equal(0, differingRoles.Length);
                }
            }

            // Many2One Composite RoleTypes (Same repository)
            var relationTypes = this.GetMany2OneRelations(this.GetMetaDomain());
            foreach (var relationType in relationTypes)
            {
                var role = relationType.RoleType;
                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                foreach (var associationType in associationTypes)
                {
                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                    foreach (var roleType in roleTypes)
                    {
                        var role1 = this.GetSession().Create(roleType);
                        var role2 = this.GetSession().Create(roleType);

                        var sourceAssociation = this.GetSession().Create(associationType);
                        var destinationAssociation = this.GetSession().Create(associationType);

                        sourceAssociation.Strategy.SetCompositeRole(relationType.RoleType, role1);
                        destinationAssociation.Strategy.SetCompositeRole(relationType.RoleType, role2);

                        var differingRoles = Diff.Execute(sourceAssociation, destinationAssociation);
                        Assert.Equal(1, differingRoles.Length);
                        Assert.Equal(role, differingRoles[0]);

                        sourceAssociation.Strategy.SetCompositeRole(relationType.RoleType, role1);
                        destinationAssociation.Strategy.SetCompositeRole(relationType.RoleType, role1);

                        differingRoles = Diff.Execute(sourceAssociation, destinationAssociation);
                        Assert.Equal(0, differingRoles.Length);
                    }
                }
            }

            // Many2Many Composite RoleTypes (Same repository)
            relationTypes = this.GetMany2ManyRelations(this.GetMetaDomain());
            foreach (var relationType in relationTypes)
            {
                var role = relationType.RoleType;
                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                foreach (var associationType in associationTypes)
                {
                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                    foreach (var roleType in roleTypes)
                    {
                        var role1 = this.GetSession().Create(roleType);
                        var role2 = this.GetSession().Create(roleType);
                        var role3 = this.GetSession().Create(roleType);

                        var sourceAssociation = this.GetSession().Create(associationType);
                        var destinationAssociation = this.GetSession().Create(associationType);

                        sourceAssociation.Strategy.AddCompositeRole(relationType.RoleType, role1);
                        sourceAssociation.Strategy.AddCompositeRole(relationType.RoleType, role2);
                        destinationAssociation.Strategy.AddCompositeRole(relationType.RoleType, role2);
                        destinationAssociation.Strategy.AddCompositeRole(relationType.RoleType, role3);

                        var differingRoles = Diff.Execute(sourceAssociation, destinationAssociation);
                        Assert.Equal(1, differingRoles.Length);
                        Assert.Equal(role, differingRoles[0]);

                        sourceAssociation.Strategy.AddCompositeRole(relationType.RoleType, role3);
                        destinationAssociation.Strategy.AddCompositeRole(relationType.RoleType, role1);

                        differingRoles = Diff.Execute(sourceAssociation, destinationAssociation);
                        Assert.Equal(0, differingRoles.Length);
                    }
                }
            }

            // Many2One Composite RoleTypes (Different repository)
            relationTypes = this.GetMany2OneRelations(this.GetMetaDomain());
            foreach (var relationType in relationTypes)
            {
                var role = relationType.RoleType;
                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                foreach (var associationType in associationTypes)
                {
                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                    foreach (var roleType in roleTypes)
                    {
                        var role1 = this.GetSession().Create(roleType);
                        var role2 = this.GetSession().Create(roleType);

                        var otherRole1 = this.GetSession2().Insert(roleType, role1.Strategy.ObjectId);
                        var otherRole2 = this.GetSession2().Insert(roleType, role2.Strategy.ObjectId);

                        var sourceAssociation = this.GetSession().Create(associationType);
                        var destinationAssociation = this.GetSession2().Insert(associationType, sourceAssociation.Strategy.ObjectId);

                        sourceAssociation.Strategy.SetCompositeRole(relationType.RoleType, role1);
                        destinationAssociation.Strategy.SetCompositeRole(relationType.RoleType, otherRole2);

                        var differingRoles = Diff.Execute(sourceAssociation, destinationAssociation);
                        Assert.Equal(1, differingRoles.Length);
                        Assert.Equal(role, differingRoles[0]);

                        sourceAssociation.Strategy.SetCompositeRole(relationType.RoleType, role1);
                        destinationAssociation.Strategy.SetCompositeRole(relationType.RoleType, otherRole1);

                        differingRoles = Diff.Execute(sourceAssociation, destinationAssociation);
                        Assert.Equal(0, differingRoles.Length);
                    }
                }
            }
        }

        private ObjectType[] GetTestTypes()
        {
            return this.GetMetaDomain().ConcreteCompositeObjectTypes;
        }
    }
}