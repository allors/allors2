// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderItemPaymentState.cs" company="Allors bvba">
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
        private static readonly Guid PaidId = new Guid("086840CD-F7A6-4c04-A565-1D0AE07FED00");
        private static readonly Guid PartiallyPaidId = new Guid("110F12F8-8AC6-40fb-8208-7697A36E88D7");

        private UniquelyIdentifiableCache<SalesOrderItemPaymentState> stateCache;

        public SalesOrderItemPaymentState Paid => this.StateCache.Get(PaidId);

        public SalesOrderItemPaymentState PartiallyPaid => this.StateCache.Get(PartiallyPaidId);

        private UniquelyIdentifiableCache<SalesOrderItemPaymentState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<SalesOrderItemPaymentState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SalesOrderItemPaymentStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new SalesOrderItemPaymentStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();
        }
    }
}