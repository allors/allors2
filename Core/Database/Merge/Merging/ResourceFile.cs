// -------------------------------------------------------------------------------------------------
// <copyright file="ResourceFile.cs" company="Allors bvba">
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
    using System.Xml.Linq;

    internal class ResourceFile
    {
        internal ResourceFile(FileInfo fileInfo)
        {
            this.FileName = fileInfo.Name;
            try
            {
                this.Document = XDocument.Load(fileInfo.FullName);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
        }

        public string FileName { get; }

        public XDocument Document { get; }

        public void Merge(FileInfo fileInfo)
        {
            var mergeDocument = XDocument.Load(fileInfo.FullName);

            foreach (var mergeData in mergeDocument.Root.Elements("data"))
            {
                var data = this.Document.Root.Elements("data")
                    .SingleOrDefault(v => v.Attribute("name")?.Value == mergeData.Attribute("name")?.Value);

                if (data != null)
                {
                    data.Value = mergeData.Value;
                }
                else
                {
                    data = new XElement(mergeData);
                    this.Document.Root.Add(data);
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
