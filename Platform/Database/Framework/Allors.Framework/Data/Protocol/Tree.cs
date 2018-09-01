// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tree.cs" company="Allors bvba">
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

namespace Allors.Data.Protocol
{
    using System;

    using Allors.Meta;

    public class Tree
    {
        public Guid? Composite { get; set; }

        public TreeNode[] Nodes { get; set; }

        public Data.Tree Load(ISession session)
        {
            var tree = new Data.Tree(this.Composite != null ? (IComposite)session.Database.MetaPopulation.Find(this.Composite.Value) : null);

            foreach (var protocolTreeNode in this.Nodes)
            {
                var roleType = protocolTreeNode.RoleType != null ? (IRoleType)session.Database.ObjectFactory.MetaPopulation.Find(protocolTreeNode.RoleType.Value) : null;
                var treeNode = new Data.TreeNode(roleType);
                tree.Nodes.Add(treeNode);
                protocolTreeNode.Load(session, treeNode);
            }

            return tree;
        }
    }
}