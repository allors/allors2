// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseMediaController.cs" company="Allors bvba">
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

namespace Allors.Server
{
    using System.Threading.Tasks;

    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;

    public abstract partial class BasePdfController : Controller
    {
        protected BasePdfController(ISessionService sessionService, IPdfService pdfService)
        {
            this.Session = sessionService.Session;
            this.PdfService = pdfService;
        }

        protected ISession Session { get; }

        protected IPdfService PdfService { get; }

        protected async Task<ActionResult> Pdf(string html, string fileName)
        {
            var pdf = await this.PdfService.FromHtmlToPdf(html);
            return this.File(pdf, "application/pdf", fileName);
        }
    }
}