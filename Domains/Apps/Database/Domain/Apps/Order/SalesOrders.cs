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

            setup.AddDependency(this.ObjectType, M.SalesOrderObjectState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var provisional = new SalesOrderObjectStates(this.Session).Provisional;
            var onHold = new SalesOrderObjectStates(this.Session).OnHold;
            var requestsApproval = new SalesOrderObjectStates(this.Session).RequestsApproval;
            var inProcess = new SalesOrderObjectStates(this.Session).InProcess;
            var cancelled = new SalesOrderObjectStates(this.Session).Cancelled;
            var rejected = new SalesOrderObjectStates(this.Session).Rejected;
            var completed = new SalesOrderObjectStates(this.Session).Completed;
            var finished = new SalesOrderObjectStates(this.Session).Finished;

            var reject = this.Meta.Reject;
            var cancel = this.Meta.Cancel;
            var complete = this.Meta.Complete;
            var finish = this.Meta.Finish;
            var approve = this.Meta.Approve;
            var hold = this.Meta.Hold;
            var @continue = this.Meta.Continue;
            var confirm = this.Meta.Confirm;
            var addNewOrderItem = this.Meta.AddNewOrderItem;

            config.Deny(this.ObjectType, provisional, reject, approve, hold, @continue, finish);
            config.Deny(this.ObjectType, requestsApproval, confirm, hold, @continue, finish);
            config.Deny(this.ObjectType, inProcess, confirm, reject, approve, @continue, finish);
            config.Deny(this.ObjectType, onHold, confirm, reject, approve, hold, finish, addNewOrderItem);
            config.Deny(this.ObjectType, rejected, reject, finish, addNewOrderItem);
            config.Deny(this.ObjectType, cancelled, cancel, finish, addNewOrderItem);
            config.Deny(this.ObjectType, completed, complete, finish, addNewOrderItem);
            config.Deny(this.ObjectType, finished, finish, addNewOrderItem);

            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);
        }
    }
}