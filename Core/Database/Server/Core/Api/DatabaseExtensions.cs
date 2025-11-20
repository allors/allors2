// <copyright file="DatabaseExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Linq;

    using Allors.Data;
    using Allors.Meta;
    using Allors.Services;


    public static class DatabaseExtensions
    {
        public static Node[] FullTree(this IDatabase @this, IComposite composite, ITreeService treeService)
        {
            var tree = treeService.Get(composite);
            if (tree == null)
            {
                tree = composite.RoleTypes.Where(v => v.ObjectType.IsComposite && ((RoleType)v).Workspace).Select(v => new Node(v)).ToArray();
                treeService.Set(composite, tree);
            }

            return tree;
        }
    }
}
