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
        public static void Resolve(this TreeNode[] treeNodes, IObject @object, IAccessControlLists acls, ISet<IObject> objects)
        {
            if (@object != null)
            {
                foreach (var node in treeNodes)
                {
                    node.Resolve(@object, acls, objects);
                }
            }
        }

        public static void Resolve(this TreeNode @this, IObject @object, IAccessControlLists acls, ISet<IObject> objects)
        {
            if (@object != null)
            {
                var acl = acls[@object];
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
                                    objects.Add(role);
                                    foreach (var node in @this.Nodes)
                                    {
                                        node.Resolve(role, acls, objects);
                                    }
                                }
                            }
                            else
                            {
                                var roles = @object.Strategy.GetCompositeRoles(roleType.RelationType);
                                foreach (IObject role in roles)
                                {
                                    objects.Add(role);
                                    foreach (var node in @this.Nodes)
                                    {
                                        node.Resolve(role, acls, objects);
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
                                objects.Add(association);
                                foreach (var node in @this.Nodes)
                                {
                                    node.Resolve(association, acls, objects);
                                }
                            }
                        }
                        else
                        {
                            var associations = @object.Strategy.GetCompositeAssociations(associationType.RelationType);
                            foreach (IObject association in associations)
                            {
                                objects.Add(association);
                                foreach (var node in @this.Nodes)
                                {
                                    node.Resolve(association, acls, objects);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
