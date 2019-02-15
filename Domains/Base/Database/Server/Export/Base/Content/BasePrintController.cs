// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasePrintController.cs" company="Allors bvba">
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
    using Allors.Domain;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public abstract partial class BasePrintController : Controller
    {
        private const int OneYearInSeconds = 60 * 60 * 24 * 356;

        protected BasePrintController(ISessionService sessionService)
        {
            this.Session = sessionService.Session;
        }

        private ISession Session { get; }

        [AllowAnonymous]
        public virtual ActionResult Download(string id)
        {
            var printable = this.Session.Instantiate(id) as Printable;

            if (printable?.ExistPrintDocument == true)
            {
                if (!printable.PrintDocument.ExistMedia)
                {
                    printable.Print();
                    this.Session.Derive();
                    this.Session.Commit();
                }

                var media = printable.PrintDocument.Media;
                var mediaContent = media?.MediaContent;

                if (mediaContent != null)
                {
                    return this.File(mediaContent.Data, mediaContent.Type, media.FileName);
                }
            }

            return this.NotFound("Printable with id " + id + " not found.");
        }

        [AllowAnonymous]
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = OneYearInSeconds, VaryByQueryKeys = new[] { "revision" })]
        public virtual ActionResult DownloadWithRevision(string id, string revision)
        {
            return this.Download(id);
        }
    }
}