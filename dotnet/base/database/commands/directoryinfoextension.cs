// <copyright file="DirectoryInfoExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System.IO;
    using System.Linq;

    public static class DirectoryInfoExtension
    {
        public static DirectoryInfo GetAncestorSibling(this DirectoryInfo @this, string name)
        {
            var ancestor = @this.Parent;
            while (true)
            {
                if (ancestor == null)
                {
                    return null;
                }

                var sibling = ancestor.GetDirectories()
                    .FirstOrDefault(v => v.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()));
                if (sibling != null)
                {
                    return sibling;
                }

                ancestor = ancestor.Parent;
            }
        }
    }
}
