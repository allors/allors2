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

using System.Linq;

namespace Allors.Domain
{
    using System;
    using Meta;
    using Resources;

    public partial class PickListItem
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (ItemIssuance itemIssuance in this.ItemIssuancesWherePickListItem)
            {
                derivation.AddDependency(itemIssuance, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.Quantity > 0 && this.QuantityPicked > this.Quantity)
            {
                derivation.Validation.AddError(this, M.PickListItem.QuantityPicked, ErrorMessages.PickListItemQuantityMoreThanAllowed);
            }

            if (this.QuantityPicked > 0 && this.ExistPickListWherePickListItem && this.PickListWherePickListItem.PickListState.Equals(new PickListStates(this.strategy.Session).Picked))
            {
                var diff = this.Quantity - this.QuantityPicked;

                foreach (ItemIssuance itemIssuance in this.ItemIssuancesWherePickListItem)
                {
                    itemIssuance.IssuanceDateTime = DateTime.UtcNow;
                    foreach (OrderShipment orderShipment in itemIssuance.ShipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        if (orderShipment.OrderItem is SalesOrderItem salesOrderItem)
                        {
                            if (diff > 0 && this.QuantityPicked != orderShipment.Quantity)
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
                        }
                    }
                }
            }
        }
    }
}