// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoiceItemStates.cs" company="Allors bvba">
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

    public partial class SalesInvoiceItemStates
    {
        public static readonly Guid PaidId = new Guid("875AD2E4-BF44-46f4-9CD6-5F5C5BD43ADC");
        public static readonly Guid PartiallyPaidId = new Guid("2C6A00F7-466A-4689-A7E0-2D5660804B15");
        public static readonly Guid NotPaidId = new Guid("9926746F-C1DC-4968-BA85-9D461260DD0C");
        public static readonly Guid ReadyForPostingId = new Guid("9585A2C8-5B4D-4063-A8E7-E1310DFE439D");
        public static readonly Guid WrittenOffId = new Guid("F4408FD5-CCA3-44ea-BC00-4FFECC5D1EB9");
        public static readonly Guid CancelledId = new Guid("D521BBFA-1E18-453c-862F-28EBC0DA10C1");
        public static readonly Guid CancelledByInvoiceId = new Guid("3EE18D08-9AEA-445D-8E19-0616E4A61B0E");

        private UniquelyIdentifiableSticky<SalesInvoiceItemState> stateCache;

        public SalesInvoiceItemState NotPaid => this.StateCache[NotPaidId];

        public SalesInvoiceItemState PartiallyPaid => this.StateCache[PartiallyPaidId];

        public SalesInvoiceItemState Paid => this.StateCache[PaidId];

        public SalesInvoiceItemState ReadyForPosting => this.StateCache[ReadyForPostingId];

        public SalesInvoiceItemState WrittenOff => this.StateCache[WrittenOffId];

        public SalesInvoiceItemState Cancelled => this.StateCache[CancelledId];

        public SalesInvoiceItemState CancelledByInvoice => this.StateCache[CancelledByInvoiceId];

        private UniquelyIdentifiableSticky<SalesInvoiceItemState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesInvoiceItemState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            

            new SalesInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(NotPaidId)
                .WithName("NotPaid")
                .Build();

            new SalesInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new SalesInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new SalesInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(ReadyForPostingId)
                .WithName("Ready For Posting")
                .Build();

            new SalesInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(WrittenOffId)
                .WithName("Written Off")
                .Build();

            new SalesInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new SalesInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(CancelledByInvoiceId)
                .WithName("Cancelled")
                .Build();
        }
    }
}
