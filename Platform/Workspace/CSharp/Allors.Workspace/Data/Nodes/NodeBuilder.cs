// <copyright file="Tree.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System;
    using System.Linq;
    using Allors.Workspace.Meta;

    public abstract class NodeBuilder
   {
        protected NodeBuilder() => this.NodeBuilders = Array.Empty<NodeBuilder>();

        public NodeBuilder[] NodeBuilders { get; private set; }

        public static implicit operator Node[](NodeBuilder nodeBuilder)
        {
            if (nodeBuilder.PropertyType != null)
            {
                throw new ArgumentException("You can only cast a root NodeBuilder to Node[] (a root NodeBuilder does not contain a PropertyType)");
            }

            return nodeBuilder.NodeBuilders.Select(v => v.Build()).ToArray();
        }

        public IPropertyType PropertyType { get; set; }

        private Node Build() => new Node(this.PropertyType, this.NodeBuilders?.Select(v => v.Build()).Cast<Node>().ToArray());

        public NodeBuilder Add(NodeBuilder nodeBuilder)
        {
            this.NodeBuilders = this.NodeBuilders.Append(nodeBuilder).ToArray();
            return this;
        }
    }
}
