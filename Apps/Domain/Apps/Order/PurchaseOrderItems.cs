// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItems.cs" company="Allors bvba">
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

    public partial class PurchaseOrderItems
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.PurchaseOrderItemObjectState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var created = new PurchaseOrderItemObjectStates(this.Session).Created;
            var inProcess = new PurchaseOrderItemObjectStates(this.Session).InProcess;
            var partiallyReceived = new PurchaseOrderItemObjectStates(this.Session).PartiallyReceived;
            var received = new PurchaseOrderItemObjectStates(this.Session).Received;
            var cancelled = new PurchaseOrderItemObjectStates(this.Session).Cancelled;
            var rejected = new PurchaseOrderItemObjectStates(this.Session).Rejected;
            var completed = new PurchaseOrderItemObjectStates(this.Session).Completed;
            var finished = new PurchaseOrderItemObjectStates(this.Session).Finished;

            var product = this.Meta.Product;
            var part = this.Meta.Part;

            config.Deny(this.ObjectType, partiallyReceived, product, part);
            config.Deny(this.ObjectType, received, product, part);

            var cancel = this.Meta.Cancel;
            var reject = this.Meta.Reject;

            // TODO: Delete
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, created, cancel, reject);
            config.Deny(this.ObjectType, completed, delete);
            config.Deny(this.ObjectType, inProcess, delete);
            config.Deny(this.ObjectType, partiallyReceived, delete, cancel, reject);
            config.Deny(this.ObjectType, received, delete, cancel, reject);

            config.Deny(this.ObjectType, inProcess, Operations.Write);
            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute);
        }
    }
}