// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItems.cs" company="Allors bvba">
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
    public partial class PurchaseOrderItems
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, PurchaseOrderItemObjectStates.Meta.ObjectType);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);

            var created = new PurchaseOrderItemObjectStates(Session).Created;
            var inProcess = new PurchaseOrderItemObjectStates(Session).InProcess;
            var partiallyReceived = new PurchaseOrderItemObjectStates(Session).PartiallyReceived;
            var received = new PurchaseOrderItemObjectStates(Session).Received;
            var cancelled = new PurchaseOrderItemObjectStates(Session).Cancelled;
            var rejected = new PurchaseOrderItemObjectStates(Session).Rejected;
            var completed = new PurchaseOrderItemObjectStates(Session).Completed;
            var finished = new PurchaseOrderItemObjectStates(Session).Finished;

            var product = Meta.Product;
            var part = Meta.Part;

            config.Deny(this.ObjectType, partiallyReceived, product, part);
            config.Deny(this.ObjectType, received, product, part);

            var cancel = Meta.Cancel;
            var reject = Meta.Reject;
            // TODO: Delete
            var delete = Meta.Delete;

            config.Deny(this.ObjectType, created, cancel, reject);
            config.Deny(this.ObjectType, completed, delete);
            config.Deny(this.ObjectType, inProcess, delete);
            config.Deny(this.ObjectType, partiallyReceived, delete, cancel, reject);
            config.Deny(this.ObjectType, received, delete, cancel, reject);
            
            config.Deny(this.ObjectType, inProcess, Operation.Write);
            config.Deny(this.ObjectType, cancelled, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, rejected, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, completed, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, finished, Operation.Execute);
        }
    }
}