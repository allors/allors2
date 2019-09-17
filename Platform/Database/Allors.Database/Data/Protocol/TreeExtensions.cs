// <copyright file="TreeExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using Allors.Meta;

    public static class TreeExtensions
    {
        public static Allors.Data.Tree Load(this Tree @this, ISession session)
        {
            var tree = new Allors.Data.Tree();

            foreach (var protocolTreeNode in @this.Nodes)
            {
                var propertyType = protocolTreeNode.PropertyType != null ? (IPropertyType)session.Database.ObjectFactory.MetaPopulation.Find(protocolTreeNode.PropertyType.Value) : null;
                var treeNode = new Allors.Data.TreeNode(propertyType);
                tree.Add(treeNode);
                protocolTreeNode.Load(session, treeNode);
            }

            return tree;
        }
    }
}
