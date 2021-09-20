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
            var received = new ShipmentStates(this.Session).Received;
            var cancelled = new ShipmentStates(this.Session).Cancelled;

            var except = new HashSet<IOperandType>
            {
                this.Meta.ElectronicDocuments.RoleType,
            };

            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.DenyExcept(this.ObjectType, received, except, Operations.Execute, Operations.Write);
        }
    }
}
