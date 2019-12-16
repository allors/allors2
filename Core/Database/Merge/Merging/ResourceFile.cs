// -------------------------------------------------------------------------------------------------
// <copyright file="ResourceFile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Log type.</summary>
// ---------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Resources
{
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    internal class ResourceFile
    {
        internal ResourceFile(FileInfo fileInfo)
        {
            this.FileName = fileInfo.Name;
            this.Document = XDocument.Load(fileInfo.FullName);
        }

        public string FileName { get; }

        public XDocument Document { get; }

        public void Merge(FileInfo fileInfo)
        {
            var mergeDocument = XDocument.Load(fileInfo.FullName);

            foreach (var mergeData in mergeDocument.Root.Elements("data"))
            {
                var root = this.Document.Root;
                var data = root.Elements("data").SingleOrDefault(v => v.Name.LocalName == mergeData.Name.LocalName);
                if (data != null)
                {
                    data.Value = mergeData.Value;
                }
                else
                {
                    data = new XElement(mergeData);
                    root.Add(data);
                }
            }
        }

        public void Write(DirectoryInfo outputDirectory)
        {
            var fileInfo = new FileInfo(Path.Combine(outputDirectory.FullName, this.FileName));
            this.Document.Save(fileInfo.FullName);
        }
    }
}
