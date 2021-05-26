namespace ExcelAddIn
{
    using Allors.Excel;

    public class ServiceLocator : IServiceLocator
    {
        private ErrorService errorService;

        private MessageService messageService;

        public IErrorService GetErrorService() => this.errorService ??= new ErrorService();

        public IMessageService GetMessageService() => this.messageService ??= new MessageService();
    }
}
