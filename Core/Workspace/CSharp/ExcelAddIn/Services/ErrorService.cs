namespace Allors.Excel
{
    using Protocol.Remote;
    using Workspace;

    public class ErrorService : IErrorService
    {
        public void Handle(Response response, ISession session) => response.HandleErrors(session);
    }
}
