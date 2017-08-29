// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestObjectStates.cs" company="Allors bvba">
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

    public partial class RequestObjectStates
    {
        private static readonly Guid AnonymousId = new Guid("2F054949-E30C-4954-9A3C-191559DE8315");
        private static readonly Guid SubmittedId = new Guid("DB03407D-BCB1-433A-B4E9-26CEA9A71BFD");
        private static readonly Guid CancelledId = new Guid("6B839C79-689D-412C-9C27-B553CF350662");
        private static readonly Guid QuotedId = new Guid("79A9BAF5-D16F-4FA7-A2A0-CA20BF79833F");
        private static readonly Guid PendingCustomerId = new Guid("671FDA2F-5AA6-4EA5-B5D6-C914F0911690");
        private static readonly Guid RejectedId = new Guid("26B1E962-9799-4C53-AE36-20E8490F757A");

        private UniquelyIdentifiableCache<RequestObjectState> stateCache;

        public RequestObjectState Anonymous => this.StateCache.Get(AnonymousId);

        public RequestObjectState Submitted => this.StateCache.Get(SubmittedId);

        public RequestObjectState Cancelled => this.StateCache.Get(CancelledId);

        public RequestObjectState Quoted => this.StateCache.Get(QuotedId);

        public RequestObjectState PendingCustomer => this.StateCache.Get(PendingCustomerId);

        public RequestObjectState Rejected => this.StateCache.Get(RejectedId);

        private UniquelyIdentifiableCache<RequestObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<RequestObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new RequestObjectStateBuilder(this.Session)
                .WithUniqueId(AnonymousId)
                .WithName("Anonymous")
                .Build();

            new RequestObjectStateBuilder(this.Session)
                .WithUniqueId(SubmittedId)
                .WithName("Submitted")
                .Build();

            new RequestObjectStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new RequestObjectStateBuilder(this.Session)
                .WithUniqueId(QuotedId)
                .WithName("Quoted")
                .Build();

            new RequestObjectStateBuilder(this.Session)
                .WithUniqueId(PendingCustomerId)
                .WithName("Pending Customer")
                .Build();

            new RequestObjectStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();
        }
    }
}