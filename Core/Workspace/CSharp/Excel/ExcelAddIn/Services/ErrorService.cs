namespace ExcelAddIn
{
    using Allors.Excel;
    using Allors.Protocol.Remote;
    using Allors.Workspace;

    public class ErrorService : IErrorService
    {
        public void Handle(Response response, ISession session) => response.HandleErrors(session);
    }
}
