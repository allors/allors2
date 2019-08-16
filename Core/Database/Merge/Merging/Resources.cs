// -------------------------------------------------------------------------------------------------
// <copyright file="Resources.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
                    using (var resx = new ResourceWriter(fileInfo.FullName))
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
