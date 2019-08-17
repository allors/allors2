// <copyright file="SalesOrders.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class SalesOrders
    {
        protected override void BasePrepare(Setup setup)
        {
            base.BasePrepare(setup);

            setup.AddDependency(this.ObjectType, M.SalesOrderState);
        }

        protected override void BaseSecure(Security config)
        {
            var provisional = new SalesOrderStates(this.Session).Provisional;
            var onHold = new SalesOrderStates(this.Session).OnHold;
            var requestsApproval = new SalesOrderStates(this.Session).RequestsApproval;
            var inProcess = new SalesOrderStates(this.Session).InProcess;
            var cancelled = new SalesOrderStates(this.Session).Cancelled;
            var rejected = new SalesOrderStates(this.Session).Rejected;
            var completed = new SalesOrderStates(this.Session).Completed;
            var finished = new SalesOrderStates(this.Session).Finished;

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

            config.Deny(this.ObjectType, provisional, reject, approve, hold, @continue, ship, invoice);
            config.Deny(this.ObjectType, requestsApproval, confirm, hold, @continue, ship, invoice);
            config.Deny(this.ObjectType, inProcess, confirm, reject, approve, @continue);
            config.Deny(this.ObjectType, onHold, confirm, reject, approve, hold, ship, invoice);
            config.Deny(this.ObjectType, rejected, reject, ship, invoice);
            config.Deny(this.ObjectType, cancelled, cancel, ship, invoice);
            config.Deny(this.ObjectType, completed, complete, reject, cancel, approve, hold, @continue, confirm, invoice);

            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);
        }
    }
}
