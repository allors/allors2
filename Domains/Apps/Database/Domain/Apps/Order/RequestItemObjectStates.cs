// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestItemObjectStates.cs" company="Allors bvba">
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

    public partial class RequestItemObjectStates
    {
        private static readonly Guid DraftId = new Guid("B173DFBE-9421-4697-8FFB-E46AFC724490");
        private static readonly Guid SubmittedId = new Guid("B118C185-DE34-4131-BE1F-E6162C1DEA4B");
        private static readonly Guid CancelledId = new Guid("E98A3001-C343-4925-9D95-CE370DFC98E7");

        private UniquelyIdentifiableCache<RequestItemObjectState> stateCache;

        public RequestItemObjectState Draft => this.StateCache.Get(DraftId);

        public RequestItemObjectState Submitted => this.StateCache.Get(SubmittedId);

        public RequestItemObjectState Cancelled => this.StateCache.Get(CancelledId);

        private UniquelyIdentifiableCache<RequestItemObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<RequestItemObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new RequestItemObjectStateBuilder(this.Session)
                .WithUniqueId(DraftId)
                .WithName("Draft")
                .Build();

            new RequestItemObjectStateBuilder(this.Session)
                .WithUniqueId(SubmittedId)
                .WithName("Submitted")
                .Build();

            new RequestItemObjectStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();
        }
    }
}