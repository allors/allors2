// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheService.cs" company="Allors bvba">
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
    using System.IO;
    using System.Threading.Tasks;

    using DinkToPdf;
    using DinkToPdf.Contracts;

    public class PdfService : IPdfService, IDisposable
    {
        private readonly IConverter converter;
        private readonly GlobalSettings globalSettings;

        public PdfService(IConverter converter)
        {
            this.converter = converter;

            this.globalSettings = new GlobalSettings
                                      {
                                          ColorMode = ColorMode.Color,
                                          Orientation = Orientation.Portrait,
                                          PaperSize = PaperKind.A4,
                                      };
        }

        public async Task<byte[]> FromHtmlToPdf(string content)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = this.globalSettings,
                Objects = 
                    {
                        new ObjectSettings 
                            {
                                PagesCount = true,
                                HtmlContent = content,
                                WebSettings = { DefaultEncoding = "utf-8" },
                            }
                    }
            };

            return this.converter.Convert(doc);
        }

        public void Dispose()
        {
        }
    }
}