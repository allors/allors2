namespace Allors.Server
{
    using Allors.Services;

    public class MediaController : CoreMediaController
    {
        public MediaController(ISessionService sessionService)
            : base(sessionService)
        {
        }
    }
}
