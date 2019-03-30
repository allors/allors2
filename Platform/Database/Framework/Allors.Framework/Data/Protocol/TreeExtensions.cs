//------------------------------------------------------------------------------------------------- 
// <copyright file="TreeExtensions.cs" company="Allors bvba">
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

    public static class TreeExtensions 
    {
        public static Allors.Data.Tree Load(this Tree @this, ISession session)
        {
            var tree = new Allors.Data.Tree(@this.Composite != null ? (IComposite)session.Database.MetaPopulation.Find(@this.Composite.Value) : null);

            foreach (var protocolTreeNode in @this.Nodes)
            {
                var roleType = protocolTreeNode.RoleType != null ? (IRoleType)session.Database.ObjectFactory.MetaPopulation.Find(protocolTreeNode.RoleType.Value) : null;
                var treeNode = new Allors.Data.TreeNode(roleType);
                tree.Nodes.Add(treeNode);
                protocolTreeNode.Load(session, treeNode);
            }

            return tree;
        }
    }
}