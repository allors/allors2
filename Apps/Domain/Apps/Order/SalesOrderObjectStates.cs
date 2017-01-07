// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderObjectStates.cs" company="Allors bvba">
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

    public partial class SalesOrderObjectStates
    {
        private static readonly Guid ProvisionalId = new Guid("29ABC67D-4BE1-4af3-B993-64E9E36C3E6B");
        private static readonly Guid RequestsApprovalId = new Guid("6B6F6E25-4DA1-455d-9C9F-21F2D4316D66");
        private static readonly Guid CancelledId = new Guid("8AE3813D-7866-4e1c-AB70-EE695154F8F7");
        private static readonly Guid PartiallyShippedId = new Guid("40B4EFB9-42A4-43d9-BCE9-39E55FD9D507");
        private static readonly Guid ShippedId = new Guid("CBDBFF96-B5DA-4be3-9B8D-EA785D08C85C");
        private static readonly Guid CompletedId = new Guid("81F80082-040C-405a-8C01-778868D57C75");
        private static readonly Guid PaidId = new Guid("0C84C6F6-3204-4f7f-9BFA-FA4CBA643177");
        private static readonly Guid PartiallyPaidId = new Guid("F9E8E105-F84E-4550-A725-25CE6E96614E");
        private static readonly Guid RejectedId = new Guid("AE2AB1DC-0E5E-4061-924C-025AB84769C0");
        private static readonly Guid OnHoldId = new Guid("F625FB7E-893E-4f68-AB7B-2BC29A644E5B");
        private static readonly Guid InProcessId = new Guid("DDBB678E-9A66-4842-87FD-4E628CFF0A75");
        private static readonly Guid FinishedId = new Guid("DFE75006-81FD-424a-AF58-2528A657155D");

        private UniquelyIdentifiableCache<SalesOrderObjectState> stateCache;

        public SalesOrderObjectState Provisional => this.StateCache.Get(ProvisionalId);

        public SalesOrderObjectState RequestsApproval => this.StateCache.Get(RequestsApprovalId);

        public SalesOrderObjectState Cancelled => this.StateCache.Get(CancelledId);

        public SalesOrderObjectState Completed => this.StateCache.Get(CompletedId);

        public SalesOrderObjectState PartiallyShipped => this.StateCache.Get(PartiallyShippedId);

        public SalesOrderObjectState Shipped => this.StateCache.Get(ShippedId);

        public SalesOrderObjectState Paid => this.StateCache.Get(PaidId);

        public SalesOrderObjectState PartiallyPaid => this.StateCache.Get(PartiallyPaidId);

        public SalesOrderObjectState Rejected => this.StateCache.Get(RejectedId);

        public SalesOrderObjectState Finished => this.StateCache.Get(FinishedId);

        public SalesOrderObjectState OnHold => this.StateCache.Get(OnHoldId);

        public SalesOrderObjectState InProcess => this.StateCache.Get(InProcessId);

        private UniquelyIdentifiableCache<SalesOrderObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<SalesOrderObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(ProvisionalId)
                .WithName("Created")
                .Build();

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(RequestsApprovalId)
                .WithName("Requests Approval")
                .Build();

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(PartiallyShippedId)
                .WithName("Partially Shipped")
                .Build();

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(ShippedId)
                .WithName("Shipped")
                .Build();

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new SalesOrderObjectStateBuilder(this.Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();
        }
    }
}