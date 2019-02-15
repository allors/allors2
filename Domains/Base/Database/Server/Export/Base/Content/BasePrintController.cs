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
    using System;

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

                if (!printable.PrintDocument.ExistMedia)
                {
                    return this.NoContent();
                }

                return this.RedirectToAction("DownloadWithRevision", new { id, revision = printable.PrintDocument.Media.Revision });
            }

            return this.NotFound("Printable with id " + id + " has no document.");
        }

        [AllowAnonymous]
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = OneYearInSeconds, VaryByQueryKeys = new[] { "revision" })]
        public virtual ActionResult DownloadWithRevision(string id, string revision)
        {
            var printable = this.Session.Instantiate(id) as Printable;

            if (printable?.ExistPrintDocument == true)
            {
                if (!printable.PrintDocument.ExistMedia)
                {
                    printable.Print();
                    this.Session.Derive();
                    this.Session.Commit();

                    if (!printable.PrintDocument.ExistMedia)
                    {
                        return this.NoContent();
                    }

                    return this.RedirectToAction("DownloadWithRevision", new { id, revision = printable.PrintDocument.Media.Revision });
                }

                if (Guid.TryParse(revision, out var revisionGuid))
                {
                    if (!revisionGuid.Equals(printable.PrintDocument.Media.Revision))
                    {
                        return this.RedirectToAction("DownloadWithRevision", new { id, revision = printable.PrintDocument.Media.Revision });
                    }
                }

                var mediaContent = printable.PrintDocument.Media.MediaContent;
                if (mediaContent?.Data == null)
                {
                    return this.NoContent();
                }

                return this.File(mediaContent.Data, mediaContent.Type, printable.PrintDocument.Media.FileName);
            }

            return this.NotFound("Printable with id " + id + " has no document.");
        }
    }
}