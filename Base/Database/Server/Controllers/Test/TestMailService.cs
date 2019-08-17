// <copyright file="TestMailService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System.Collections.Generic;

    using Allors.Domain;
    using Allors.Services;

    public class TestMailService : IMailService
    {
        public TestMailService() => this.Sent = new List<EmailMessage>();

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
