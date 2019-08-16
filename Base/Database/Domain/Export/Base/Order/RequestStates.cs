// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestStates.cs" company="Allors bvba">
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

    public partial class RequestStates
    {
        private static readonly Guid AnonymousId = new Guid("2F054949-E30C-4954-9A3C-191559DE8315");
        private static readonly Guid SubmittedId = new Guid("DB03407D-BCB1-433A-B4E9-26CEA9A71BFD");
        private static readonly Guid CancelledId = new Guid("6B839C79-689D-412C-9C27-B553CF350662");
        private static readonly Guid QuotedId = new Guid("79A9BAF5-D16F-4FA7-A2A0-CA20BF79833F");
        private static readonly Guid PendingCustomerId = new Guid("671FDA2F-5AA6-4EA5-B5D6-C914F0911690");
        private static readonly Guid RejectedId = new Guid("26B1E962-9799-4C53-AE36-20E8490F757A");

        private UniquelyIdentifiableSticky<RequestState> stateCache;

        public RequestState Anonymous => this.StateCache[AnonymousId];

        public RequestState Submitted => this.StateCache[SubmittedId];

        public RequestState Cancelled => this.StateCache[CancelledId];

        public RequestState Quoted => this.StateCache[QuotedId];

        public RequestState PendingCustomer => this.StateCache[PendingCustomerId];

        public RequestState Rejected => this.StateCache[RejectedId];

        private UniquelyIdentifiableSticky<RequestState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<RequestState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {


            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new RequestStateBuilder(this.Session)
                .WithUniqueId(AnonymousId)
                .WithName("Anonymous")
                .Build();

            new RequestStateBuilder(this.Session)
                .WithUniqueId(SubmittedId)
                .WithName("Submitted")
                .Build();

            new RequestStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new RequestStateBuilder(this.Session)
                .WithUniqueId(QuotedId)
                .WithName("Quoted")
                .Build();

            new RequestStateBuilder(this.Session)
                .WithUniqueId(PendingCustomerId)
                .WithName("Pending Customer")
                .Build();

            new RequestStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();
        }
    }
}