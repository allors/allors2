// <copyright file="TreeNodes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Allors.Workspace.Meta;

    public class TreeNodes : IEnumerable<TreeNode>
    {
        private readonly IComposite composite;

        private readonly List<TreeNode> items = new List<TreeNode>();

        public TreeNodes(IComposite composite) => this.composite = composite;

        public int Count => this.items.Count;

        public TreeNode this[int i] => this.items[i];

        public void Add(TreeNode treeNode)
        {
            IComposite addedComposite = null;

            if (treeNode.PropertyType is IRoleType roleType)
            {
                addedComposite = (IComposite)roleType.AssociationType.ObjectType;
            }
            else if (treeNode.PropertyType is IAssociationType associationType)
            {
                addedComposite = (IComposite)associationType.RoleType.ObjectType;
            }

            if (addedComposite == null || (!addedComposite.IsAssignableFrom(this.composite) && !this.composite.IsAssignableFrom((IComposite)addedComposite)))
            {
                throw new ArgumentException(treeNode.PropertyType + " is not a valid tree node on " + this.composite + ".");
            }

            this.items.Add(treeNode);
        }

        public IEnumerator<TreeNode> GetEnumerator() => this.items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
