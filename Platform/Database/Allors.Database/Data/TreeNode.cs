// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;

    public class TreeNode
    {
        public TreeNode(IPropertyType propertyType, TreeNode[] nodes = null)
        {
            this.PropertyType = propertyType;
            this.Composite = propertyType.ObjectType.IsComposite ? (IComposite)propertyType.ObjectType : null;

            if (propertyType.ObjectType.IsComposite)
            {
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        this.AssertAssignable(node);
                    }
                }

                this.Nodes = nodes ?? new TreeNode[0];
            }
        }

        public IPropertyType PropertyType { get; }

        public IComposite Composite { get; }

        public TreeNode[] Nodes { get; private set; }

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
            if (this.Nodes == null || this.Nodes.Length == 0)
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

        public TreeNode Add(IEnumerable<IPropertyType> propertyTypes)
        {
            foreach (var propertyType in propertyTypes)
            {
                this.Add(propertyType);
            }

            return this;
        }

        public TreeNode Add(IPropertyType propertyType)
        {
            var treeNode = new TreeNode(propertyType);
            this.Add(treeNode);
            return this;
        }

        public TreeNode Add(IPropertyType propertyType, TreeNode[] subTree)
        {
            var treeNode = new TreeNode(propertyType, subTree);
            this.Add(treeNode);
            return this;
        }

        public TreeNode Add(IConcreteRoleType concreteRoleType) => this.Add(concreteRoleType.RoleType);

        public TreeNode Add(IConcreteRoleType concreteRoleType, TreeNode[] subTree)
        {
            var treeNode = new TreeNode(concreteRoleType.RoleType, subTree);
            this.Add(treeNode);
            return this;
        }

        internal void Add(TreeNode treeNode)
        {
            this.AssertAssignable(treeNode);
            this.Nodes = this.Nodes.Append(treeNode).ToArray();
        }

        private void AssertAssignable(TreeNode treeNode)
        {
            IComposite addedComposite = null;

            if (treeNode.PropertyType is IRoleType roleType)
            {
                addedComposite = roleType.AssociationType.ObjectType;
            }
            else if (treeNode.PropertyType is IAssociationType associationType)
            {
                addedComposite = (IComposite)associationType.RoleType.ObjectType;
            }

            if (addedComposite == null || (!addedComposite.IsAssignableFrom(this.Composite) && !this.Composite.IsAssignableFrom(addedComposite)))
            {
                throw new ArgumentException(treeNode.PropertyType + " is not a valid tree node on " + this.Composite + ".");
            }
        }
    }
}
