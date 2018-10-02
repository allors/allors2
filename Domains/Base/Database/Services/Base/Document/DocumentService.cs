// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentService.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    using Sandwych.Reporting;
    using Sandwych.Reporting.OpenDocument;

    public class DocumentService : IDocumentService
    {
        private readonly ConcurrentDictionary<string, OdtTemplate> templateByName;

        public DocumentService()
        {
            this.templateByName = new ConcurrentDictionary<string, OdtTemplate>();
        }

        public DocumentService Register(string name, FileInfo fileInfo)
        {
            using (var stream = File.OpenRead(fileInfo.FullName))
            {
                return this.Register(name, stream);
            }
        }

        public DocumentService Register(string name, Stream stream)
        {
            var odt = OdfDocument.LoadFrom(stream);
            var template = new OdtTemplate(odt);
            this.templateByName[name] = template;

            return this;
        }

        public async Task<byte[]> Render(string templateName, IReadOnlyDictionary<string, object> data)
        {
            var context = new TemplateContext(data);

            var template = this.templateByName[templateName];
            var result = await template.RenderAsync(context);

            using (var outputStream = new MemoryStream())
            {
                await result.SaveAsync(outputStream);
                return outputStream.ToArray();
            }
        }
    }
}
