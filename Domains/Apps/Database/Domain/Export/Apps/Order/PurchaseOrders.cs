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

            var created = new PurchaseOrderStates(this.Session).Provisional;
            var onHold = new PurchaseOrderStates(this.Session).OnHold;
            var requestsApproval = new PurchaseOrderStates(this.Session).RequestsApproval;
            var inProcess = new PurchaseOrderStates(this.Session).InProcess;
            var cancelled = new PurchaseOrderStates(this.Session).Cancelled;
            var rejected = new PurchaseOrderStates(this.Session).Rejected;
            var completed = new PurchaseOrderStates(this.Session).Completed;
            var finished = new PurchaseOrderStates(this.Session).Finished;

            var approve = this.Meta.Approve;
            var reject = this.Meta.Reject;
            var hold = this.Meta.Hold;
            var @continue = this.Meta.Continue;
            var confirm = this.Meta.Confirm;
            var quickReceive = this.Meta.QuickReceive;

            config.Deny(this.ObjectType, created, quickReceive, approve, @continue);
            config.Deny(this.ObjectType, requestsApproval, confirm, approve, quickReceive, @continue);
            config.Deny(this.ObjectType, inProcess, confirm, reject, approve, @continue);
            config.Deny(this.ObjectType, onHold, confirm, reject, approve, hold, quickReceive);

            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);
        }
    }
}