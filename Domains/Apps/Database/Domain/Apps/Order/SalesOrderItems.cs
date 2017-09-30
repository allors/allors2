// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderItems.cs" company="Allors bvba">
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
    using Allors;
    using Meta;

    public partial class SalesOrderItems
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.SalesOrderItemState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var created = new SalesOrderItemStates(this.Session).Created;
            var partiallyShipped = new SalesOrderItemStates(this.Session).PartiallyShipped;
            var shipped = new SalesOrderItemStates(this.Session).Shipped;
            var inProcess = new SalesOrderItemStates(this.Session).InProcess;
            var cancelled = new SalesOrderItemStates(this.Session).Cancelled;
            var rejected = new SalesOrderItemStates(this.Session).Rejected;
            var completed = new SalesOrderItemStates(this.Session).Completed;
            var finished = new SalesOrderItemStates(this.Session).Finished;

            var product = this.Meta.Product;
            config.Deny(this.ObjectType, shipped, product);
            config.Deny(this.ObjectType, partiallyShipped, product);

            var cancel = this.Meta.Cancel;
            var reject = this.Meta.Reject;
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, created, cancel, reject);
            config.Deny(this.ObjectType, partiallyShipped, delete, cancel, reject);
            config.Deny(this.ObjectType, shipped, delete, cancel, reject);
            config.Deny(this.ObjectType, inProcess, delete);

            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);
        }
    }
}