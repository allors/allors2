// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using System.Collections.Generic;
    using Meta;
    using Security;

    public static class NodeExtensions
    {
        public static void Resolve(this Node @this, IObject @object, IAccessControl acls, ISet<IObject> objects)
        {
            if (@object != null)
            {
                var acl = acls[@object];
                // TODO: Access check for AssociationType
                if (@this.PropertyType is IAssociationType || acl.CanRead((IRoleType)@this.PropertyType))
                {
                    if (@this.PropertyType is IRoleType roleType)
                    {
                        if (roleType.ObjectType.IsComposite)
                        {
                            if (roleType.IsOne)
                            {
                                var role = @object.Strategy.GetCompositeRole(roleType);
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
                                foreach (var role in @object.Strategy.GetCompositesRole<IObject>(roleType))
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
                            var association = @object.Strategy.GetCompositeAssociation(associationType);
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
                            foreach (var association in @object.Strategy.GetCompositesAssociation<IObject>(associationType))
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
