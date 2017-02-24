// ------------------------------------------------------------------------------------------------- 
// <copyright file="Resources.cs" company="Allors bvba">
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
    using System.Resources;

    public class Resources
    {
        private readonly DirectoryInfo outputDirectory;

        private readonly IList<ResourceDirectory> resourceDirectories = new List<ResourceDirectory>();
        private readonly HashSet<string> fileNames = new HashSet<string>();

        public Resources(DirectoryInfo[] inputDirectories, DirectoryInfo outputDirectory)
        {
            this.outputDirectory = outputDirectory;

            foreach (var directory in inputDirectories)
            {
                this.resourceDirectories.Add(new ResourceDirectory(directory));
            }

            foreach (var resourceDirectory in this.resourceDirectories)
            {
                resourceDirectory.CollectFileNames(this.fileNames);
            }
        }

        public void Merge()
        {
            foreach (var fileName in this.fileNames)
            {
                var dictionary = new Dictionary<string, object>();
                foreach (var resourceDirectory in this.resourceDirectories)
                {
                    resourceDirectory.Merge(fileName, dictionary);

                    var fileInfo = new FileInfo(Path.Combine(this.outputDirectory.FullName, fileName));
                    using (var resx = new ResXResourceWriter(fileInfo.FullName))
                    {
                        var keys = new List<string>(dictionary.Keys);
                        keys.Sort(string.CompareOrdinal);

                        foreach (var key in keys)
                        {
                            resx.AddResource(key, dictionary[key]);
                        }
                    }
                }
            }
        }
    }
}
