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

            setup.AddDependency(this.ObjectType, M.PurchaseInvoiceState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var approved = new PurchaseInvoiceStates(this.Session).Approved;
            var received = new PurchaseInvoiceStates(this.Session).Received;
            var readyForPosting = new PurchaseInvoiceStates(this.Session).ReadyForPosting;
            var paid = new PurchaseInvoiceStates(this.Session).Paid;
            var cancelled = new PurchaseInvoiceStates(this.Session).Cancelled;

            var ready = this.Meta.Ready;
            var approve = this.Meta.Approve;
            var cancel = this.Meta.CancelInvoice;
            var createSalesInvoice = this.Meta.CreateSalesInvoice;

            config.Deny(this.ObjectType, approved, approve);
            config.Deny(this.ObjectType, received, ready, approve, cancel, createSalesInvoice);
            config.Deny(this.ObjectType, readyForPosting, ready, approve, createSalesInvoice);

            config.Deny(this.ObjectType, paid, Operations.Execute);
            config.Deny(this.ObjectType, cancelled, Operations.Execute);
        }
    }
}
