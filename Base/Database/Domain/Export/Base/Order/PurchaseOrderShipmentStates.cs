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
        public static readonly Guid NotReceivedId = new Guid("32267C1E-FB87-43E5-B2D8-6477FA1CA4C8");
        public static readonly Guid PartiallyReceivedId = new Guid("77ED251D-B004-41e7-B0C4-9769CF7AE73E");
        public static readonly Guid ReceivedId = new Guid("BCCB68CE-A517-44c6-ADDA-DBEB0464D575");

        private UniquelyIdentifiableSticky<PurchaseOrderShipmentState> stateCache;

        public PurchaseOrderShipmentState NotReceived => this.StateCache[NotReceivedId];

        public PurchaseOrderShipmentState PartiallyReceived => this.StateCache[PartiallyReceivedId];

        public PurchaseOrderShipmentState Received => this.StateCache[ReceivedId];


        private UniquelyIdentifiableSticky<PurchaseOrderShipmentState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<PurchaseOrderShipmentState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new PurchaseOrderShipmentStateBuilder(this.Session)
                .WithUniqueId(NotReceivedId)
                .WithName("Not Received")
                .Build();

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
