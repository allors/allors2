
// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Meta;

namespace Allors.Workspace.Data
{
    using System.Linq;

    public class TreeNode
    {
        public TreeNode(IRoleType roleType, IComposite composite = null, TreeNodes nodes = null)
        {
            this.RoleType = roleType;
            this.Composite = composite ?? (roleType.ObjectType.IsComposite ? (IComposite)roleType.ObjectType : null);

            if (this.Composite != null)
            {
                this.Nodes = nodes ?? new TreeNodes(this.Composite);
            }
        }

        public IRoleType RoleType { get; }

        public IComposite Composite { get; }

        public TreeNodes Nodes { get; }

        public Protocol.Data.TreeNode ToJson() =>
            new Protocol.Data.TreeNode
            {
                RoleType = this.RoleType.Id,
                Nodes = this.Nodes.Select(v => v.ToJson()).ToArray()
            };
    }
}
