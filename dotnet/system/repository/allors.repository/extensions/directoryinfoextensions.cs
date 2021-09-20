// <copyright file="DirectoryInfoExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository
{
    using System.IO;
    using System.Linq;

    public static class DirectoryInfoExtensions
    {
        public static bool Contains(this DirectoryInfo @this, FileInfo fileInfo)
        {
            var allFiles = @this.GetFiles("*", SearchOption.AllDirectories);
            return allFiles.Any(v => v.FullName.Equals(fileInfo.FullName));
        }
    }
}
