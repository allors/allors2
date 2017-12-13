     // --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrders.cs" company="Allors bvba">
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
    using System;
    using Meta;

    public partial class SalesOrders
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.SalesOrderState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

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
            config.Deny(this.ObjectType, completed, complete, ship, invoice);

            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);
        }
    }
}