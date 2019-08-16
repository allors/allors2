// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItemShipmentStates.cs" company="Allors bvba">
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

    public partial class PurchaseOrderItemShipmentStates
    {
        internal static readonly Guid NotReceivedId = new Guid("CF26D4F9-E8AF-4A1D-9841-73B8C5266117");
        internal static readonly Guid PartiallyReceivedId = new Guid("C142144A-8CAE-4D2B-A56B-94BAF236227A");
        internal static readonly Guid ReceivedId = new Guid("AD66619F-BB48-42AF-B019-3E4028AD7B6B");

        private UniquelyIdentifiableSticky<PurchaseOrderItemShipmentState> stateCache;

        public PurchaseOrderItemShipmentState NotReceived => this.StateCache[NotReceivedId];

        public PurchaseOrderItemShipmentState PartiallyReceived => this.StateCache[PartiallyReceivedId];

        public PurchaseOrderItemShipmentState Received => this.StateCache[ReceivedId];

        private UniquelyIdentifiableSticky<PurchaseOrderItemShipmentState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<PurchaseOrderItemShipmentState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {


            new PurchaseOrderItemShipmentStateBuilder(this.Session)
                .WithUniqueId(NotReceivedId)
                .WithName("Not Received")
                .Build();

            new PurchaseOrderItemShipmentStateBuilder(this.Session)
                .WithUniqueId(PartiallyReceivedId)
                .WithName("Partially Received")
                .Build();

            new PurchaseOrderItemShipmentStateBuilder(this.Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();
        }
    }
}