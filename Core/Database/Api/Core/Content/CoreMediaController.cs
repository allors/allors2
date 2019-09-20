// <copyright file="CoreMediaController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Api
{
    using System;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public abstract partial class CoreMediaController : Controller
    {
        protected const int OneYearInSeconds = 60 * 60 * 24 * 356;

        protected CoreMediaController(ISessionService sessionService) => this.Session = sessionService.Session;

        private ISession Session { get; }

        [AllowAnonymous]
        public virtual IActionResult Download(string id)
        {
            if (Guid.TryParse(id, out var uniqueId))
            {
                var media = new Medias(this.Session).FindBy(M.Media.UniqueId, uniqueId);
                if (media != null)
                {
                    return this.RedirectToAction("DownloadWithRevision", new { id, revision = media.Revision });
                }
            }

            return this.NotFound("Media with unique id " + id + " not found.");
        }

        [AllowAnonymous]
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = OneYearInSeconds, VaryByQueryKeys = new[] { "revision" })]
        public virtual IActionResult DownloadWithRevision(string id, string revision)
        {
            if (Guid.TryParse(id, out var uniqueId))
            {
                var media = new Medias(this.Session).FindBy(M.Media.UniqueId, uniqueId);
                if (media != null)
                {
                    if (media.MediaContent?.Data == null)
                    {
                        return this.NoContent();
                    }

                    return this.File(media.MediaContent.Data, media.MediaContent.Type, media.FileName);
                }
            }

            return this.NotFound("Media with unique id " + id + " not found.");
        }
    }
}
