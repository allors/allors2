// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Services
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

            message.From.Add(new MimeKit.MailboxAddress(senderName, sender));


            if (emailMesssage.ExistRecipientEmailAddress)
            {
                message.To.Add(new MimeKit.MailboxAddress(emailMesssage.RecipientEmailAddress));
            }

            foreach (User recipient in emailMesssage.Recipients)
            {
                message.To.Add(new MimeKit.MailboxAddress(recipient.UserEmail));
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
