// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommunicationEventPurposes.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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

        private UniquelyIdentifiableSticky<WorkEffortPurpose> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<WorkEffortPurpose>(this.Session));

        protected override void BaseSetup(Setup setup)
        {


            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Meeting")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vergadering").WithLocale(dutchLocale).Build())
                .WithUniqueId(MeetingId)
                .WithIsActive(true)
                .Build();

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Phone Call")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Telefoon gesprek").WithLocale(dutchLocale).Build())
                .WithUniqueId(PhonecallId)
                .WithIsActive(true)
                .Build();

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Email")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Email").WithLocale(dutchLocale).Build())
                .WithUniqueId(EmailId)
                .WithIsActive(true)
                .Build();

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Support")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Support").WithLocale(dutchLocale).Build())
                .WithUniqueId(SupportId)
                .WithIsActive(true)
                .Build();

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Maintenance")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Onderhoud").WithLocale(dutchLocale).Build())
                .WithUniqueId(MaintenanceId)
                .WithIsActive(true)
                .Build();

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Production Run")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Productie run").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProductionRunId)
                .WithIsActive(true)
                .Build();

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Workflow")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Workflow").WithLocale(dutchLocale).Build())
                .WithUniqueId(WorkFlowId)
                .WithIsActive(true)
                .Build();

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Research")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Onderzoek").WithLocale(dutchLocale).Build())
                .WithUniqueId(ResearchId)
                .WithIsActive(true)
                .Build();
        }
    }
}
