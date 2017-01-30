// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmploymentApplicationStatuses.cs" company="Allors bvba">
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

    public partial class EmploymentApplicationStatuses
    {
        private static readonly Guid ReceivedId = new Guid("0BB970D8-9273-42af-AD26-58217B1D8501");
        private static readonly Guid ReviewedId = new Guid("835D288A-940D-4597-A18D-1A818749070A");
        private static readonly Guid FiledId = new Guid("25FAF6A1-07B7-4ec4-AEF1-EF5C95CBFBDB");
        private static readonly Guid RejectedId = new Guid("EDEA15DA-714D-40c4-BA10-436BD84140D7");
        private static readonly Guid NotifiedOfNonInterestedId = new Guid("E889B083-22D4-4aa8-85BE-6416AA0839F9");
        private static readonly Guid EmployedId = new Guid("2CB5ABD4-8799-46c1-9E2B-538DB076492E");

        private UniquelyIdentifiableCache<EmploymentApplicationStatus> cache;

        public EmploymentApplicationStatus Received => this.Cache.Get(ReceivedId);

        public EmploymentApplicationStatus Reviewed => this.Cache.Get(ReviewedId);

        public EmploymentApplicationStatus Filed => this.Cache.Get(FiledId);

        public EmploymentApplicationStatus Rejected => this.Cache.Get(RejectedId);

        public EmploymentApplicationStatus NotifiedOfNonInterested => this.Cache.Get(NotifiedOfNonInterestedId);

        public EmploymentApplicationStatus Employed => this.Cache.Get(EmployedId);

        private UniquelyIdentifiableCache<EmploymentApplicationStatus> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableCache<EmploymentApplicationStatus>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new EmploymentApplicationStatusBuilder(this.Session)
                .WithName("Received")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Received").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ontvangen").WithLocale(dutchLocale).Build())
                .WithUniqueId(ReceivedId)
                .Build();
            
            new EmploymentApplicationStatusBuilder(this.Session)
                .WithName("Reviewed")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Reviewed").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gereviewed").WithLocale(dutchLocale).Build())
                .WithUniqueId(ReviewedId)
                .Build();
            
            new EmploymentApplicationStatusBuilder(this.Session)
                .WithName("Filed")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Filed").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ingediend").WithLocale(dutchLocale).Build())
                .WithUniqueId(FiledId)
                .Build();
            
            new EmploymentApplicationStatusBuilder(this.Session)
                .WithName("Rejected")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Rejected").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Geweigerd").WithLocale(dutchLocale).Build())
                .WithUniqueId(RejectedId)
                .Build();
            
            new EmploymentApplicationStatusBuilder(this.Session)
                .WithName("Notified Of Non Interested")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Notified Of Non Interested").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Niet geïnteresseerd beantwoord").WithLocale(dutchLocale).Build())
                .WithUniqueId(NotifiedOfNonInterestedId)
                .Build();
            
            new EmploymentApplicationStatusBuilder(this.Session)
                .WithName("Employed")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Employed").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aangenomen").WithLocale(dutchLocale).Build())
                .WithUniqueId(EmployedId)
                .Build();
        }
    }
}
