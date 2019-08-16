//-------------------------------------------------------------------------------------------------
// <copyright file="TreeNodeExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
