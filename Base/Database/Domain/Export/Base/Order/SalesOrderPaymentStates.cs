// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderPaymentState.cs" company="Allors bvba">
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

    public partial class SalesOrderPaymentStates
    {
        internal static readonly Guid NotPaidId = new Guid("8F5E0C7D-893F-4C7F-8297-3B4BD6319D02");
        internal static readonly Guid PaidId = new Guid("0C84C6F6-3204-4f7f-9BFA-FA4CBA643177");
        internal static readonly Guid PartiallyPaidId = new Guid("F9E8E105-F84E-4550-A725-25CE6E96614E");

        private UniquelyIdentifiableSticky<SalesOrderPaymentState> stateCache;

        public SalesOrderPaymentState NotPaid => this.StateCache[NotPaidId];

        public SalesOrderPaymentState PartiallyPaid => this.StateCache[PartiallyPaidId];

        public SalesOrderPaymentState Paid => this.StateCache[PaidId];

        private UniquelyIdentifiableSticky<SalesOrderPaymentState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesOrderPaymentState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {


            new SalesOrderPaymentStateBuilder(this.Session)
                .WithUniqueId(NotPaidId)
                .WithName("Not Paid")
                .Build();

            new SalesOrderPaymentStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new SalesOrderPaymentStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();
        }
    }
}