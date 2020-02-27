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

    public class Node
    {
        public Node(IPropertyType propertyType, Node[] nodes = null)
        {
            this.PropertyType = propertyType;
            this.Composite = this.PropertyType.ObjectType.IsComposite ? (IComposite)propertyType.ObjectType : null;

            if (propertyType.ObjectType.IsComposite)
            {
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        this.AssertAssignable(node);
                    }
                }

                this.Nodes = nodes ?? new Node[0];
            }
        }

        public IPropertyType PropertyType { get; }

        public IComposite Composite { get; }

        public Node[] Nodes { get; private set; }

        public Protocol.Data.Node Save() =>
            new Protocol.Data.Node
            {
                PropertyType = this.PropertyType.Id,
                Nodes = this.Nodes.Select(v => v.Save()).ToArray(),
            };

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

        public Node Add(IEnumerable<IPropertyType> propertyTypes)
        {
            foreach (var propertyType in propertyTypes)
            {
                this.Add(propertyType);
            }

            return this;
        }

        public Node Add(IPropertyType propertyType)
        {
            var treeNode = new Node(propertyType);
            this.Add(treeNode);
            return this;
        }

        public Node Add(IPropertyType propertyType, Node[] subTree)
        {
            var treeNode = new Node(propertyType, subTree);
            this.Add(treeNode);
            return this;
        }

        public Node Add(IConcreteRoleType concreteRoleType) => this.Add(concreteRoleType.RoleType);

        public Node Add(IConcreteRoleType concreteRoleType, Node[] subTree)
        {
            var treeNode = new Node(concreteRoleType.RoleType, subTree);
            this.Add(treeNode);
            return this;
        }

        internal void Add(Node node)
        {
            this.AssertAssignable(node);
            this.Nodes = this.Nodes.Append(node).ToArray();
        }

        private void AssertAssignable(Node node)
        {
            if (this.Composite != null)
            {
                IComposite addedComposite = null;

                if (node.PropertyType is IRoleType roleType)
                {
                    addedComposite = roleType.AssociationType.ObjectType;
                }
                else if (node.PropertyType is IAssociationType associationType)
                {
                    addedComposite = (IComposite)associationType.RoleType.ObjectType;
                }

                if (addedComposite == null || !(this.Composite.Equals(addedComposite) || this.Composite.Classes.Intersect(addedComposite.Classes).Any()))
                {
                    throw new ArgumentException(node.PropertyType + " is not a valid tree node on " + this.Composite + ".");
                }
            }
        }
    }
}
