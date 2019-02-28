// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderShipmentState.cs" company="Allors bvba">
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

    public partial class SalesOrderShipmentStates
    {
        internal static readonly Guid NotShippedId = new Guid("28256661-0110-42C8-A97E-A4655EFE7974");
        internal static readonly Guid ShippedId = new Guid("CBDBFF96-B5DA-4be3-9B8D-EA785D08C85C");
        internal static readonly Guid PartiallyShippedId = new Guid("40B4EFB9-42A4-43d9-BCE9-39E55FD9D507");

        private UniquelyIdentifiableSticky<SalesOrderShipmentState> stateCache;

        public SalesOrderShipmentState NotShipped => this.StateCache[NotShippedId];

        public SalesOrderShipmentState Shipped => this.StateCache[ShippedId];

        public SalesOrderShipmentState PartiallyShipped => this.StateCache[PartiallyShippedId];


        private UniquelyIdentifiableSticky<SalesOrderShipmentState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesOrderShipmentState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new SalesOrderShipmentStateBuilder(this.Session)
                .WithUniqueId(NotShippedId)
                .WithName("Not Shipped")
                .Build();

            new SalesOrderShipmentStateBuilder(this.Session)
                .WithUniqueId(PartiallyShippedId)
                .WithName("Partially Shipped")
                .Build();

            new SalesOrderShipmentStateBuilder(this.Session)
                .WithUniqueId(ShippedId)
                .WithName("Shipped")
                .Build();
        }
    }
}