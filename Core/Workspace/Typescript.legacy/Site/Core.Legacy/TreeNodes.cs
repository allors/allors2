// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeNodes.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Allors.Meta;

    public class TreeNodes : IEnumerable<TreeNode>
    {
        private readonly IComposite composite;

        private readonly List<TreeNode> items = new List<TreeNode>();

        public TreeNodes(IComposite composite) => this.composite = composite;

        public int Count => this.items.Count;

        public TreeNode this[int i] => this.items[i];

        public void Add(TreeNode treeNode)
        {
            var addedComposite = treeNode.RoleType.AssociationType.ObjectType;

            if (!addedComposite.IsAssignableFrom(this.composite) && !this.composite.IsAssignableFrom(addedComposite))
            {
                throw new ArgumentException(treeNode.RoleType + " is not a valid tree node on " + this.composite + ".");
            }

            this.items.Add(treeNode);
        }

        public IEnumerator<TreeNode> GetEnumerator() => this.items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
