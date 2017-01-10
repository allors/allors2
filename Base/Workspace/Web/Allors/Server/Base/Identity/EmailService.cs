namespace Allors.Web.Identity
{
    using System.Configuration;
    using System.Net.Mail;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var client = new SmtpClient();
            var from = ConfigurationManager.AppSettings["allors.identity.mailservice.from"] ?? "support@" + System.Environment.MachineName.ToLower();
            client.Send(from, message.Destination, message.Subject, message.Body);
           
            return Task.FromResult(0);
        }
    }
}