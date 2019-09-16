// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;

    public class TreeNode
    {
        public TreeNode(IPropertyType propertyType, IComposite composite = null, TreeNodes nodes = null)
        {
            this.PropertyType = propertyType;
            this.Composite = composite ?? (propertyType.ObjectType.IsComposite ? (IComposite)propertyType.ObjectType : null);

            if (this.Composite != null)
            {
                this.Nodes = nodes ?? new TreeNodes(this.Composite);
            }
        }

        public IPropertyType PropertyType { get; }

        public IComposite Composite { get; }

        public TreeNodes Nodes { get; }

        public Protocol.Data.TreeNode Save() =>
            new Protocol.Data.TreeNode
            {
                PropertyType = this.PropertyType.Id,
                Nodes = this.Nodes.Select(v => v.Save()).ToArray(),
            };

        public void Resolve(IObject obj, HashSet<IObject> objects)
        {
            if (obj != null)
            {
                if (this.PropertyType is IRoleType roleType)
                {
                    if (roleType.ObjectType.IsComposite)
                    {
                        if (roleType.IsOne)
                        {
                            var role = obj.Strategy.GetCompositeRole(roleType.RelationType);
                            if (role != null)
                            {
                                objects.Add(role);

                                foreach (var node in this.Nodes)
                                {
                                    node.Resolve(role, objects);
                                }
                            }
                        }
                        else
                        {
                            var roles = obj.Strategy.GetCompositeRoles(roleType.RelationType);
                            foreach (IObject role in roles)
                            {
                                objects.Add(role);

                                foreach (var node in this.Nodes)
                                {
                                    node.Resolve(role, objects);
                                }
                            }
                        }
                    }
                }
                else if (this.PropertyType is IAssociationType associationType)
                {
                    if (associationType.IsOne)
                    {
                        var association = obj.Strategy.GetCompositeAssociation(associationType.RelationType);
                        if (association != null)
                        {
                            objects.Add(association);

                            foreach (var node in this.Nodes)
                            {
                                node.Resolve(association, objects);
                            }
                        }
                    }
                    else
                    {
                        var associations = obj.Strategy.GetCompositeAssociations(associationType.RelationType);
                        foreach (IObject role in associations)
                        {
                            objects.Add(role);

                            foreach (var node in this.Nodes)
                            {
                                node.Resolve(role, objects);
                            }
                        }
                    }
                }
            }
        }

        public void BuildPrefetchPolicy(PrefetchPolicyBuilder prefetchPolicyBuilder)
        {
            if (this.Nodes == null || this.Nodes.Count == 0)
            {
                prefetchPolicyBuilder.WithRule(this.PropertyType);
            }
            else
            {
                var nestedPrefetchPolicyBuilder = new PrefetchPolicyBuilder();
                foreach (var node in this.Nodes)
                {
                    node.BuildPrefetchPolicy(nestedPrefetchPolicyBuilder);
                }

                var nestedPrefetchPolicy = nestedPrefetchPolicyBuilder.Build();
                prefetchPolicyBuilder.WithRule(this.PropertyType, nestedPrefetchPolicy);
            }
        }
    }
}
