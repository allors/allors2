// <copyright file="Tree.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Allors.Workspace.Meta;

    public class Node
    {
        public Node(IPropertyType propertyType = null, Node[] nodes = null)
        {
            this.PropertyType = propertyType;
            this.Nodes = nodes ?? new Node[0];
        }

        public IPropertyType PropertyType { get; }

        public Node[] Nodes { get; private set; }

        public Node Add(IPropertyType propertyType)
        {
            var node = new Node(propertyType, null);
            this.Nodes = this.Nodes.Append(node).ToArray();
            return this;
        }

        public Node Add(IPropertyType propertyType, Node childNode)
        {
            var node = new Node(propertyType, childNode.Nodes);
            this.Nodes = this.Nodes.Append(node).ToArray();
            return this;
        }

        public Protocol.Data.TreeNode ToData()
        {
            var data = new Protocol.Data.TreeNode
            {
                PropertyType = this.PropertyType.Id,
                Nodes = this.Nodes.Select(v => v.ToData()).ToArray(),
            };

            return data;
        }

        public override string ToString()
        {
            var toString = new StringBuilder();
            toString.Append(this.PropertyType.Name + "\n");
            this.ToString(toString, this.Nodes, 1);
            return toString.ToString();
        }

        private void ToString(StringBuilder toString, IReadOnlyCollection<Node> nodes, int level)
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
