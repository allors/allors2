// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryItemExtensions.cs" company="Allors bvba">
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
    using System.Linq;

    public static partial class InventoryItemExtensions
    {
        public static void AppsOnPreDerive(this InventoryItem @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (@this.Part is FinishedGood finishedGood)
            {
                foreach (Good good in finishedGood.GoodsWhereFinishedGood)
                {
                    derivation.AddDependency(good, @this);
                }
            }
        }

        public static void AppsOnDerive(this InventoryItem @this, ObjectOnDerive method)
        {
            var session = @this.Strategy.Session;
            var internalOrganisations = new Organisations(session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!@this.ExistInventoryOwnershipsWhereInventoryItem && internalOrganisations.Count() == 1)
            {
                new InventoryOwnershipBuilder(session)
                    .WithInventoryItem(@this)
                    .WithInternalOrganisation(internalOrganisations.First())
                    .Build();
            }

            if (!@this.ExistFacility && internalOrganisations.Count() == 1)
            {
                @this.Facility = internalOrganisations.First().DefaultFacility;
            }
        }

        public static void AppsOnDeriveQuantityOnHand(this InventoryItem @this, IDerivation derivation)
        {
            // TODO: Test for changes in these relations for performance reasons
            @this.QuantityOnHand = 0M;

            foreach (InventoryItemVariance inventoryItemVariance in @this.InventoryItemVariances)
            {
                @this.QuantityOnHand += inventoryItemVariance.Quantity;
            }

            foreach (PickListItem pickListItem in @this.PickListItemsWhereInventoryItem)
            {
                if (pickListItem.ActualQuantity.HasValue && pickListItem.PickListWherePickListItem.PickListState.Equals(new PickListStates(@this.Strategy.Session).Picked))
                {
                    @this.QuantityOnHand -= pickListItem.ActualQuantity.Value;
                }
            }

            foreach (ShipmentReceipt shipmentReceipt in @this.ShipmentReceiptsWhereInventoryItem)
            {
                if (shipmentReceipt.ExistShipmentItem)
                {
                    var purchaseShipment = (PurchaseShipment)shipmentReceipt.ShipmentItem.ShipmentWhereShipmentItem;
                    if (purchaseShipment.PurchaseShipmentState.Equals(new PurchaseShipmentStates(@this.Strategy.Session).Completed))
                    {
                        @this.QuantityOnHand += shipmentReceipt.QuantityAccepted;
                    }
                }
            }
        }
    }
}