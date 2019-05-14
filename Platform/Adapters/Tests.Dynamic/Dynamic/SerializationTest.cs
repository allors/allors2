// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationTest.cs" company="Allors bvba">
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
    using System.Collections;
    using System.Xml;

    using Allors;
    using Allors.Extra;
    using Allors.Meta;

    using Xunit;

    /// <summary>
    /// Serialization Tests.
    /// </summary>
    public abstract class SerializationTest : Test
    {
        /// <summary>
        /// Test the Save and Load.
        /// </summary>
        [Fact]
        [Trait("Category", "Dynamic")]
        public void SaveAndLoad()
        {
            int[] stringLengths = { 0, 1, 10 };

            foreach (var stringLength in stringLengths)
            {
                var sessionPairs = new ISession[2][];
                sessionPairs[0] = new ISession[2];
                sessionPairs[1] = new ISession[2];

                // From/To same population type
                sessionPairs[0][0] = this.GetSession();
                sessionPairs[0][1] = this.GetSession2();

                // From Memory Population
                sessionPairs[1][0] = this.CreateMemoryPopulation().CreateSession();
                sessionPairs[1][1] = this.GetSession2();

                // To Memory Population
                sessionPairs[1][0] = this.GetSession();
                sessionPairs[1][1] = this.CreateMemoryPopulation().CreateSession();

                foreach (var sessionPair in sessionPairs)
                {
                    var saveSession = sessionPair[0];
                    var loadSession = sessionPair[1];

                    var concreteClasses = this.GetTestTypes();

                    var objectsByMetaType = new Hashtable();
                    foreach (var concreteClass in concreteClasses)
                    {
                        objectsByMetaType[concreteClass] = new ArrayList();
                    }

                    for (var i = 0; i < this.ObjectsPerConcreteClass; i++)
                    {
                        foreach (var concreteClass in concreteClasses)
                        {
                            ((ArrayList)objectsByMetaType[concreteClass]).Add(saveSession.Create(concreteClass));
                        }
                    }

                    foreach (var t in concreteClasses)
                    {
                        for (var j = 0; j < this.ObjectsPerConcreteClass; j++)
                        {
                            var concreteClass = t;
                            ((ArrayList)objectsByMetaType[concreteClass]).Add(saveSession.Create(concreteClass));
                        }
                    }

                    // Unit Role
                    foreach (var concreteClass in concreteClasses)
                    {
                        IObject[] extent = saveSession.Extent(concreteClass);

                        foreach (var allorsObject in extent)
                        {
                            var roles = concreteClass.UnitRoleTypes;
                            foreach (var role in roles)
                            {
                                var unitTypeTag = (UnitTypeTags)role.ObjectType.UnitTag;
                                switch (unitTypeTag)
                                {
                                    case UnitTypeTags.AllorsString:
                                        allorsObject.Strategy.SetUnitRole(role, this.ValueGenerator.GenerateString(stringLength));
                                        break;
                                    case UnitTypeTags.AllorsInteger:
                                        allorsObject.Strategy.SetUnitRole(role, this.ValueGenerator.GenerateInteger());
                                        break;
                                    case UnitTypeTags.AllorsLong:
                                        allorsObject.Strategy.SetUnitRole(role, this.ValueGenerator.GenerateLong());
                                        break;
                                    case UnitTypeTags.AllorsDecimal:
                                        allorsObject.Strategy.SetUnitRole(role, this.ValueGenerator.GenerateDecimal());
                                        break;
                                    case UnitTypeTags.AllorsDouble:
                                        allorsObject.Strategy.SetUnitRole(role, this.ValueGenerator.GenerateDouble());
                                        break;
                                    case UnitTypeTags.AllorsBoolean:
                                        allorsObject.Strategy.SetUnitRole(role, this.ValueGenerator.GenerateBoolean());
                                        break;
                                    case UnitTypeTags.AllorsDate:
                                        allorsObject.Strategy.SetUnitRole(role, this.ValueGenerator.GenerateDate());
                                        break;
                                    case UnitTypeTags.AllorsDateTime:
                                        allorsObject.Strategy.SetUnitRole(role, this.ValueGenerator.GenerateDateTime());
                                        break;
                                    case UnitTypeTags.AllorsUnique:
                                        allorsObject.Strategy.SetUnitRole(role, this.ValueGenerator.GenerateUnique());
                                        break;
                                    case UnitTypeTags.AllorsBinary:
                                        allorsObject.Strategy.SetUnitRole(role, this.ValueGenerator.GenerateBinary(10));
                                        break;
                                    default:
                                        throw new ArgumentException("Unknown Unit ObjectType: " + unitTypeTag);
                                }
                            }
                        }
                    }

                    // One2One Composite RoleTypes
                    var relationTypes = this.GetOne2OneRelations(this.GetMetaDomain());
                    foreach (var relationType in relationTypes)
                    {
                        var roles = new ArrayList();
                        var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                        for (var iRoleType = roleTypes.Length - 1; iRoleType >= 0; iRoleType--)
                        {
                            var roleType = roleTypes[iRoleType];

                            var rolesForType = (ArrayList)objectsByMetaType[roleType];
                            for (var j = rolesForType.Count - 1; j >= 0; j--)
                            {
                                roles.Add(rolesForType[j]);
                            }
                        }

                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                        foreach (var associationType in associationTypes)
                        {
                            var associations = (ArrayList)objectsByMetaType[associationType];
                            foreach (IObject association in associations)
                            {
                                IObject role = null;
                                if (roles.Count != 0)
                                {
                                    role = (IObject)roles[0];
                                    roles.Remove(0);
                                }

                                association.Strategy.SetCompositeRole(relationType.RoleType, role);
                            }
                        }
                    }

                    // Many2One Composite RoleTypes
                    relationTypes = this.GetMany2OneRelations(this.GetMetaDomain());
                    foreach (var relationType in relationTypes)
                    {
                        var roles = new ArrayList();
                        var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                        for (var iRoleType = roleTypes.Length - 1; iRoleType >= 0; iRoleType--)
                        {
                            var roleType = roleTypes[iRoleType];

                            var rolesForType = (ArrayList)objectsByMetaType[roleType];
                            var reuseRoleType = false;
                            for (var j = rolesForType.Count - 1; j >= 0; j--)
                            {
                                roles.Add(rolesForType[j]);
                                if (reuseRoleType)
                                {
                                    reuseRoleType = false;
                                    roles.Add(rolesForType[j]); // many2one, so role can be reused with other associations
                                }
                                else
                                {
                                    reuseRoleType = true;
                                }
                            }
                        }

                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                        foreach (var associationType in associationTypes)
                        {
                            var associations = (ArrayList)objectsByMetaType[associationType];
                            foreach (IObject association in associations)
                            {
                                IObject role = null;
                                if (roles.Count != 0)
                                {
                                    role = (IObject)roles[0];
                                    roles.Remove(0);
                                }

                                association.Strategy.SetCompositeRole(relationType.RoleType, role);
                            }
                        }
                    }

                    // One2Many Composite RoleTypes
                    relationTypes = this.GetOne2ManyRelations(this.GetMetaDomain());
                    foreach (var relationType in relationTypes)
                    {
                        var roles = new ArrayList();
                        var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                        for (var iRoleType = roleTypes.Length - 1; iRoleType >= 0; iRoleType--)
                        {
                            var roleType = roleTypes[iRoleType];

                            var rolesForType = (ArrayList)objectsByMetaType[roleType];
                            for (var j = rolesForType.Count - 1; j >= 0; j--)
                            {
                                roles.Add(rolesForType[j]);
                            }
                        }

                        // Interleave AssociationTypes
                        var interleavedAssociations = new ArrayList();
                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                        for (var i = 0; i < this.ObjectsPerConcreteClass; i++)
                        {
                            foreach (var associationType in associationTypes)
                            {
                                var associations = (ArrayList)objectsByMetaType[associationType];
                                interleavedAssociations.Add(associations[i]);
                            }
                        }

                        var addTwice = false;
                        foreach (var interleavedAssociation in interleavedAssociations)
                        {
                            var association = (IObject)interleavedAssociation;

                            IObject role = null;
                            if (roles.Count != 0)
                            {
                                role = (IObject)roles[0];
                                roles.Remove(0);
                            }

                            association.Strategy.AddCompositeRole(relationType.RoleType, role);

                            if (addTwice)
                            {
                                addTwice = false;
                                if (roles.Count != 0)
                                {
                                    role = (IObject)roles[0];
                                    roles.Remove(0);
                                }

                                association.Strategy.AddCompositeRole(relationType.RoleType, role);
                            }
                            else
                            {
                                addTwice = true;
                            }
                        }
                    }

                    // Many2Many Composite RoleTypes
                    relationTypes = this.GetMany2ManyRelations(this.GetMetaDomain());
                    foreach (var relationType in relationTypes)
                    {
                        var roles = new ArrayList();
                        var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                        for (var iRoleType = roleTypes.Length - 1; iRoleType >= 0; iRoleType--)
                        {
                            var roleType = roleTypes[iRoleType];

                            var rolesForType = (ArrayList)objectsByMetaType[roleType];
                            var reuseRoleType = false;
                            for (var j = rolesForType.Count - 1; j >= 0; j--)
                            {
                                roles.Add(rolesForType[j]);
                                if (reuseRoleType)
                                {
                                    reuseRoleType = false;
                                    roles.Add(rolesForType[j]); // many2many, so role can be reused with other associations
                                }
                                else
                                {
                                    reuseRoleType = true;
                                }
                            }
                        }

                        // Interleave AssociationTypes
                        var interleavedAssociations = new ArrayList();
                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                        for (var i = 0; i < this.ObjectsPerConcreteClass; i++)
                        {
                            foreach (var associationType in associationTypes)
                            {
                                var associations = (ArrayList)objectsByMetaType[associationType];
                                interleavedAssociations.Add(associations[i]);
                            }
                        }

                        var addTwice = false;
                        foreach (var interleavedAssociation in interleavedAssociations)
                        {
                            var association = (IObject)interleavedAssociation;

                            IObject role = null;
                            if (roles.Count != 0)
                            {
                                role = (IObject)roles[0];
                                roles.Remove(0);
                            }

                            association.Strategy.AddCompositeRole(relationType.RoleType, role);

                            if (addTwice)
                            {
                                addTwice = false;
                                if (roles.Count != 0)
                                {
                                    role = (IObject)roles[0];
                                    roles.Remove(0);
                                }

                                association.Strategy.AddCompositeRole(relationType.RoleType, role);
                            }
                            else
                            {
                                addTwice = true;
                            }
                        }
                    }

                    saveSession.Commit();

                    var xml = this.Save(saveSession);

                    // File.WriteAllText("Population.xml", xml);
                    AssertSchema(xml);

                    this.Load(loadSession, xml);

                    // Use diff
                    foreach (var concreteClass in concreteClasses)
                    {
                        IObject[] saveExtent = saveSession.Extent(concreteClass);

                        foreach (var saveObject in saveExtent)
                        {
                            var loadObject = loadSession.Instantiate(saveObject.Strategy.ObjectId);

                            Assert.False(Diff.Execute(saveObject, loadObject).Length > 0);
                        }
                    }

                    loadSession.Commit();
                }
            }
        }

        /// <summary>
        /// Asserts that the xml conforms to the schema.
        /// </summary>
        /// <param name="xml">The XML document.</param>
        private static void AssertSchema(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);

            var allors = document.DocumentElement;
            Assert.NotNull(allors);
            Assert.Equal("allors", allors.Name);

            var population = (XmlElement)allors.SelectSingleNode("*[1]");
            Assert.NotNull(population);
            Assert.Equal("population", population.Name);
            Assert.Equal("1", population.GetAttribute("version"));
            Assert.Equal(1, allors.SelectNodes("population").Count);

            var objects = (XmlElement)population.SelectSingleNode("*[1]");
            Assert.NotNull(objects);
            Assert.Equal("objects", objects.Name);
            Assert.Equal(1, population.SelectNodes("objects").Count);

            foreach (XmlElement ot in objects.SelectNodes("*/*"))
            {
                Assert.Equal("ot", ot.Name);
                Assert.Equal(0, ot.SelectNodes("*").Count);
                Assert.True(ot.InnerText.Length > 0);
                Assert.AreNotEqual(Guid.Empty, new Guid(ot.GetAttribute("i")));
            }

            var relations = (XmlElement)population.SelectSingleNode("*[2]");
            Assert.NotNull(relations);
            Assert.Equal("relations", relations.Name);
            Assert.Equal(1, population.SelectNodes("relations").Count);

            foreach (XmlElement rtx in relations.SelectNodes("*/*"))
            {
                Assert.True(rtx.Name.Equals("rtu") || rtx.Name.Equals("rtc"));
                Assert.True(rtx.SelectNodes("*").Count > 0);
                Assert.AreNotEqual(Guid.Empty, new Guid(rtx.GetAttribute("i")));

                foreach (XmlElement r in rtx.SelectNodes("*"))
                {
                    Assert.Equal("r", r.Name);
                    Assert.Equal(0, r.SelectNodes("*").Count);

                    if (rtx.Name.Equals("rtc"))
                    {
                        Assert.True(r.InnerText.Length > 0);
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