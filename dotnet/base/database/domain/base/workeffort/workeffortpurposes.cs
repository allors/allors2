// <copyright file="WorkEffortPurposes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class WorkEffortPurposes
    {
        private static readonly Guid MeetingId = new Guid("59C5AC07-9F1C-4641-90C3-B8126F19E7FF");
        private static readonly Guid PhonecallId = new Guid("622A4722-DC89-4FF2-8A6E-CB1491F2A377");
        private static readonly Guid EmailId = new Guid("931A8772-C42B-447F-AFEF-92D0F922C5E5");
        private static readonly Guid SupportId = new Guid("5FA66105-DA9F-415B-8E26-2D0AECF5510B");
        private static readonly Guid MaintenanceId = new Guid("A3DA2C62-53D7-481D-84D8-3437022A76D0");
        private static readonly Guid ProductionRunId = new Guid("D6DD8620-7A43-46BD-AE20-F5652A14727C");
        private static readonly Guid WorkFlowId = new Guid("553D67FC-9853-46B3-B87C-9CD1A7CB01A5");
        private static readonly Guid ResearchId = new Guid("6888DBF5-BFFC-4942-A16D-8BB680863261");

        private UniquelyIdentifiableSticky<WorkEffortPurpose> cache;

        public WorkEffortPurpose Meeting => this.Cache[MeetingId];

        public WorkEffortPurpose Phonecall => this.Cache[PhonecallId];

        public WorkEffortPurpose Email => this.Cache[EmailId];

        public WorkEffortPurpose Support => this.Cache[SupportId];

        public WorkEffortPurpose Maintenance => this.Cache[MaintenanceId];

        public WorkEffortPurpose ProductionRun => this.Cache[ProductionRunId];

        public WorkEffortPurpose WorkFlow => this.Cache[WorkFlowId];

        public WorkEffortPurpose Research => this.Cache[ResearchId];

        private UniquelyIdentifiableSticky<WorkEffortPurpose> Cache => this.cache ??= new UniquelyIdentifiableSticky<WorkEffortPurpose>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(MeetingId, v =>
            {
                v.Name = "Meeting";
                localisedName.Set(v, dutchLocale, "Vergadering");
                v.IsActive = true;
            });

            merge(PhonecallId, v =>
            {
                v.Name = "Phone Call";
                localisedName.Set(v, dutchLocale, "Telefoon gesprek");
                v.IsActive = true;
            });

            merge(EmailId, v =>
            {
                v.Name = "Email";
                localisedName.Set(v, dutchLocale, "Email");
                v.IsActive = true;
            });

            merge(SupportId, v =>
            {
                v.Name = "Support";
                localisedName.Set(v, dutchLocale, "Support");
                v.IsActive = true;
            });

            merge(MaintenanceId, v =>
            {
                v.Name = "Maintenance";
                localisedName.Set(v, dutchLocale, "Onderhoud");
                v.IsActive = true;
            });

            merge(ProductionRunId, v =>
            {
                v.Name = "Production Run";
                localisedName.Set(v, dutchLocale, "Productie run");
                v.IsActive = true;
            });

            merge(WorkFlowId, v =>
            {
                v.Name = "Workflow";
                localisedName.Set(v, dutchLocale, "Workflow");
                v.IsActive = true;
            });

            merge(ResearchId, v =>
            {
                v.Name = "Research";
                localisedName.Set(v, dutchLocale, "Onderzoek");
                v.IsActive = true;
            });
        }
    }
}
