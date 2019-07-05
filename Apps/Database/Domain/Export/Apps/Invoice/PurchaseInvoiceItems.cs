// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceItems.cs" company="Allors bvba">
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

    public partial class PurchaseInvoiceItems
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.PurchaseInvoiceItemState);
        }

        protected override void AppsSecure(Security config)
        {
            

            var created = new PurchaseInvoiceItemStates(this.Session).Created;
            var awaitingApproval = new PurchaseInvoiceItemStates(this.Session).AwaitingApproval;
            var received = new PurchaseInvoiceItemStates(this.Session).Received;
            var paid = new PurchaseInvoiceItemStates(this.Session).Paid;
            var cancelled = new PurchaseInvoiceItemStates(this.Session).Cancelled;
            var cancelledByinvoice = new PurchaseInvoiceItemStates(this.Session).CancelledByinvoice;
            var rejected = new PurchaseInvoiceItemStates(this.Session).Rejected;

            var cancel = this.Meta.Cancel;
            var reject = this.Meta.Reject;
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, created, cancel, reject);
            config.Deny(this.ObjectType, awaitingApproval, cancel, delete);

            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, cancelledByinvoice, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, paid, Operations.Execute, Operations.Write);
        }
    }
}
