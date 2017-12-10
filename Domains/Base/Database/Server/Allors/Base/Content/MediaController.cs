namespace Allors.Server.Controllers
{
    using Allors.Services;

    public class MediaController : BaseMediaController
    {
        public MediaController(ISessionService sessionService)
            : base(sessionService)
        {
        }
    }
}
