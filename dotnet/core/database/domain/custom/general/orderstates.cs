// <copyright file="OrderStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>

namespace Allors.Domain
{
    using System;

    public partial class OrderStates
    {
        private static readonly Guid InitialId = new Guid("173EF3AF-B5AC-4610-8AC1-916D2C09C1D1");
        private static readonly Guid ConfirmedId = new Guid("BD2FF235-301A-445A-B794-5D76B86006B3");
        private static readonly Guid ClosedId = new Guid("0750D8B3-3B10-465F-BBC0-81D12F40A3DF");
        private static readonly Guid CancelledId = new Guid("F72CEBEE-D12C-4321-83A3-77019A7B8C76");

        private UniquelyIdentifiableSticky<OrderState> cache;

        public Sticky<Guid, OrderState> Cache => this.cache ??= new UniquelyIdentifiableSticky<OrderState>(this.Session);

        public OrderState Initial => this.Cache[InitialId];

        public OrderState Confirmed => this.Cache[ConfirmedId];

        public OrderState Closed => this.Cache[ClosedId];

        public OrderState Cancelled => this.Cache[CancelledId];

        protected override void CustomSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(InitialId, v => v.Name = "Initial");
            merge(ConfirmedId, v => v.Name = "Confirmed");
            merge(ClosedId, v => v.Name = "Closed");
            merge(CancelledId, v => v.Name = "Cancelled");
        }
    }
}
