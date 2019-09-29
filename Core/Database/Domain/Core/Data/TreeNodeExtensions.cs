// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;
    using Domain;
    using Meta;

    public static class TreeNodeExtensions
    {
        public static void Resolve(this TreeNode[] treeNodes, IObject @object, IAccessControlListFactory aclFactory, IDictionary<IObject, IAccessControlList> aclByObject)
        {
            if (@object != null)
            {
                foreach (var node in treeNodes)
                {
                    node.Resolve(@object, aclFactory, aclByObject);
                }
            }
        }

        public static void Resolve(this TreeNode @this, IObject @object, IAccessControlListFactory aclFactory, IDictionary<IObject, IAccessControlList> aclByObject)
        {
            if (@object != null)
            {
                if (!aclByObject.TryGetValue(@object, out var acl))
                {
                    acl = aclFactory.Create(@object);
                    aclByObject.Add(@object, acl);
                }

                if (acl.CanRead(@this.PropertyType))
                {
                    if (@this.PropertyType is IRoleType roleType)
                    {
                        if (roleType.ObjectType.IsComposite)
                        {
                            if (roleType.IsOne)
                            {
                                var role = @object.Strategy.GetCompositeRole(roleType.RelationType);
                                if (role != null)
                                {
                                    foreach (var node in @this.Nodes)
                                    {
                                        node.Resolve(role, aclFactory, aclByObject);
                                    }
                                }
                            }
                            else
                            {
                                var roles = @object.Strategy.GetCompositeRoles(roleType.RelationType);
                                foreach (IObject role in roles)
                                {
                                    foreach (var node in @this.Nodes)
                                    {
                                        node.Resolve(role, aclFactory, aclByObject);
                                    }
                                }
                            }
                        }
                    }
                    else if (@this.PropertyType is IAssociationType associationType)
                    {
                        if (associationType.IsOne)
                        {
                            var association = @object.Strategy.GetCompositeAssociation(associationType.RelationType);
                            if (association != null)
                            {
                                foreach (var node in @this.Nodes)
                                {
                                    node.Resolve(association, aclFactory, aclByObject);
                                }
                            }
                        }
                        else
                        {
                            var associations = @object.Strategy.GetCompositeAssociations(associationType.RelationType);
                            foreach (IObject role in associations)
                            {
                                foreach (var node in @this.Nodes)
                                {
                                    node.Resolve(role, aclFactory, aclByObject);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
