// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderItems.cs" company="Allors bvba">
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
    using Allors;

    public partial class SalesOrderItems
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, SalesOrderItemObjectStates.Meta.ObjectType);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);

            var created = new SalesOrderItemObjectStates(Session).Created;
            var partiallyShipped = new SalesOrderItemObjectStates(Session).PartiallyShipped;
            var shipped = new SalesOrderItemObjectStates(Session).Shipped;
            var inProcess = new SalesOrderItemObjectStates(Session).InProcess;
            var cancelled = new SalesOrderItemObjectStates(Session).Cancelled;
            var rejected = new SalesOrderItemObjectStates(Session).Rejected;
            var completed = new SalesOrderItemObjectStates(Session).Completed;
            var finished = new SalesOrderItemObjectStates(Session).Finished;

            var product = Meta.Product;
            config.Deny(this.ObjectType, shipped, product);
            config.Deny(this.ObjectType, partiallyShipped, product);

            var cancel = Meta.Cancel;
            var reject = Meta.Reject;
            var delete = Meta.Delete;

            config.Deny(this.ObjectType, created, cancel, reject);
            config.Deny(this.ObjectType, partiallyShipped, delete, cancel, reject);
            config.Deny(this.ObjectType, shipped, delete, cancel, reject);
            config.Deny(this.ObjectType, inProcess, delete);

            config.Deny(this.ObjectType, cancelled, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, rejected, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, completed, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, finished, Operation.Execute, Operation.Write);
        }
    }
}