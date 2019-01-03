// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Template.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.IO;

    using Sandwych.Reporting;
    using Sandwych.Reporting.OpenDocument;

    public partial class Template
    {
        public byte[] Render(IReadOnlyDictionary<string, object> model)
        {
            if (this.TemplateType.IsOdtTemplate)
            {
                var context = new TemplateContext(model);

                using (var stream = new MemoryStream(this.Media.MediaContent.Data))
                {
                    var odt = OdfDocument.LoadFrom(stream);
                    var template = new OdtTemplate(odt);
                    var result = template.Render(context);
                    using (var outputStream = new MemoryStream())
                    {
                        result.Save(outputStream);
                        return outputStream.ToArray();
                    }
                }
            }

            return null;
        }
    }
}