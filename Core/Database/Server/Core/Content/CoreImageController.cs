// <copyright file="CoreMediaController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using System.IO;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SkiaSharp;

    public abstract partial class CoreImageController : Controller
    {
        protected const int OneYearInSeconds = 60 * 60 * 24 * 356;

        protected CoreImageController(ISessionService sessionService) => this.Session = sessionService.Session;

        private ISession Session { get; }

        [AllowAnonymous]
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = OneYearInSeconds)]
        [HttpGet("/image/{idString}/{revisionString}/{*name}")]
        public virtual IActionResult DownloadWithRevision(string idString, string revisionString, int? w)
        {
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

                        return this.File(data, media.MediaContent.Type, media.FileName);
                    }
                }
            }

            return this.NotFound("Image with id " + id + " not found.");
        }


        public byte[] Resize(byte[] src, int width, SKFilterQuality quality = SKFilterQuality.Medium)
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
    }
}
