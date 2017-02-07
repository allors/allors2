// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseReturnObjectStates.cs" company="Allors bvba">
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

    public partial class PurchaseReturnObjectStates
    {
        private static readonly Guid CreatedId = new Guid("58DF6842-CC62-41e2-9B49-794B856C558A");
        private static readonly Guid CancelledId = new Guid("36950F2C-1340-4da7-9342-D2185818E04D");

        private UniquelyIdentifiableCache<PurchaseReturnObjectState> stateCache;

        public PurchaseReturnObjectState Created => this.StateCache.Get(CreatedId);

        public PurchaseReturnObjectState Cancelled => this.StateCache.Get(CancelledId);

        private UniquelyIdentifiableCache<PurchaseReturnObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PurchaseReturnObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var engllishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PurchaseReturnObjectStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PurchaseReturnObjectStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();
        }
    }
}