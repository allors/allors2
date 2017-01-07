// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrders.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using Meta;

    public partial class PurchaseOrders
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.PurchaseOrderObjectState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            config.GrantSupplier(this.ObjectType, this.Meta.OrderNumber, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.OrderDate, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.CurrentOrderStatus, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.CurrentPaymentStatus, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.CurrentShipmentStatus, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.OrderStatuses, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.PaymentStatuses, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.ShipmentStatuses, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.TotalBasePrice, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.TotalDiscount, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.TotalSurcharge, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.TotalExVat, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.TotalVat, Operations.Read);
            config.GrantSupplier(this.ObjectType, this.Meta.TotalIncVat, Operations.Read);

            var created = new PurchaseOrderObjectStates(this.Session).Provisional;
            var onHold = new PurchaseOrderObjectStates(this.Session).OnHold;
            var requestsApproval = new PurchaseOrderObjectStates(this.Session).RequestsApproval;
            var inProcess = new PurchaseOrderObjectStates(this.Session).InProcess;
            var cancelled = new PurchaseOrderObjectStates(this.Session).Cancelled;
            var rejected = new PurchaseOrderObjectStates(this.Session).Rejected;
            var completed = new PurchaseOrderObjectStates(this.Session).Completed;
            var finished = new PurchaseOrderObjectStates(this.Session).Finished;

            var approve = this.Meta.Approve;
            var reject = this.Meta.Reject;
            var hold = this.Meta.Hold;
            var @continue = this.Meta.Continue;
            var confirm = this.Meta.Confirm;

            config.Deny(this.ObjectType, created, reject, approve, hold, @continue);
            config.Deny(this.ObjectType, requestsApproval, confirm, reject, approve, @continue);
            config.Deny(this.ObjectType, inProcess, confirm, reject, approve, @continue);
            config.Deny(this.ObjectType, onHold, confirm, reject, approve, hold);

            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);
        }
    }
}