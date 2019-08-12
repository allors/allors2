 // --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipmentItem.cs" company="Allors bvba">
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
    public partial class ShipmentItem
    {
        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            method.SecurityTokens = this.SyncedShipment?.SecurityTokens.ToArray();
            method.DeniedPermissions = this.SyncedShipment?.DeniedPermissions.ToArray();
        }

        public void BaseDelete(DeletableDelete method)
        {
            if (this.ExistItemIssuancesWhereShipmentItem)
            {
                foreach (ItemIssuance itemIssuance in this.ItemIssuancesWhereShipmentItem)
                {
                    itemIssuance.Delete();
                }
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            derivation.AddDependency(this.ShipmentWhereShipmentItem, this);
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.BaseOnDeriveCustomerShipmentItem(derivation);

            this.BaseOnDerivePurchaseShipmentItem(derivation);
        }

        public void BaseOnDerivePurchaseShipmentItem(IDerivation derivation)
        {
            if (this.ShipmentWhereShipmentItem is PurchaseShipment)
            {
                this.Quantity = 0;
                var shipmentReceipt = this.ShipmentReceiptWhereShipmentItem;
                this.Quantity += shipmentReceipt.QuantityAccepted + shipmentReceipt.QuantityRejected;
            }
        }

        public void BaseOnDeriveCustomerShipmentItem(IDerivation derivation)
        {
            if (this.ShipmentWhereShipmentItem is CustomerShipment)
            {
                this.QuantityShipped = 0;
                foreach (PackagingContent packagingContent in this.PackagingContentsWhereShipmentItem)
                {
                    this.QuantityShipped += packagingContent.Quantity;
                }
            }
        }

        public void Sync(Shipment shipment)
        {
            this.SyncedShipment = shipment;
        }
    }
}