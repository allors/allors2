// <copyright file="CustomerShipments.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class CustomerShipments
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.ShipmentState);

        protected override void BaseSecure(Security config)
        {
            var created = new ShipmentStates(this.Session).Created;
            var picking = new ShipmentStates(this.Session).Picking;
            var picked = new ShipmentStates(this.Session).Picked;
            var packed = new ShipmentStates(this.Session).Packed;
            var shipped = new ShipmentStates(this.Session).Shipped;
            var cancelled = new ShipmentStates(this.Session).Cancelled;
            var onHold = new ShipmentStates(this.Session).OnHold;

            var pick = this.Meta.Pick;
            var hold = this.Meta.Hold;
            var @continue = this.Meta.Continue;
            var ship = this.Meta.Ship;

            var except = new HashSet<IOperandType>
            {
                this.Meta.ElectronicDocuments.RoleType,
            };

            config.Deny(this.ObjectType, onHold, pick, ship, hold, @continue);
            config.Deny(this.ObjectType, created, ship, hold, @continue);
            config.Deny(this.ObjectType, picked, pick, @continue);
            config.Deny(this.ObjectType, packed, pick, @continue);
            config.Deny(this.ObjectType, picking, pick);

            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.DenyExcept(this.ObjectType, picking, except, Operations.Write);
            config.DenyExcept(this.ObjectType, picked, except, Operations.Write);
            config.DenyExcept(this.ObjectType, packed, except, Operations.Write);
            config.DenyExcept(this.ObjectType, onHold, except, Operations.Write);
            config.DenyExcept(this.ObjectType, shipped, except, Operations.Execute, Operations.Write);
        }
    }
}
