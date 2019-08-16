
// <copyright file="CorePrintController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

// ReSharper disable Mvc.ActionNotResolved

namespace Allors.Server
{
    using System;

    using Allors.Domain;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public abstract partial class CorePrintController : Controller
    {
        private const int OneYearInSeconds = 60 * 60 * 24 * 356;

        protected CorePrintController(ISessionService sessionService) => this.Session = sessionService.Session;

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
