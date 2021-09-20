// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using System.Collections.Generic;

    public static class NodesExtensions
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
    }
}
