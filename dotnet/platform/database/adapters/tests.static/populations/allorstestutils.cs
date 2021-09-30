// <copyright file="AllorsTestUtils.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using Allors;

    public class AllorsTestUtils
    {
        public static void ForceRoleCaching(IObject allorsObject)
        {
            foreach (var role in allorsObject.Strategy.Class.RoleTypes)
            {
                allorsObject.Strategy.GetRole(role.RelationType);
            }
        }

        public static void ForceAssociationCaching(IObject allorsObject)
        {
            foreach (var association in allorsObject.Strategy.Class.AssociationTypes)
            {
                allorsObject.Strategy.GetAssociation(association.RelationType);
            }
        }
    }
}
