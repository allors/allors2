﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoreMediaController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Server
{
    using System;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public abstract partial class CoreMediaController : Controller
    {
        private const int OneYearInSeconds = 60 * 60 * 24 * 356;

        protected CoreMediaController(ISessionService sessionService) => this.Session = sessionService.Session;

        private ISession Session { get; }

        [AllowAnonymous]
        public virtual ActionResult Download(string id)
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
        public virtual ActionResult DownloadWithRevision(string id, string revision)
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
