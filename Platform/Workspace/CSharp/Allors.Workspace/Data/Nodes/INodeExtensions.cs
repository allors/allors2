// <copyright file="INode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Linq;

    public static class INodeExtensions
    {
        public static Protocol.Data.TreeNode ToData(this INode @this)
        {
            var data = new Protocol.Data.TreeNode
            {
                PropertyType = @this.PropertyType.Id,
                Nodes = @this.Nodes.Select(v => v.ToData()).ToArray(),
            };

            return data;
        }
    }
}
