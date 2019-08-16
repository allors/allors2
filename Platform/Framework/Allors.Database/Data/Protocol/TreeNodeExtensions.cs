//------------------------------------------------------------------------------------------------- 
// <copyright file="TreeNodeExtensions.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Protocol.Data
{
    using Allors.Meta;

    public static class TreeNodeExtensions
    {
        public static void Load(this TreeNode @this, ISession session, Allors.Data.TreeNode treeNode)
        {
            if (@this.Nodes != null)
            {
                foreach (var childProtocolTreeNode in @this.Nodes)
                {
                    var childRoleType = childProtocolTreeNode.RoleType != null ? (IRoleType)session.Database.ObjectFactory.MetaPopulation.Find(childProtocolTreeNode.RoleType.Value) : null;
                    var childTreeNode = new Allors.Data.TreeNode(childRoleType);
                    treeNode.Nodes.Add(childTreeNode);
                    childProtocolTreeNode.Load(session, childTreeNode);
                }
            }
        }
    }
}
