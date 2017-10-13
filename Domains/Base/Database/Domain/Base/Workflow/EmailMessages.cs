// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailMessages.cs" company="Allors bvba">
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
