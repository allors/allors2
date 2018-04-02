// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceStates.cs" company="Allors bvba">
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

    public partial class PurchaseInvoiceStates
    {
        private static readonly Guid InProcessId = new Guid("C6501188-7145-4abd-85FC-BEF746C74E9E");
        private static readonly Guid ReceivedId = new Guid("FC9EC85B-2419-4c97-92F6-45F5C6D3DF61");
        private static readonly Guid PaidId = new Guid("2982C8BE-657E-4594-BCAF-98997AFEA9F8");
        private static readonly Guid CancelledId = new Guid("60650051-F1F1-4dd6-90C8-5E744093D2EE");
        private static readonly Guid FinishedId = new Guid("26E27DDC-0782-4C29-B4BE-FF1E7AEE788A");

        private UniquelyIdentifiableSticky<PurchaseInvoiceState> stateCache;

        public PurchaseInvoiceState InProcess => this.StateCache[InProcessId];

        public PurchaseInvoiceState Received => this.StateCache[ReceivedId];

        public PurchaseInvoiceState Paid => this.StateCache[PaidId];

        public PurchaseInvoiceState Cancelled => this.StateCache[CancelledId];

        public PurchaseInvoiceState Finished => this.StateCache[FinishedId];

        private UniquelyIdentifiableSticky<PurchaseInvoiceState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<PurchaseInvoiceState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();
        }
    }
}
