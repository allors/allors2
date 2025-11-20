// <copyright file="EmailMessages.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public partial class EmailMessages
    {
        public void Send()
        {
            var session = this.Session;

            var mailService = session.ServiceProvider.GetRequiredService<IMailService>();
            var emailMessages = this.Extent();
            emailMessages.Filter.AddNot().AddExists(this.Meta.DateSending);
            emailMessages.Filter.AddNot().AddExists(this.Meta.DateSent);

            foreach (EmailMessage emailMessage in emailMessages)
            {
                try
                {
                    emailMessage.DateSending = session.Now();

                    session.Derive();
                    session.Commit();

                    mailService.Send(emailMessage);
                    emailMessage.DateSent = session.Now();

                    session.Derive();
                    session.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    session.Rollback();
                    break;
                }
            }
        }
    }
}
