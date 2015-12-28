     // --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrders.cs" company="Allors bvba">
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

    public partial class SalesOrders
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, SalesOrderObjectStates.Meta.ObjectType);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);

            config.GrantSales(this.ObjectType, full);
            config.GrantCustomer(this.ObjectType, full);
            config.GrantOwner(this.ObjectType, full);
            config.GrantOperations(this.ObjectType, full);

            var provisional = new SalesOrderObjectStates(Session).Provisional;
            var onHold = new SalesOrderObjectStates(Session).OnHold;
            var requestsApproval = new SalesOrderObjectStates(Session).RequestsApproval;
            var inProcess = new SalesOrderObjectStates(Session).InProcess;
            var cancelled = new SalesOrderObjectStates(Session).Cancelled;
            var rejected = new SalesOrderObjectStates(Session).Rejected;
            var completed = new SalesOrderObjectStates(Session).Completed;
            var finished = new SalesOrderObjectStates(Session).Finished;

            var reject = Meta.Reject;
            var approve = Meta.Approve;
            var hold = Meta.Hold;
            var @continue = Meta.Continue;
            var confirm = Meta.Confirm;

            config.Deny(this.ObjectType, provisional, reject, approve, hold, @continue);
            config.Deny(this.ObjectType, requestsApproval, confirm, hold, @continue);
            config.Deny(this.ObjectType, inProcess, confirm, reject, approve, @continue);
            config.Deny(this.ObjectType, onHold, confirm, reject, approve, hold);
            config.Deny(this.ObjectType, rejected, reject);

            config.Deny(this.ObjectType, cancelled, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, rejected, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, completed, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, finished, Operation.Execute, Operation.Write);
        }
    }
}