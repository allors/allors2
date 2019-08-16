// ------------------------------------------------------------------------------------------------- 
// <copyright file="ResourceDirectory.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
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
            ResourceFile resourceFile;
            if (this.resourceByFileName.TryGetValue(fileName, out resourceFile))
            {
                resourceFile.Merge(dictionary);
            }
        }
    }
}
