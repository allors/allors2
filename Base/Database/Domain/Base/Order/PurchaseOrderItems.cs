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
            var created = new PurchaseOrderItemStates(this.Session).Created;
            var onHold = new PurchaseOrderItemStates(this.Session).OnHold;
            var cancelled = new PurchaseOrderItemStates(this.Session).Cancelled;
            var rejected = new PurchaseOrderItemStates(this.Session).Rejected;
            var awaitingApproval = new PurchaseOrderItemStates(this.Session).AwaitingApproval;
            var inProcess = new PurchaseOrderItemStates(this.Session).InProcess;
            var partiallyReceived = new PurchaseOrderItemShipmentStates(this.Session).PartiallyReceived;
            var received = new PurchaseOrderItemShipmentStates(this.Session).Received;
            var completed = new PurchaseOrderItemStates(this.Session).Completed;
            var finished = new PurchaseOrderItemStates(this.Session).Finished;

            var part = this.Meta.Part;
            var cancel = this.Meta.Cancel;
            var reject = this.Meta.Reject;
            var quickReceive = this.Meta.QuickReceive;
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, partiallyReceived, part);
            config.Deny(this.ObjectType, received, part);

            config.Deny(this.ObjectType, created, cancel, reject);
            config.Deny(this.ObjectType, onHold, quickReceive);
            config.Deny(this.ObjectType, awaitingApproval, cancel, quickReceive);
            config.Deny(this.ObjectType, inProcess, delete, quickReceive);
            config.Deny(this.ObjectType, completed, delete);
            config.Deny(this.ObjectType, partiallyReceived, delete, cancel, reject, quickReceive);
            config.Deny(this.ObjectType, received, delete, cancel, reject, quickReceive);

            config.Deny(this.ObjectType, inProcess, Operations.Write);
            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute);
        }
    }
}
