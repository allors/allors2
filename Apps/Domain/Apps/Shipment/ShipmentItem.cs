 // --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipmentItem.cs" company="Allors bvba">
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
    public partial class ShipmentItem
    {
        public void AppsDelete(DeletableDelete method)
        {
            if (this.ExistItemIssuancesWhereShipmentItem)
            {
                foreach (ItemIssuance itemIssuance in this.ItemIssuancesWhereShipmentItem)
                {
                    itemIssuance.Delete();
                }
            }
        }
        
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistQuantity)
            {
                this.Quantity = 0;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                derivation.AddDependency(this.ShipmentWhereShipmentItem, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.DeriveCustomerShipmentItem(derivation);

            this.DerivePurchaseShipmentItem(derivation);
        }

        public void AppsOnDerivePurchaseShipmentItem(IDerivation derivation)
        {
            if (this.ShipmentWhereShipmentItem is PurchaseShipment)
            {
                this.Quantity = 0;
                var shipmentReceipt = this.ShipmentReceiptWhereShipmentItem;
                this.Quantity += shipmentReceipt.QuantityAccepted + shipmentReceipt.QuantityRejected;
            }
        }

        public void AppsOnDeriveCustomerShipmentItem(IDerivation derivation)
        {
            if (this.ShipmentWhereShipmentItem is CustomerShipment)
            {
                this.QuantityShipped = 0;
                foreach (PackagingContent packagingContent in PackagingContentsWhereShipmentItem)
                {
                    this.QuantityShipped += packagingContent.Quantity;
                }
            }
        }
    }
}