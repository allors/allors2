// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderShipment.cs" company="Allors bvba">
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
    public partial class OrderShipment
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            

            this.Picked = false;

            if (!this.ExistQuantity)
            {
                this.Quantity = 0;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Log.AssertAtLeastOne(this, OrderShipments.Meta.SalesOrderItem, OrderShipments.Meta.PurchaseOrderItem);
            derivation.Log.AssertExistsAtMostOne(this, OrderShipments.Meta.SalesOrderItem, OrderShipments.Meta.PurchaseOrderItem);
        }
    }
}