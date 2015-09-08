// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseShipmentObjectStates.cs" company="Allors bvba">
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

    public partial class PurchaseShipmentObjectStates
    {
        public static readonly Guid CreatedId = new Guid("DF78516E-FC7C-48f2-B07B-1C53DA08D9B8");
        public static readonly Guid CompletedId = new Guid("97776286-4AE6-4aba-BE1B-2F1286E7F28E");

        private UniquelyIdentifiableCache<PurchaseShipmentObjectState> stateCache;

        public PurchaseShipmentObjectState Created
        {
            get { return this.StateCache.Get(CreatedId); }
        }

        public PurchaseShipmentObjectState Completed
        {
            get { return this.StateCache.Get(CompletedId); }
        }

        private UniquelyIdentifiableCache<PurchaseShipmentObjectState> StateCache
        {
            get
            {
                return this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PurchaseShipmentObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);
            
            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new PurchaseShipmentObjectStateBuilder(Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PurchaseShipmentObjectStateBuilder(Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();
        }

        protected override void AppsSecure(Domain.Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}