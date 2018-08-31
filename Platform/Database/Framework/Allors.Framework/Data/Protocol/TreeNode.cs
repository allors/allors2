// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeNode.cs" company="Allors bvba">
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

    public class TreeNode
    {
        public Guid? RoleType { get; set; }

        public TreeNode[] Nodes { get; set; }

        public void Load(ISession session, Data.TreeNode treeNode)
        {
            foreach (var childProtocolTreeNode in this.Nodes)
            {
                var childRoleType = childProtocolTreeNode.RoleType != null ? (IRoleType)session.Database.ObjectFactory.MetaPopulation.Find(childProtocolTreeNode.RoleType.Value) : null;
                var childTreeNode = new Data.TreeNode(childRoleType);
                treeNode.Nodes.Add(childTreeNode);
                childProtocolTreeNode.Load(session, childTreeNode);
            }
        }
    }
}