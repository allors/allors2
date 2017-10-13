//------------------------------------------------------------------------------------------------- 
// <copyright file="PaymentStates.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
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
// <summary>Defines the HomeAddress type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class PaymentStates
    {
        private static readonly Guid UnpaidId = new Guid("FC38E48D-C8C4-4F26-A8F1-5D4E962B6F93");
        private static readonly Guid PartiallyPaidId = new Guid("1801737F-2760-4600-9243-7E6BDD8A224D");
        private static readonly Guid PaidId = new Guid("04FAD96A-2B0F-4F07-ABB7-57657A34E422");

        private UniquelyIdentifiableSticky<PaymentState> sticky;

        public Sticky<Guid, PaymentState> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<PaymentState>(this.Session));

        public PaymentState Unpaid => this.Sticky[UnpaidId];

        public PaymentState PartiallyPaid => this.Sticky[PartiallyPaidId];

        public PaymentState Paid => this.Sticky[PaidId];

        protected override void BaseSetup(Setup config)
        {
            new PaymentStateBuilder(this.Session).WithUniqueId(UnpaidId).WithName("Unpaid").Build();
            new PaymentStateBuilder(this.Session).WithUniqueId(PartiallyPaidId).WithName("PartiallyPaid").Build();
            new PaymentStateBuilder(this.Session).WithUniqueId(PaidId).WithName("Paid").Build();
        }
    }
}