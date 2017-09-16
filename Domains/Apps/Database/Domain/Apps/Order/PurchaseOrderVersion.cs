// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderVersion.cs" company="Allors bvba">
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
    public partial class PurchaseOrderVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (PurchaseOrderVersionBuilder) method.Builder;
            var purchaseOrder = builder.PurchaseOrder;

            if (purchaseOrder != null)
            {
                this.InternalComment = purchaseOrder.InternalComment;
                this.CustomerReference = purchaseOrder.CustomerReference;
                this.Fee = purchaseOrder.Fee;
                this.OrderTerms = purchaseOrder.OrderTerms;
                this.Message = purchaseOrder.Message;
                this.DiscountAdjustment = purchaseOrder.DiscountAdjustment;
                this.OrderKind = purchaseOrder.OrderKind;
                this.VatRegime = purchaseOrder.VatRegime;
                this.ShippingAndHandlingCharge = purchaseOrder.ShippingAndHandlingCharge;
                this.OrderDate = purchaseOrder.OrderDate;
                this.DeliveryDate = purchaseOrder.DeliveryDate;
                this.SurchargeAdjustment = purchaseOrder.SurchargeAdjustment;
                this.PurchaseOrderItems = purchaseOrder.PurchaseOrderItems;
                this.TakenViaSupplier = purchaseOrder.TakenViaSupplier;
                this.TakenViaContactMechanism = purchaseOrder.TakenViaContactMechanism;
                this.BillToContactMechanism = purchaseOrder.BillToContactMechanism;
                this.ShipToBuyer = purchaseOrder.ShipToBuyer;
                this.Facility = purchaseOrder.Facility;
                this.ShipToAddress = purchaseOrder.ShipToAddress;
                this.BillToPurchaser = purchaseOrder.BillToPurchaser;
                this.CurrentObjectState = purchaseOrder.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}