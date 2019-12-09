// <copyright file="CommunicationEventPurposes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class CommunicationEventPurposes
    {
        private static readonly Guid SupportCallId = new Guid("376805EE-5CBE-4cf0-95F4-F2649174FCBF");
        private static readonly Guid InquiryId = new Guid("C038D09A-5365-42ec-BAF9-A2D7D1E9E783");
        private static readonly Guid CustomerServiceCallId = new Guid("C31D91C7-1A96-47d5-8E85-76B89CED8E79");
        private static readonly Guid SalesFollowUpId = new Guid("8C95CE16-43ED-4450-B896-F6DBF66BC271");
        private static readonly Guid AppointmentId = new Guid("9C572283-BAEE-4F36-AD26-F0B22EDE0AD6");
        private static readonly Guid MeetingId = new Guid("4A953F5A-3E35-4ba7-9BA7-9E636908E00E");
        private static readonly Guid SeminarId = new Guid("25328926-7028-4e7d-A35F-DADDBD9EE926");
        private static readonly Guid ActivityRequestId = new Guid("D93A0B3C-0641-4b2a-B966-26964A505F68");
        private static readonly Guid ConferenceId = new Guid("475C801C-4965-49d8-B62A-5B3FDFEE2F72");
        private static readonly Guid SalesMeetingId = new Guid("344920EA-0903-4213-BC7C-837A74089706");
        private static readonly Guid InterviewId = new Guid("7143C121-1DF5-4a92-90BA-10800022BA12");

        private UniquelyIdentifiableSticky<CommunicationEventPurpose> cache;

        public CommunicationEventPurpose SupportCall => this.Cache[SupportCallId];

        public CommunicationEventPurpose Inquiry => this.Cache[InquiryId];

        public CommunicationEventPurpose CustomerServiceCall => this.Cache[CustomerServiceCallId];

        public CommunicationEventPurpose SalesFollowUp => this.Cache[SalesFollowUpId];

        public CommunicationEventPurpose Appointment => this.Cache[AppointmentId];

        public CommunicationEventPurpose Meeting => this.Cache[MeetingId];

        public CommunicationEventPurpose Seminar => this.Cache[SeminarId];

        public CommunicationEventPurpose ActivityRequest => this.Cache[ActivityRequestId];

        public CommunicationEventPurpose Conference => this.Cache[ConferenceId];

        public CommunicationEventPurpose SalesMeeting => this.Cache[SalesMeetingId];

        public CommunicationEventPurpose Interview => this.Cache[InterviewId];

        private UniquelyIdentifiableSticky<CommunicationEventPurpose> Cache => this.cache ??= new UniquelyIdentifiableSticky<CommunicationEventPurpose>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;
            
            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(SupportCallId, v =>
            {
                v.Name = "Support call";
                localisedName.Set(v, dutchLocale, "Support call");
                v.IsActive = true;
            });

            merge(InquiryId, v =>
            {
                v.Name = "Inquiry";
                localisedName.Set(v, dutchLocale, "Vraag");
                v.IsActive = true;
            });

            merge(CustomerServiceCallId, v =>
            {
                v.Name = "CustomerServiceCall";
                localisedName.Set(v, dutchLocale, "Klantendienst");
                v.IsActive = true;
            });

            merge(SalesFollowUpId, v =>
            {
                v.Name = "SalesFollowUp";
                localisedName.Set(v, dutchLocale, "Verkoop opvolging");
                v.IsActive = true;
            });

            merge(AppointmentId, v =>
            {
                v.Name = "Appointment";
                localisedName.Set(v, dutchLocale, "Afspraak");
                v.IsActive = true;
            });

            merge(MeetingId, v =>
            {
                v.Name = "Meeting";
                localisedName.Set(v, dutchLocale, "Vergadering");
                v.IsActive = true;
            });

            merge(SeminarId, v =>
            {
                v.Name = "Seminar";
                localisedName.Set(v, dutchLocale, "Seminar");
                v.IsActive = true;
            });

            merge(ActivityRequestId, v =>
            {
                v.Name = "ActivityRequest";
                localisedName.Set(v, dutchLocale, "Aanvraag activiteit");
                v.IsActive = true;
            });

            merge(ConferenceId, v =>
            {
                v.Name = "Conference";
                localisedName.Set(v, dutchLocale, "Conferentie");
                v.IsActive = true;
            });

            merge(SalesMeetingId, v =>
            {
                v.Name = "SalesMeeting";
                localisedName.Set(v, dutchLocale, "Verkoops meeting");
                v.IsActive = true;
            });

            merge(InterviewId, v =>
            {
                v.Name = "Interview";
                localisedName.Set(v, dutchLocale, "Interview");
                v.IsActive = true;
            });
        }
    }
}
