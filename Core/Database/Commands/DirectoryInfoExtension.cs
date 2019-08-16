// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryInfoExtension.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
