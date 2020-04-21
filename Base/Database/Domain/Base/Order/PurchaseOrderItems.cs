// <copyright file="PurchaseOrderItems.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class PurchaseOrderItems
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.PurchaseOrderItemState);

        protected override void BaseSecure(Security config)
        {
            var itemStates = new PurchaseOrderItemStates(this.Session);
            var created = itemStates.Created;
            var onHold = itemStates.OnHold;
            var cancelled = itemStates.Cancelled;
            var rejected = itemStates.Rejected;
            var awaitingApproval = itemStates.AwaitingApproval;
            var inProcess = itemStates.InProcess;
            var completed = itemStates.Completed;
            var finished = itemStates.Finished;

            var shipmentStates = new PurchaseOrderItemShipmentStates(this.Session);
            var partiallyReceived = shipmentStates.PartiallyReceived;
            var received = shipmentStates.Received;

            var part = this.Meta.Part;
            var cancel = this.Meta.Cancel;
            var reject = this.Meta.Reject;
            var quickReceive = this.Meta.QuickReceive;
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, partiallyReceived, part);
            config.Deny(this.ObjectType, received, part);

            config.Deny(this.ObjectType, created, cancel, reject);
            config.Deny(this.ObjectType, onHold, quickReceive, delete);
            config.Deny(this.ObjectType, awaitingApproval, cancel, quickReceive, delete);
            config.Deny(this.ObjectType, inProcess, delete, quickReceive, delete);
            config.Deny(this.ObjectType, completed, delete);
            config.Deny(this.ObjectType, partiallyReceived, delete, cancel, reject, quickReceive);
            config.Deny(this.ObjectType, received, delete, cancel, reject, quickReceive);

            config.Deny(this.ObjectType, inProcess, Operations.Write);
            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);
        }
    }
}
