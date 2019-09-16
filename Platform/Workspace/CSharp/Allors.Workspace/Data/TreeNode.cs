// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Linq;
    using Allors.Workspace.Meta;

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

        public Protocol.Data.TreeNode ToJson() =>
            new Protocol.Data.TreeNode
            {
                PropertyType = this.PropertyType.Id,
                Nodes = this.Nodes.Select(v => v.ToJson()).ToArray(),
            };
    }
}
