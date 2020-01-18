// <copyright file="CoreMediaController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using System.Linq;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;
    using ISession = Allors.ISession;

    public abstract partial class CoreMediaController : Controller
    {
        protected CoreMediaController(ISessionService sessionService) => this.Session = sessionService.Session;

        private ISession Session { get; }

        [AllowAnonymous]
        [HttpGet("/media/{idString}/{*name}")]
        public virtual IActionResult Get(string idString, string revision)
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

                    if (Guid.TryParse(revision, out var requestRevision))
                    {
                        // Use Caching
                        if (media.Revision == requestRevision)
                        {
                            this.Response.Headers[HeaderNames.CacheControl] = "max-age=31536000";
                        }
                        else
                        {
                            return this.RedirectToAction("Get", new { idString, revision = media.Revision });
                        }
                    }
                    else
                    {
                        // Use Etags
                        this.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var requestEtagValues);
                        if (requestEtagValues != StringValues.Empty)
                        {
                            var requestEtag = requestEtagValues.FirstOrDefault()?.Replace("\"", string.Empty);
                            if (Guid.TryParse(requestEtag, out requestRevision))
                            {
                                if (media.Revision.Equals(requestRevision))
                                {
                                    this.Response.StatusCode = StatusCodes.Status304NotModified;
                                    this.Response.ContentLength = 0L;
                                    return this.Content(string.Empty);
                                }
                            }
                        }

                        this.Response.Headers[HeaderNames.ETag] = $"\"{media.Revision}\"";
                    }

                    var data = media.MediaContent.Data;
                    return this.File(data, media.MediaContent.Type, media.FileName);
                }
            }

            return this.NotFound("Image with id " + id + " not found.");
        }
    }
}
