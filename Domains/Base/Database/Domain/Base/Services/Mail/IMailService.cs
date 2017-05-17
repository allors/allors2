namespace Allors.Services
{
    using Allors.Domain;

    public interface IMailService : IService
    {
        void Send(EmailMessage emailMesssage);
    }
}
