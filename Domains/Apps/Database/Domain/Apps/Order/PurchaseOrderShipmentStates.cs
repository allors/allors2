// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderShipmentStates.cs" company="Allors bvba">
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

    public partial class PurchaseOrderShipmentStates
    {
        private static readonly Guid PartiallyReceivedId = new Guid("77ED251D-B004-41e7-B0C4-9769CF7AE73E");
        private static readonly Guid ReceivedId = new Guid("BCCB68CE-A517-44c6-ADDA-DBEB0464D575");

        private UniquelyIdentifiableCache<PurchaseOrderShipmentState> stateCache;

        public PurchaseOrderShipmentState PartiallyReceived => this.StateCache.Get(PartiallyReceivedId);

        public PurchaseOrderShipmentState Received => this.StateCache.Get(ReceivedId);


        private UniquelyIdentifiableCache<PurchaseOrderShipmentState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PurchaseOrderShipmentState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new PurchaseOrderShipmentStateBuilder(this.Session)
                .WithUniqueId(PartiallyReceivedId)
                .WithName("Partially Received")
                .Build();

            new PurchaseOrderShipmentStateBuilder(this.Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();
        }
    }
}