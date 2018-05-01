// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfService.cs" company="Allors bvba">
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

        public async Task<byte[]> FromHtmlToPdf(string content, string header = null, string footer = null)
        {
            string headerUrl = null;
            string footerUrl = null;

            try
            {
                if (!string.IsNullOrEmpty(header))
                {
                    headerUrl = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".html");
                    File.WriteAllText(headerUrl, header);
                }

                if (!string.IsNullOrEmpty(footer))
                {
                    footerUrl = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".html");
                    File.WriteAllText(footerUrl, footer);
                }

                var doc = new HtmlToPdfDocument()
                              {
                                  GlobalSettings = this.globalSettings,
                                  Objects =
                                      {
                                          new ObjectSettings
                                              {
                                                  PagesCount = true,
                                                  HtmlContent = content,
                                                  HeaderSettings = !string.IsNullOrEmpty(headerUrl)
                                                          ? new
                                                            HeaderSettings
                                                                {
                                                                    HtmUrl = headerUrl
                                                                }
                                                          : null,
                                                  FooterSettings = !string.IsNullOrEmpty(footerUrl)
                                                          ? new
                                                            FooterSettings
                                                                {
                                                                    HtmUrl = footerUrl,
                                                                }
                                                          : null,
                                                  WebSettings =
                                                      {
                                                          DefaultEncoding = "utf-8",
                                                          PrintMediaType = true
                                                      },
                                              }
                                      }
                              };

                return this.converter.Convert(doc);
            }
            finally
            {
                try
                {
                    if (headerUrl != null)
                    {
                        File.Delete(headerUrl);
                    }
                }
                catch{}

                try
                {
                    if (footerUrl != null)
                    {
                        File.Delete(footerUrl);
                    }
                }
                catch{}
            }
        }

        public void Dispose()
        {
        }
    }
}