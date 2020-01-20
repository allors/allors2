// <copyright file="SalesOrders.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class SalesOrders
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.SalesOrderState);

        protected override void BaseSecure(Security config)
        {
            var provisional = new SalesOrderStates(this.Session).Provisional;
            var onHold = new SalesOrderStates(this.Session).OnHold;
            var requestsApproval = new SalesOrderStates(this.Session).RequestsApproval;
            var readyForPosting = new SalesOrderStates(this.Session).ReadyForPosting;
            var inProcess = new SalesOrderStates(this.Session).InProcess;
            var cancelled = new SalesOrderStates(this.Session).Cancelled;
            var rejected = new SalesOrderStates(this.Session).Rejected;
            var completed = new SalesOrderStates(this.Session).Completed;
            var finished = new SalesOrderStates(this.Session).Finished;

            var partiallyShipped = new SalesOrderShipmentStates(this.Session).PartiallyShipped;
            var inProgress = new SalesOrderShipmentStates(this.Session).InProgress;
            var shipped = new SalesOrderShipmentStates(this.Session).Shipped;

            var send = this.Meta.Send;
            var reject = this.Meta.Reject;
            var complete = this.Meta.Complete;
            var cancel = this.Meta.Cancel;
            var reopen = this.Meta.Reopen;
            var approve = this.Meta.Approve;
            var hold = this.Meta.Hold;
            var @continue = this.Meta.Continue;
            var confirm = this.Meta.Confirm;
            var ship = this.Meta.Ship;
            var invoice = this.Meta.Invoice;

            config.Deny(this.ObjectType, provisional, reject, approve, hold, @continue, ship, invoice, send);
            config.Deny(this.ObjectType, requestsApproval, confirm, hold, @continue, ship, invoice, send);
            config.Deny(this.ObjectType, readyForPosting, confirm, approve, complete, @continue, ship, invoice);
            config.Deny(this.ObjectType, inProcess, confirm, reject, approve, @continue, send);
            config.Deny(this.ObjectType, onHold, confirm, reject, approve, hold, ship, invoice, send);
            config.Deny(this.ObjectType, rejected, reject, ship, invoice, send);
            config.Deny(this.ObjectType, cancelled, cancel, ship, invoice, send);
            config.Deny(this.ObjectType, completed, complete, reject, cancel, approve, hold, @continue, confirm, invoice, send);

            config.Deny(this.ObjectType, inProgress, cancel, reject);
            config.Deny(this.ObjectType, partiallyShipped, cancel, reject);
            config.Deny(this.ObjectType, shipped, cancel, reject);

            config.Deny(this.ObjectType, inProcess, Operations.Write);
            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);
        }
    }
}
