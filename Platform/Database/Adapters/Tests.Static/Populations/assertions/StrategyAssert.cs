// <copyright file="StrategyAssert.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using System;

    using Allors;
    using Allors.Meta;

    using Xunit;

    public class StrategyAssert
    {
        public static void AssociationExistHasException(IObject allorsObject, IAssociationType associationType)
        {
            var exceptionOccured = false;
            try
            {
                var o = allorsObject.Strategy.GetAssociation(associationType.RelationType);
            }
            catch
            {
                exceptionOccured = true;
            }

            if (!exceptionOccured)
            {
                Assert.True(false, "Exist didn't threw an Exception for association " + associationType);
            }
        }

        public static void AssociationGetHasException(IObject allorsObject, IAssociationType associationType)
        {
            var exceptionOccured = false;
            try
            {
                var o = allorsObject.Strategy.GetAssociation(associationType.RelationType);
            }
            catch
            {
                exceptionOccured = true;
            }

            if (!exceptionOccured)
            {
                Assert.True(false); // Fail
            }
        }

        public static void AssociationsExistExclusive(IObject allorsObject, params IAssociationType[] associationTypes)
        {
            foreach (var associationType in associationTypes)
            {
                if (!allorsObject.Strategy.Class.ExistAssociationType(associationType))
                {
                    Assert.True(false); // Fail
                }
            }

            foreach (var associationType in allorsObject.Strategy.Class.AssociationTypes)
            {
                if (Array.IndexOf(associationTypes, associationType) >= 0)
                {
                    if (!allorsObject.Strategy.ExistAssociation(associationType.RelationType))
                    {
                        Assert.True(false); // Fail
                    }
                }
                else
                {
                    if (allorsObject.Strategy.ExistAssociation(associationType.RelationType))
                    {
                        if (allorsObject.Strategy.ExistAssociation(associationType.RelationType))
                        {
                            Assert.True(false); // Fail
                        }
                    }
                }
            }
        }

        public static void RoleExistHasException(IObject allorsObject, IRoleType roleType)
        {
            var exceptionOccured = false;
            try
            {
                object o = allorsObject.Strategy.ExistRole(roleType.RelationType);
            }
            catch
            {
                exceptionOccured = true;
            }

            if (!exceptionOccured)
            {
                Assert.True(false, "Exist didn't threw an Exception for role " + roleType);
            }
        }

        public static void RoleGetHasException(IObject allorsObject, IRoleType roleType)
        {
            var exceptionOccured = false;
            try
            {
                var o = allorsObject.Strategy.GetRole(roleType.RelationType);
            }
            catch
            {
                exceptionOccured = true;
            }

            if (!exceptionOccured)
            {
                Assert.True(false); // Fail
            }
        }

        public static void RolesExistExclusive(IObject allorsObject, params IRoleType[] roleTypes)
        {
            foreach (var roleType in roleTypes)
            {
                if (!allorsObject.Strategy.Class.ExistRoleType(roleType))
                {
                    Assert.True(false); // Fail
                }
            }

            foreach (var roleType in allorsObject.Strategy.Class.RoleTypes)
            {
                if (Array.IndexOf(roleTypes, roleType) >= 0)
                {
                    if (!allorsObject.Strategy.ExistRole(roleType.RelationType))
                    {
                        Assert.True(false); // Fail
                    }
                }
                else
                {
                    if (allorsObject.Strategy.ExistRole(roleType.RelationType))
                    {
                        Assert.True(false); // Fail
                    }
                }
            }
        }
    }
}
