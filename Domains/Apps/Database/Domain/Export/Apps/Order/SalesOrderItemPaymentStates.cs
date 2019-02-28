// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderItemPaymentStates.cs" company="Allors bvba">
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

    public partial class SalesOrderItemPaymentStates
    {
        internal static readonly Guid NotPaidId = new Guid("2B859188-A3FA-4E53-8841-B316A81CD3BC");
        internal static readonly Guid PaidId = new Guid("086840CD-F7A6-4c04-A565-1D0AE07FED00");
        internal static readonly Guid PartiallyPaidId = new Guid("110F12F8-8AC6-40fb-8208-7697A36E88D7");

        private UniquelyIdentifiableSticky<SalesOrderItemPaymentState> stateCache;

        public SalesOrderItemPaymentState NotPaid => this.StateCache[NotPaidId];

        public SalesOrderItemPaymentState Paid => this.StateCache[PaidId];

        public SalesOrderItemPaymentState PartiallyPaid => this.StateCache[PartiallyPaidId];

        private UniquelyIdentifiableSticky<SalesOrderItemPaymentState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesOrderItemPaymentState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);
            
            new SalesOrderItemPaymentStateBuilder(this.Session)
                .WithUniqueId(NotPaidId)
                .WithName("Not Paid")
                .Build();

            new SalesOrderItemPaymentStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new SalesOrderItemPaymentStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();
        }
    }
}