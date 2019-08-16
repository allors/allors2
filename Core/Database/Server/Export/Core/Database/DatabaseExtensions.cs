// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Server
{
    using System.Linq;

    using Allors.Data;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public static class DatabaseExtensions
    {
        public static Tree FullTree(this IDatabase @this, IComposite composite)
        {
            var treeService = @this.ServiceProvider.GetRequiredService<ITreeService>();
            var tree = treeService.Get(composite);
            if (tree == null)
            {
                tree = new Tree(composite);
                foreach (var compositeRoleType in composite.RoleTypes.Where(v => v.ObjectType.IsComposite && ((RoleType)v).Workspace))
                {
                    tree.Add(compositeRoleType);
                }

                treeService.Set(composite, tree);
            }

            return tree;
        }
    }
}
