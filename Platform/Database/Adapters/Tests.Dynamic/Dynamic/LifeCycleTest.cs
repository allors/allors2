// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LifeCycleTest.cs" company="Allors bv">
//   Copyright Allors bv.
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

namespace Allors.Database.Adapters
{
    using Xunit;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using Allors;
    using Allors.Database.Adapters;
    using Allors.Meta;
    using Xunit;

    public abstract class LifeCycleTest : Test
    {
        private readonly bool[] manyFlags = { false, true };

        [Fact]
        [Trait("Category", "Dynamic")]
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

                                    Assert.Equal(objectCount, allorsObjects.Count());

                                    this.Commit(transactionFlag);

                                    var ids = new ArrayList();
                                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                    {
                                        Assert.Equal(objectCount, allorsObjects.Count());
                                        for (var iAllorsType = 0; iAllorsType < objectCount; iAllorsType++)
                                        {
                                            var allorsObject = allorsObjects[iAllorsType];
                                            Assert.False(ids.Contains(allorsObject.Strategy.ObjectId.ToString()));
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

                                    Assert.Equal(objectCount, allorsObjects.Count());

                                    this.GetSession().Rollback();

                                    allorsObjects = this.GetSession().Instantiate(ids);

                                    Assert.Equal(0, allorsObjects.Count());
                                }
                            }
                        }
                    }
                }
            }
        }

        [Fact]
        [Trait("Category", "Dynamic")]
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
                                                    Assert.False(allorsObject.Strategy.IsDeleted);
                                                }

                                                this.Commit(secondTransactionFlag);
                                                allorsObject.Strategy.Delete();
                                                this.Commit(thirdTransactionFlag);

                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.True(allorsObject.Strategy.IsDeleted);
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
                                                    Assert.Null(allorsObject);
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
                                                    Assert.True(proxy.Strategy.IsDeleted);
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
                                                    Assert.Equal(beforeExtent.Count(), afterExtent.Count() + 1);
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
                                                for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
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

                                                            Assert.True(exceptionThrown);
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
                                            var relationTypes = this.GetOne2OneRelations(this.GetMetaPopulation());
                                            for (var relationIndex = 0; relationIndex < relationTypes.Count(); relationIndex++)
                                            {
                                                var relationType = relationTypes[relationIndex];
                                                for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                                {
                                                    bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];
                                                    for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                    {
                                                        bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];

                                                        var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                                        {
                                                            var associationType = associationTypes[iAssociationType];
                                                            var roleTypes = this.GetClasses(relationType);
                                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                                            {
                                                                var roleType = roleTypes[iRoleType];

                                                                // delete association
                                                                var association = this.GetSession().Create(associationType);
                                                                var role = this.GetSession().Create(roleType);

                                                                if (useRoleCachingFlag)
                                                                {
                                                                    association.Strategy.GetRole(relationType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    role.Strategy.GetCompositeAssociation(relationType);
                                                                }

                                                                association.Strategy.SetCompositeRole(relationType, role);
                                                                this.Commit(secondTransactionFlag);
                                                                association.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        association.Strategy.GetRole(relationType);
                                                                        role.Strategy.GetCompositeAssociation(relationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);

                                                                    Assert.Null(role.Strategy.GetCompositeAssociation(relationType));

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
                                                                    association.Strategy.GetRole(relationType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    role.Strategy.GetCompositeAssociation(relationType);
                                                                }

                                                                association.Strategy.SetCompositeRole(relationType, role);
                                                                this.Commit(secondTransactionFlag);
                                                                role.Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        role.Strategy.GetCompositeAssociation(relationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);

                                                                    Assert.Null(association.Strategy.GetRole(relationType));

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
                                                                        association.Strategy.SetCompositeRole(relationType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);
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
                                                                        association.Strategy.SetCompositeRole(relationType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            // Many2One
                                            relationTypes = this.GetMany2OneRelations(this.GetMetaPopulation());
                                            for (var relationIndex = 0; relationIndex < relationTypes.Count(); relationIndex++)
                                            {
                                                var relationType = relationTypes[relationIndex];
                                                for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                                {
                                                    bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                    for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                    {
                                                        bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];

                                                        var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                                        {
                                                            var associationType = associationTypes[iAssociationType];
                                                            var roleTypes = this.GetClasses(relationType);
                                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                                    associations[0].Strategy.GetRole(relationType);
                                                                    associations[1].Strategy.GetRole(relationType);
                                                                    associations[2].Strategy.GetRole(relationType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                    roles[1].Strategy.GetCompositeAssociations(relationType);
                                                                }

                                                                associations[0].Strategy.SetCompositeRole(relationType, roles[0]);
                                                                associations[1].Strategy.SetCompositeRole(relationType, roles[1]);
                                                                associations[2].Strategy.SetCompositeRole(relationType, roles[1]);
                                                                this.Commit(secondTransactionFlag);
                                                                associations[0].Strategy.Delete();
                                                                associations[1].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        associations[0].Strategy.GetRole(relationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);
                                                                    exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        associations[1].Strategy.GetRole(relationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);

                                                                    Assert.Equal(roles[1], associations[2].Strategy.GetRole(relationType));

                                                                    Assert.Equal(0, ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                    Assert.Equal(1, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                    Assert.Equal(associations[2], ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType))[0]);

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
                                                                    associations[0].Strategy.GetRole(relationType);
                                                                    associations[1].Strategy.GetRole(relationType);
                                                                    associations[2].Strategy.GetRole(relationType);
                                                                }

                                                                if (useRoleCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                    roles[1].Strategy.GetCompositeAssociations(relationType);
                                                                }

                                                                associations[0].Strategy.SetCompositeRole(relationType, roles[0]);
                                                                associations[1].Strategy.SetCompositeRole(relationType, roles[1]);
                                                                associations[2].Strategy.SetCompositeRole(relationType, roles[1]);
                                                                this.Commit(secondTransactionFlag);
                                                                roles[0].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    Assert.Equal(null, associations[0].Strategy.GetRole(relationType));
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);

                                                                    Assert.Equal(roles[1], associations[1].Strategy.GetRole(relationType));
                                                                    Assert.Equal(roles[1], associations[2].Strategy.GetRole(relationType));

                                                                    Assert.Equal(2, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                    Assert.Contains(associations[1], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType));
                                                                    Assert.Contains(associations[2], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType));

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
                                                                        association.Strategy.SetCompositeRole(relationType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);
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
                                                                        association.Strategy.SetCompositeRole(relationType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            // One2Many
                                            relationTypes = this.GetOne2ManyRelations(this.GetMetaPopulation());
                                            for (var relationIndex = 0; relationIndex < relationTypes.Count(); relationIndex++)
                                            {
                                                var relationType = relationTypes[relationIndex];
                                                for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                                {
                                                    bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                    for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                    {
                                                        bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];
                                                        var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                                        {
                                                            var associationType = associationTypes[iAssociationType];
                                                            var roleTypes = this.GetClasses(relationType);
                                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                                    associations[0].Strategy.GetCompositeRoles(relationType);
                                                                    associations[1].Strategy.GetCompositeRoles(relationType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociation(relationType);
                                                                    roles[1].Strategy.GetCompositeAssociation(relationType);
                                                                    roles[2].Strategy.GetCompositeAssociation(relationType);
                                                                }

                                                                associations[0].Strategy.AddCompositeRole(relationType, roles[0]);
                                                                associations[0].Strategy.AddCompositeRole(relationType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType, roles[2]);
                                                                this.Commit(secondTransactionFlag);
                                                                associations[0].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        associations[0].Strategy.GetCompositeRoles(relationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);

                                                                    IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType);
                                                                    Assert.DoesNotContain(roles[0], association1Roles);
                                                                    Assert.DoesNotContain(roles[1], association1Roles);
                                                                    Assert.Contains(roles[2], association1Roles);

                                                                    Assert.Null(roles[0].Strategy.GetCompositeAssociation(relationType));
                                                                    Assert.Null(roles[1].Strategy.GetCompositeAssociation(relationType));
                                                                    Assert.Equal(associations[1], roles[2].Strategy.GetCompositeAssociation(relationType));

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
                                                                    associations[0].Strategy.GetCompositeRoles(relationType);
                                                                    associations[1].Strategy.GetCompositeRoles(relationType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociation(relationType);
                                                                    roles[1].Strategy.GetCompositeAssociation(relationType);
                                                                    roles[2].Strategy.GetCompositeAssociation(relationType);
                                                                }

                                                                associations[0].Strategy.AddCompositeRole(relationType, roles[0]);
                                                                associations[0].Strategy.AddCompositeRole(relationType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType, roles[2]);
                                                                this.Commit(secondTransactionFlag);
                                                                roles[2].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType);
                                                                    Assert.Contains(roles[0], association0Roles);
                                                                    Assert.Contains(roles[1], association0Roles);
                                                                    Assert.Equal(associations[0], roles[0].Strategy.GetCompositeAssociation(relationType));
                                                                    Assert.Equal(associations[0], roles[1].Strategy.GetCompositeAssociation(relationType));

                                                                    Assert.Equal(0, ((IObject[])associations[1].Strategy.GetCompositeRoles(relationType)).Count());
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        roles[2].Strategy.GetCompositeAssociation(relationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);

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
                                                                        association.Strategy.AddCompositeRole(relationType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);
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
                                                                        association.Strategy.AddCompositeRole(relationType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            // Many2Many
                                            relationTypes = this.GetMany2ManyRelations(this.GetMetaPopulation());
                                            for (var relationIndex = 0; relationIndex < relationTypes.Count(); relationIndex++)
                                            {
                                                var relationType = relationTypes[relationIndex];
                                                for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                                {
                                                    bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                    for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                    {
                                                        bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];
                                                        var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                                        {
                                                            var associationType = associationTypes[iAssociationType];
                                                            var roleTypes = this.GetClasses(relationType);
                                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                                    associations[0].Strategy.GetCompositeRoles(relationType);
                                                                    associations[1].Strategy.GetCompositeRoles(relationType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                    roles[1].Strategy.GetCompositeAssociations(relationType);
                                                                    roles[2].Strategy.GetCompositeAssociations(relationType);
                                                                }

                                                                associations[0].Strategy.AddCompositeRole(relationType, roles[0]);
                                                                associations[0].Strategy.AddCompositeRole(relationType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType, roles[2]);
                                                                this.Commit(secondTransactionFlag);
                                                                associations[0].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        associations[0].Strategy.GetCompositeRoles(relationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);

                                                                    IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType);
                                                                    Assert.DoesNotContain(roles[0], association1Roles);
                                                                    Assert.Contains(roles[1], association1Roles);
                                                                    Assert.Contains(roles[2], association1Roles);

                                                                    Assert.Equal(0, ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                    Assert.Equal(1, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                    Assert.Equal(associations[1], ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType))[0]);
                                                                    Assert.Equal(1, ((IObject[])roles[2].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                    Assert.Equal(associations[1], ((IObject[])roles[2].Strategy.GetCompositeAssociations(relationType))[0]);

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
                                                                    associations[0].Strategy.GetCompositeRoles(relationType);
                                                                    associations[1].Strategy.GetCompositeRoles(relationType);
                                                                }

                                                                if (useAssociationCachingFlag)
                                                                {
                                                                    roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                    roles[1].Strategy.GetCompositeAssociations(relationType);
                                                                    roles[2].Strategy.GetCompositeAssociations(relationType);
                                                                }

                                                                associations[0].Strategy.AddCompositeRole(relationType, roles[0]);
                                                                associations[0].Strategy.AddCompositeRole(relationType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType, roles[1]);
                                                                associations[1].Strategy.AddCompositeRole(relationType, roles[2]);
                                                                this.Commit(secondTransactionFlag);
                                                                roles[0].Strategy.Delete();
                                                                this.Commit(thirdTransactionFlag);

                                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                                {
                                                                    var exceptionThrown = false;
                                                                    try
                                                                    {
                                                                        roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);

                                                                    IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType);
                                                                    Assert.Equal(1, association0Roles.Count());
                                                                    Assert.Contains(roles[1], association0Roles);

                                                                    IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType);
                                                                    Assert.Equal(2, association1Roles.Count());
                                                                    Assert.Contains(roles[1], association1Roles);
                                                                    Assert.Contains(roles[2], association1Roles);

                                                                    Assert.Equal(2, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                    Assert.Contains(associations[0], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType));
                                                                    Assert.Contains(associations[1], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType));

                                                                    Assert.Equal(1, ((IObject[])roles[2].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                    Assert.Equal(associations[1], ((IObject[])roles[2].Strategy.GetCompositeAssociations(relationType))[0]);

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
                                                                        association.Strategy.AddCompositeRole(relationType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);
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
                                                                        association.Strategy.AddCompositeRole(relationType, role);
                                                                    }
                                                                    catch
                                                                    {
                                                                        exceptionThrown = true;
                                                                    }

                                                                    Assert.True(exceptionThrown);
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
                                                Assert.True(allorsObject.Strategy.IsDeleted);
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
                                                Assert.Null(allorsObject);
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
                                                Assert.False(allorsObject.Strategy.IsDeleted);
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
                                                Assert.False(allorsObject.Strategy.IsDeleted);
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
                                                Assert.False(proxy.Strategy.IsDeleted);
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
                                                Assert.Equal(beforeExtent.Count(), afterExtent.Count());
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

                                            Assert.True(exceptionThrown);

                                            // Units
                                            var testRoleTypes = this.GetUnitRoles(testType);
                                            var beforeValues = new Units(true);
                                            for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
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

                                                        Assert.True(exceptionThrown);
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
                                        var relationTypes = this.GetOne2OneRelations(this.GetMetaPopulation());
                                        for (var relationIndex = 0; relationIndex < relationTypes.Count(); relationIndex++)
                                        {
                                            var relationType = relationTypes[relationIndex];
                                            for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                            {
                                                bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];
                                                for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                {
                                                    bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];

                                                    var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                                    for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                                    {
                                                        var associationType = associationTypes[iAssociationType];
                                                        var roleTypes = this.GetClasses(relationType);
                                                        for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                                        {
                                                            var roleType = roleTypes[iRoleType];

                                                            var association = this.GetSession().Create(associationType);
                                                            var role = this.GetSession().Create(roleType);

                                                            if (useRoleCachingFlag)
                                                            {
                                                                association.Strategy.GetRole(relationType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                role.Strategy.GetCompositeAssociation(relationType);
                                                            }

                                                            association.Strategy.SetCompositeRole(relationType, role);
                                                            this.GetSession().Commit();

                                                            // delete association
                                                            association.Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType));
                                                                Assert.Equal(role, association.Strategy.GetRole(relationType));

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
                                                                Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType));
                                                                Assert.Equal(role, association.Strategy.GetRole(relationType));

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
                                                            association.Strategy.SetCompositeRole(relationType, role);

                                                            association.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.SetCompositeRole(relationType, role);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // Many2One
                                        relationTypes = this.GetMany2OneRelations(this.GetMetaPopulation());
                                        for (var relationIndex = 0; relationIndex < relationTypes.Count(); relationIndex++)
                                        {
                                            var relationType = relationTypes[relationIndex];
                                            for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                            {
                                                bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                {
                                                    bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];

                                                    var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                                    for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                                    {
                                                        var associationType = associationTypes[iAssociationType];
                                                        var roleTypes = this.GetClasses(relationType);
                                                        for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                                associations[0].Strategy.GetRole(relationType);
                                                                associations[1].Strategy.GetRole(relationType);
                                                                associations[2].Strategy.GetRole(relationType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                roles[1].Strategy.GetCompositeAssociations(relationType);
                                                            }

                                                            associations[0].Strategy.SetCompositeRole(relationType, roles[0]);
                                                            associations[1].Strategy.SetCompositeRole(relationType, roles[1]);
                                                            associations[2].Strategy.SetCompositeRole(relationType, roles[1]);
                                                            this.GetSession().Commit();
                                                            associations[0].Strategy.Delete();
                                                            associations[1].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                Assert.Equal(roles[0], associations[0].Strategy.GetRole(relationType));
                                                                Assert.Equal(roles[1], associations[1].Strategy.GetRole(relationType));
                                                                Assert.Equal(roles[1], associations[2].Strategy.GetRole(relationType));

                                                                Assert.Equal(1, ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                Assert.Equal(associations[0], ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType))[0]);
                                                                Assert.Equal(2, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                Assert.Contains(associations[1], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType));
                                                                Assert.Contains(associations[2], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType));

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
                                                                associations[0].Strategy.GetRole(relationType);
                                                                associations[1].Strategy.GetRole(relationType);
                                                                associations[2].Strategy.GetRole(relationType);
                                                            }

                                                            if (useRoleCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                roles[1].Strategy.GetCompositeAssociations(relationType);
                                                            }

                                                            associations[0].Strategy.SetCompositeRole(relationType, roles[0]);
                                                            associations[1].Strategy.SetCompositeRole(relationType, roles[1]);
                                                            associations[2].Strategy.SetCompositeRole(relationType, roles[1]);
                                                            this.GetSession().Commit();
                                                            roles[0].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                Assert.Equal(roles[0], associations[0].Strategy.GetRole(relationType));
                                                                Assert.Equal(roles[1], associations[1].Strategy.GetRole(relationType));
                                                                Assert.Equal(roles[1], associations[2].Strategy.GetRole(relationType));

                                                                Assert.Equal(1, ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                Assert.Equal(associations[0], ((IObject[])roles[0].Strategy.GetCompositeAssociations(relationType))[0]);
                                                                Assert.Equal(2, ((IObject[])roles[1].Strategy.GetCompositeAssociations(relationType)).Count());
                                                                Assert.Contains(associations[1], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType));
                                                                Assert.Contains(associations[2], (IObject[])roles[1].Strategy.GetCompositeAssociations(relationType));

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
                                                            association.Strategy.SetCompositeRole(relationType, role);

                                                            association.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.SetCompositeRole(relationType, role);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // One2Many
                                        relationTypes = this.GetOne2ManyRelations(this.GetMetaPopulation());
                                        for (var relationIndex = 0; relationIndex < relationTypes.Count(); relationIndex++)
                                        {
                                            var relationType = relationTypes[relationIndex];
                                            for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                            {
                                                bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                {
                                                    bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];
                                                    var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                                    for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                                    {
                                                        var associationType = associationTypes[iAssociationType];
                                                        var roleTypes = this.GetClasses(relationType);
                                                        for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                                associations[0].Strategy.GetCompositeRoles(relationType);
                                                                associations[1].Strategy.GetCompositeRoles(relationType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociation(relationType);
                                                                roles[1].Strategy.GetCompositeAssociation(relationType);
                                                                roles[2].Strategy.GetCompositeAssociation(relationType);
                                                            }

                                                            associations[0].Strategy.AddCompositeRole(relationType, roles[0]);
                                                            associations[0].Strategy.AddCompositeRole(relationType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType, roles[2]);
                                                            this.GetSession().Commit();
                                                            associations[0].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType);
                                                                Assert.Equal(2, association0Roles.Count());
                                                                Assert.Contains(roles[0], association0Roles);
                                                                Assert.Contains(roles[1], association0Roles);

                                                                IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType);
                                                                Assert.Equal(1, association1Roles.Count());
                                                                Assert.Contains(roles[2], association1Roles);

                                                                Assert.Equal(associations[0], roles[0].Strategy.GetCompositeAssociation(relationType));
                                                                Assert.Equal(associations[0], roles[1].Strategy.GetCompositeAssociation(relationType));
                                                                Assert.Equal(associations[1], roles[2].Strategy.GetCompositeAssociation(relationType));

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
                                                                associations[0].Strategy.GetCompositeRoles(relationType);
                                                                associations[1].Strategy.GetCompositeRoles(relationType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociation(relationType);
                                                                roles[1].Strategy.GetCompositeAssociation(relationType);
                                                                roles[2].Strategy.GetCompositeAssociation(relationType);
                                                            }

                                                            associations[0].Strategy.AddCompositeRole(relationType, roles[0]);
                                                            associations[0].Strategy.AddCompositeRole(relationType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType, roles[2]);
                                                            this.GetSession().Commit();
                                                            roles[2].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType);
                                                                Assert.Equal(2, association0Roles.Count());
                                                                Assert.Contains(roles[0], association0Roles);
                                                                Assert.Contains(roles[1], association0Roles);

                                                                IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType);
                                                                Assert.Equal(1, association1Roles.Count());
                                                                Assert.Contains(roles[2], association1Roles);

                                                                Assert.Equal(associations[0], roles[0].Strategy.GetCompositeAssociation(relationType));
                                                                Assert.Equal(associations[0], roles[1].Strategy.GetCompositeAssociation(relationType));
                                                                Assert.Equal(associations[1], roles[2].Strategy.GetCompositeAssociation(relationType));
                                                            }

                                                            // reuse
                                                            var association = this.GetSession().Create(associationType);
                                                            var role = this.GetSession().Create(roleType);

                                                            this.GetSession().Commit();

                                                            role.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.AddCompositeRole(relationType, role);

                                                            association.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.AddCompositeRole(relationType, role);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // Many2Many
                                        relationTypes = this.GetMany2ManyRelations(this.GetMetaPopulation());
                                        for (var relationIndex = 0; relationIndex < relationTypes.Count(); relationIndex++)
                                        {
                                            var relationType = relationTypes[relationIndex];
                                            for (var useRoleCachingFlagIndex = 0; useRoleCachingFlagIndex < this.GetBooleanFlags().Length; useRoleCachingFlagIndex++)
                                            {
                                                bool useRoleCachingFlag = this.GetBooleanFlags()[useRoleCachingFlagIndex];

                                                for (var useAssociationCachingFlagIndex = 0; useAssociationCachingFlagIndex < this.GetBooleanFlags().Length; useAssociationCachingFlagIndex++)
                                                {
                                                    bool useAssociationCachingFlag = this.GetBooleanFlags()[useAssociationCachingFlagIndex];
                                                    var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                                    for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                                    {
                                                        var associationType = associationTypes[iAssociationType];
                                                        var roleTypes = this.GetClasses(relationType);
                                                        for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                                associations[0].Strategy.GetCompositeRoles(relationType);
                                                                associations[1].Strategy.GetCompositeRoles(relationType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                roles[1].Strategy.GetCompositeAssociations(relationType);
                                                                roles[2].Strategy.GetCompositeAssociations(relationType);
                                                            }

                                                            associations[0].Strategy.AddCompositeRole(relationType, roles[0]);
                                                            associations[0].Strategy.AddCompositeRole(relationType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType, roles[2]);
                                                            this.GetSession().Commit();
                                                            associations[0].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType);
                                                                Assert.Equal(2, association0Roles.Count());
                                                                Assert.Contains(roles[0], association0Roles);
                                                                Assert.Contains(roles[1], association0Roles);

                                                                IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType);
                                                                Assert.Equal(2, association1Roles.Count());
                                                                Assert.Contains(roles[1], association1Roles);
                                                                Assert.Contains(roles[2], association1Roles);

                                                                IObject[] role0Associations = roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                Assert.Equal(1, role0Associations.Count());
                                                                Assert.Equal(associations[0], role0Associations[0]);
                                                                IObject[] role1Associations = roles[1].Strategy.GetCompositeAssociations(relationType);
                                                                Assert.Equal(2, role1Associations.Count());
                                                                Assert.Contains(associations[0], role1Associations);
                                                                Assert.Contains(associations[1], role1Associations);
                                                                IObject[] role2Associations = roles[2].Strategy.GetCompositeAssociations(relationType);
                                                                Assert.Equal(1, role2Associations.Count());
                                                                Assert.Equal(associations[1], role2Associations[0]);

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
                                                                associations[0].Strategy.GetCompositeRoles(relationType);
                                                                associations[1].Strategy.GetCompositeRoles(relationType);
                                                            }

                                                            if (useAssociationCachingFlag)
                                                            {
                                                                roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                roles[1].Strategy.GetCompositeAssociations(relationType);
                                                                roles[2].Strategy.GetCompositeAssociations(relationType);
                                                            }

                                                            associations[0].Strategy.AddCompositeRole(relationType, roles[0]);
                                                            associations[0].Strategy.AddCompositeRole(relationType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType, roles[1]);
                                                            associations[1].Strategy.AddCompositeRole(relationType, roles[2]);
                                                            this.GetSession().Commit();
                                                            roles[0].Strategy.Delete();
                                                            this.GetSession().Rollback();

                                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                            {
                                                                IObject[] association0Roles = associations[0].Strategy.GetCompositeRoles(relationType);
                                                                Assert.Equal(2, association0Roles.Count());
                                                                Assert.Contains(roles[0], association0Roles);
                                                                Assert.Contains(roles[1], association0Roles);

                                                                IObject[] association1Roles = associations[1].Strategy.GetCompositeRoles(relationType);
                                                                Assert.Equal(2, association1Roles.Count());
                                                                Assert.Contains(roles[1], association1Roles);
                                                                Assert.Contains(roles[2], association1Roles);

                                                                IObject[] role0Associations = roles[0].Strategy.GetCompositeAssociations(relationType);
                                                                Assert.Equal(1, role0Associations.Count());
                                                                Assert.Equal(associations[0], role0Associations[0]);
                                                                IObject[] role1Associations = roles[1].Strategy.GetCompositeAssociations(relationType);
                                                                Assert.Equal(2, role1Associations.Count());
                                                                Assert.Contains(associations[0], role1Associations);
                                                                Assert.Contains(associations[1], role1Associations);
                                                                IObject[] role2Associations = roles[2].Strategy.GetCompositeAssociations(relationType);
                                                                Assert.Equal(1, role2Associations.Count());
                                                                Assert.Equal(associations[1], role2Associations[0]);
                                                            }

                                                            // reuse
                                                            var association = this.GetSession().Create(associationType);
                                                            var role = this.GetSession().Create(roleType);

                                                            this.GetSession().Commit();

                                                            role.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.AddCompositeRole(relationType, role);

                                                            association.Strategy.Delete();
                                                            this.GetSession().Rollback();
                                                            association.Strategy.AddCompositeRole(relationType, role);
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

        [Fact]
        [Trait("Category", "Dynamic")]
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
                                                    Assert.Equal(anObject, aProxy);
                                                    Assert.Equal(anotherObject, anotherProxy);
                                                    Assert.NotEqual(anObject, anotherObject);
                                                    Assert.NotEqual(anObject, anotherProxy);
                                                    Assert.NotEqual(aProxy, anotherObject);
                                                    Assert.NotEqual(aProxy, anotherProxy);

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
                                                    Assert.Equal(anObject, aProxy);
                                                    Assert.Equal(anotherObject, anotherProxy);
                                                    Assert.NotEqual(anObject, anotherObject);
                                                    Assert.NotEqual(anObject, anotherProxy);
                                                    Assert.NotEqual(aProxy, anotherObject);
                                                    Assert.NotEqual(aProxy, anotherProxy);

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
                                                Assert.Equal(anObject, aProxy);
                                                Assert.Equal(anotherObject, anotherProxy);
                                                Assert.NotEqual(anObject, anotherObject);
                                                Assert.NotEqual(anObject, anotherProxy);
                                                Assert.NotEqual(aProxy, anotherObject);
                                                Assert.NotEqual(aProxy, anotherProxy);

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
                                                Assert.Null(anObject);
                                                Assert.Null(aProxy);
                                                Assert.Null(anotherObject);
                                                Assert.Null(anotherProxy);

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

        [Fact]
        [Trait("Category", "Dynamic")]
        public void InstantiateTest()
        {
            for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
            {
                var repeat = this.GetRepeats()[iRepeat];
                for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                {
                    var testRepeat = this.GetTestRepeats()[iTestRepeat];
                    for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                    {
                        for (var iManyFlag = 0; iManyFlag < this.manyFlags.Count(); iManyFlag++)
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
                                            Assert.Null(unexistingObject);
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
                                                Assert.Equal(anObject, sameObject);
                                                Assert.Equal(anObject.Strategy.ObjectId, sameObject.Strategy.ObjectId);
                                                this.Commit(transactionFlag);
                                            }

                                            sameObject = this.Instantiate(id, manyFlag);

                                            for (var testRepeatIndex = 0;
                                                 testRepeatIndex < testRepeat;
                                                 testRepeatIndex++)
                                            {
                                                Assert.Equal(anObject, sameObject);
                                                Assert.Equal(anObject.Strategy.ObjectId, sameObject.Strategy.ObjectId);
                                                this.Commit(transactionFlag);
                                            }

                                            anObject = this.Instantiate(id, manyFlag);

                                            for (var testRepeatIndex = 0;
                                                 testRepeatIndex < testRepeat;
                                                 testRepeatIndex++)
                                            {
                                                Assert.Equal(anObject, sameObject);
                                                Assert.Equal(anObject.Strategy.ObjectId, sameObject.Strategy.ObjectId);
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
                                             testRoleTypeIndex < testRoleTypes.Count();
                                             testRoleTypeIndex++)
                                        {
                                            var testRoleType = testRoleTypes[testRoleTypeIndex];
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            IObject proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);

                                            Assert.Equal(valueB, subject.Strategy.GetUnitRole(testRoleType));
                                            Assert.Equal(valueB, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueC);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueD);

                                            Assert.Equal(valueD, subject.Strategy.GetUnitRole(testRoleType));
                                            Assert.Equal(valueD, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);
                                            this.Commit(transactionFlag);
                                            Assert.Equal(valueB, subject.Strategy.GetUnitRole(testRoleType));
                                            Assert.Equal(valueB, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueC);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueD);
                                            this.Commit(transactionFlag);
                                            Assert.Equal(valueD, subject.Strategy.GetUnitRole(testRoleType));
                                            Assert.Equal(valueD, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);
                                            Assert.Equal(valueB, subject.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);
                                            Assert.Equal(valueB, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueC);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueD);
                                            Assert.Equal(valueD, subject.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);
                                            Assert.Equal(valueD, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);
                                            this.Commit(transactionFlag);
                                            Assert.Equal(valueB, subject.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);
                                            Assert.Equal(valueB, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);

                                            subject.Strategy.SetUnitRole(testRoleType, valueC);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueD);
                                            this.Commit(transactionFlag);
                                            Assert.Equal(valueD, subject.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);
                                            Assert.Equal(valueD, proxy.Strategy.GetUnitRole(testRoleType));
                                            this.Commit(transactionFlag);
                                        }
                                    }
                                }

                                {
                                    // One2One RelationTypes
                                    var relationTypes = this.GetOne2OneRelations(this.GetMetaPopulation());
                                    for (var iRelation = 0; iRelation < relationTypes.Count(); iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes =
                                            relationType.AssociationType.ObjectType.Classes.ToArray();
                                        for (var iAssociationType = 0;
                                             iAssociationType < associationTypes.Count();
                                             iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = this.GetClasses(relationType);
                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, association.Strategy.GetRole(relationType));
                                                        Assert.Equal(associationProxy, role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(role, associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(association, roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, association.Strategy.GetRole(relationType));
                                                        Assert.Equal(associationProxy, role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(role, associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(association, roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, association.Strategy.GetRole(relationType));
                                                        Assert.Equal(associationProxy, role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(role, associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(association, roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, association.Strategy.GetRole(relationType));
                                                        Assert.Equal(associationProxy, role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(role, associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(association, roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, association.Strategy.GetRole(relationType));
                                                        Assert.Equal(associationProxy, role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(role, associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(association, roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // One2Many RelationTypes
                                    var relationTypes = this.GetOne2ManyRelations(this.GetMetaPopulation());
                                    for (var iRelation = 0; iRelation < relationTypes.Count(); iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes =
                                            relationType.AssociationType.ObjectType.Classes.ToArray();
                                        for (var iAssociationType = 0;
                                             iAssociationType < associationTypes.Count();
                                             iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = this.GetClasses(relationType);
                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                    association.Strategy.AddCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(associationProxy, role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(association, roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(associationProxy, role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(association, roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(associationProxy, role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(association, roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(associationProxy, role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(association, roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(associationProxy, role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(association, roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                        this.Commit(transactionFlag);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // Many2One RelationTypes
                                    var relationTypes = this.GetMany2OneRelations(this.GetMetaPopulation());
                                    for (var iRelation = 0; iRelation < relationTypes.Count(); iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = this.GetClasses(relationType);
                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, association.Strategy.GetRole(relationType));
                                                        Assert.Equal(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        Assert.Equal(role, associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, association.Strategy.GetRole(relationType));
                                                        Assert.Equal(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        Assert.Equal(role, associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, association.Strategy.GetRole(relationType));
                                                        Assert.Equal(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        Assert.Equal(role, associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, association.Strategy.GetRole(relationType));
                                                        Assert.Equal(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        Assert.Equal(role, associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, association.Strategy.GetRole(relationType));
                                                        Assert.Equal(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        Assert.Equal(role, associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRole(relationType, null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        this.Commit(transactionFlag);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // Many2Many RelationTypes
                                    var relationTypes = this.GetMany2ManyRelations(this.GetMetaPopulation());
                                    for (var iRelation = 0; iRelation < relationTypes.Count(); iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes =
                                            relationType.AssociationType.ObjectType.Classes.ToArray();
                                        for (var iAssociationType = 0;
                                             iAssociationType < associationTypes.Count();
                                             iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = this.GetClasses(relationType);
                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                    association.Strategy.AddCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        Assert.Equal(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType, (IObject[])null);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        Assert.Equal(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        Assert.Equal(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType, role);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        Assert.Equal(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        this.Commit(transactionFlag);
                                                    }

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType, roleProxy);
                                                    this.Commit(transactionFlag);

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(roleProxy, ((IObject[])association.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(associationProxy, ((IObject[])role.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        Assert.Equal(role, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType))[0]);
                                                        Assert.Equal(association, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType))[0]);
                                                        this.Commit(transactionFlag);
                                                    }

                                                    association.Strategy.SetCompositeRoles(relationType, (IObject[])null);
                                                    this.Commit(transactionFlag);
                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
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
                            for (var iManyFlag = 0; iManyFlag < this.manyFlags.Count(); iManyFlag++)
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
                                                Assert.Equal(anObject, sameObject);
                                                Assert.Equal(anObject.Strategy.ObjectId, sameObject.Strategy.ObjectId);
                                            }

                                            this.GetSession().Rollback();

                                            anObject = this.Instantiate(id, manyFlag);

                                            for (var testRepeatIndex = 0;
                                                 testRepeatIndex < testRepeat;
                                                 testRepeatIndex++)
                                            {
                                                Assert.Equal(anObject, sameObject);
                                                Assert.Equal(anObject.Strategy.ObjectId, sameObject.Strategy.ObjectId);
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
                                             testRoleTypeIndex < testRoleTypes.Count();
                                             testRoleTypeIndex++)
                                        {
                                            var testRoleType = testRoleTypes[testRoleTypeIndex];

                                            // set subject, set proxy
                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            IObject proxy = this.Instantiate(id, manyFlag);

                                            this.GetSession().Rollback();

                                            Assert.False(subject.Strategy.ExistRole(testRoleType));
                                            Assert.False(proxy.Strategy.ExistRole(testRoleType));

                                            this.GetSession().Commit();

                                            // set subject, instantiate proxy
                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            proxy = this.Instantiate(id, manyFlag);

                                            this.GetSession().Rollback();

                                            Assert.False(subject.Strategy.ExistRole(testRoleType));
                                            Assert.False(proxy.Strategy.ExistRole(testRoleType));

                                            this.GetSession().Commit();

                                            // instantiate both , set subject 
                                            subject = this.Instantiate(id, manyFlag);
                                            proxy = this.Instantiate(id, manyFlag);
                                            subject.Strategy.SetUnitRole(testRoleType, valueA);

                                            this.GetSession().Rollback();

                                            Assert.False(subject.Strategy.ExistRole(testRoleType));
                                            Assert.False(proxy.Strategy.ExistRole(testRoleType));

                                            // instantiate both , set proxy 
                                            subject = this.Instantiate(id, manyFlag);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);

                                            this.GetSession().Rollback();

                                            Assert.False(subject.Strategy.ExistRole(testRoleType));
                                            Assert.False(proxy.Strategy.ExistRole(testRoleType));

                                            // instantiate both , set subject & proxy
                                            subject = this.Instantiate(id, manyFlag);
                                            proxy = this.Instantiate(id, manyFlag);
                                            subject.Strategy.SetUnitRole(testRoleType, valueA);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);

                                            this.GetSession().Rollback();

                                            Assert.False(subject.Strategy.ExistRole(testRoleType));
                                            Assert.False(proxy.Strategy.ExistRole(testRoleType));

                                            // instantiate both , set proxy & subject  
                                            subject = this.Instantiate(id, manyFlag);
                                            proxy = this.Instantiate(id, manyFlag);
                                            proxy.Strategy.SetUnitRole(testRoleType, valueB);
                                            subject.Strategy.SetUnitRole(testRoleType, valueA);

                                            this.GetSession().Rollback();

                                            Assert.False(subject.Strategy.ExistRole(testRoleType));
                                            Assert.False(proxy.Strategy.ExistRole(testRoleType));
                                        }
                                    }
                                }

                                {
                                    // One2One RelationTypes
                                    var relationTypes = this.GetOne2OneRelations(this.GetMetaPopulation());
                                    for (var iRelation = 0; iRelation < relationTypes.Count(); iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = this.GetClasses(relationType);
                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                    }

                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // One2Many RelationTypes
                                    var relationTypes = this.GetOne2ManyRelations(this.GetMetaPopulation());
                                    for (var iRelation = 0; iRelation < relationTypes.Count(); iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes =
                                            relationType.AssociationType.ObjectType.Classes.ToArray();
                                        for (var iAssociationType = 0;
                                             iAssociationType < associationTypes.Count();
                                             iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = this.GetClasses(relationType);
                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                    association.Strategy.AddCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Null(roleProxy.Strategy.GetCompositeAssociation(relationType));
                                                    }

                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // Many2One RelationTypes
                                    var relationTypes = this.GetMany2OneRelations(this.GetMetaPopulation());
                                    for (var iRelation = 0; iRelation < relationTypes.Count(); iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = this.GetClasses(relationType);
                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.SetCompositeRole(relationType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Null(association.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Null(associationProxy.Strategy.GetRole(relationType));
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                    }

                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }

                                {
                                    // Many2Many RelationTypes
                                    var relationTypes = this.GetMany2ManyRelations(this.GetMetaPopulation());
                                    for (var iRelation = 0; iRelation < relationTypes.Count(); iRelation++)
                                    {
                                        var relationType = relationTypes[iRelation];
                                        var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                        for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                        {
                                            var associationType = associationTypes[iAssociationType];
                                            var roleTypes = this.GetClasses(relationType);
                                            for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                                    association.Strategy.AddCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set association with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    association.Strategy.AddCompositeRole(relationType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with role
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType, role);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
                                                    }

                                                    this.GetSession().Commit();

                                                    // instantiate all, set associationProxy with roleProxy
                                                    association = this.Instantiate(associationId, manyFlag);
                                                    associationProxy = this.Instantiate(associationId, manyFlag);
                                                    role = this.Instantiate(roleId, manyFlag);
                                                    roleProxy = this.Instantiate(roleId, manyFlag);
                                                    associationProxy.Strategy.AddCompositeRole(relationType, roleProxy);
                                                    this.GetSession().Rollback();

                                                    for (var testRepeatIndex = 0;
                                                         testRepeatIndex < testRepeat;
                                                         testRepeatIndex++)
                                                    {
                                                        Assert.Equal(0, ((IObject[])association.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])role.Strategy.GetCompositeAssociations(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])associationProxy.Strategy.GetCompositeRoles(relationType)).Count());
                                                        Assert.Equal(0, ((IObject[])roleProxy.Strategy.GetCompositeAssociations(relationType)).Count());
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

        [Fact]
        [Trait("Category", "Dynamic")]
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

                                    for (var iAllorsType = 0; iAllorsType < allorsObjects.Count(); iAllorsType++)
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
                                    Assert.Empty(allorsObjects);
                                    this.Commit(transactionFlag);
                                }

                                ids = new[] { (int.MaxValue - 1).ToString() };
                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                {
                                    var allorsObjects = this.GetSession().Instantiate(ids);
                                    Assert.Empty(allorsObjects);
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

                                    for (var iAllorsType = 0; iAllorsType < allorsObjects.Count(); iAllorsType++)
                                    {
                                        Assert.Empty(allorsObjects);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [Fact]
        [Trait("Category", "Dynamic")]
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
                                                    Assert.False(allorsObject.Strategy.IsDeleted);
                                                }

                                                this.Commit(secondTransactionFlag);
                                                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                                {
                                                    Assert.False(allorsObject.Strategy.IsDeleted);
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
                                                    Assert.False(allorsObject.Strategy.IsDeleted);
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
                                                    Assert.False(proxy.Strategy.IsDeleted);
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
                                                Assert.False(allorsObject.Strategy.IsDeleted);
                                            }

                                            this.GetSession().Rollback();
                                            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                            {
                                                Assert.True(allorsObject.Strategy.IsDeleted);
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
                                                Assert.Null(allorsObject);
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
                                                Assert.True(proxy.Strategy.IsDeleted);
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

        public void GetUnit(IObject allorsObject, RelationType role, Units values)
        {
            var unit = role.RoleType.ObjectType as Unit;

            if (unit.IsString)
            {
                values.String = (string)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (unit.IsInteger)
            {
                values.Integer = (int)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (unit.IsDecimal)
            {
                values.Decimal = (decimal)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (unit.IsFloat)
            {
                values.Float = (double)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (unit.IsBoolean)
            {
                values.Boolean = (bool)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (unit.IsDateTime)
            {
                values.DateTime = (DateTime)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (unit.IsUnique)
            {
                values.Unique = (Guid)allorsObject.Strategy.GetUnitRole(role);
            }
            else if (unit.IsBinary)
            {
                values.Binary = (byte[])allorsObject.Strategy.GetUnitRole(role);
            }
        }

        public void SetUnit(IObject allorsObject, RelationType relationType, Units values)
        {
            var unitType = relationType.RoleType.ObjectType as Unit;

            if (unitType.IsString)
            {
                allorsObject.Strategy.SetUnitRole(relationType, values.String);
            }
            else if (unitType.IsInteger)
            {
                allorsObject.Strategy.SetUnitRole(relationType, values.Integer);
            }
            else if (unitType.IsDecimal)
            {
                allorsObject.Strategy.SetUnitRole(relationType, values.Decimal);
            }
            else if (unitType.IsFloat)
            {
                allorsObject.Strategy.SetUnitRole(relationType, values.Float);
            }
            else if (unitType.IsBoolean)
            {
                allorsObject.Strategy.SetUnitRole(relationType, values.Boolean);
            }
            else if (unitType.IsDateTime)
            {
                allorsObject.Strategy.SetUnitRole(relationType, values.DateTime);
            }
            else if (unitType.IsUnique)
            {
                allorsObject.Strategy.SetUnitRole(relationType, values.Unique);
            }
            else if (unitType.IsBinary)
            {
                allorsObject.Strategy.SetUnitRole(relationType, values.Binary);
            }
        }

        private Class[] GetTestTypes()
        {
            return this.GetMetaPopulation().Classes.ToArray();
        }

        private IObject Instantiate(int id, bool many)
        {
            if (many)
            {
                string[] ids = { id.ToString(CultureInfo.InvariantCulture) };
                var results = this.GetSession().Instantiate(ids);
                if (results.Count() > 0)
                {
                    return results[0];
                }

                return null;
            }

            return this.GetSession().Instantiate(id.ToString(CultureInfo.InvariantCulture));
        }
    }
}
