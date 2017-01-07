// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderItemObjectStates.cs" company="Allors bvba">
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

    public partial class SalesOrderItemObjectStates
    {
        private static readonly Guid CreatedId = new Guid("5B0993B5-5784-4e8d-B1AD-93AFFAC9A913");
        private static readonly Guid CancelledId = new Guid("8B6FD903-B4A6-4360-A63C-9EBDFB7243AA");
        private static readonly Guid PartiallyShippedId = new Guid("E0FF4A01-CF9B-4dc7-ACF6-145F38F48AD1");
        private static readonly Guid ShippedId = new Guid("E91BAA87-DF5F-4a6c-B380-B683AD17AE18");
        private static readonly Guid CompletedId = new Guid("AC46B106-D266-46d7-BFD7-4196394A5AE0");
        private static readonly Guid PaidId = new Guid("086840CD-F7A6-4c04-A565-1D0AE07FED00");
        private static readonly Guid PartiallyPaidId = new Guid("110F12F8-8AC6-40fb-8208-7697A36E88D7");
        private static readonly Guid RejectedId = new Guid("F39F2F64-49A8-4a70-ACBC-B7F581F31EEF");
        private static readonly Guid OnHoldId = new Guid("3B185D51-AF4A-441e-BE0D-F91CFCBDB5C8");
        private static readonly Guid InProcessId = new Guid("E08401F7-1DEB-4b27-B0C5-8F034BFFEBD5");
        private static readonly Guid FinishedId = new Guid("33C0ED0C-FDFE-45ff-A008-7A638094A94A");

        private UniquelyIdentifiableCache<SalesOrderItemObjectState> stateCache;

        public SalesOrderItemObjectState Created => this.StateCache.Get(CreatedId);

        public SalesOrderItemObjectState Cancelled => this.StateCache.Get(CancelledId);

        public SalesOrderItemObjectState Completed => this.StateCache.Get(CompletedId);

        public SalesOrderItemObjectState PartiallyShipped => this.StateCache.Get(PartiallyShippedId);

        public SalesOrderItemObjectState Shipped => this.StateCache.Get(ShippedId);

        public SalesOrderItemObjectState Paid => this.StateCache.Get(PaidId);

        public SalesOrderItemObjectState PartiallyPaid => this.StateCache.Get(PartiallyPaidId);

        public SalesOrderItemObjectState Rejected => this.StateCache.Get(RejectedId);

        public SalesOrderItemObjectState Finished => this.StateCache.Get(FinishedId);

        public SalesOrderItemObjectState OnHold => this.StateCache.Get(OnHoldId);

        public SalesOrderItemObjectState InProcess => this.StateCache.Get(InProcessId);

        private UniquelyIdentifiableCache<SalesOrderItemObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<SalesOrderItemObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SalesOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new SalesOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new SalesOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(PartiallyShippedId)
                .WithName("Partially Shipped")
                .Build();

            new SalesOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(ShippedId)
                .WithName("Shipped")
                .Build();

            new SalesOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new SalesOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new SalesOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new SalesOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new SalesOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new SalesOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new SalesOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();
        }
    }
}