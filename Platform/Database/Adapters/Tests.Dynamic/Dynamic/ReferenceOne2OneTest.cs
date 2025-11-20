// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceOne2OneTest.cs" company="Allors bv">
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
    using System;
    using System.Linq;
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType));
                                            Assert.Null(association2.Strategy.GetRole(relationType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType));
                                            Assert.False(association2.Strategy.ExistRole(relationType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType));
                                            Assert.Null(association2.Strategy.GetRole(relationType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType));
                                            Assert.Null(association2.Strategy.GetRole(relationType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                            Assert.Null(association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType);
                                            association2.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, null);
                                            association2.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association1.Strategy.GetRole(relationType));
                                            Assert.Equal(role2, association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association1, role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Equal(association2, role2.Strategy.GetCompositeAssociation(relationType));
                                        }
                                    }
                                }
                            }

                            // Set1 Set2 Rollback Set1 Set2 Set1 Rollback Set1 Commit Set2 Rollback Set2 Commit Set1 Rollback Remove1 Rollback SetNull1 Rollback Remove2 Rollback SetNull2 Rollback SetNull1 SetNull2 Rollback Remove1 Remove2 Rollback(Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType));
                                            Assert.False(association2.Strategy.ExistRole(relationType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType));
                                            Assert.False(association2.Strategy.ExistRole(relationType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType));
                                            Assert.False(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType));
                                            Assert.True(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType));
                                            Assert.True(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType));
                                            Assert.True(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType));
                                            Assert.True(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType));
                                            Assert.True(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType);
                                            association2.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType));
                                            Assert.True(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, null);
                                            association2.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType));
                                            Assert.True(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType));
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        var role = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType));
                                            Assert.Null(association2.Strategy.GetRole(relationType));
                                            Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role, association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role, association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role, association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association1.Strategy.GetRole(relationType));
                                                Assert.Equal(role, association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association1.Strategy.GetRole(relationType));
                                                Assert.Null(association2.Strategy.GetRole(relationType));
                                                Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType));
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        IObject association1 = this.GetSession().Create(associationType);
                                        IObject association2 = this.GetSession().Create(associationType);
                                        var role = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType));
                                            Assert.False(association2.Strategy.ExistRole(relationType));
                                            Assert.False(role.Strategy.ExistAssociation(relationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.RemoveRole(relationType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.False(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.False(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association1.Strategy.ExistRole(relationType));
                                                Assert.True(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association1.Strategy.ExistRole(relationType));
                                                Assert.False(association2.Strategy.ExistRole(relationType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType));
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType));
                                            Assert.Null(association2.Strategy.GetRole(relationType));
                                            Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType));
                                            Assert.Null(association2.Strategy.GetRole(relationType));
                                            Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role, association1.Strategy.GetRole(relationType));
                                            Assert.Null(association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association1, role.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType));
                                            Assert.Equal(role, association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType));
                                            Assert.Equal(role, association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association1.Strategy.GetRole(relationType));
                                            Assert.Equal(role, association2.Strategy.GetRole(relationType));
                                            Assert.Equal(association2, role.Strategy.GetCompositeAssociation(relationType));
                                        }
                                    }
                                }
                            }

                            // Set1 Set2 Rollback Set1 Set2 Set1 Rollback Set1 Commit Set2 Rollback Set2 Commit Set1 Rollback Remove Rollback SetNull Rollback(Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType));
                                            Assert.False(association2.Strategy.ExistRole(relationType));
                                            Assert.False(role.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType));
                                            Assert.False(association2.Strategy.ExistRole(relationType));
                                            Assert.False(role.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association1.Strategy.ExistRole(relationType));
                                            Assert.False(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association1.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType));
                                            Assert.True(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType));
                                            Assert.True(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association2.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association1.Strategy.ExistRole(relationType));
                                            Assert.True(association2.Strategy.ExistRole(relationType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType));
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        var association = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association.Strategy.GetRole(relationType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association.Strategy.GetRole(relationType));
                                                Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role2, association.Strategy.GetRole(relationType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association.Strategy.GetRole(relationType));
                                                Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association.Strategy.GetRole(relationType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role2, association.Strategy.GetRole(relationType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association.Strategy.GetRole(relationType));
                                                Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role2, association.Strategy.GetRole(relationType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association.Strategy.GetRole(relationType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association.Strategy.GetRole(relationType));
                                                Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role2, association.Strategy.GetRole(relationType));
                                                Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role1, association.Strategy.GetRole(relationType));
                                                Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType));
                                                Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        var association = this.GetSession().Create(associationType);
                                        IObject role1 = this.GetSession().Create(roleType);
                                        IObject role2 = this.GetSession().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association.Strategy.ExistRole(relationType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association.Strategy.ExistRole(relationType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association.Strategy.ExistRole(relationType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType));
                                                Assert.False(role1.Strategy.ExistAssociation(relationType));
                                                Assert.True(role2.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType));
                                                Assert.True(role1.Strategy.ExistAssociation(relationType));
                                                Assert.False(role2.Strategy.ExistAssociation(relationType));
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association.Strategy.GetRole(relationType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association.Strategy.GetRole(relationType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role1, association.Strategy.GetRole(relationType));
                                            Assert.Equal(association, role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Null(role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role2, association.Strategy.GetRole(relationType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role2, association.Strategy.GetRole(relationType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role2, association.Strategy.GetRole(relationType));
                                            Assert.Null(role1.Strategy.GetCompositeAssociation(relationType));
                                            Assert.Equal(association, role2.Strategy.GetCompositeAssociation(relationType));
                                        }
                                    }
                                }
                            }

                            // Set1 Set2 Rollback Set1 Set2 Set1 Rollback Set1 Commit Set2 Rollback Set2 Commit Set1 Rollback Remove Rollback SetNull Rollback(Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association.Strategy.ExistRole(relationType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association.Strategy.ExistRole(relationType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType));
                                            Assert.True(role1.Strategy.ExistAssociation(relationType));
                                            Assert.False(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role2);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role1);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType));
                                            Assert.False(role1.Strategy.ExistAssociation(relationType));
                                            Assert.True(role2.Strategy.ExistAssociation(relationType));
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                    {
                                        var roleType = roleTypes[iRoleType];
                                        var association = this.GetSession().Create(associationType);
                                        var role = this.GetSession().Create(roleType);
                                        IObject roleOtherDatabase = this.GetSession2().Create(roleType);

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association.Strategy.GetRole(relationType));
                                            Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association.Strategy.GetRole(relationType));
                                                Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association.Strategy.GetRole(relationType));
                                                Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association.Strategy.GetRole(relationType));
                                                Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Null(association.Strategy.GetRole(relationType));
                                                Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
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
                                                association.Strategy.SetCompositeRole(relationType, roleOtherDatabase);
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
                                                Assert.Null(association.Strategy.GetRole(relationType));
                                                Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                            Assert.False(association.Strategy.ExistRole(relationType));
                                            Assert.False(role.Strategy.ExistAssociation(relationType));
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association.Strategy.ExistRole(relationType));
                                                Assert.False(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.True(association.Strategy.ExistRole(relationType));
                                                Assert.True(role.Strategy.ExistAssociation(relationType));
                                                if (transactionFlag)
                                                {
                                                    this.GetSession().Commit();
                                                }
                                            }
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, null);
                                            if (transactionFlag)
                                            {
                                                this.GetSession().Commit();
                                            }

                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.False(association.Strategy.ExistRole(relationType));
                                                Assert.False(role.Strategy.ExistAssociation(relationType));
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
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                            association.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Null(association.Strategy.GetRole(relationType));
                                            Assert.Null(role.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.Equal(role, association.Strategy.GetRole(relationType));
                                            Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                            {
                                                Assert.Equal(role, association.Strategy.GetRole(relationType));
                                                Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType));
                                            }
                                        }
                                    }
                                }
                            }

                            // Set Rollback Set Commit Remove Rollback SetNull Rollback(Exist)
                            for (int iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                            {
                                var relationType = this.GetRelations()[iRelation];
                                var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                                for (int iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                                {
                                    var associationType = associationTypes[iAssociationType];
                                    var roleTypes = this.GetClasses(relationType);
                                    for (int iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
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
                                            association.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.False(association.Strategy.ExistRole(relationType));
                                            Assert.False(role.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, role);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Commit();
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.RemoveRole(relationType);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType));
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            association.Strategy.SetCompositeRole(relationType, null);
                                        }

                                        for (int repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                                        {
                                            this.GetSession().Rollback();
                                        }

                                        for (int testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                                        {
                                            Assert.True(association.Strategy.ExistRole(relationType));
                                            Assert.True(role.Strategy.ExistAssociation(relationType));
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
            return this.GetOne2OneRelations(this.GetMetaPopulation());
        }
    }
}
