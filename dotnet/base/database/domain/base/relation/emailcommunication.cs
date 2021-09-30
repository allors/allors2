// <copyright file="EmailCommunication.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class EmailCommunication
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.EmailCommunication, M.EmailCommunication.CommunicationEventState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void BaseOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistSubject && this.ExistEmailTemplate && this.EmailTemplate.ExistSubjectTemplate)
            {
                this.Subject = this.EmailTemplate.SubjectTemplate;
            }

            this.WorkItemDescription = $"Email to {this.ToEmail} about {this.Subject}";
        }
    }
}
