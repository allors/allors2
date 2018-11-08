// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerialisedItemStates.cs" company="Allors bvba">
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

    public partial class SerialisedItemStates
    {
        private static readonly Guid NaId = new Guid("E5AD6F2D-2EDF-4563-8AD4-59EF1211273F");
        private static readonly Guid SoldId = new Guid("FECCF869-98D7-4E9C-8979-5611A43918BC");
        private static readonly Guid InRentId = new Guid("9ACC6C05-60B5-4085-8B43-EB730939DB47");

        private UniquelyIdentifiableSticky<SerialisedItemState> stateCache;

        public SerialisedItemState NA => this.StateCache[NaId];

        public SerialisedItemState Sold => this.StateCache[SoldId];

        public SerialisedItemState InRent => this.StateCache[InRentId];

        private UniquelyIdentifiableSticky<SerialisedItemState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SerialisedItemState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(NaId)
                .WithName("N/A")
                .WithIsActive(true)
                .Build();

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(SoldId)
                .WithName("Sold")
                .WithIsActive(true)
                .Build();

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(InRentId)
                .WithName("InRent")
                .WithIsActive(true)
                .Build();
        }
    }
}