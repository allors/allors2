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

            setup.AddDependency(this.ObjectType, M.PurchaseOrderState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var created = new PurchaseOrderStates(this.Session).Created;
            var onHold = new PurchaseOrderStates(this.Session).OnHold;
            var cancelled = new PurchaseOrderStates(this.Session).Cancelled;
            var rejected = new PurchaseOrderStates(this.Session).Rejected;
            var awaitingApprovalLevel1 = new PurchaseOrderStates(this.Session).AwaitingApprovalLevel1;
            var awaitingApprovalLevel2 = new PurchaseOrderStates(this.Session).AwaitingApprovalLevel2;
            var inProcess = new PurchaseOrderStates(this.Session).InProcess;
            var sent = new PurchaseOrderStates(this.Session).Sent;
            var completed = new PurchaseOrderStates(this.Session).Completed;
            var finished = new PurchaseOrderStates(this.Session).Finished;

            var approve = this.Meta.Approve;
            var reject = this.Meta.Reject;
            var hold = this.Meta.Hold;
            var @continue = this.Meta.Continue;
            var confirm = this.Meta.Confirm;
            var cancel = this.Meta.Cancel;
            var reopen = this.Meta.Reopen;
            var send = this.Meta.Send;
            var quickReceive = this.Meta.QuickReceive;
            var invoice = this.Meta.Invoice;

            config.Deny(this.ObjectType, created, approve, reject, @continue, reopen, send, quickReceive, invoice);
            config.Deny(this.ObjectType, onHold, approve, hold, confirm, reopen, send, quickReceive, invoice);
            config.Deny(this.ObjectType, cancelled, approve, reject, hold, @continue, confirm, cancel, send, quickReceive, invoice);
            config.Deny(this.ObjectType, rejected, approve, reject, hold, @continue, confirm, cancel, send, quickReceive, invoice);
            config.Deny(this.ObjectType, awaitingApprovalLevel1, hold, @continue, confirm, cancel, reopen, send, quickReceive, @continue);
            config.Deny(this.ObjectType, awaitingApprovalLevel2, hold, @continue, confirm, cancel, reopen, send, quickReceive, @continue);
            config.Deny(this.ObjectType, inProcess, approve, reject, @continue, confirm, reopen, quickReceive);
            config.Deny(this.ObjectType, sent, approve, reject, hold, @continue, confirm, reopen, send);

            config.Deny(this.ObjectType, cancelled, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);
        }
    }
}