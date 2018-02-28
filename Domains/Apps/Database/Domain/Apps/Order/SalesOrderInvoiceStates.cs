// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderInvoiceStates.cs" company="Allors bvba">
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

    public partial class SalesOrderInvoiceStates
    {
        private static readonly Guid PartiallyInvoicedId = new Guid("40B4EFB9-42A4-43d9-BCE9-39E55FD9D507");
        private static readonly Guid InvoicedId = new Guid("CBDBFF96-B5DA-4be3-9B8D-EA785D08C85C");

        private UniquelyIdentifiableSticky<SalesOrderInvoiceState> stateCache;

        public SalesOrderInvoiceState PartiallyInvoiced => this.StateCache[PartiallyInvoicedId];

        public SalesOrderInvoiceState Invoiced => this.StateCache[InvoicedId];

        private UniquelyIdentifiableSticky<SalesOrderInvoiceState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesOrderInvoiceState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new SalesOrderInvoiceStateBuilder(this.Session)
                .WithUniqueId(PartiallyInvoicedId)
                .WithName("Partially Invoiced")
                .Build();

            new SalesOrderInvoiceStateBuilder(this.Session)
                .WithUniqueId(InvoicedId)
                .WithName("Invoiced")
                .Build();
        }
    }
}