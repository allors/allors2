// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoices.cs" company="Allors bvba">
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

    public partial class PurchaseInvoices
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.PurchaseInvoiceObjectState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var approved = new PurchaseInvoiceObjectStates(this.Session).Approved;
            var received = new PurchaseInvoiceObjectStates(this.Session).Received;
            var readyForPosting = new PurchaseInvoiceObjectStates(this.Session).ReadyForPosting;
            var paid = new PurchaseInvoiceObjectStates(this.Session).Paid;
            var cancelled = new PurchaseInvoiceObjectStates(this.Session).Cancelled;

            var ready = this.Meta.Ready;
            var approve = this.Meta.Approve;
            var cancel = this.Meta.Cancel;

            config.Deny(this.ObjectType, approved, approve);
            config.Deny(this.ObjectType, received, ready, approve, cancel);
            config.Deny(this.ObjectType, readyForPosting, ready, approve);

            config.Deny(this.ObjectType, paid, Operations.Execute);
            config.Deny(this.ObjectType, cancelled, Operations.Execute);
        }
    }
}
