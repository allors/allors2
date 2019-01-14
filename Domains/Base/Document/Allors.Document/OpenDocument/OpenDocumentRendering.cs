// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenDocumentRendering.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba
// This file is licenses under the Lesser General Public Licence v3 (LGPL)
// The LGPL License is included in the file LICENSE.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Document.OpenDocument
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Xml;

    using Antlr4.StringTemplate;
    using Antlr4.StringTemplate.Misc;

    public class OpenDocumentRendering
    {
        private const string MainTemplateName = "main";

        private readonly IDictionary<string, object> model;
        private readonly string stringTemplate;
        private readonly char leftDelimiter;
        private readonly char rightDelimiter;
        private readonly IDictionary<string, byte[]> fileByFileName;

        private readonly Image[] images;

        private byte[] manifestFile;

        public OpenDocumentRendering(IDictionary<string, object> model, byte[] manifestFile, IDictionary<string, byte[]> imageByName, string stringTemplate, char leftDelimiter, char rightDelimiter, Dictionary<string, byte[]> fileByFileName)
        {
            this.model = model;
            this.manifestFile = manifestFile;
            this.stringTemplate = stringTemplate;
            this.leftDelimiter = leftDelimiter;
            this.rightDelimiter = rightDelimiter;
            this.fileByFileName = fileByFileName;

            this.images = imageByName?.Select(v => new Image { Name = v.Key, Contents = v.Value }).ToArray() ?? new Image[0];
        }

        public byte[] Execute()
        {
            var errorBuffer = new ErrorBuffer();

            var argumentNames = this.model.Keys.ToArray();
            var group = "main(" + string.Join(",", argumentNames) + ")" + this.stringTemplate;
            var templateGroup = new TemplateGroupString("main", group, this.leftDelimiter, this.rightDelimiter)
            {
                ErrorManager = new ErrorManager(errorBuffer)
            };

            // TODO: register renderers on templateGroup
            //templateGroup.RegisterRenderer(typeof(String), new StringRenderer(this));
            
            var template = templateGroup.GetInstanceOf(MainTemplateName);

            if (errorBuffer.Errors.Count > 0)
            {
                throw new TemplateException(errorBuffer.Errors);
            }

            foreach (var kvp in this.model)
            {
                template.Add(kvp.Key, kvp.Value);
            }

            var generated = template.Render();

            if (errorBuffer.Errors.Count > 0)
            {
                throw new TemplateException(errorBuffer.Errors);
            }

            if (this.images.Length > 0)
            {
                var document = new XmlDocument();
                document.LoadXml(generated);
                var frameElements = document.GetElementsByTagName("draw:frame");
                foreach (XmlElement frameElement in frameElements)
                {
                    var drawNs = frameElement.GetNamespaceOfPrefix("draw");
                    var name = frameElement.GetAttribute("name", drawNs).Trim();
                    var image = this.images.FirstOrDefault(v => v.Name.Equals(name));
                    if (image != null)
                    {
                        var imageElement = (XmlElement)frameElement.GetElementsByTagName("draw:image")[0];
                        var xlinkNs = imageElement.GetNamespaceOfPrefix("xlink");
                        image.OriginalFullPath = imageElement.GetAttribute("href", xlinkNs);
                        imageElement.SetAttribute("href", xlinkNs, image.FullPath);
                    }
                }

                generated = document.OuterXml;

                var manifest = new XmlDocument();
                manifest.Load(new MemoryStream(this.manifestFile));
                {
                    var documentElement = manifest.DocumentElement;
                    var elements = documentElement.ChildNodes.OfType<XmlElement>().ToArray();
                    foreach (var element in elements)
                    {
                        var manifestNs = element.GetNamespaceOfPrefix("manifest");
                        var fullPath = element.GetAttribute("full-path", manifestNs);

                        foreach (var image in this.images.Where(v => v.OriginalFullPath.Equals(fullPath)))
                        {
                            var newElement = (XmlElement)element.Clone();
                            documentElement.AppendChild(newElement);

                            manifestNs = newElement.GetNamespaceOfPrefix("manifest");
                            newElement.SetAttribute("full-path", manifestNs, image.FullPath);
                        }
                    }
                }

                using (var output = new MemoryStream())
                {
                    manifest.Save(output);
                    this.manifestFile = output.ToArray();
                }
            }

            foreach (var image in this.images.Where(v => v.OriginalFullPath != null))
            {
                this.fileByFileName.Add(image.FullPath, image.Contents);
            }

            using (var zip = new MemoryStream())
            {
                using (var archive = new ZipArchive(zip, ZipArchiveMode.Create))
                {
                    {
                        var zipContent = archive.CreateEntry(OpenDocumentTemplate.ContentFileName);
                        var buffer = Encoding.UTF8.GetBytes(generated);
                        using (var memoryStream = new MemoryStream(buffer))
                        {
                            using (var zipStream = zipContent.Open())
                            {
                                memoryStream.CopyTo(zipStream);
                            }
                        }
                    }

                    {
                        var zipContent = archive.CreateEntry(OpenDocumentTemplate.MetaFileName);
                        using (var memoryStream = new MemoryStream(this.manifestFile))
                        {
                            using (var zipStream = zipContent.Open())
                            {
                                memoryStream.CopyTo(zipStream);
                            }
                        }
                    }

                    foreach (var kvp in this.fileByFileName)
                    {
                        var fileName = kvp.Key;
                        var file = kvp.Value;

                        var zipContent = archive.CreateEntry(fileName);

                        using (var memoryStream = new MemoryStream(file))
                        {
                            using (var zipStream = zipContent.Open())
                            {
                                memoryStream.CopyTo(zipStream);
                            }
                        }
                    }
                }

                return zip.ToArray();
            }
        }
    }
}