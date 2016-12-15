// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrders.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class PurchaseOrders
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, PurchaseOrderObjectStates.Meta.ObjectType);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);

            config.GrantProcurement(this.ObjectType, full);
            config.GrantOperations(this.ObjectType, full);

            config.GrantSupplier(this.ObjectType, Meta.OrderNumber, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.OrderDate, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.CurrentOrderStatus, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.CurrentPaymentStatus, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.CurrentShipmentStatus, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.OrderStatuses, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.PaymentStatuses, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.ShipmentStatuses, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalBasePrice, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalDiscount, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalSurcharge, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalExVat, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalVat, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalIncVat, Operations.Read);

            config.GrantProcurement(this.ObjectType, Meta.OrderNumber, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.OrderDate, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.CurrentOrderStatus, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.CurrentPaymentStatus, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.CurrentShipmentStatus, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.OrderStatuses, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.PaymentStatuses, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.ShipmentStatuses, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalBasePrice, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalDiscount, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalSurcharge, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalExVat, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalVat, Operations.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalIncVat, Operations.Read);

            var created = new PurchaseOrderObjectStates(Session).Provisional;
            var onHold = new PurchaseOrderObjectStates(Session).OnHold;
            var requestsApproval = new PurchaseOrderObjectStates(Session).RequestsApproval;
            var inProcess = new PurchaseOrderObjectStates(Session).InProcess;
            var cancelled = new PurchaseOrderObjectStates(Session).Cancelled;
            var rejected = new PurchaseOrderObjectStates(Session).Rejected;
            var completed = new PurchaseOrderObjectStates(Session).Completed;
            var finished = new PurchaseOrderObjectStates(Session).Finished;

            var approve = Meta.Approve;
            var reject = Meta.Reject;
            var hold = Meta.Hold;
            var @continue = Meta.Continue;
            var confirm = Meta.Confirm;

            config.Deny(this.ObjectType, created, reject, approve, hold, @continue);
            config.Deny(this.ObjectType, requestsApproval, confirm, reject, approve, @continue);
            config.Deny(this.ObjectType, inProcess, confirm, reject, approve, @continue);
            config.Deny(this.ObjectType, onHold, confirm, reject, approve, hold);

            config.Deny(this.ObjectType, cancelled, Operation.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operation.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operation.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operation.Execute, Operations.Write);
        }
    }
}