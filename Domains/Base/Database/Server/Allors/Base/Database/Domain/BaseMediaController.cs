namespace Allors.Server
{
    using System;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public abstract partial class BaseMediaController : Controller
    {
        private const int OneYearInSeconds = 60 * 60 * 24 * 356;

        protected BaseMediaController(ISessionService sessionService)
        {
            this.Session = sessionService.Session;
        }

        private ISession Session { get; }

        [AllowAnonymous]
        [ResponseCache(Duration = OneYearInSeconds)]
        public virtual ActionResult Download(string id, string revision)
        {
            if (Guid.TryParse(id, out Guid uniqueId))
            {
                var media = new Medias(this.Session).FindBy(M.Media.UniqueId, uniqueId);
                var mediaContent = media?.MediaContent;

                if (mediaContent != null)
                {
                    return this.File(mediaContent.Data, mediaContent.Type, media.FileName);
                }
            }

            return this.NotFound("Media with unique id " + uniqueId + " not found.");
        }
    }
}