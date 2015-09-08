// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommunicationEventPurposes.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

    public partial class WorkEffortPurposes
    {
        public static readonly Guid MeetingId = new Guid("59C5AC07-9F1C-4641-90C3-B8126F19E7FF");
        public static readonly Guid PhonecallId = new Guid("622A4722-DC89-4FF2-8A6E-CB1491F2A377");
        public static readonly Guid EmailId = new Guid("931A8772-C42B-447F-AFEF-92D0F922C5E5");
        public static readonly Guid ProjectId = new Guid("A3DA2C62-53D7-481D-84D8-3437022A76D0");
        public static readonly Guid SupportId = new Guid("5FA66105-DA9F-415B-8E26-2D0AECF5510B");

        private UniquelyIdentifiableCache<WorkEffortPurpose> cache;

        public WorkEffortPurpose Meeting
        {
            get { return this.Cache.Get(MeetingId); }
        }

        public WorkEffortPurpose Phonecall
        {
            get { return this.Cache.Get(PhonecallId); }
        }

        public WorkEffortPurpose Email
        {
            get { return this.Cache.Get(EmailId); }
        }

        public WorkEffortPurpose Project
        {
            get { return this.Cache.Get(ProjectId); }
        }

        public WorkEffortPurpose Support
        {
            get { return this.Cache.Get(SupportId); }
        }

        private UniquelyIdentifiableCache<WorkEffortPurpose> Cache
        {
            get 
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<WorkEffortPurpose>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Meeting")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Meeting").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vergadering").WithLocale(dutchLocale).Build())
                .WithUniqueId(MeetingId)
                .Build();

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Phonecall")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Phonecall").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Telefoon gesprek").WithLocale(dutchLocale).Build())
                .WithUniqueId(PhonecallId)
                .Build();

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Email")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Email").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Email").WithLocale(dutchLocale).Build())
                .WithUniqueId(EmailId)
                .Build();

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Project")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Project").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Project").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProjectId)
                .Build();

            new WorkEffortPurposeBuilder(this.Session)
                .WithName("Support")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Support").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Support").WithLocale(dutchLocale).Build())
                .WithUniqueId(SupportId)
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
