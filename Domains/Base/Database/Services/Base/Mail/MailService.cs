// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailService.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Services.Base
{
    using Allors.Domain;

    using MailKit.Net.Smtp;

    using MimeKit;

    public class MailService : IMailService
    {
        public string DefaultSender { get; set; }

        public string DefaultSenderName { get; set; }

        public void Send(EmailMessage emailMesssage)
        {
            var message = new MimeMessage
                                      {
                                          Subject = emailMesssage.Subject,
                                          Body = new TextPart("html") { Text = emailMesssage.Body }
                                      };

            var sender = emailMesssage.Sender?.UserEmail ?? this.DefaultSender;
            var senderName = emailMesssage.Sender?.UserName ?? this.DefaultSenderName;

            message.From.Add(new MailboxAddress(senderName, sender));


            if (emailMesssage.ExistRecipientEmailAddress)
            {
                message.To.Add(new MailboxAddress(emailMesssage.RecipientEmailAddress));
            }

            foreach (User recipient in emailMesssage.Recipients)
            {
                message.To.Add(new MailboxAddress(recipient.UserEmail));
            }
            
            using (var client = new SmtpClient())
            {
                client.Connect("smtp");
                client.Send(message);
            }
        }

        public void Dispose()
        {
        }
    }
}