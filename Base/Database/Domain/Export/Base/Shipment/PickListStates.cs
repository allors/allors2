// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PickListStates.cs" company="Allors bvba">
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

    public partial class PickListStates
    {
        private static readonly Guid CreatedId = new Guid("E65872C0-AD3C-4802-A253-CF99F8209011");
        private static readonly Guid PickedId = new Guid("93C6B430-91BD-4e6c-AC2C-C287F2A8021D");
        private static readonly Guid CancelledId = new Guid("CD552AF5-E695-4329-BF87-5644C2EA98F3");
        private static readonly Guid OnHoldId = new Guid("1733E2B0-48CA-4731-8F3C-93C6CF3A9543");

        private UniquelyIdentifiableSticky<PickListState> stateCache;

        public PickListState Created => this.StateCache[CreatedId];

        public PickListState Picked => this.StateCache[PickedId];

        public PickListState Cancelled => this.StateCache[CancelledId];

        public PickListState OnHold => this.StateCache[OnHoldId];

        private UniquelyIdentifiableSticky<PickListState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<PickListState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {


            new PickListStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PickListStateBuilder(this.Session)
                .WithUniqueId(PickedId)
                .WithName("Picked")
                .Build();

            new PickListStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new PickListStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();
        }
    }
}
