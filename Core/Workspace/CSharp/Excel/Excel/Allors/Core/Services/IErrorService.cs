namespace Allors.Excel
{
    using Protocol.Remote;
    using Workspace;

    public interface IErrorService
    {
        void Handle(Response response, ISession session);
    }
}
