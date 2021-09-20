namespace Allors.Excel
{
    public interface IServiceLocator
    {
        IErrorService GetErrorService();

        IMessageService GetMessageService();
    }
}
