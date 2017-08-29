// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuoteItemObjectStates.cs" company="Allors bvba">
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

    public partial class QuoteItemObjectStates
    {
        private static readonly Guid DraftId = new Guid("84AD17A3-10F7-4FDB-B76A-41BDB1EDB0E6");
        private static readonly Guid SubmittedId = new Guid("E511EA2D-6EB9-428D-A982-B097938A8FF8");
        private static readonly Guid CancelledId = new Guid("6433F6F7-22D6-4142-8FC5-8941F4F0B6A8");

        private UniquelyIdentifiableCache<QuoteItemObjectState> stateCache;

        public QuoteItemObjectState Draft => this.StateCache.Get(DraftId);

        public QuoteItemObjectState Submitted => this.StateCache.Get(SubmittedId);

        public QuoteItemObjectState Cancelled => this.StateCache.Get(CancelledId);

        private UniquelyIdentifiableCache<QuoteItemObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<QuoteItemObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new QuoteItemObjectStateBuilder(this.Session)
                .WithUniqueId(DraftId)
                .WithName("Anonymous")
                .Build();

            new QuoteItemObjectStateBuilder(this.Session)
                .WithUniqueId(SubmittedId)
                .WithName("Submitted")
                .Build();

            new QuoteItemObjectStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();
        }
    }
}