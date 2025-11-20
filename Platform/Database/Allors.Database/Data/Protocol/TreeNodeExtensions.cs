// <copyright file="TreeNodeExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using Allors.Meta;

    public static class TreeNodeExtensions
    {
        public static void Load(this Node @this, ISession session, Allors.Data.Node node)
        {
            if (@this.nodes != null)
            {
                foreach (var childProtocolTreeNode in @this.nodes)
                {
                    var childPropertyType = childProtocolTreeNode.propertyType != null ? (IPropertyType)session.Database.ObjectFactory.MetaPopulation.Find(childProtocolTreeNode.propertyType.Value) : null;
                    var childTreeNode = new Allors.Data.Node(childPropertyType);
                    node.Add(childTreeNode);
                    childProtocolTreeNode.Load(session, childTreeNode);
                }
            }
        }
    }
}
