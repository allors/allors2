// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderItemObjectStates.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class SalesOrderItemObjectStates
    {
        public static readonly Guid CreatedId = new Guid("5B0993B5-5784-4e8d-B1AD-93AFFAC9A913");
        public static readonly Guid CancelledId = new Guid("8B6FD903-B4A6-4360-A63C-9EBDFB7243AA");
        public static readonly Guid PartiallyShippedId = new Guid("E0FF4A01-CF9B-4dc7-ACF6-145F38F48AD1");
        public static readonly Guid ShippedId = new Guid("E91BAA87-DF5F-4a6c-B380-B683AD17AE18");
        public static readonly Guid CompletedId = new Guid("AC46B106-D266-46d7-BFD7-4196394A5AE0");
        public static readonly Guid PaidId = new Guid("086840CD-F7A6-4c04-A565-1D0AE07FED00");
        public static readonly Guid PartiallyPaidId = new Guid("110F12F8-8AC6-40fb-8208-7697A36E88D7");
        public static readonly Guid RejectedId = new Guid("F39F2F64-49A8-4a70-ACBC-B7F581F31EEF");
        public static readonly Guid OnHoldId = new Guid("3B185D51-AF4A-441e-BE0D-F91CFCBDB5C8");
        public static readonly Guid InProcessId = new Guid("E08401F7-1DEB-4b27-B0C5-8F034BFFEBD5");
        public static readonly Guid FinishedId = new Guid("33C0ED0C-FDFE-45ff-A008-7A638094A94A");

        private UniquelyIdentifiableCache<SalesOrderItemObjectState> stateCache;

        public SalesOrderItemObjectState Created
        {
            get { return this.StateCache.Get(CreatedId); }
        }

        public SalesOrderItemObjectState Cancelled
        {
            get { return this.StateCache.Get(CancelledId); }
        }

        public SalesOrderItemObjectState Completed
        {
            get { return this.StateCache.Get(CompletedId); }
        }

        public SalesOrderItemObjectState PartiallyShipped
        {
            get { return this.StateCache.Get(PartiallyShippedId); }
        }

        public SalesOrderItemObjectState Shipped
        {
            get { return this.StateCache.Get(ShippedId); }
        }

        public SalesOrderItemObjectState Paid
        {
            get { return this.StateCache.Get(PaidId); }
        }

        public SalesOrderItemObjectState PartiallyPaid
        {
            get { return this.StateCache.Get(PartiallyPaidId); }
        }

        public SalesOrderItemObjectState Rejected
        {
            get { return this.StateCache.Get(RejectedId); }
        }

        public SalesOrderItemObjectState Finished
        {
            get { return this.StateCache.Get(FinishedId); }
        }

        public SalesOrderItemObjectState OnHold
        {
            get { return this.StateCache.Get(OnHoldId); }
        }

        public SalesOrderItemObjectState InProcess
        {
            get { return this.StateCache.Get(InProcessId); }
        }

        private UniquelyIdentifiableCache<SalesOrderItemObjectState> StateCache
        {
            get
            {
                return this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<SalesOrderItemObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new SalesOrderItemObjectStateBuilder(Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new SalesOrderItemObjectStateBuilder(Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new SalesOrderItemObjectStateBuilder(Session)
                .WithUniqueId(PartiallyShippedId)
                .WithName("Partially Shipped")
                .Build();

            new SalesOrderItemObjectStateBuilder(Session)
                .WithUniqueId(ShippedId)
                .WithName("Shipped")
                .Build();

            new SalesOrderItemObjectStateBuilder(Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new SalesOrderItemObjectStateBuilder(Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new SalesOrderItemObjectStateBuilder(Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new SalesOrderItemObjectStateBuilder(Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new SalesOrderItemObjectStateBuilder(Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new SalesOrderItemObjectStateBuilder(Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new SalesOrderItemObjectStateBuilder(Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}