// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseExtensions.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
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