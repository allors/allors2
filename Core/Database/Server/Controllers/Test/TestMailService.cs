namespace Allors.Services
{
    using System.Collections.Generic;

    using Allors.Domain;

    public class TestMailService : IMailService
    {
        public TestMailService()
        {
            this.Sent = new List<EmailMessage>();
        }

        public List<EmailMessage> Sent { get; }

        public string DefaultSender { get; set; }

        public void Send(EmailMessage emailMesssage)
        {
        }

        public void Dispose()
        {
        }
    }
}
