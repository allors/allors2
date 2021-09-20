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
    using System.Linq;
    using System.Runtime.InteropServices.ComTypes;

    public class Merger
    {
        private readonly Dictionary<string, ResourceFile> resourceFileByFilename;

        public Merger() => this.resourceFileByFilename = new Dictionary<string, ResourceFile>();

        public void Input(DirectoryInfo[] inputDirectories)
        {
            foreach (var inputDirectory in inputDirectories)
            {
                inputDirectory.Refresh();
                if (inputDirectory.Exists)
                {
                    var files = inputDirectory.GetFiles();
                    foreach (var inputFile in files.Where(v => v.Extension?.ToLower() == ".resx"))
                    {
                        if (!this.resourceFileByFilename.TryGetValue(inputFile.Name, out var resourceFile))
                        {
                            resourceFile = new ResourceFile(inputFile);
                            this.resourceFileByFilename[inputFile.Name] = resourceFile;
                        }
                        else
                        {
                            resourceFile.Merge(inputFile);
                        }
                    }
                }
            }
        }

        public void Output(DirectoryInfo outputDirectory)
        {
            foreach (var resourceFile in this.resourceFileByFilename.Values)
            {
                resourceFile.Write(outputDirectory);
            }
        }
    }
}
