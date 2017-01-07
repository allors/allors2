// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropShipmentObjectStates.cs" company="Allors bvba">
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

    public partial class DropShipmentObjectStates
    {
        public static readonly Guid CreatedId = new Guid("40B4CAC6-861D-45a8-9B77-0945558A1262");
        public static readonly Guid CancelledId = new Guid("D96047D7-10EC-49aa-90C5-54B1297831DE");

        private UniquelyIdentifiableCache<DropShipmentObjectState> stateCache;

        public DropShipmentObjectState Created => this.StateCache.Get(CreatedId);

        public DropShipmentObjectState Cancelled => this.StateCache.Get(CancelledId);

        private UniquelyIdentifiableCache<DropShipmentObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<DropShipmentObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new DropShipmentObjectStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new DropShipmentObjectStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();
        }
    }
}