// <copyright file="TreeNode.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Linq;

    public static class NodeExtensions
    {
        public static PrefetchPolicy BuildPrefetchPolicy(this Node[] treeNodes)
        {
            var prefetchPolicyBuilder = new PrefetchPolicyBuilder();

            foreach (var node in treeNodes)
            {
                node.BuildPrefetchPolicy(prefetchPolicyBuilder);
            }

            return prefetchPolicyBuilder.Build();
        }

        public static Protocol.Data.Node[] Save(this Node[] treeNodes) =>
            treeNodes.Select(v => v.Save()).ToArray();
    }
}
