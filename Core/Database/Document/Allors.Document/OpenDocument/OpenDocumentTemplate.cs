// <copyright file="OpenDocumentTemplate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Document.OpenDocument
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using Allors.Document.Xml;
    using Antlr4.StringTemplate;
    using Antlr4.StringTemplate.Misc;

    public class OpenDocumentTemplate
    {
        internal const string ContentFileName = "content.xml";

        internal const string StylesFileName = "styles.xml";

        internal const string MetaFileName = "META-INF/manifest.xml";

        protected const char DefaultLeftDelimiter = '\u02C2';

        protected const char DefaultRightDelimiter = '\u02C3';

        private const string MainTemplateName = "main";

        private readonly Dictionary<string, byte[]> fileByFileName;

        private readonly TemplateGroup ContentTemplateGroup;

        private readonly TemplateGroup StylesTemplateGroup;

        private readonly byte[] manifest;

        public OpenDocumentTemplate(byte[] document, string arguments, char leftDelimiter = DefaultLeftDelimiter, char rightDelimiter = DefaultRightDelimiter)
        {
            this.fileByFileName = new Dictionary<string, byte[]>();

            using (var zip = new MemoryStream(document))
            {
                using (var archive = new ZipArchive(zip, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var zipStream = entry.Open())
                            {
                                zipStream.CopyTo(memoryStream);
                                var file = memoryStream.ToArray();

                                var xmlStringRenderer = new XmlStringRenderer();

                                if (entry.FullName.Equals(ContentFileName))
                                {
                                    var errorBuffer = new ErrorBuffer();

                                    var content = new OpenDocumentTemplateContent(file, leftDelimiter, rightDelimiter);
                                    var stringTemplate = content.ToStringTemplate();
                                    var group = MainTemplateName + "(" + arguments + ")" + stringTemplate;
                                    this.ContentTemplateGroup = new TemplateGroupString(MainTemplateName, group, leftDelimiter, rightDelimiter)
                                    {
                                        ErrorManager = new ErrorManager(errorBuffer),
                                    };

                                    // Force a compilation of the templates to check for errors
                                    this.ContentTemplateGroup.GetInstanceOf(MainTemplateName);
                                    if (errorBuffer.Errors.Count > 0)
                                    {
                                        throw new TemplateException(errorBuffer.Errors);
                                    }

                                    this.ContentTemplateGroup.RegisterRenderer(typeof(string), xmlStringRenderer);
                                }
                                else if (entry.FullName.Equals(StylesFileName))
                                {
                                    var errorBuffer = new ErrorBuffer();

                                    var content = new OpenDocumentTemplateContent(file, leftDelimiter, rightDelimiter);
                                    var stringTemplate = content.ToStringTemplate();
                                    var group = MainTemplateName + "(" + arguments + ")" + stringTemplate;
                                    this.StylesTemplateGroup = new TemplateGroupString(MainTemplateName, group, leftDelimiter, rightDelimiter)
                                    {
                                        ErrorManager = new ErrorManager(errorBuffer),
                                    };

                                    // Force a compilation of the templates to check for errors
                                    this.StylesTemplateGroup.GetInstanceOf(MainTemplateName);
                                    if (errorBuffer.Errors.Count > 0)
                                    {
                                        throw new TemplateException(errorBuffer.Errors);
                                    }

                                    this.StylesTemplateGroup.RegisterRenderer(typeof(string), xmlStringRenderer);
                                }
                                else if (entry.FullName.Equals(MetaFileName))
                                {
                                    this.manifest = file;
                                }
                                else
                                {
                                    this.fileByFileName.Add(entry.FullName, file);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static string InferArguments(Type fromType) => string.Join(",", fromType.GetProperties().Select(v => v.Name));

        public byte[] Render(IDictionary<string, object> model, IDictionary<string, byte[]> imageByName = null)
        {
            var clonedFileByFileName = new Dictionary<string, byte[]>(this.fileByFileName);
            var render = new OpenDocumentRendering(model, this.manifest, imageByName, this.ContentTemplateGroup, this.StylesTemplateGroup, clonedFileByFileName);
            return render.Execute();
        }
    }
}
