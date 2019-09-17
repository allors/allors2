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

    public class Tree : ITree
    {
        public Tree(IPropertyType propertyType = null, INode[] nodes = null)
        {
            this.PropertyType = propertyType;
            this.Nodes = nodes ?? new INode[0];
        }

        public IPropertyType PropertyType { get; }

        public INode[] Nodes { get; private set; }

        public Tree Add(IPropertyType propertyType)
        {
            var node = new Tree(propertyType, null);
            this.Nodes = this.Nodes.Append(node).ToArray();
            return this;
        }

        public Tree Add(IPropertyType propertyType, ITree nodes)
        {
            var node = new Tree(propertyType, nodes.Nodes);
            this.Nodes = this.Nodes.Append(node).ToArray();
            return this;
        }

        public Protocol.Data.Tree ToData() => new Protocol.Data.Tree
        {
            Nodes = this.Nodes.Select(v => v.ToData()).ToArray(),
        };

        public override string ToString()
        {
            var toString = new StringBuilder();
            toString.Append(this.PropertyType.Name + "\n");
            this.ToString(toString, this.Nodes, 1);
            return toString.ToString();
        }

        private void ToString(StringBuilder toString, IReadOnlyCollection<INode> nodes, int level)
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
