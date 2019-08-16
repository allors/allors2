//-------------------------------------------------------------------------------------------------
// <copyright file="TreeExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
