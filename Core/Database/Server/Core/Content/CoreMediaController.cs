// <copyright file="CoreMediaController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;

    public abstract partial class CoreMediaController : Controller
    {
        protected const int OneYearInSeconds = 60 * 60 * 24 * 356;

        protected CoreMediaController(ISessionService sessionService) => this.Session = sessionService.Session;

        private ISession Session { get; }

        [Authorize]
        [AllowAnonymous]
        [HttpGet("/media/{idString}/{*name}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public virtual IActionResult RedirectOrNotFound(string idString, string name)
        {
            if (Guid.TryParse(idString, out var id))
            {
                var media = new Medias(this.Session).FindBy(M.Media.UniqueId, id);
                if (media != null)
                {
                    return this.RedirectToAction(nameof(Get), new { idString = media.Id.ToString("N"), revision = media.Revision?.ToString("N") });
                }
            }

            return this.NotFound("Media with id " + id + " not found.");
        }

        [Authorize]
        [AllowAnonymous]
        [HttpGet("/media/{idString}/{revisionString}/{*name}")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = OneYearInSeconds)]
        public virtual IActionResult Get(string idString, string revisionString, string name)
        {
            if (Guid.TryParse(idString, out var id))
            {
                var media = new Medias(this.Session).FindBy(M.Media.UniqueId, id);
                if (media != null)
                {
                    if (media.MediaContent?.Data == null)
                    {
                        return this.NoContent();
                    }

                    if (Guid.TryParse(revisionString, out var revision))
                    {
                        if (media.Revision != revision)
                        {
                            return this.RedirectToAction(nameof(RedirectOrNotFound), new { idString, name });
                        }
                    }
                    else
                    {
                        return this.RedirectToAction(nameof(RedirectOrNotFound), new { idString, name });
                    }

                    // Use Etags
                    this.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var requestEtagValues);
                    if (requestEtagValues != StringValues.Empty)
                    {
                        var etagValueString = requestEtagValues.FirstOrDefault()?.Replace("\"", string.Empty);
                        if (Guid.TryParse(etagValueString, out var etagValue))
                        {
                            if (media.Revision.Equals(etagValue))
                            {
                                this.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status304NotModified;
                                this.Response.ContentLength = 0L;
                                return this.Content(string.Empty);
                            }
                        }
                    }

                    this.Response.Headers[HeaderNames.ETag] = $"\"{media.Revision}\"";


                    var data = media.MediaContent.Data;
                    return this.File(data, media.MediaContent.Type, name ?? media.FileName);
                }
            }

            return this.NotFound("Media with id " + id + " not found.");
        }
    }
}
