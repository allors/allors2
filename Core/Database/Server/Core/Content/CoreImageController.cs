// <copyright file="CoreMediaController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;
    using SkiaSharp;
    using ISession = Allors.ISession;

    public abstract partial class CoreImageController : Controller
    {
        protected const int OneYearInSeconds = 60 * 60 * 24 * 356;

        protected CoreImageController(ISessionService sessionService)
        {
            this.Session = sessionService.Session;
            this.ETagByPath = new ConcurrentDictionary<string, string>();
        }

        private ISession Session { get; }

        private ConcurrentDictionary<string, string> ETagByPath { get; }

        [AllowAnonymous]
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = OneYearInSeconds)]
        [HttpGet("/image/{idString}/{revisionString}/{*name}")]
        public virtual IActionResult Get(string idString, string revisionString, string name, int? w, int? q, string t, string b)
        {
            this.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var requestEtagValues);
            var requestEtag = requestEtagValues.FirstOrDefault();
            if (requestEtag != null && this.ETagByPath.TryGetValue(requestEtag, out var etagPath))
            {
                if (etagPath.Equals(this.Request.Path))
                {
                    return this.NotModified();
                }
            }

            if (Guid.TryParse(idString, out var id))
            {
                if (Guid.TryParse(revisionString, out var revision))
                {
                    var media = new Medias(this.Session).FindBy(M.Media.UniqueId, id);
                    if (media != null)
                    {
                        if (media.Revision != revision)
                        {
                            return this.RedirectPermanent($"/image/{id}/{media.Revision}");
                        }

                        if (media.MediaContent?.Data == null)
                        {
                            return this.NoContent();
                        }

                        var data = media.MediaContent.Data;

                        if (w != null)
                        {
                            data = this.Resize(data, w.Value);
                        }

                        var type = t;
                        var quality = q;
                        var background = b;

                        var responseEtag = this.Etag(data);

                        if (responseEtag.Equals(requestEtag))
                        {
                            return this.NotModified();
                        }
                        
                        this.Response.Headers[HeaderNames.ETag] = responseEtag;
                        
                        return this.File(data, media.MediaContent.Type, name ?? media.FileName);
                    }
                }
            }

            return this.NotFound("Image with id " + id + " not found.");
        }

        private IActionResult NotModified()
        {
            this.Response.StatusCode = StatusCodes.Status304NotModified;
            this.Response.ContentLength = 0L;
            return this.Content(string.Empty);
        }

        private byte[] Resize(byte[] src, int width, SKFilterQuality quality = SKFilterQuality.High)
        {
            using var ms = new MemoryStream(src);
            using var sourceBitmap = SKBitmap.Decode(ms);

            var aspectRatio = (float)sourceBitmap.Height / sourceBitmap.Width;
            var height = (int)Math.Round(width * aspectRatio);

            using var scaledBitmap = sourceBitmap.Resize(new SKImageInfo(width, height), quality);
            using var scaledImage = SKImage.FromBitmap(scaledBitmap);
            using var data = scaledImage.Encode();

            return data.ToArray();
        }
        
        public string Etag(byte[] binary)
        {
            using var sha1 = new SHA1Managed();
            var hash = sha1.ComputeHash(binary);
            return Convert.ToBase64String(hash);
        }
    }
}
