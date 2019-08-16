
// <copyright file="SalesOrderInvoiceStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SalesOrderInvoiceStates
    {
        internal static readonly Guid NotInvoicedId = new Guid("4F5B8C2D-13A1-4E94-A122-89582BBF0B80");
        internal static readonly Guid InvoicedId = new Guid("CBDBFF96-B5DA-4be3-9B8D-EA785D08C85C");
        internal static readonly Guid PartiallyInvoicedId = new Guid("40B4EFB9-42A4-43d9-BCE9-39E55FD9D507");

        private UniquelyIdentifiableSticky<SalesOrderInvoiceState> stateCache;

        public SalesOrderInvoiceState NotInvoiced => this.StateCache[NotInvoicedId];

        public SalesOrderInvoiceState PartiallyInvoiced => this.StateCache[PartiallyInvoicedId];

        public SalesOrderInvoiceState Invoiced => this.StateCache[InvoicedId];

        private UniquelyIdentifiableSticky<SalesOrderInvoiceState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesOrderInvoiceState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new SalesOrderInvoiceStateBuilder(this.Session)
                .WithUniqueId(NotInvoicedId)
                .WithName("Not Invoiced")
                .Build();

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
