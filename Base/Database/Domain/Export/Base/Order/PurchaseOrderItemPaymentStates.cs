// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItemPaymentStates.cs" company="Allors bvba">
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

    public partial class PurchaseOrderItemPaymentStates
    {
        internal static readonly Guid NotPaidId = new Guid("CAA6FCD8-3350-4113-945D-4C6A6398F317");
        internal static readonly Guid PaidId = new Guid("25AC349F-ECCD-4AB0-B032-0E5F14618083");
        internal static readonly Guid PartiallyPaidId = new Guid("B4269B24-DBCF-4548-A559-BC3D7A617D97");

        private UniquelyIdentifiableSticky<PurchaseOrderItemPaymentState> stateCache;

        public PurchaseOrderItemPaymentState NotPaid => this.StateCache[NotPaidId];

        public PurchaseOrderItemPaymentState Paid => this.StateCache[PaidId];

        public PurchaseOrderItemPaymentState PartiallyPaid => this.StateCache[PartiallyPaidId];

        private UniquelyIdentifiableSticky<PurchaseOrderItemPaymentState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<PurchaseOrderItemPaymentState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            
            
            new PurchaseOrderItemPaymentStateBuilder(this.Session)
                .WithUniqueId(NotPaidId)
                .WithName("Not Paid")
                .Build();

            new PurchaseOrderItemPaymentStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new PurchaseOrderItemPaymentStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();
        }
    }
}