namespace Allors.Server
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Allors.Domain;
    using Allors.Meta;

    using Microsoft.AspNetCore.Authorization;

    public abstract partial class BaseMediaController : AllorsController
    {
        private const int OneYearInSeconds = 60 * 60 * 24 * 356;

        public BaseMediaController(IAllorsContext allorsContext)
            : base(allorsContext)
        {
        }

        [AllowAnonymous]
        [ResponseCache(Duration = OneYearInSeconds)]
        public virtual ActionResult Download(string id, string revision)
        {
            if (Guid.TryParse(id, out Guid uniqueId))
            {
                var media = new Medias(this.AllorsSession).FindBy(M.Media.UniqueId, uniqueId);
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