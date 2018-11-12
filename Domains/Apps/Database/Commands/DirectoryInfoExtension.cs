// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryInfoExtension.cs" company="Allors bvba">
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