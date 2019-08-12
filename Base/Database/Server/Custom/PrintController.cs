namespace Allors.Server
{
    using Allors.Services;

    public class PrintController : BasePrintController
    {
        public PrintController(ISessionService sessionService)
            : base(sessionService)
        {
        }
    }
}
