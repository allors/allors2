// -------------------------------------------------------------------------------------------------
// <copyright file="ResourceDirectory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Log type.</summary>
// ---------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Resources
{
    using System.Collections.Generic;
    using System.IO;

    internal class ResourceDirectory
    {
        private readonly Dictionary<string, ResourceFile> resourceByFileName = new Dictionary<string, ResourceFile>();

        internal ResourceDirectory(DirectoryInfo directory)
        {
            directory.Refresh();

            foreach (var file in directory.EnumerateFiles("*.resx"))
            {
                var resourceFile = new ResourceFile(file);
                this.resourceByFileName[file.Name] = resourceFile;
            }
        }

        internal void CollectFileNames(HashSet<string> fileNames) => fileNames.UnionWith(this.resourceByFileName.Keys);

        internal void Merge(string fileName, Dictionary<string, object> dictionary)
        {
            if (this.resourceByFileName.TryGetValue(fileName, out var resourceFile))
            {
                resourceFile.Merge(dictionary);
            }
        }
    }
}
