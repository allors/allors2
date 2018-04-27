// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullRequestTreeNodeExtensions.cs" company="Allors bvba">
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

namespace Allors.Server
{
    using System;

    using Allors.Meta;

    public static class PullRequestTreeNodeExtensions
    {
        public static void Parse(this PullRequestTreeNode @this, MetaPopulation metaPopulation, out TreeNode treeNode)
        {
            var roleType = (RoleType)metaPopulation.Find(new Guid(@this.RT));
            treeNode = new TreeNode(roleType);

            if (@this.N != null)
            {
                foreach (var n in @this.N)
                {
                    n.Parse(metaPopulation, out TreeNode childTreeNode);
                    treeNode.Nodes.Add(childTreeNode);
                }
            }
        }
    }
}