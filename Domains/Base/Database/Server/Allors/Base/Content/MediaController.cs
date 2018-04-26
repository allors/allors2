namespace Allors.Server.Controllers
{
    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;

    public class MediaController : BaseMediaController
    {
        public MediaController(ISessionService sessionService)
            : base(sessionService)
        {
        }

        [Route("Media/{id}/{*name}")]
        public override ActionResult Download(string id, string revision)
        {
            return base.Download(id, revision);
        }
    }
}
