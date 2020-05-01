// <copyright file="SalesOrderItems.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;
    using Allors.Meta;

    public partial class SalesOrderItems
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.SalesOrderItemState);

        protected override void BaseSecure(Security config)
        {
            var readyForPosting = new SalesOrderItemStates(this.Session).ReadyForPosting;
            var awaitingAcceptance = new SalesOrderItemStates(this.Session).AwaitingAcceptance;
            var inProcess = new SalesOrderItemStates(this.Session).InProcess;
            var cancelled = new SalesOrderItemStates(this.Session).Cancelled;
            var rejected = new SalesOrderItemStates(this.Session).Rejected;
            var completed = new SalesOrderItemStates(this.Session).Completed;
            var finished = new SalesOrderItemStates(this.Session).Finished;

            var partiallyShipped = new SalesOrderItemShipmentStates(this.Session).PartiallyShipped;
            var shipmentInProgress = new SalesOrderItemShipmentStates(this.Session).InProgress;
            var shipped = new SalesOrderItemShipmentStates(this.Session).Shipped;

            var product = this.Meta.Product;
            config.Deny(this.ObjectType, shipped, product);
            config.Deny(this.ObjectType, partiallyShipped, product);

            var cancel = this.Meta.Cancel;
            var reject = this.Meta.Reject;
            var delete = this.Meta.Delete;
            var reopen = this.Meta.Reopen;
            var approve = this.Meta.Approve;

            config.Deny(this.ObjectType, inProcess, delete, reopen, approve);
            config.Deny(this.ObjectType, completed, delete, cancel, reject, reopen, approve);

            config.Deny(this.ObjectType, shipmentInProgress, delete, cancel, reject, reopen, approve);
            config.Deny(this.ObjectType, partiallyShipped, delete, cancel, reject, reopen, approve);
            config.Deny(this.ObjectType, shipped, delete, cancel, reject, reopen, approve);
            config.Deny(this.ObjectType, cancelled, cancel, reject, approve);
            config.Deny(this.ObjectType, rejected, cancel, reject, approve);

            config.Deny(this.ObjectType, awaitingAcceptance, Operations.Write);
            config.Deny(this.ObjectType, inProcess, Operations.Write);
            config.Deny(this.ObjectType, cancelled, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);
        }
    }
}
