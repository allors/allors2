// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseShipmentStates.cs" company="Allors bvba">
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

    public partial class PurchaseShipmentStates
    {
        private static readonly Guid CreatedId = new Guid("DF78516E-FC7C-48f2-B07B-1C53DA08D9B8");
        private static readonly Guid CompletedId = new Guid("97776286-4AE6-4aba-BE1B-2F1286E7F28E");

        private UniquelyIdentifiableCache<PurchaseShipmentState> stateCache;

        public PurchaseShipmentState Created => this.StateCache.Get(CreatedId);

        public PurchaseShipmentState Completed => this.StateCache.Get(CompletedId);

        private UniquelyIdentifiableCache<PurchaseShipmentState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PurchaseShipmentState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);
            
            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PurchaseShipmentStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PurchaseShipmentStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();
        }
    }
}