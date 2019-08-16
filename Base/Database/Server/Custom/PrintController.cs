namespace Allors.Server
{
    using Allors.Services;

    public class PrintController : CorePrintController
    {
        public PrintController(ISessionService sessionService)
            : base(sessionService)
        {
        }
    }
}
