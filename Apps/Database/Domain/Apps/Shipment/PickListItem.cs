// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PickListItem.cs" company="Allors bvba">
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
    using Meta;
    using Resources;

    public partial class PickListItem
    {
        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistActualQuantity && this.ActualQuantity > this.RequestedQuantity)
            {
                derivation.Validation.AddError(this, M.PickListItem.ActualQuantity, ErrorMessages.PickListItemQuantityMoreThanAllowed);
            }

            this.AppsOnDeriveOrderItemAdjustment(derivation);
        }

        public void AppsOnDeriveOrderItemAdjustment(IDerivation derivation)
        {
            if (this.ActualQuantity.HasValue && this.ExistPickListWherePickListItem && this.PickListWherePickListItem.CurrentObjectState.Equals(new PickListObjectStates(this.strategy.Session).Picked))
            {
                var diff = this.RequestedQuantity - this.ActualQuantity.Value;

                foreach (ItemIssuance itemIssuance in this.ItemIssuancesWherePickListItem)
                {
                    itemIssuance.IssuanceDateTime = DateTime.UtcNow;
                    foreach (OrderShipment orderShipment in itemIssuance.ShipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        if (!orderShipment.Picked)
                        {
                            if (diff > 0)
                            {
                                if (orderShipment.Quantity >= diff)
                                {
                                    orderShipment.Quantity -= diff;
                                    orderShipment.ShipmentItem.Quantity -= diff;
                                    itemIssuance.Quantity -= diff;
                                    diff = 0;
                                }
                                else
                                {
                                    orderShipment.ShipmentItem.Quantity -= orderShipment.Quantity;
                                    itemIssuance.Quantity -= orderShipment.Quantity;
                                    diff -= orderShipment.Quantity;
                                    orderShipment.Quantity = 0;
                                }
                            }

                            orderShipment.SalesOrderItem.AppsOnDeriveOnPicked(derivation, orderShipment.Quantity);
                            orderShipment.Picked = true;
                        }
                    }
                }
            }
        }
    }
}