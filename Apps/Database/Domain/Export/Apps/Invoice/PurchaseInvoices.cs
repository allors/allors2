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

            var created = new PurchaseInvoiceStates(this.Session).Created;
            var awaitingApproval = new PurchaseInvoiceStates(this.Session).AwaitingApproval;
            var inProcess = new PurchaseInvoiceStates(this.Session).InProcess;
            var received = new PurchaseInvoiceStates(this.Session).Received;
            var paid = new PurchaseInvoiceStates(this.Session).Paid;
            var cancelled = new PurchaseInvoiceStates(this.Session).Cancelled;
            var rejected = new PurchaseInvoiceStates(this.Session).Rejected;

            var approve = this.Meta.Approve;
            var reject = this.Meta.Reject;
            var confirm = this.Meta.Confirm;
            var cancel = this.Meta.Cancel;
            var reopen = this.Meta.Reopen;
            var createSalesInvoice = this.Meta.CreateSalesInvoice;

            config.Deny(this.ObjectType, created, approve, reject, reopen, createSalesInvoice);
            config.Deny(this.ObjectType, cancelled, approve, reject, confirm, cancel, createSalesInvoice);
            config.Deny(this.ObjectType, rejected, approve, reject, confirm, cancel, createSalesInvoice);
            config.Deny(this.ObjectType, awaitingApproval, confirm, cancel, reopen);
            config.Deny(this.ObjectType, inProcess, approve, confirm, reopen);
            config.Deny(this.ObjectType, received, createSalesInvoice);

            config.Deny(this.ObjectType, paid, Operations.Write, Operations.Execute);
            config.Deny(this.ObjectType, cancelled, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Write, Operations.Execute);
        }
    }
}
