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
    using PuppeteerSharp;

    public class PdfService : IPdfService, IDisposable
    {
        private Browser browser;

        public async Task<byte[]> FromHtmlToPdf(string content)
        {
            if (this.browser == null)
            {
                var launchOptions = new LaunchOptions
                                        {
                                            Headless = true
                                        };

                await Downloader.CreateDefault().DownloadRevisionAsync(Downloader.DefaultRevision);
                this.browser = await Puppeteer.LaunchAsync(launchOptions, Downloader.DefaultRevision);
            }        
            
            var page = await this.browser.NewPageAsync();
            await page.SetContentAsync(content);
            var stream = await page.PdfStreamAsync();
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public void Dispose()
        {
            this.browser?.Dispose();
        }
    }
}