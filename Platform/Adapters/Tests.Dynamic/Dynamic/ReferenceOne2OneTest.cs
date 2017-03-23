// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceOne2OneTest.cs" company="Allors bvba">
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

    using Allors;
    using Allors.Meta;

    using Xunit;

    public abstract class ReferenceOne2OneTest : Test
    {
        [Fact]
        [Trait("Category", "Dynamic")]
        public void DifferentAssociationDifferentRole()
        {
            for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
            {
                var repeat = this.GetRepeats()[iRepeat];
                for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                {
                    var testRepeat = this.GetTestRepeats()[iTestRepeat];
                    for (int iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                    {
                        for (int iTransactionFlag = 0; iTransactionFlag < this.GetBooleanFlags().Length; iTransactionFlag++)
                        {
                            var transactionFlag = this.GetBooleanFlags()[iTransactionFlag];

                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType.RoleType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType.RoleType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // Set1 Set2 Set1 Remove1 Remove2 Set2 Set1 Set2 SetNull2 SetNull1 Set1 Set2 Set1(Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType.RoleType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType.RoleType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (this.IsRollbackSupported())
                        {
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType.RoleType);
                                            association2.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }
                                    }
                                }
                            }

                            // Set1 Set2 Rollback Set1 Set2 Set1 Rollback Set1 Commit Set2 Rollback Set2 Commit Set1 Rollback Remove1 Rollback SetNull1 Rollback Remove2 Rollback SetNull2 Rollback SetNull1 SetNull2 Rollback Remove1 Remove2 Rollback(Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType.RoleType);
                                            association2.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
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
        public void DifferentAssociationSameRole()
        {
            for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
            {
                var repeat = this.GetRepeats()[iRepeat];
                for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                {
                    var testRepeat = this.GetTestRepeats()[iTestRepeat];
                    for (int iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                    {
                        for (int iTransactionFlag = 0; iTransactionFlag < this.GetBooleanFlags().Length; iTransactionFlag++)
                        {
                            var transactionFlag = this.GetBooleanFlags()[iTransactionFlag];

                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        var role = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType.RoleType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(role, association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association1.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // Set1 Set2 Set1 Remove Set2 Set1 Set2 SetNull Set1 Set2 Set1 (Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        var role = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role.Strategy.ExistAssociation(relationType.AssociationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType.RoleType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (this.IsRollbackSupported())
                        {
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        var role = this.GetSession().Create(roleType);

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role, association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(role, association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(role, association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(role, association2.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }
                                    }
                                }
                            }

                            // Set1 Set2 Rollback Set1 Set2 Set1 Rollback Set1 Commit Set2 Rollback Set2 Commit Set1 Rollback Remove Rollback SetNull Rollback(Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        var role = this.GetSession().Create(roleType);

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(association2.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
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
        public void SameAssociationDifferentRole()
        {
            for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
            {
                var repeat = this.GetRepeats()[iRepeat];
                for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                {
                    var testRepeat = this.GetTestRepeats()[iTestRepeat];
                    for (int iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                    {
                        for (int iTransactionFlag = 0; iTransactionFlag < this.GetBooleanFlags().Length; iTransactionFlag++)
                        {
                            var transactionFlag = this.GetBooleanFlags()[iTransactionFlag];
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        var association = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role2, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType.RoleType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role2, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role2, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role2, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // Set1 Set2 Set1 Remove Set2 Set1 Set2 SetNull Set1 Set2 Set1 (Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        var association = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType.RoleType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (this.IsRollbackSupported())
                        {
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        var association = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role2, association.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role2, association.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role2, association.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }
                                    }
                                }
                            }

                            // Set1 Set2 Rollback Set1 Set2 Set1 Rollback Set1 Commit Set2 Rollback Set2 Commit Set1 Rollback Remove Rollback SetNull Rollback(Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        var association = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType.AssociationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType.AssociationType));
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
        public void SameAssociationSameRole()
        {
            for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
            {
                var repeat = this.GetRepeats()[iRepeat];
                for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
                {
                    var testRepeat = this.GetTestRepeats()[iTestRepeat];
                    for (int iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                    {
                        for (int iTransactionFlag = 0; iTransactionFlag < this.GetBooleanFlags().Length; iTransactionFlag++)
                        {
                            var transactionFlag = this.GetBooleanFlags()[iTransactionFlag];

                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        var association = this.GetSession().Create(associationType);
                                        var role = this.GetSession().Create(roleType);
                                        IObject roleOtherDatabase = this.GetSession2().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType.RoleType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        // Set different Population
                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            try
                                            {
                                                association.Strategy.SetCompositeRole(relationType.RoleType, roleOtherDatabase);
                                                Assert.True(false); // Fail
                                            }
                                            catch (ArgumentException exception)
                                            {
                                                Assert.NotNull(exception);
                                            }

                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // Set Remove Set SetNull (Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        var association = this.GetSession().Create(associationType);
                                        var role = this.GetSession().Create(roleType);
                                        if (transactionFlag)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role.Strategy.ExistAssociation(relationType.AssociationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType.RoleType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association.Strategy.ExistRole(relationType.RoleType));
                                                Assert.False(role.Strategy.ExistAssociation(relationType.AssociationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (this.IsRollbackSupported())
                        {
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        var association = this.GetSession().Create(associationType);
                                        var role = this.GetSession().Create(roleType);

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association.Strategy.GetRole(relationType.RoleType));
                                            Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role, association.Strategy.GetRole(relationType.RoleType));
                                            Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association.Strategy.GetRole(relationType.RoleType));
                                                Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                            }
                                        }
                                    }
                                }
                            }

                            // Set Rollback Set Commit Remove Rollback SetNull Rollback(Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.ConcreteClasses;
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Length; iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = relationType.RoleType.ObjectType.ConcreteClasses;
                                    for (int iRoleType = 0; iRoleType < roleTypes.Length; iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        var association = this.GetSession().Create(associationType);
                                        var role = this.GetSession().Create(roleType);

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association.Strategy.ExistRole(relationType.RoleType));
                                            Assert.False(role.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType.RoleType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType.RoleType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType.RoleType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType.AssociationType));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private RelationType[] GetRelations()
        {
            return this.GetOne2OneRelations(this.GetMetaDomain());
        }
    }
}