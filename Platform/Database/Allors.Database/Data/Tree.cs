// <copyright file="Tree.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Allors.Meta;

    public class Tree
    {
        public Tree()
        {
            this.Nodes = new TreeNode[0];
        }

        public TreeNode[] Nodes { get; private set; }

        public PrefetchPolicy BuildPrefetchPolicy()
        {
            var prefetchPolicyBuilder = new PrefetchPolicyBuilder();

            foreach (var node in this.Nodes)
            {
                node.BuildPrefetchPolicy(prefetchPolicyBuilder);
            }

            return prefetchPolicyBuilder.Build();
        }

        public Tree Add(IEnumerable<IPropertyType> propertyTypes)
        {
            new List<IPropertyType>(propertyTypes).ForEach(v => this.Add(v));
            return this;
        }

        public Tree Add(IPropertyType propertyType)
        {
            var treeNode = new TreeNode(propertyType);
            this.Add(treeNode);
            return this;
        }

        public Tree Add(IPropertyType propertyType, Tree tree)
        {
            var treeNode = new TreeNode(propertyType, tree.Nodes);
            this.Add(treeNode);

            return this;
        }

        public Tree Add(IConcreteRoleType concreteRoleType) => this.Add(concreteRoleType.RoleType);

        public Tree Add(IConcreteRoleType concreteRoleType, Tree tree)
        {
            var treeNode = new TreeNode(concreteRoleType.RoleType, tree.Nodes);
            this.Add(treeNode);
            return this;
        }

        public void Resolve(IObject obj, HashSet<IObject> objects)
        {
            if (obj != null)
            {
                foreach (var node in this.Nodes)
                {
                    node.Resolve(obj, objects);
                }
            }
        }

        public Protocol.Data.Tree Save() =>
            new Protocol.Data.Tree
            {
                Nodes = this.Nodes.Select(v => v.Save()).ToArray(),
            };

        public override string ToString()
        {
            var toString = new StringBuilder();
            this.ToString(toString, this.Nodes, 1);
            return toString.ToString();
        }

        internal void Add(TreeNode treeNode)
        {
            this.Nodes = this.Nodes.Append(treeNode).ToArray();
        }

        private void ToString(StringBuilder toString, TreeNode[] nodes, int level)
        {
            foreach (var node in nodes)
            {
                var indent = new string(' ', level * 2);
                toString.Append(indent + "- " + node.PropertyType + "\n");
                this.ToString(toString, node.Nodes, level + 1);
            }
        }
    }
}
