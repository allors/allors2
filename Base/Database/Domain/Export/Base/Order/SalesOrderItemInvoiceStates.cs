// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderItemInvoiceStates.cs" company="Allors bvba">
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

    public partial class SalesOrderItemInvoiceStates
    {
        internal static readonly Guid NotInvoicedId = new Guid("B5A95098-CB50-4A76-8968-92F01B460959");
        internal static readonly Guid InvoicedId = new Guid("F7F0142F-65EF-4D7A-8485-B5A44623BFAC");
        internal static readonly Guid PartiallyInvoicedId = new Guid("718DCE12-201C-4ECA-A385-767E6AAA89E4");

        private UniquelyIdentifiableSticky<SalesOrderItemInvoiceState> stateCache;

        public SalesOrderItemInvoiceState NotInvoiced => this.StateCache[NotInvoicedId];

        public SalesOrderItemInvoiceState Invoiced => this.StateCache[InvoicedId];

        public SalesOrderItemInvoiceState PartiallyInvoiced => this.StateCache[PartiallyInvoicedId];

        private UniquelyIdentifiableSticky<SalesOrderItemInvoiceState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesOrderItemInvoiceState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new SalesOrderItemInvoiceStateBuilder(this.Session)
                .WithUniqueId(NotInvoicedId)
                .WithName("Not Invoiced")
                .Build();

            new SalesOrderItemInvoiceStateBuilder(this.Session)
                .WithUniqueId(PartiallyInvoicedId)
                .WithName("Partially Invoiced")
                .Build();

            new SalesOrderItemInvoiceStateBuilder(this.Session)
                .WithUniqueId(InvoicedId)
                .WithName("Invoiced")
                .Build();
        }
    }
}
