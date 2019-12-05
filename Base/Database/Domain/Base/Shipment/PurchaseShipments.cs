// <copyright file="PurchaseShipments.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class PurchaseShipments
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.ShipmentState);

        protected override void BaseSecure(Security config)
        {
            var created = new ShipmentStates(this.Session).Created;
            var picked = new ShipmentStates(this.Session).Picked;
            var packed = new ShipmentStates(this.Session).Packed;
            var delivered = new ShipmentStates(this.Session).Delivered;
            var cancelled = new ShipmentStates(this.Session).Cancelled;
            var onHold = new ShipmentStates(this.Session).OnHold;

            var except = new HashSet<IOperandType>
            {
                this.Meta.ElectronicDocuments.RoleType,
            };

            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.DenyExcept(this.ObjectType, delivered, except, Operations.Execute, Operations.Write);
        }
    }
}
