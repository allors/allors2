// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LifeCycleTest.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
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
    using System.Globalization;

    using Allors;
    using Allors.Adapters;
    using Allors.Meta;

    using NUnit.Framework;

    public abstract class LifeCycleTest : Test
    {
        private readonly bool[] manyFlags = { false, true };

        [Test]
        [Category("Dynamic")]
        public void CreateMany()
        {
            for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
            {
                var repeat = this.GetRepeats()[iRepeat];
                for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                {
                    for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                    {
                        for (var transactionFlagIndex = 0; transactionFlagIndex < this.GetBooleanFlags().Length; transactionFlagIndex++)
                        {
                            var transactionFlag = this.GetBooleanFlags()[transactionFlagIndex];

                            for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                            {
                                var testType = this.GetTestTypes()[iTestType];

                                for (var objectCount = 1; objectCount < 100 * 10; objectCount = objectCount + 100)
                                {
                                    var allorsObjects = this.GetSession().Create(testType, objectCount);

                                    Assert.AreEqual(objectCount, allorsObjects.Length);

                                    this.Commit(transactionFlag);

                                    var ids = new ArrayList();
                                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                    {
                                        Assert.AreEqual(objectCount, allorsObjects.Length);
                                        for (var iAllorsType = 0; iAllorsType < objectCount; iAllorsType++)
                                        {
                                            var allorsObject = allorsObjects[iAllorsType];
                                            Assert.IsFalse(ids.Contains(allorsObject.Strategy.ObjectId.ToString()));
                                            ids.Add(allorsObject.Strategy.ObjectId.ToString());
                                        }

                                        this.Commit(transactionFlag);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (this.IsRollbackSupported())
            {
                for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
                {
                    for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                    {
                        for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                        {
                            for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                            {
                                var testType = this.GetTestTypes()[iTestType];

                                for (var objectCount = 1; objectCount < 5; objectCount = objectCount + 1000)
                                {
                                    var allorsObjects = this.GetSession().Create(testType, objectCount);
                                    var ids = new string[objectCount];
                                    for (var i = 0; i < objectCount; i++)
                                    {
                                        var allorsObject = allorsObjects[i];
                                        ids[i] = allorsObject.Strategy.ObjectId.ToString();
                                    }

                                    Assert.AreEqual(objectCount, allorsObjects.Length);

                                    this.GetSession().Rollback();

                                    allorsObjects = this.GetSession().Instantiate(ids);

                                    Assert.AreEqual(0, allorsObjects.Length);
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test, Explicit]
        [Category("Dynamic")]
        public void Delete()
        {
            for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
            {
                var repeat = this.GetRepeats()[iRepeat];
                for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                {
                    for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                    {
                        for (var transactionFlagIndex = 0; transactionFlagIndex < this.GetBooleanFlags().Length; transactionFlagIndex++)
                        {
                            var transactionFlag = this.GetBooleanFlags()[transactionFlagIndex];

                            for (var secondTransactionFlagIndex = 0; secondTransactionFlagIndex < this.GetBooleanFlags().Length; secondTransactionFlagIndex++)
                            {
                                var secondTransactionFlag = this.GetBooleanFlags()[secondTransactionFlagIndex];
                                for (var thirdTransactionFlagIndex = 0; thirdTransactionFlagIndex < this.GetBooleanFlags().Length; thirdTransactionFlagIndex++)
                                {
                                    var thirdTransactionFlag = this.GetBooleanFlags()[thirdTransactionFlagIndex];

                                    for (var fourthTransactionFlagIndex = 0; fourthTransactionFlagIndex < this.GetBooleanFlags().Length; fourthTransactionFlagIndex++)
                                    {
                                        var fourthTransactionFlag = this.GetBooleanFlags()[fourthTransactionFlagIndex];

                                        for (var useRollbackFlagIndex = 0; useRollbackFlagIndex < this.GetBooleanFlags().Length; useRollbackFlagIndex++)
                                        {
                                            var useRollbackFlag = this.GetBooleanFlags()[useRollbackFlagIndex];
                                            if (!this.IsRollbackSupported())
                                            {
                                                useRollbackFlag = false;
                                            }

                                            for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                                            {
                                                var testType = this.GetTestTypes()[iTestType];

                                                var allorsObject = this.GetSession().Create(testType);
                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.IsFalse(allorsObject.Strategy.IsDeleted);
                                                }

                                                this.Commit(secondTransactionFlag);
                                                allorsObject.Strategy.Delete();
                                                this.Commit(thirdTransactionFlag);

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.IsTrue(allorsObject.Strategy.IsDeleted);
                                                    if (useRollbackFlag)
                                                    {
                                                        this.Rollback(transactionFlag);
                                                    }
                                                    else
                                                    {
                                                        this.Commit(transactionFlag);
                                                    }
                                                }

                                                allorsObject = this.GetSession().Create(testType);
                                                string id = allorsObject.Strategy.ObjectId.ToString();
                                                this.Commit(secondTransactionFlag);
                                                allorsObject.Strategy.Delete();
                                                this.Commit(thirdTransactionFlag);
                                                allorsObject = this.GetSession().Instantiate(id);
                                                this.Commit(fourthTransactionFlag);

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.IsNull(allorsObject);
                                                    if (useRollbackFlag)
                                                    {
                                                        this.Rollback(transactionFlag);
                                                    }
                                                    else
                                                    {
                                                        this.Commit(transactionFlag);
                                                    }
                                                }

                                                IObject proxy = this.GetSession().Create(testType);
                                                id = proxy.Strategy.ObjectId.ToString();
                                                this.Commit(secondTransactionFlag);
                                                IObject subject = this.GetSession().Instantiate(id);
                                                subject.Strategy.Delete();
                                                this.Commit(thirdTransactionFlag);

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.IsTrue(proxy.Strategy.IsDeleted);
                                                    if (useRollbackFlag)
                                                    {
                                                        this.Rollback(transactionFlag);
                                                    }
                                                    else
                                                    {
                                                        this.Commit(transactionFlag);
                                                    }
                                                }

                                                allorsObject = this.GetSession().Create(testType);
                                                IObject[] beforeExtent = this.GetSession().Extent(testType);
                                                this.Commit(secondTransactionFlag);
                                                allorsObject.Strategy.Delete();
                                                this.Commit(thirdTransactionFlag);

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    IObject[] afterExtent = this.GetSession().Extent(testType);
                                                    Assert.AreEqual(beforeExtent.Length, afterExtent.Length + 1);
                                                    if (useRollbackFlag)
                                                    {
                                                        this.Rollback(transactionFlag);
                                                    }
                                                    else
                                                    {
                                                        this.Commit(transactionFlag);
                                                    }
                                                }

                                                // Units
                                                var testRoleTypes = this.GetUnitRoles(testType);
                                                var beforeValues = new Units(true);
                                                for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Length; testRoleTypeIndex++)
                                                {
                                                    var testRoleType = testRoleTypes[testRoleTypeIndex];
                                                    for (var useCachingFlagIndex = 0; useCachingFlagIndex < this.GetBooleanFlags().Length; useCachingFlagIndex++)
                                                    {
                                                        bool useCachingFlag = this.GetBooleanFlags()[useCachingFlagIndex];

                                                        allorsObject = this.GetSession().Create(testType);
                                                        if (useCachingFlag)
                                                        {
                                                            try
                                                            {
                                                                this.GetUnit(allorsObject, testRoleType, Units.Dummy);
                                                            }
                                                            catch
                                                            {
                                                            }
                                                        }

                                                        this.SetUnit(allorsObject, testRoleType, beforeValues);
                                                        this.Commit(secondTransactionFlag);
                                                        allorsObject.Strategy.Delete();
                                                        this.Commit(thirdTransactionFlag);

                                                        for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                        {
                                                            var exceptionThrown = false;
                                                            try
                                                            {
                                                                this.GetUnit(allorsObject, testRoleType, Units.Dummy);
                                                            }
                                                            catch
                                                            {
                                                                exceptionThrown = true;
                                                            }

                                                            Assert.IsTrue(exceptionThrown);
                                                            if (useRollbackFlag)
                                                            {
                                                                this.Rollback(transactionFlag);
                                                            }
                                                            else
                                                            {
                                                                this.Commit(transactionFlag);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            // One2One
                                            var relationTypes = this.GetOne2OneRelations(this.GetMetaDomain());
                                            for (var relationIndex = 0; relationIndex < relationTypes.Length; relationIndex++)
                                            {
                                                var relationType = relationTypes[relationIndex];
                                                for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                                {
                                                    bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];
                                                    for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                    {
                                                        bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];

                                                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                                        {
                                                            var associationType = associationTypes[iAssociationType];
                                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                                            {
                                                                var roleType = roleTypes[iRoleType];

                                                                // delete association
                                                                var association = this.GetSession().Create(associationType);
                                                                var role = this.GetSession().Create(roleType);
                                                                
                                                                if (useRoleCachingFlag)
                                                                {
                                                                    association.Strategy.GetRole(relationType.RoleType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    role.Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                }

                                                                association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                                this.Commit(secondTransactionFlag);
                                                                association.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        association.Strategy.GetRole(relationType.RoleType);
                                                                        role.Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);

                                                                    Assert.IsNull(role.Strategy.GetCompositeAssociation(relationType.AssociationType));

                                                                    if (useRollbackFlag)
                                                                    {
                                                                        this.Rollback(transactionFlag);
                                                                    }
                                                                    else
                                                                    {
                                                                        this.Commit(transactionFlag);
                                                                    }
                                                                }

                                                                // delete role
                                                                association = this.GetSession().Create(associationType);
                                                                role = this.GetSession().Create(roleType);
                                                                
                                                                if (useRoleCachingFlag)
                                                                {
                                                                    association.Strategy.GetRole(relationType.RoleType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    role.Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                }

                                                                association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                                this.Commit(secondTransactionFlag);
                                                                role.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        role.Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);

                                                                    Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));

                                                                    if (useRollbackFlag)
                                                                    {
                                                                        this.Rollback(transactionFlag);
                                                                    }
                                                                    else
                                                                    {
                                                                        this.Commit(transactionFlag);
                                                                    }
                                                                }

                                                                // reuse
                                                                association = this.GetSession().Create(associationType);
                                                                role = this.GetSession().Create(roleType);
                                                                
                                                                this.Commit(secondTransactionFlag);
                                                                role.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);
                                                                }

                                                                association = this.GetSession().Create(associationType);
                                                                role = this.GetSession().Create(roleType);
                                                                
                                                                this.Commit(secondTransactionFlag);
                                                                association.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            // Many2One
                                            relationTypes = this.GetMany2OneRelations(this.GetMetaDomain());
                                            for (var relationIndex = 0; relationIndex < relationTypes.Length; relationIndex++)
                                            {
                                                var relationType = relationTypes[relationIndex];
                                                for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                                {
                                                    bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                    for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                    {
                                                        bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];

                                                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                                        {
                                                            var associationType = associationTypes[iAssociationType];
                                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                                            {
                                                                var roleType = roleTypes[iRoleType];

                                                                // AssociationType
                                                                IObject[] associations = this.CreateArray(relationType.AssociationType.ObjectType, 3);
                                                                associations[0] = this.GetSession().Create(associationType);
                                                                associations[1] = this.GetSession().Create(associationType);
                                                                associations[2] = this.GetSession().Create(associationType);
                                                                IObject[] roles = this.CreateArray(relationType.RoleType.ObjectType, 2);
                                                                roles[0] = this.GetSession().Create(roleType);
                                                                
                                                                roles[1] = this.GetSession().Create(roleType);
                                                                
                                                                if (useRoleCachingFlag)
                                                                {
                                                                    associations[0].Strategy.GetRole(relationType.RoleType);
                                                                    associations[1].Strategy.GetRole(relationType.RoleType);
                                                                    associations[2].Strategy.GetRole(relationType.RoleType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                    roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                }

                                                                associations[0].Strategy.SetCompositeRole(relationType.RoleType, roles[0]);
                                                                associations[1].Strategy.SetCompositeRole(relationType.RoleType, roles[1]);
                                                                associations[2].Strategy.SetCompositeRole(relationType.RoleType, roles[1]);
                                                                this.Commit(secondTransactionFlag);
                                                                associations[0].Strategy.Delete();
                                                                associations[1].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        associations[0].Strategy.GetRole(relationType.RoleType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);
                                                                    exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        associations[1].Strategy.GetRole(relationType.RoleType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);

                                                                    Assert.AreEqual(roles[1], associations[2].Strategy.GetRole(relationType.RoleType));

                                                                    Assert.AreEqual(0, ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                    Assert.AreEqual(1, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                    Assert.AreEqual(associations[2], ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);

                                                                    if (useRollbackFlag)
                                                                    {
                                                                        this.Rollback(transactionFlag);
                                                                    }
                                                                    else
                                                                    {
                                                                        this.Commit(transactionFlag);
                                                                    }
                                                                }

                                                                // Role
                                                                associations = this.CreateArray(relationType.AssociationType.ObjectType, 3);
                                                                associations[0] = this.GetSession().Create(associationType);
                                                                associations[1] = this.GetSession().Create(associationType);
                                                                associations[2] = this.GetSession().Create(associationType);
                                                                roles = this.CreateArray(relationType.RoleType.ObjectType, 2);
                                                                roles[0] = this.GetSession().Create(roleType);
                                                                
                                                                roles[1] = this.GetSession().Create(roleType);

                                                                if (useRoleCachingFlag)
                                                                {
                                                                    associations[0].Strategy.GetRole(relationType.RoleType);
                                                                    associations[1].Strategy.GetRole(relationType.RoleType);
                                                                    associations[2].Strategy.GetRole(relationType.RoleType);
                                                                }

                                                                if (useRoleCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                    roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                }

                                                                associations[0].Strategy.SetCompositeRole(relationType.RoleType, roles[0]);
                                                                associations[1].Strategy.SetCompositeRole(relationType.RoleType, roles[1]);
                                                                associations[2].Strategy.SetCompositeRole(relationType.RoleType, roles[1]);
                                                                this.Commit(secondTransactionFlag);
                                                                roles[0].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    Assert.AreEqual(null, associations[0].Strategy.GetRole(relationType.RoleType));
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);

                                                                    Assert.AreEqual(roles[1], associations[1].Strategy.GetRole(relationType.RoleType));
                                                                    Assert.AreEqual(roles[1], associations[2].Strategy.GetRole(relationType.RoleType));

                                                                    Assert.AreEqual(2, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                    Assert.Contains(associations[1], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType));
                                                                    Assert.Contains(associations[2], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType));

                                                                    if (useRollbackFlag)
                                                                    {
                                                                        this.Rollback(transactionFlag);
                                                                    }
                                                                    else
                                                                    {
                                                                        this.Commit(transactionFlag);
                                                                    }
                                                                }

                                                                // reuse
                                                                var association = this.GetSession().Create(associationType);
                                                                var role = this.GetSession().Create(roleType);
                                                                
                                                                this.Commit(secondTransactionFlag);
                                                                role.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);
                                                                }

                                                                association = this.GetSession().Create(associationType);
                                                                role = this.GetSession().Create(roleType);
                                                                
                                                                this.Commit(secondTransactionFlag);
                                                                association.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            // One2Many
                                            relationTypes = this.GetOne2ManyRelations(this.GetMetaDomain());
                                            for (var relationIndex = 0; relationIndex < relationTypes.Length; relationIndex++)
                                            {
                                                var relationType = relationTypes[relationIndex];
                                                for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                                {
                                                    bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                    for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                    {
                                                        bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];
                                                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                                        {
                                                            var associationType = associationTypes[iAssociationType];
                                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                                            {
                                                                var roleType = roleTypes[iRoleType];

                                                                // AssociationType
                                                                IObject[] associations = this.CreateArray(relationType.AssociationType.ObjectType, 2);
                                                                associations[0] = this.GetSession().Create(associationType);
                                                                associations[1] = this.GetSession().Create(associationType);
                                                                IObject[] roles = this.CreateArray(relationType.RoleType.ObjectType, 3);
                                                                roles[0] = this.GetSession().Create(roleType);
                                                                
                                                                roles[1] = this.GetSession().Create(roleType);
                                                                
                                                                roles[2] = this.GetSession().Create(roleType);

                                                                if (useRoleCachingFlag)
                                                                {
                                                                    associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                    associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                    roles[1].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                    roles[2].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                }

                                                                associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[0]);
                                                                associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[2]);
                                                                this.Commit(secondTransactionFlag);
                                                                associations[0].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);

                                                                    IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                    CollectionAssert.DoesNotContain(association1Roles, roles[0]);
                                                                    CollectionAssert.DoesNotContain(association1Roles, roles[1]);
                                                                    Assert.Contains(roles[2], association1Roles);

                                                                    Assert.IsNull(roles[0].Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                                    Assert.IsNull(roles[1].Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                                    Assert.AreEqual(associations[1], roles[2].Strategy.GetCompositeAssociation(relationType.AssociationType));

                                                                    if (useRollbackFlag)
                                                                    {
                                                                        this.Rollback(transactionFlag);
                                                                    }
                                                                    else
                                                                    {
                                                                        this.Commit(transactionFlag);
                                                                    }
                                                                }

                                                                // Role
                                                                associations = this.CreateArray(relationType.AssociationType.ObjectType, 2);
                                                                associations[0] = this.GetSession().Create(associationType);
                                                                associations[1] = this.GetSession().Create(associationType);
                                                                roles = this.CreateArray(relationType.RoleType.ObjectType, 3);
                                                                roles[0] = this.GetSession().Create(roleType);
                                                                
                                                                roles[1] = this.GetSession().Create(roleType);
                                                                
                                                                roles[2] = this.GetSession().Create(roleType);

                                                                if (useRoleCachingFlag)
                                                                {
                                                                    associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                    associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                    roles[1].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                    roles[2].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                }

                                                                associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[0]);
                                                                associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[2]);
                                                                this.Commit(secondTransactionFlag);
                                                                roles[2].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                    Assert.Contains(roles[0], association0Roles);
                                                                    Assert.Contains(roles[1], association0Roles);
                                                                    Assert.AreEqual(associations[0], roles[0].Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                                    Assert.AreEqual(associations[0], roles[1].Strategy.GetCompositeAssociation(relationType.AssociationType));

                                                                    Assert.AreEqual(0, ((IObject[])associations[1].Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        roles[2].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);

                                                                    if (useRollbackFlag)
                                                                    {
                                                                        this.Rollback(transactionFlag);
                                                                    }
                                                                    else
                                                                    {
                                                                        this.Commit(transactionFlag);
                                                                    }
                                                                }

                                                                // reuse
                                                                var association = this.GetSession().Create(associationType);
                                                                var role = this.GetSession().Create(roleType);
                                                                
                                                                this.Commit(secondTransactionFlag);
                                                                role.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);
                                                                }

                                                                association = this.GetSession().Create(associationType);
                                                                role = this.GetSession().Create(roleType);
                                                                
                                                                this.Commit(secondTransactionFlag);
                                                                association.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            // Many2Many
                                            relationTypes = this.GetMany2ManyRelations(this.GetMetaDomain());
                                            for (var relationIndex = 0; relationIndex < relationTypes.Length; relationIndex++)
                                            {
                                                var relationType = relationTypes[relationIndex];
                                                for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                                {
                                                    bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                    for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                    {
                                                        bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];
                                                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                                        {
                                                            var associationType = associationTypes[iAssociationType];
                                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                                            {
                                                                var roleType = roleTypes[iRoleType];

                                                                // AssociationType
                                                                IObject[] associations = this.CreateArray(relationType.AssociationType.ObjectType, 2);
                                                                associations[0] = this.GetSession().Create(associationType);
                                                                associations[1] = this.GetSession().Create(associationType);
                                                                IObject[] roles = this.CreateArray(relationType.RoleType.ObjectType, 3);
                                                                roles[0] = this.GetSession().Create(roleType);
                                                                
                                                                roles[1] = this.GetSession().Create(roleType);
                                                                
                                                                roles[2] = this.GetSession().Create(roleType);
                                                                
                                                                if (useRoleCachingFlag)
                                                                {
                                                                    associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                    associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                    roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                    roles[2].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                }

                                                                associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[0]);
                                                                associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[2]);
                                                                this.Commit(secondTransactionFlag);
                                                                associations[0].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);

                                                                    IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                    CollectionAssert.DoesNotContain(association1Roles, roles[0]);
                                                                    Assert.Contains(roles[1], association1Roles);
                                                                    Assert.Contains(roles[2], association1Roles);

                                                                    Assert.AreEqual(0, ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                    Assert.AreEqual(1, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                    Assert.AreEqual(associations[1], ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                                    Assert.AreEqual(1, ((IObject[])roles[2].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                    Assert.AreEqual(associations[1], ((IObject[])roles[2].Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);

                                                                    if (useRollbackFlag)
                                                                    {
                                                                        this.Rollback(transactionFlag);
                                                                    }
                                                                    else
                                                                    {
                                                                        this.Commit(transactionFlag);
                                                                    }
                                                                }

                                                                // Role
                                                                associations = this.CreateArray(relationType.AssociationType.ObjectType, 2);
                                                                associations[0] = this.GetSession().Create(associationType);
                                                                associations[1] = this.GetSession().Create(associationType);
                                                                roles = this.CreateArray(relationType.RoleType.ObjectType, 3);
                                                                roles[0] = this.GetSession().Create(roleType);
                                                                roles[1] = this.GetSession().Create(roleType);
                                                                roles[2] = this.GetSession().Create(roleType);

                                                                if (useRoleCachingFlag)
                                                                {
                                                                    associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                    associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                    roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                    roles[2].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                }

                                                                associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[0]);
                                                                associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[2]);
                                                                this.Commit(secondTransactionFlag);
                                                                roles[0].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);

                                                                    IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                    Assert.AreEqual(1, association0Roles.Length);
                                                                    Assert.Contains(roles[1], association0Roles);

                                                                    IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                    Assert.AreEqual(2, association1Roles.Length);
                                                                    Assert.Contains(roles[1], association1Roles);
                                                                    Assert.Contains(roles[2], association1Roles);

                                                                    Assert.AreEqual(2, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                    Assert.Contains(associations[0], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType));
                                                                    Assert.Contains(associations[1], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType));

                                                                    Assert.AreEqual(1, ((IObject[])roles[2].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                    Assert.AreEqual(associations[1], ((IObject[])roles[2].Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);

                                                                    if (useRollbackFlag)
                                                                    {
                                                                        this.Rollback(transactionFlag);
                                                                    }
                                                                    else
                                                                    {
                                                                        this.Commit(transactionFlag);
                                                                    }
                                                                }

                                                                // reuse
                                                                var association = this.GetSession().Create(associationType);
                                                                var role = this.GetSession().Create(roleType);
                                                                
                                                                this.Commit(secondTransactionFlag);
                                                                role.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);
                                                                }

                                                                association = this.GetSession().Create(associationType);
                                                                role = this.GetSession().Create(roleType);
                                                                
                                                                this.Commit(secondTransactionFlag);
                                                                association.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.IsTrue(exceptionThrown);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (this.IsRollbackSupported())
            {
                for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
                {
                    var repeat = this.GetRepeats()[iRepeat];
                    for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                    {
                        for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                        {
                            for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                            {
                                var testType = this.GetTestTypes()[iTestType];

                                for (var useRollbackFlagIndex = 0; useRollbackFlagIndex < this.GetBooleanFlags().Length; useRollbackFlagIndex++)
                                {
                                    var useRollbackFlag = this.GetBooleanFlags()[useRollbackFlagIndex];

                                    for (var transactionFlagIndex = 0; transactionFlagIndex < this.GetBooleanFlags().Length; transactionFlagIndex++)
                                    {
                                        var transactionFlag = this.GetBooleanFlags()[transactionFlagIndex];

                                        for (var secondTransactionFlagIndex = 0; secondTransactionFlagIndex < this.GetBooleanFlags().Length; secondTransactionFlagIndex++)
                                        {
                                            var secondTransactionFlag = this.GetBooleanFlags()[secondTransactionFlagIndex];

                                            // Rollback
                                            var allorsObject = this.GetSession().Create(testType);
                                            allorsObject.Strategy.Delete();
                                            this.GetSession().Rollback();

                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.IsTrue(allorsObject.Strategy.IsDeleted);
                                                if (useRollbackFlag)
                                                {
                                                    this.Rollback(transactionFlag);
                                                }
                                                else
                                                {
                                                    this.Commit(transactionFlag);
                                                }
                                            }

                                            allorsObject = this.GetSession().Create(testType);
                                            string id = allorsObject.Strategy.ObjectId.ToString();
                                            allorsObject.Strategy.Delete();
                                            this.GetSession().Rollback();
                                            allorsObject = this.GetSession().Instantiate(id);
                                            this.Commit(secondTransactionFlag);

                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.IsNull(allorsObject);
                                                if (useRollbackFlag)
                                                {
                                                    this.Rollback(transactionFlag);
                                                }
                                                else
                                                {
                                                    this.Commit(transactionFlag);
                                                }
                                            }

                                            // Commit + Rollback
                                            allorsObject = this.GetSession().Create(testType);
                                            this.GetSession().Commit();
                                            allorsObject.Strategy.Delete();
                                            this.GetSession().Rollback();

                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.IsFalse(allorsObject.Strategy.IsDeleted);
                                                if (useRollbackFlag)
                                                {
                                                    this.Rollback(transactionFlag);
                                                }
                                                else
                                                {
                                                    this.Commit(transactionFlag);
                                                }
                                            }

                                            allorsObject = this.GetSession().Create(testType);
                                            id = allorsObject.Strategy.ObjectId.ToString();
                                            this.GetSession().Commit();
                                            allorsObject.Strategy.Delete();
                                            this.GetSession().Rollback();
                                            allorsObject = this.GetSession().Instantiate(id);
                                            this.Commit(secondTransactionFlag);

                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.IsFalse(allorsObject.Strategy.IsDeleted);
                                                if (useRollbackFlag)
                                                {
                                                    this.Rollback(transactionFlag);
                                                }
                                                else
                                                {
                                                    this.Commit(transactionFlag);
                                                }
                                            }

                                            IObject proxy = this.GetSession().Create(testType);
                                            id = proxy.Strategy.ObjectId.ToString();
                                            this.GetSession().Commit();
                                            IObject subject = this.GetSession().Instantiate(id);
                                            subject.Strategy.Delete();
                                            this.GetSession().Rollback();

                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.IsFalse(proxy.Strategy.IsDeleted);
                                                if (useRollbackFlag)
                                                {
                                                    this.Rollback(transactionFlag);
                                                }
                                                else
                                                {
                                                    this.Commit(transactionFlag);
                                                }
                                            }

                                            allorsObject = this.GetSession().Create(testType);
                                            IObject[] beforeExtent = this.GetSession().Extent(testType);
                                            id = allorsObject.Strategy.ObjectId.ToString();
                                            this.GetSession().Commit();
                                            allorsObject.Strategy.Delete();
                                            this.GetSession().Rollback();
                                            this.GetSession().Instantiate(id);
                                            this.Commit(secondTransactionFlag);

                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                IObject[] afterExtent = this.GetSession().Extent(testType);
                                                Assert.AreEqual(beforeExtent.Length, afterExtent.Length);
                                                if (useRollbackFlag)
                                                {
                                                    this.Rollback(transactionFlag);
                                                }
                                                else
                                                {
                                                    this.Commit(transactionFlag);
                                                }
                                            }

                                            // Rollback + Rollback
                                            allorsObject = this.GetSession().Create(testType);
                                            this.GetSession().Rollback();
                                            var exceptionThrown = false;
                                            try
                                            {
                                                allorsObject.Strategy.Delete();
                                            }
                                            catch
                                            {
                                                exceptionThrown = true;
                                            }

                                            Assert.IsTrue(exceptionThrown);

                                            // Units
                                            var testRoleTypes = this.GetUnitRoles(testType);
                                            var beforeValues = new Units(true);
                                            for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Length; testRoleTypeIndex++)
                                            {
                                                var testRoleType = testRoleTypes[testRoleTypeIndex];
                                                for (var useCachingFlagIndex = 0; useCachingFlagIndex < this.GetBooleanFlags().Length; useCachingFlagIndex++)
                                                {
                                                    bool useCachingFlag = this.GetBooleanFlags()[useCachingFlagIndex];

                                                    // Rollback
                                                    allorsObject = this.GetSession().Create(testType);
                                                    if (useCachingFlag)
                                                    {
                                                        this.GetUnit(allorsObject, testRoleType, Units.Dummy);
                                                    }

                                                    this.SetUnit(allorsObject, testRoleType, beforeValues);
                                                    allorsObject.Strategy.Delete();
                                                    this.GetSession().Rollback();

                                                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                    {
                                                        exceptionThrown = false;
                                                        try
                                                        {
                                                            this.GetUnit(allorsObject, testRoleType, Units.Dummy);
                                                        }
                                                        catch
                                                        {
                                                            exceptionThrown = true;
                                                        }

                                                        Assert.IsTrue(exceptionThrown);
                                                        if (useRollbackFlag)
                                                        {
                                                            this.Rollback(transactionFlag);
                                                        }
                                                        else
                                                        {
                                                            this.Commit(transactionFlag);
                                                        }
                                                    }

                                                    // Commit + Rollback
                                                    allorsObject = this.GetSession().Create(testType);
                                                    if (useCachingFlag)
                                                    {
                                                        this.GetUnit(allorsObject, testRoleType, Units.Dummy);
                                                    }

                                                    this.SetUnit(allorsObject, testRoleType, beforeValues);
                                                    this.GetSession().Commit();
                                                    allorsObject.Strategy.Delete();
                                                    this.GetSession().Rollback();

                                                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                    {
                                                        this.GetUnit(allorsObject, testRoleType, Units.Dummy);
                                                        if (useRollbackFlag)
                                                        {
                                                            this.Rollback(transactionFlag);
                                                        }
                                                        else
                                                        {
                                                            this.Commit(transactionFlag);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // One2One
                                        var relationTypes = this.GetOne2OneRelations(this.GetMetaDomain());
                                        for (var relationIndex = 0; relationIndex < relationTypes.Length; relationIndex++)
                                        {
                                            var relationType = relationTypes[relationIndex];
                                            for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                            {
                                                bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];
                                                for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                {
                                                    bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];

                                                    var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                                    for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                                    {
                                                        var associationType = associationTypes[iAssociationType];
                                                        var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                                        for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                                        {
                                                            var roleType = roleTypes[iRoleType];

                                                            var association = this.GetSession().Create(associationType);
                                                            var role = this.GetSession().Create(roleType);
                                                            
                                                            if (useRoleCachingFlag)
                                                            {
                                                                association.Strategy.GetRole(relationType.RoleType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                role.Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                            }

                                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                            this.GetSession().Commit();

                                                            // delete association
                                                            association.Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                Assert.AreEqual(association, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                                Assert.AreEqual(role, association.Strategy.GetRole(relationType.RoleType));

                                                                if (useRollbackFlag)
                                                                {
                                                                    this.Rollback(transactionFlag);
                                                                }
                                                                else
                                                                {
                                                                    this.Commit(transactionFlag);
                                                                }
                                                            }

                                                            // delete role
                                                            role.Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                Assert.AreEqual(association, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                                Assert.AreEqual(role, association.Strategy.GetRole(relationType.RoleType));

                                                                if (useRollbackFlag)
                                                                {
                                                                    this.Rollback(transactionFlag);
                                                                }
                                                                else
                                                                {
                                                                    this.Commit(transactionFlag);
                                                                }
                                                            }

                                                            // reuse
                                                            association = this.GetSession().Create(associationType);
                                                            role = this.GetSession().Create(roleType);
                                                            
                                                            this.GetSession().Commit();

                                                            role.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);

                                                            association.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // Many2One
                                        relationTypes = this.GetMany2OneRelations(this.GetMetaDomain());
                                        for (var relationIndex = 0; relationIndex < relationTypes.Length; relationIndex++)
                                        {
                                            var relationType = relationTypes[relationIndex];
                                            for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                            {
                                                bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                {
                                                    bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];

                                                    var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                                    for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                                    {
                                                        var associationType = associationTypes[iAssociationType];
                                                        var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                                        for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                                        {
                                                            var roleType = roleTypes[iRoleType];

                                                            // AssociationType
                                                            IObject[] associations = this.CreateArray(relationType.AssociationType.ObjectType, 3);
                                                            associations[0] = this.GetSession().Create(associationType);
                                                            associations[1] = this.GetSession().Create(associationType);
                                                            associations[2] = this.GetSession().Create(associationType);
                                                            IObject[] roles = this.CreateArray(relationType.RoleType.ObjectType, 2);
                                                            roles[0] = this.GetSession().Create(roleType);
                                                            
                                                            roles[1] = this.GetSession().Create(roleType);

                                                            if (useRoleCachingFlag)
                                                            {
                                                                associations[0].Strategy.GetRole(relationType.RoleType);
                                                                associations[1].Strategy.GetRole(relationType.RoleType);
                                                                associations[2].Strategy.GetRole(relationType.RoleType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                            }

                                                            associations[0].Strategy.SetCompositeRole(relationType.RoleType, roles[0]);
                                                            associations[1].Strategy.SetCompositeRole(relationType.RoleType, roles[1]);
                                                            associations[2].Strategy.SetCompositeRole(relationType.RoleType, roles[1]);
                                                            this.GetSession().Commit();
                                                            associations[0].Strategy.Delete();
                                                            associations[1].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                Assert.AreEqual(roles[0], associations[0].Strategy.GetRole(relationType.RoleType));
                                                                Assert.AreEqual(roles[1], associations[1].Strategy.GetRole(relationType.RoleType));
                                                                Assert.AreEqual(roles[1], associations[2].Strategy.GetRole(relationType.RoleType));

                                                                Assert.AreEqual(1, ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                Assert.AreEqual(associations[0], ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                                Assert.AreEqual(2, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                Assert.Contains(associations[1], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType));
                                                                Assert.Contains(associations[2], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType));

                                                                if (useRollbackFlag)
                                                                {
                                                                    this.Rollback(transactionFlag);
                                                                }
                                                                else
                                                                {
                                                                    this.Commit(transactionFlag);
                                                                }
                                                            }

                                                            // Role
                                                            associations = this.CreateArray(relationType.AssociationType.ObjectType, 3);
                                                            associations[0] = this.GetSession().Create(associationType);
                                                            associations[1] = this.GetSession().Create(associationType);
                                                            associations[2] = this.GetSession().Create(associationType);
                                                            roles = this.CreateArray(relationType.RoleType.ObjectType, 2);
                                                            roles[0] = this.GetSession().Create(roleType);
                                                            
                                                            roles[1] = this.GetSession().Create(roleType);

                                                            if (useRoleCachingFlag)
                                                            {
                                                                associations[0].Strategy.GetRole(relationType.RoleType);
                                                                associations[1].Strategy.GetRole(relationType.RoleType);
                                                                associations[2].Strategy.GetRole(relationType.RoleType);
                                                            }

                                                            if (useRoleCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                            }

                                                            associations[0].Strategy.SetCompositeRole(relationType.RoleType, roles[0]);
                                                            associations[1].Strategy.SetCompositeRole(relationType.RoleType, roles[1]);
                                                            associations[2].Strategy.SetCompositeRole(relationType.RoleType, roles[1]);
                                                            this.GetSession().Commit();
                                                            roles[0].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                Assert.AreEqual(roles[0], associations[0].Strategy.GetRole(relationType.RoleType));
                                                                Assert.AreEqual(roles[1], associations[1].Strategy.GetRole(relationType.RoleType));
                                                                Assert.AreEqual(roles[1], associations[2].Strategy.GetRole(relationType.RoleType));

                                                                Assert.AreEqual(1, ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                Assert.AreEqual(associations[0], ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                                Assert.AreEqual(2, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                                Assert.Contains(associations[1], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType));
                                                                Assert.Contains(associations[2], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType));

                                                                if (useRollbackFlag)
                                                                {
                                                                    this.Rollback(transactionFlag);
                                                                }
                                                                else
                                                                {
                                                                    this.Commit(transactionFlag);
                                                                }
                                                            }

                                                            // reuse
                                                            var association = this.GetSession().Create(associationType);
                                                            var role = this.GetSession().Create(roleType);
                                                            
                                                            this.GetSession().Commit();

                                                            role.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);

                                                            association.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // One2Many
                                        relationTypes = this.GetOne2ManyRelations(this.GetMetaDomain());
                                        for (var relationIndex = 0; relationIndex < relationTypes.Length; relationIndex++)
                                        {
                                            var relationType = relationTypes[relationIndex];
                                            for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                            {
                                                bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                {
                                                    bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];
                                                    var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                                    for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                                    {
                                                        var associationType = associationTypes[iAssociationType];
                                                        var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                                        for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                                        {
                                                            var roleType = roleTypes[iRoleType];

                                                            // AssociationType
                                                            IObject[] associations = this.CreateArray(relationType.AssociationType.ObjectType, 2);
                                                            associations[0] = this.GetSession().Create(associationType);
                                                            associations[1] = this.GetSession().Create(associationType);
                                                            IObject[] roles = this.CreateArray(relationType.RoleType.ObjectType, 3);
                                                            roles[0] = this.GetSession().Create(roleType);
                                                            
                                                            roles[1] = this.GetSession().Create(roleType);
                                                            
                                                            roles[2] = this.GetSession().Create(roleType);

                                                            if (useRoleCachingFlag)
                                                            {
                                                                associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                roles[1].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                roles[2].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                            }

                                                            associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[0]);
                                                            associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[2]);
                                                            this.GetSession().Commit();
                                                            associations[0].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                Assert.AreEqual(2, association0Roles.Length);
                                                                Assert.Contains(roles[0], association0Roles);
                                                                Assert.Contains(roles[1], association0Roles);

                                                                IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                Assert.AreEqual(1, association1Roles.Length);
                                                                Assert.Contains(roles[2], association1Roles);

                                                                Assert.AreEqual(associations[0], roles[0].Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                                Assert.AreEqual(associations[0], roles[1].Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                                Assert.AreEqual(associations[1], roles[2].Strategy.GetCompositeAssociation(relationType.AssociationType));

                                                                if (useRollbackFlag)
                                                                {
                                                                    this.Rollback(transactionFlag);
                                                                }
                                                                else
                                                                {
                                                                    this.Commit(transactionFlag);
                                                                }
                                                            }

                                                            // Role
                                                            associations = this.CreateArray(relationType.AssociationType.ObjectType, 2);
                                                            associations[0] = this.GetSession().Create(associationType);
                                                            associations[1] = this.GetSession().Create(associationType);
                                                            roles = this.CreateArray(relationType.RoleType.ObjectType, 3);
                                                            roles[0] = this.GetSession().Create(roleType);
                                                            
                                                            roles[1] = this.GetSession().Create(roleType);
                                                            
                                                            roles[2] = this.GetSession().Create(roleType);
                                                            
                                                            if (useRoleCachingFlag)
                                                            {
                                                                associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                roles[1].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                                roles[2].Strategy.GetCompositeAssociation(relationType.AssociationType);
                                                            }

                                                            associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[0]);
                                                            associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[2]);
                                                            this.GetSession().Commit();
                                                            roles[2].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                Assert.AreEqual(2, association0Roles.Length);
                                                                Assert.Contains(roles[0], association0Roles);
                                                                Assert.Contains(roles[1], association0Roles);

                                                                IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                Assert.AreEqual(1, association1Roles.Length);
                                                                Assert.Contains(roles[2], association1Roles);

                                                                Assert.AreEqual(associations[0], roles[0].Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                                Assert.AreEqual(associations[0], roles[1].Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                                Assert.AreEqual(associations[1], roles[2].Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                            }

                                                            // reuse
                                                            var association = this.GetSession().Create(associationType);
                                                            var role = this.GetSession().Create(roleType);
                                                            
                                                            this.GetSession().Commit();

                                                            role.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.AddCompositeRole(relationType.RoleType, role);

                                                            association.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // Many2Many
                                        relationTypes = this.GetMany2ManyRelations(this.GetMetaDomain());
                                        for (var relationIndex = 0; relationIndex < relationTypes.Length; relationIndex++)
                                        {
                                            var relationType = relationTypes[relationIndex];
                                            for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                            {
                                                bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                {
                                                    bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];
                                                    var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                                    for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                                    {
                                                        var associationType = associationTypes[iAssociationType];
                                                        var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                                        for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                                        {
                                                            var roleType = roleTypes[iRoleType];

                                                            // AssociationType
                                                            IObject[] associations = this.CreateArray(relationType.AssociationType.ObjectType, 2);
                                                            associations[0] = this.GetSession().Create(associationType);
                                                            associations[1] = this.GetSession().Create(associationType);
                                                            IObject[] roles = this.CreateArray(relationType.RoleType.ObjectType, 3);
                                                            roles[0] = this.GetSession().Create(roleType);
                                                            
                                                            roles[1] = this.GetSession().Create(roleType);
                                                            
                                                            roles[2] = this.GetSession().Create(roleType);

                                                            if (useRoleCachingFlag)
                                                            {
                                                                associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                roles[2].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                            }

                                                            associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[0]);
                                                            associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[2]);
                                                            this.GetSession().Commit();
                                                            associations[0].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                Assert.AreEqual(2, association0Roles.Length);
                                                                Assert.Contains(roles[0], association0Roles);
                                                                Assert.Contains(roles[1], association0Roles);

                                                                IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                Assert.AreEqual(2, association1Roles.Length);
                                                                Assert.Contains(roles[1], association1Roles);
                                                                Assert.Contains(roles[2], association1Roles);

                                                                IObject[] role0Associations = roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                Assert.AreEqual(1, role0Associations.Length);
                                                                Assert.AreEqual(associations[0], role0Associations[0]);
                                                                IObject[] role1Associations = roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                Assert.AreEqual(2, role1Associations.Length);
                                                                Assert.Contains(associations[0], role1Associations);
                                                                Assert.Contains(associations[1], role1Associations);
                                                                IObject[] role2Associations = roles[2].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                Assert.AreEqual(1, role2Associations.Length);
                                                                Assert.AreEqual(associations[1], role2Associations[0]);

                                                                if (useRollbackFlag)
                                                                {
                                                                    this.Rollback(transactionFlag);
                                                                }
                                                                else
                                                                {
                                                                    this.Commit(transactionFlag);
                                                                }
                                                            }

                                                            // Role
                                                            associations = this.CreateArray(relationType.AssociationType.ObjectType, 2);
                                                            associations[0] = this.GetSession().Create(associationType);
                                                            associations[1] = this.GetSession().Create(associationType);
                                                            roles = this.CreateArray(relationType.RoleType.ObjectType, 3);
                                                            roles[0] = this.GetSession().Create(roleType);
                                                            roles[1] = this.GetSession().Create(roleType);
                                                            roles[2] = this.GetSession().Create(roleType);

                                                            if (useRoleCachingFlag)
                                                            {
                                                                associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                roles[2].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                            }

                                                            associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[0]);
                                                            associations[0].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType.RoleType, roles[2]);
                                                            this.GetSession().Commit();
                                                            roles[0].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                Assert.AreEqual(2, association0Roles.Length);
                                                                Assert.Contains(roles[0], association0Roles);
                                                                Assert.Contains(roles[1], association0Roles);

                                                                IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType.RoleType);
                                                                Assert.AreEqual(2, association1Roles.Length);
                                                                Assert.Contains(roles[1], association1Roles);
                                                                Assert.Contains(roles[2], association1Roles);

                                                                IObject[] role0Associations = roles[0].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                Assert.AreEqual(1, role0Associations.Length);
                                                                Assert.AreEqual(associations[0], role0Associations[0]);
                                                                IObject[] role1Associations = roles[1].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                Assert.AreEqual(2, role1Associations.Length);
                                                                Assert.Contains(associations[0], role1Associations);
                                                                Assert.Contains(associations[1], role1Associations);
                                                                IObject[] role2Associations = roles[2].Strategy.GetCompositeAssociations(relationType.AssociationType);
                                                                Assert.AreEqual(1, role2Associations.Length);
                                                                Assert.AreEqual(associations[1], role2Associations[0]);
                                                            }

                                                            // reuse
                                                            var association = this.GetSession().Create(associationType);
                                                            var role = this.GetSession().Create(roleType);
                                                            
                                                            this.GetSession().Commit();

                                                            role.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.AddCompositeRole(relationType.RoleType, role);

                                                            association.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        [Category("Dynamic")]
        public void Identity()
        {
            for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
            {
                var repeat = this.GetRepeats()[iRepeat];
                for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                {
                    for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                    {
                        for (var transactionFlagIndex = 0; transactionFlagIndex < this.GetBooleanFlags().Length; transactionFlagIndex++)
                        {
                            var transactionFlag = this.GetBooleanFlags()[transactionFlagIndex];

                            for (var secondTransactionFlagIndex = 0; secondTransactionFlagIndex < this.GetBooleanFlags().Length; secondTransactionFlagIndex++)
                            {
                                var secondTransactionFlag = this.GetBooleanFlags()[secondTransactionFlagIndex];
                                for (var thirdTransactionFlagIndex = 0; thirdTransactionFlagIndex < this.GetBooleanFlags().Length; thirdTransactionFlagIndex++)
                                {
                                    for (var fourthTransactionFlagIndex = 0; fourthTransactionFlagIndex < this.GetBooleanFlags().Length; fourthTransactionFlagIndex++)
                                    {
                                        for (var useRollbackFlagIndex = 0; useRollbackFlagIndex < this.GetBooleanFlags().Length; useRollbackFlagIndex++)
                                        {
                                            var useRollbackFlag = this.GetBooleanFlags()[useRollbackFlagIndex];
                                            if (!this.IsRollbackSupported())
                                            {
                                                useRollbackFlag = false;
                                            }

                                            for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                                            {
                                                var testType = this.GetTestTypes()[iTestType];

                                                var anObject = this.GetSession().Create(testType);
                                                var anId = anObject.Strategy.ObjectId.ToString();
                                                var aProxy = this.GetSession().Instantiate(anId);

                                                var anotherObject = this.GetSession().Create(testType);
                                                var anotherId = anotherObject.Strategy.ObjectId.ToString();
                                                var anotherProxy = this.GetSession().Instantiate(anotherId);

                                                this.Commit(secondTransactionFlag);
                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.AreEqual(anObject, aProxy);
                                                    Assert.AreEqual(anotherObject, anotherProxy);
                                                    Assert.AreNotEqual(anObject, anotherObject);
                                                    Assert.AreNotEqual(anObject, anotherProxy);
                                                    Assert.AreNotEqual(aProxy, anotherObject);
                                                    Assert.AreNotEqual(aProxy, anotherProxy);

                                                    if (useRollbackFlag)
                                                    {
                                                        this.Rollback(transactionFlag);
                                                    }
                                                    else
                                                    {
                                                        this.Commit(transactionFlag);
                                                    }
                                                }

                                                anObject = this.GetSession().Create(testType);
                                                anId = anObject.Strategy.ObjectId.ToString();

                                                anotherObject = this.GetSession().Create(testType);
                                                anotherId = anotherObject.Strategy.ObjectId.ToString();

                                                this.Commit(secondTransactionFlag);

                                                anObject = this.GetSession().Instantiate(anId);
                                                aProxy = this.GetSession().Instantiate(anId);
                                                anotherObject = this.GetSession().Instantiate(anotherId);
                                                anotherProxy = this.GetSession().Instantiate(anotherId);

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.AreEqual(anObject, aProxy);
                                                    Assert.AreEqual(anotherObject, anotherProxy);
                                                    Assert.AreNotEqual(anObject, anotherObject);
                                                    Assert.AreNotEqual(anObject, anotherProxy);
                                                    Assert.AreNotEqual(aProxy, anotherObject);
                                                    Assert.AreNotEqual(aProxy, anotherProxy);

                                                    if (useRollbackFlag)
                                                    {
                                                        this.Rollback(transactionFlag);
                                                    }
                                                    else
                                                    {
                                                        this.Commit(transactionFlag);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (this.IsRollbackSupported())
            {
                for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
                {
                    var repeat = this.GetRepeats()[iRepeat];
                    for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                    {
                        for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                        {
                            for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                            {
                                var testType = this.GetTestTypes()[iTestType];

                                for (var useRollbackFlagIndex = 0; useRollbackFlagIndex < this.GetBooleanFlags().Length; useRollbackFlagIndex++)
                                {
                                    var useRollbackFlag = this.GetBooleanFlags()[useRollbackFlagIndex];

                                    for (var transactionFlagIndex = 0; transactionFlagIndex < this.GetBooleanFlags().Length; transactionFlagIndex++)
                                    {
                                        var transactionFlag = this.GetBooleanFlags()[transactionFlagIndex];

                                        for (var secondTransactionFlagIndex = 0; secondTransactionFlagIndex < this.GetBooleanFlags().Length; secondTransactionFlagIndex++)
                                        {
                                            var anObject = this.GetSession().Create(testType);
                                            var anId = anObject.Strategy.ObjectId.ToString();
                                            var aProxy = this.GetSession().Instantiate(anId);

                                            var anotherObject = this.GetSession().Create(testType);
                                            var anotherId = anotherObject.Strategy.ObjectId.ToString();
                                            var anotherProxy = this.GetSession().Instantiate(anotherId);

                                            this.GetSession().Rollback();
                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.AreEqual(anObject, aProxy);
                                                Assert.AreEqual(anotherObject, anotherProxy);
                                                Assert.AreNotEqual(anObject, anotherObject);
                                                Assert.AreNotEqual(anObject, anotherProxy);
                                                Assert.AreNotEqual(aProxy, anotherObject);
                                                Assert.AreNotEqual(aProxy, anotherProxy);

                                                if (useRollbackFlag)
                                                {
                                                    this.Rollback(transactionFlag);
                                                }
                                                else
                                                {
                                                    this.Commit(transactionFlag);
                                                }
                                            }

                                            anObject = this.GetSession().Create(testType);
                                            anId = anObject.Strategy.ObjectId.ToString();
                                            this.GetSession().Instantiate(anId); // aProxy

                                            anotherObject = this.GetSession().Create(testType);
                                            anotherId = anotherObject.Strategy.ObjectId.ToString();
                                            this.GetSession().Instantiate(anotherId); // anotherProxy

                                            this.GetSession().Rollback();

                                            anObject = this.GetSession().Instantiate(anId);
                                            aProxy = this.GetSession().Instantiate(anId);

                                            anotherObject = this.GetSession().Instantiate(anotherId);
                                            anotherProxy = this.GetSession().Instantiate(anotherId);

                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.IsNull(anObject);
                                                Assert.IsNull(aProxy);
                                                Assert.IsNull(anotherObject);
                                                Assert.IsNull(anotherProxy);

                                                if (useRollbackFlag)
                                                {
                                                    this.Rollback(transactionFlag);
                                                }
                                                else
                                                {
                                                    this.Commit(transactionFlag);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        [Category("Dynamic")]
        public void Insert()
        {
            var session = this.GetSession() as IDatabaseSession;

            if (session != null)
            {
                for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
                {
                    for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                    {
                        var testRepeat = this.GetTestRepeats()[iTestRepeat];
                        for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                        {
                            for (var iTransactionFlag = 0; iTransactionFlag < this.GetBooleanFlags().Length; iTransactionFlag++)
                            {
                                var transactionFlag = this.GetBooleanFlags()[iTransactionFlag];

                                for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                                {
                                    var testType = this.GetTestTypes()[iTestType];
                                    var object1 = session.Create(testType);
                                    IObject object2;

                                    this.Commit(transactionFlag);

                                    var id = int.Parse(object1.Strategy.ObjectId.ToString());
                                    if (id >= 0)
                                    {
                                        object2 = session.Insert(testType, (id + 1).ToString());
                                    }
                                    else
                                    {
                                        object2 = session.Insert(testType, (id - 1).ToString());
                                    }

                                    var object3 = session.Create(testType);

                                    for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                    {
                                        Assert.AreNotEqual(object1.Strategy.ObjectId, object2.Strategy.ObjectId);
                                        Assert.AreNotEqual(object1.Strategy.ObjectId, object3.Strategy.ObjectId);
                                        Assert.AreNotEqual(object2.Strategy.ObjectId, object3.Strategy.ObjectId);
                                        this.Commit(transactionFlag);
                                    }

                                    IObject[] objects = { object1, object2, object3 };
                                    for (var iObject = 0; iObject < objects.Length; iObject++)
                                    {
                                        var exceptionThrown = false;
                                        try
                                        {
                                            session.Insert(testType, objects[iObject].Strategy.ObjectId);
                                        }
                                        catch
                                        {
                                            exceptionThrown = true;
                                        }

                                        Assert.IsTrue(exceptionThrown);
                                        this.Commit(transactionFlag);
                                    }

                                    // different repository
                                    var otherObject1 = this.GetSession2().Insert(testType, object1.Strategy.ObjectId);
                                    Assert.AreNotEqual(object1, otherObject1);
                                }
                            }
                        }
                    }
                }

                if (this.IsRollbackSupported())
                {
                    for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
                    {
                        for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                        {
                            var testRepeat = this.GetTestRepeats()[iTestRepeat];
                            for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                            {
                                for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                                {
                                    var testType = this.GetTestTypes()[iTestType];
                                    var object1 = session.Create(testType);
                                    IObject object2;

                                    var id = int.Parse(object1.Strategy.ObjectId.ToString());
                                    if (id >= 0)
                                    {
                                        object2 = session.Insert(testType, (id + 1).ToString(CultureInfo.InvariantCulture));
                                    }
                                    else
                                    {
                                        object2 = session.Insert(testType, (id - 1).ToString(CultureInfo.InvariantCulture));
                                    }

                                    var object3 = session.Create(testType);

                                    for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                    {
                                        Assert.AreNotEqual(object1.Strategy.ObjectId, object2.Strategy.ObjectId);
                                        Assert.AreNotEqual(object1.Strategy.ObjectId, object3.Strategy.ObjectId);
                                        Assert.AreNotEqual(object2.Strategy.ObjectId, object3.Strategy.ObjectId);
                                    }

                                    session.Rollback();

                                    IObject[] objects = { object1, object2, object3 };
                                    for (var iObject = 0; iObject < objects.Length; iObject++)
                                    {
                                        session.Insert(testType, objects[iObject].Strategy.ObjectId);
                                    }

                                    for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                    {
                                        Assert.AreNotEqual(object1.Strategy.ObjectId, object2.Strategy.ObjectId);
                                        Assert.AreNotEqual(object1.Strategy.ObjectId, object3.Strategy.ObjectId);
                                        Assert.AreNotEqual(object2.Strategy.ObjectId, object3.Strategy.ObjectId);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        [Category("Dynamic")]
        public void Instantiate()
        {
            for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
            {
                var repeat = this.GetRepeats()[iRepeat];
                for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                {
                    var testRepeat = this.GetTestRepeats()[iTestRepeat];
                    for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                    {
                        for (var iManyFlag = 0; iManyFlag < this.manyFlags.Length; iManyFlag++)
                        {
                            bool manyFlag = this.manyFlags[iManyFlag];

                            for (var transactionFlagIndex = 0; transactionFlagIndex < this.GetBooleanFlags().Length; transactionFlagIndex++)
                            {
                                var transactionFlag = this.GetBooleanFlags()[transactionFlagIndex];

                                for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                                {
                                    var testType = this.GetTestTypes()[iTestType];
                                    {
                                        // Non existing Id's
                                        for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            IObject unexistingObject = this.Instantiate(int.MaxValue - 1, manyFlag);
                                            Assert.IsNull(unexistingObject);
                                        }
                                    }

                                    {
                                        // Equality & Id's
                                        var anObject = this.GetSession().Create(testType);
                                        var id = int.Parse(anObject.Strategy.ObjectId.ToString());
                                        IObject sameObject = this.Instantiate(id, manyFlag);

                                        for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            for (var testRepeatIndex = 0;
                                                 testRepeatIndex < testRepeat;
                                                 testRepeatIndex++)
                                            {
                                                Assert.AreEqual(anObject, sameObject);
                                                Assert.AreEqual(anObject.Strategy.ObjectId, sameObject.Strategy.ObjectId);
                                                this.Commit(transactionFlag);
                                            }

                                            sameObject = this.Instantiate(id, manyFlag);

                                            for (var testRepeatIndex = 0;
                                                 testRepeatIndex < testRepeat;
                                                 testRepeatIndex++)
                                            {
                                                Assert.AreEqual(anObject, sameObject);
                                                Assert.AreEqual(anObject.Strategy.ObjectId, sameObject.Strategy.ObjectId);
                                                this.Commit(transactionFlag);
                                            }

                                            anObject = this.Instantiate(id, manyFlag);

                                            for (var testRepeatIndex = 0;
                                                 testRepeatIndex < testRepeat;
                                                 testRepeatIndex++)
                                            {
                                                Assert.AreEqual(anObject, sameObject);
                                                Assert.AreEqual(anObject.Strategy.ObjectId, sameObject.Strategy.ObjectId);
                                                this.Commit(transactionFlag);
                                            }
                                        }
                                    }

                                    {
                                        // String RelationTypes
                                        IObject subject = this.GetSession().Create(testType);
                                        var id = int.Parse(subject.Strategy.ObjectId.ToString());
                                        var testRoleTypes = this.GetStringRoles(testType);

                                        string valueA = this.ValueGenerator.GenerateString(100);
                                        string valueB = this.ValueGenerator.GenerateString(100);
                                        string valueC = this.ValueGenerator.GenerateString(100);
                                        string valueD = this.ValueGenerator.GenerateString(100);

                                        for (var testRoleTypeIndex = 0;
                                             testRoleTypeIndex < testRoleTypes.Length;
                                             testRoleTypeIndex++)
                                        {
                                            var testRoleType = testRoleTypes[testRoleTypeIndex];
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            IObject proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);

                                            Assert.AreEqual(valueB, subject.Strategy.GetUnitRole(testRoleType));
                                            Assert.AreEqual(valueB, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueC);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueD);

                                            Assert.AreEqual(valueD, subject.Strategy.GetUnitRole(testRoleType));
                                            Assert.AreEqual(valueD, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);
                                            this.Commit(transactionFlag);
                                            Assert.AreEqual(valueB, subject.Strategy.GetUnitRole(testRoleType));
                                            Assert.AreEqual(valueB, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueC);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueD);
                                            this.Commit(transactionFlag);
                                            Assert.AreEqual(valueD, subject.Strategy.GetUnitRole(testRoleType));
                                            Assert.AreEqual(valueD, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);
                                            Assert.AreEqual(valueB, subject.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);
                                            Assert.AreEqual(valueB, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueC);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueD);
                                            Assert.AreEqual(valueD, subject.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);
                                            Assert.AreEqual(valueD, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);
                                            this.Commit(transactionFlag);
                                            Assert.AreEqual(valueB, subject.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);
                                            Assert.AreEqual(valueB, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueC);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueD);
                                            this.Commit(transactionFlag);
                                            Assert.AreEqual(valueD, subject.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);
                                            Assert.AreEqual(valueD, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);
                                        }
                                    }
                                }

                                {
                                    // One2One RelationTypes
                                    var relationTypes = this.GetOne2OneRelations(this.GetMetaDomain());
                                    for (var iRelation = 0; iRelation < relationTypes.Length; iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes =
                                            relationType.AssociationType.ObjectType.ConcreteClasses;
                                        for (var iAssociationType = 0;
                                             iAssociationType < associationTypes.Length;
                                             iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                            {
                                                var roleType = roleTypes[iRoleType];
                                                var association = this.GetSession().Create(associationType);
                                                var role = this.GetSession().Create(roleType);
                                                var associationId = int.Parse(association.Strategy.ObjectId.ToString());
                                                var roleId = int.Parse(role.Strategy.ObjectId.ToString());
                                                var associationProxy = this.Instantiate(associationId, manyFlag);
                                                var roleProxy = this.Instantiate(roleId, manyFlag);
                                                this.Commit(transactionFlag);

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    // set association
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(associationProxy, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(role, associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(association, roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            role.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            roleProxy.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(associationProxy, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(role, associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(association, roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(associationProxy, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(role, associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(association, roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            role.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            roleProxy.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(associationProxy, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(role, associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(association, roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            role.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            roleProxy.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(associationProxy, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(role, associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(association, roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            role.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            roleProxy.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // One2Many RelationTypes
                                    var relationTypes = this.GetOne2ManyRelations(this.GetMetaDomain());
                                    for (var iRelation = 0; iRelation < relationTypes.Length; iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes =
                                            relationType.AssociationType.ObjectType.ConcreteClasses;
                                        for (var iAssociationType = 0;
                                             iAssociationType < associationTypes.Length;
                                             iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                            {
                                                var roleType = roleTypes[iRoleType];
                                                var association = this.GetSession().Create(associationType);
                                                var role = this.GetSession().Create(roleType);
                                                var associationId = int.Parse(association.Strategy.ObjectId.ToString());
                                                var roleId = int.Parse(role.Strategy.ObjectId.ToString());
                                                var associationProxy = this.Instantiate(associationId, manyFlag);
                                                var roleProxy = this.Instantiate(roleId, manyFlag);
                                                this.Commit(transactionFlag);

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    // set association
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(associationProxy, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(association, roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType.RoleType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(associationProxy, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(association, roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType.RoleType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(associationProxy, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(association, roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType.RoleType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(associationProxy, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(association, roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType.RoleType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType.RoleType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(associationProxy, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(association, roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType.RoleType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(role.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        this.Commit(transactionFlag);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // Many2One RelationTypes
                                    var relationTypes = this.GetMany2OneRelations(this.GetMetaDomain());
                                    for (var iRelation = 0; iRelation < relationTypes.Length; iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                            {
                                                var roleType = roleTypes[iRoleType];
                                                var association = this.GetSession().Create(associationType);
                                                var role = this.GetSession().Create(roleType);
                                                var associationId = int.Parse(association.Strategy.ObjectId.ToString());
                                                var roleId = int.Parse(role.Strategy.ObjectId.ToString());
                                                var associationProxy = this.Instantiate(associationId, manyFlag);
                                                var roleProxy = this.Instantiate(roleId, manyFlag);
                                                this.Commit(transactionFlag);

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    // set association
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        Assert.AreEqual(role, associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId,  manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy,  association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(associationProxy,  ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        Assert.AreEqual(role, associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(association,  ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        Assert.AreEqual(role, associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        Assert.AreEqual(role, associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        Assert.AreEqual(role, associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        this.Commit(transactionFlag);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // Many2Many RelationTypes
                                    var relationTypes = this.GetMany2ManyRelations(this.GetMetaDomain());
                                    for (var iRelation = 0; iRelation < relationTypes.Length; iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes =
                                            relationType.AssociationType.ObjectType.ConcreteClasses;
                                        for (var iAssociationType = 0;
                                             iAssociationType < associationTypes.Length;
                                             iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                            {
                                                var roleType = roleTypes[iRoleType];
                                                var association = this.GetSession().Create(associationType);
                                                var role = this.GetSession().Create(roleType);
                                                var associationId = int.Parse(association.Strategy.ObjectId.ToString());
                                                var roleId = int.Parse(role.Strategy.ObjectId.ToString());
                                                var associationProxy = this.Instantiate(associationId, manyFlag);
                                                var roleProxy = this.Instantiate(roleId, manyFlag);
                                                this.Commit(transactionFlag);

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    // set association
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        Assert.AreEqual(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType.RoleType, (IObject[])null);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        Assert.AreEqual(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType.RoleType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        Assert.AreEqual(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType.RoleType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        Assert.AreEqual(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType.RoleType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType.RoleType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        Assert.AreEqual(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType))[0]);
                                                        Assert.AreEqual(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType.RoleType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        this.Commit(transactionFlag);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (this.IsRollbackSupported())
            {
                for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
                {
                    var repeat = this.GetRepeats()[iRepeat];
                    for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                    {
                        var testRepeat = this.GetTestRepeats()[iTestRepeat];
                        for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                        {
                            for (var iManyFlag = 0; iManyFlag < this.manyFlags.Length; iManyFlag++)
                            {
                                bool manyFlag = this.manyFlags[iManyFlag];
                                for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                                {
                                    var testType = this.GetTestTypes()[iTestType];
                                    {
                                        // Equality & Id's
                                        var anObject = this.GetSession().Create(testType);
                                        var id = int.Parse(anObject.Strategy.ObjectId.ToString());
                                        this.Instantiate(id, manyFlag);
                                        this.GetSession().Commit();

                                        for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            IObject sameObject = this.Instantiate(id, manyFlag);

                                            for (var testRepeatIndex = 0;
                                                 testRepeatIndex < testRepeat;
                                                 testRepeatIndex++)
                                            {
                                                Assert.AreEqual(anObject, sameObject);
                                                Assert.AreEqual(anObject.Strategy.ObjectId, sameObject.Strategy.ObjectId);
                                            }

                                            this.GetSession().Rollback();

                                            anObject = this.Instantiate(id, manyFlag);

                                            for (var testRepeatIndex = 0;
                                                 testRepeatIndex < testRepeat;
                                                 testRepeatIndex++)
                                            {
                                                Assert.AreEqual(anObject, sameObject);
                                                Assert.AreEqual(anObject.Strategy.ObjectId, sameObject.Strategy.ObjectId);
                                            }

                                            this.GetSession().Rollback();
                                        }
                                    }

                                    {
                                        // String RelationTypes
                                        IObject subject = this.GetSession().Create(testType);
                                        var id = int.Parse(subject.Strategy.ObjectId.ToString());
                                        this.Instantiate(id, manyFlag);
                                        this.GetSession().Commit();

                                        string valueA = this.ValueGenerator.GenerateString(100);
                                        string valueB = this.ValueGenerator.GenerateString(100);

                                        var testRoleTypes = this.GetStringRoles(testType);
                                        for (var testRoleTypeIndex = 0;
                                             testRoleTypeIndex < testRoleTypes.Length;
                                             testRoleTypeIndex++)
                                        {
                                            var testRoleType = testRoleTypes[testRoleTypeIndex];

                                            // set subject, set proxy
                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            IObject proxy = this.Instantiate(id, manyFlag);

                                            this.GetSession().Rollback();

                                            Assert.IsFalse(subject.Strategy.ExistRole(testRoleType));
                                            Assert.IsFalse(proxy.Strategy.ExistRole(testRoleType));

                                            this.GetSession().Commit();

                                            // set subject, instantiate proxy
                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            proxy = this.Instantiate(id, manyFlag);

                                            this.GetSession().Rollback();

                                            Assert.IsFalse(subject.Strategy.ExistRole(testRoleType));
                                            Assert.IsFalse(proxy.Strategy.ExistRole(testRoleType));

                                            this.GetSession().Commit();

                                            // instantiate both , set subject 
                                            subject = this.Instantiate(id, manyFlag);
                                            proxy = this.Instantiate(id, manyFlag);
                                            subject.Strategy.SetUnitRole(testRoleType, valueA);

                                            this.GetSession().Rollback();

                                            Assert.IsFalse(subject.Strategy.ExistRole(testRoleType));
                                            Assert.IsFalse(proxy.Strategy.ExistRole(testRoleType));

                                            // instantiate both , set proxy 
                                            subject = this.Instantiate(id, manyFlag);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);

                                            this.GetSession().Rollback();

                                            Assert.IsFalse(subject.Strategy.ExistRole(testRoleType));
                                            Assert.IsFalse(proxy.Strategy.ExistRole(testRoleType));

                                            // instantiate both , set subject & proxy
                                            subject = this.Instantiate(id, manyFlag);
                                            proxy = this.Instantiate(id, manyFlag);
                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);

                                            this.GetSession().Rollback();

                                            Assert.IsFalse(subject.Strategy.ExistRole(testRoleType));
                                            Assert.IsFalse(proxy.Strategy.ExistRole(testRoleType));

                                            // instantiate both , set proxy & subject  
                                            subject = this.Instantiate(id, manyFlag);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);
                                            subject.Strategy.SetUnitRole(testRoleType, valueA);

                                            this.GetSession().Rollback();

                                            Assert.IsFalse(subject.Strategy.ExistRole(testRoleType));
                                            Assert.IsFalse(proxy.Strategy.ExistRole(testRoleType));
                                        }
                                    }
                                }

                                {
                                    // One2One RelationTypes
                                    var relationTypes = this.GetOne2OneRelations(this.GetMetaDomain());
                                    for (var iRelation = 0; iRelation < relationTypes.Length; iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                            {
                                                var roleType = roleTypes[iRoleType];
                                                var association = this.GetSession().Create(associationType);
                                                var role = this.GetSession().Create(roleType);
                                                var associationId = int.Parse(association.Strategy.ObjectId.ToString());
                                                var roleId = int.Parse(role.Strategy.ObjectId.ToString());
                                                var associationProxy = this.Instantiate(associationId, manyFlag);
                                                var roleProxy = this.Instantiate(roleId, manyFlag);
                                                this.GetSession().Commit();

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    // set association
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            role.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            roleProxy.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            role.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            roleProxy.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            role.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            roleProxy.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            role.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            roleProxy.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            role.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.IsNull(
                                                            roleProxy.Strategy.GetCompositeAssociation(
                                                                relationType.AssociationType));
                                                    }

                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // One2Many RelationTypes
                                    var relationTypes = this.GetOne2ManyRelations(this.GetMetaDomain());
                                    for (var iRelation = 0; iRelation < relationTypes.Length; iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes =
                                            relationType.AssociationType.ObjectType.ConcreteClasses;
                                        for (var iAssociationType = 0;
                                             iAssociationType < associationTypes.Length;
                                             iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                            {
                                                var roleType = roleTypes[iRoleType];
                                                var association = this.GetSession().Create(associationType);
                                                var role = this.GetSession().Create(roleType);
                                                var associationId = int.Parse(association.Strategy.ObjectId.ToString());
                                                var roleId = int.Parse(role.Strategy.ObjectId.ToString());
                                                var associationProxy = this.Instantiate(associationId, manyFlag);
                                                var roleProxy = this.Instantiate(roleId, manyFlag);
                                                this.GetSession().Commit();

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    // set association
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType.RoleType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.IsNull(roleProxy.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                    }

                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // Many2One RelationTypes
                                    var relationTypes = this.GetMany2OneRelations(this.GetMetaDomain());
                                    for (var iRelation = 0; iRelation < relationTypes.Length; iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                            {
                                                var roleType = roleTypes[iRoleType];
                                                var association = this.GetSession().Create(associationType);
                                                var role = this.GetSession().Create(roleType);
                                                var associationId = int.Parse(association.Strategy.ObjectId.ToString());
                                                var roleId = int.Parse(role.Strategy.ObjectId.ToString());
                                                var associationProxy = this.Instantiate(associationId, manyFlag);
                                                var roleProxy = this.Instantiate(roleId, manyFlag);
                                                this.GetSession().Commit();

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    // set association
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType.RoleType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.IsNull(association.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.IsNull(associationProxy.Strategy.GetRole(relationType.RoleType));
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                    }

                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // Many2Many RelationTypes
                                    var relationTypes = this.GetMany2ManyRelations(this.GetMetaDomain());
                                    for (var iRelation = 0; iRelation < relationTypes.Length; iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                            for (var iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                            {
                                                var roleType = roleTypes[iRoleType];
                                                var association = this.GetSession().Create(associationType);
                                                var role = this.GetSession().Create(roleType);
                                                var associationId = int.Parse(association.Strategy.ObjectId.ToString());
                                                var roleId = int.Parse(role.Strategy.ObjectId.ToString());
                                                var associationProxy = this.Instantiate(associationId, manyFlag);
                                                var roleProxy = this.Instantiate(roleId, manyFlag);
                                                this.GetSession().Commit();

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    // set association
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType.RoleType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType.RoleType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType.RoleType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.AreEqual(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType.RoleType)).Length);
                                                        Assert.AreEqual(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType.AssociationType)).Length);
                                                    }

                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        [Category("Dynamic")]
        public void InstantiateMany()
        {
            for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
            {
                var repeat = this.GetRepeats()[iRepeat];
                for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                {
                    for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                    {
                        for (var transactionFlagIndex = 0; transactionFlagIndex < this.GetBooleanFlags().Length; transactionFlagIndex++)
                        {
                            var transactionFlag = this.GetBooleanFlags()[transactionFlagIndex];

                            for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                            {
                                var testType = this.GetTestTypes()[iTestType];

                                var ids = new string[10];
                                for (var i = 0; i < 10; i++)
                                {
                                    var anObject = this.GetSession().Create(testType);
                                    ids[i] = anObject.Strategy.ObjectId.ToString();
                                }

                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                {
                                    var allorsObjects = this.GetSession().Instantiate(ids);

                                    for (var iAllorsType = 0; iAllorsType < allorsObjects.Length; iAllorsType++)
                                    {
                                        var allorsObject = allorsObjects[iAllorsType];
                                        Assert.Contains(allorsObject.Strategy.ObjectId.ToString(), ids);
                                    }

                                    this.Commit(transactionFlag);
                                }

                                ids = new string[0];
                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                {
                                    var allorsObjects = this.GetSession().Instantiate(ids);
                                    Assert.IsEmpty(allorsObjects);
                                    this.Commit(transactionFlag);
                                }

                                ids = new[] { (int.MaxValue - 1).ToString() };
                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                {
                                    var allorsObjects = this.GetSession().Instantiate(ids);
                                    Assert.IsEmpty(allorsObjects);
                                    this.Commit(transactionFlag);
                                }
                            }
                        }
                    }
                }
            }

            if (this.IsRollbackSupported())
            {
                for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
                {
                    var repeat = this.GetRepeats()[iRepeat];
                    for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                    {
                        for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                        {
                            for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                            {
                                var testType = this.GetTestTypes()[iTestType];

                                var ids = new string[10];
                                for (var i = 0; i < 10; i++)
                                {
                                    var anObject = this.GetSession().Create(testType);
                                    ids[i] = anObject.Strategy.ObjectId.ToString();
                                }

                                this.GetSession().Rollback();

                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                {
                                    var allorsObjects = this.GetSession().Instantiate(ids);

                                    for (var iAllorsType = 0; iAllorsType < allorsObjects.Length; iAllorsType++)
                                    {
                                        Assert.IsEmpty(allorsObjects);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        [Category("Dynamic")]
        public void IsDeleted()
        {
            for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
            {
                var repeat = this.GetRepeats()[iRepeat];
                for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                {
                    for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                    {
                        for (var transactionFlagIndex = 0; transactionFlagIndex < this.GetBooleanFlags().Length; transactionFlagIndex++)
                        {
                            var transactionFlag = this.GetBooleanFlags()[transactionFlagIndex];

                            for (var secondTransactionFlagIndex = 0; secondTransactionFlagIndex < this.GetBooleanFlags().Length; secondTransactionFlagIndex++)
                            {
                                var secondTransactionFlag = this.GetBooleanFlags()[secondTransactionFlagIndex];
                                for (var thirdTransactionFlagIndex = 0; thirdTransactionFlagIndex < this.GetBooleanFlags().Length; thirdTransactionFlagIndex++)
                                {
                                    for (var fourthTransactionFlagIndex = 0; fourthTransactionFlagIndex < this.GetBooleanFlags().Length; fourthTransactionFlagIndex++)
                                    {
                                        for (var useRollbackFlagIndex = 0; useRollbackFlagIndex < this.GetBooleanFlags().Length; useRollbackFlagIndex++)
                                        {
                                            var useRollbackFlag = this.GetBooleanFlags()[useRollbackFlagIndex];
                                            if (!this.IsRollbackSupported())
                                            {
                                                useRollbackFlag = false;
                                            }

                                            for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                                            {
                                                var testType = this.GetTestTypes()[iTestType];

                                                // Without delete
                                                var allorsObject = this.GetSession().Create(testType);
                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.IsFalse(allorsObject.Strategy.IsDeleted);
                                                }

                                                this.Commit(secondTransactionFlag);
                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.IsFalse(allorsObject.Strategy.IsDeleted);
                                                    if (useRollbackFlag)
                                                    {
                                                        this.Rollback(transactionFlag);
                                                    }
                                                    else
                                                    {
                                                        this.Commit(transactionFlag);
                                                    }
                                                }

                                                allorsObject = this.GetSession().Create(testType);
                                                string id = allorsObject.Strategy.ObjectId.ToString();
                                                this.Commit(secondTransactionFlag);
                                                allorsObject = this.GetSession().Instantiate(id);

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.IsFalse(allorsObject.Strategy.IsDeleted);
                                                    if (useRollbackFlag)
                                                    {
                                                        this.Rollback(transactionFlag);
                                                    }
                                                    else
                                                    {
                                                        this.Commit(transactionFlag);
                                                    }
                                                }

                                                IObject proxy = this.GetSession().Create(testType);
                                                this.Commit(secondTransactionFlag);

                                                // AllorsObject subject = GetSession().instantiate( testType, id);
                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.IsFalse(proxy.Strategy.IsDeleted);
                                                    if (useRollbackFlag)
                                                    {
                                                        this.Rollback(transactionFlag);
                                                    }
                                                    else
                                                    {
                                                        this.Commit(transactionFlag);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (this.IsRollbackSupported())
            {
                for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
                {
                    var repeat = this.GetRepeats()[iRepeat];
                    for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                    {
                        for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                        {
                            for (var iTestType = 0; iTestType < this.GetTestTypes().Length; iTestType++)
                            {
                                var testType = this.GetTestTypes()[iTestType];

                                for (var useRollbackFlagIndex = 0; useRollbackFlagIndex < this.GetBooleanFlags().Length; useRollbackFlagIndex++)
                                {
                                    var useRollbackFlag = this.GetBooleanFlags()[useRollbackFlagIndex];

                                    for (var transactionFlagIndex = 0; transactionFlagIndex < this.GetBooleanFlags().Length; transactionFlagIndex++)
                                    {
                                        var transactionFlag = this.GetBooleanFlags()[transactionFlagIndex];

                                        for (var secondTransactionFlagIndex = 0; secondTransactionFlagIndex < this.GetBooleanFlags().Length; secondTransactionFlagIndex++)
                                        {
                                            // Without delete
                                            var allorsObject = this.GetSession().Create(testType);
                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.IsFalse(allorsObject.Strategy.IsDeleted);
                                            }

                                            this.GetSession().Rollback();
                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.IsTrue(allorsObject.Strategy.IsDeleted);
                                                if (useRollbackFlag)
                                                {
                                                    this.Rollback(transactionFlag);
                                                }
                                                else
                                                {
                                                    this.Commit(transactionFlag);
                                                }
                                            }

                                            allorsObject = this.GetSession().Create(testType);
                                            string id = allorsObject.Strategy.ObjectId.ToString();
                                            this.GetSession().Rollback();
                                            allorsObject = this.GetSession().Instantiate(id);

                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.IsNull(allorsObject);
                                                if (useRollbackFlag)
                                                {
                                                    this.Rollback(transactionFlag);
                                                }
                                                else
                                                {
                                                    this.Commit(transactionFlag);
                                                }
                                            }

                                            IObject proxy = this.GetSession().Create(testType);
                                            id = proxy.Strategy.ObjectId.ToString();
                                            this.GetSession().Rollback();
                                            this.GetSession().Instantiate(id);

                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.IsTrue(proxy.Strategy.IsDeleted);
                                                if (useRollbackFlag)
                                                {
                                                    this.Rollback(transactionFlag);
                                                }
                                                else
                                                {
                                                    this.Commit(transactionFlag);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void GetUnit(IObject allorsObject, RoleType role, Units values)
        {
            if (role.ObjectType.IsString)
            {
                values.String = (string)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (role.ObjectType.IsInteger)
            {
                values.Integer = (int)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (role.ObjectType.IsLong)
            {
                values.Long = (long)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (role.ObjectType.IsDecimal)
            {
                values.Decimal = (decimal)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (role.ObjectType.IsDouble)
            {
                values.Double = (double)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (role.ObjectType.IsBoolean)
            {
                values.Boolean = (bool)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (role.ObjectType.IsDateTime)
            {
                values.DateTime = (DateTime)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (role.ObjectType.IsDate)
            {
                values.Date = (DateTime)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (role.ObjectType.IsUnique)
            {
                values.Unique = (Guid)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (role.ObjectType.IsBinary)
            {
                values.Binary = (byte[])allorsObject.Strategy.GetUnitRole(role);
            }
        }

        public void SetUnit(IObject allorsObject, RoleType role, Units values)
        {
            if (role.ObjectType.IsString)
            {
                allorsObject.Strategy.SetUnitRole(role, values.String);
            }
            else if (role.ObjectType.IsInteger)
            {
                allorsObject.Strategy.SetUnitRole(role, values.Integer);
            }
            else if (role.ObjectType.IsLong)
            {
                allorsObject.Strategy.SetUnitRole(role, values.Long);
            }
            else if (role.ObjectType.IsDecimal)
            {
                allorsObject.Strategy.SetUnitRole(role, values.Decimal);
            }
            else if (role.ObjectType.IsDouble)
            {
                allorsObject.Strategy.SetUnitRole(role, values.Double);
            }
            else if (role.ObjectType.IsBoolean)
            {
                allorsObject.Strategy.SetUnitRole(role, values.Boolean);
            }
            else if (role.ObjectType.IsDateTime)
            {
                allorsObject.Strategy.SetUnitRole(role, values.DateTime);
            }
            else if (role.ObjectType.IsDate)
            {
                allorsObject.Strategy.SetUnitRole(role, values.Date);
            }
            else if (role.ObjectType.IsUnique)
            {
                allorsObject.Strategy.SetUnitRole(role, values.Unique);
            }
            else if (role.ObjectType.IsBinary)
            {
                allorsObject.Strategy.SetUnitRole(role, values.Binary);
            }
        }

        private ObjectType[] GetTestTypes()
        {
            return this.GetMetaDomain().ConcreteCompositeObjectTypes;
        }

        private IObject Instantiate(int id, bool many)
        {
            if (many)
            {
                string[] ids = { id.ToString(CultureInfo.InvariantCulture) };
                var results = this.GetSession().Instantiate(ids);
                if (results.Length > 0)
                {
                    return results[0];
                }

                return null;
            }

            return this.GetSession().Instantiate(id.ToString(CultureInfo.InvariantCulture));
        }
    }
}