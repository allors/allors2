//------------------------------------------------------------------------------------------------- 
// <copyright file="OrderObjectStates.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
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
// <summary>Defines the HomeAddress type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class OrderObjectStates
    {
        private static readonly Guid InitialId = new Guid("173EF3AF-B5AC-4610-8AC1-916D2C09C1D1");
        private static readonly Guid ConfirmedId = new Guid("BD2FF235-301A-445A-B794-5D76B86006B3");
        private static readonly Guid ClosedId = new Guid("0750D8B3-3B10-465F-BBC0-81D12F40A3DF");
        private static readonly Guid CancelledId = new Guid("F72CEBEE-D12C-4321-83A3-77019A7B8C76");

        private UniquelyIdentifiableCache<OrderObjectState> cache;

        public Cache<Guid, OrderObjectState> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableCache<OrderObjectState>(this.Session));

        public OrderObjectState Initial => this.Cache[InitialId];

        public OrderObjectState Confirmed => this.Cache[ConfirmedId];

        public OrderObjectState Closed => this.Cache[ClosedId];

        public OrderObjectState Cancelled => this.Cache[CancelledId];

        protected override void BaseSetup(Setup config)
        {
            new OrderObjectStateBuilder(this.Session).WithUniqueId(InitialId).WithName("Initial").Build();
            new OrderObjectStateBuilder(this.Session).WithUniqueId(ConfirmedId).WithName("Confirmed").Build();
            new OrderObjectStateBuilder(this.Session).WithUniqueId(ClosedId).WithName("Closed").Build();
            new OrderObjectStateBuilder(this.Session).WithUniqueId(CancelledId).WithName("Cancelled").Build();
        }
    }
}