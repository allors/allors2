// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderVersion.cs" company="Allors bvba">
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
    public partial class SalesOrderVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (SalesOrderVersionBuilder) method.Builder;
            var salesOrder = builder.SalesOrder;

            if (salesOrder != null)
            {
                this.InternalComment = salesOrder.InternalComment;
                this.CustomerReference = salesOrder.CustomerReference;
                this.Fee = salesOrder.Fee;
                this.OrderTerms = salesOrder.OrderTerms;
                this.Message = salesOrder.Message;
                this.DiscountAdjustment = salesOrder.DiscountAdjustment;
                this.OrderKind = salesOrder.OrderKind;
                this.VatRegime = salesOrder.VatRegime;
                this.ShippingAndHandlingCharge = salesOrder.ShippingAndHandlingCharge;
                this.OrderDate = salesOrder.OrderDate;
                this.DeliveryDate = salesOrder.DeliveryDate;
                this.SurchargeAdjustment = salesOrder.SurchargeAdjustment;
                this.TakenByContactMechanism = salesOrder.TakenByContactMechanism;
                this.ShipToCustomer = salesOrder.ShipToCustomer;
                this.BillToCustomer = salesOrder.BillToCustomer;
                this.ShipmentMethod = salesOrder.ShipmentMethod;
                this.ShipToAddress = salesOrder.ShipToAddress;
                this.BillToContactMechanism = salesOrder.BillToContactMechanism;
                this.SalesReps = salesOrder.SalesReps;
                this.PartiallyShip = salesOrder.PartiallyShip;
                this.Store = salesOrder.Store;
                this.BillFromContactMechanism = salesOrder.BillFromContactMechanism;
                this.PaymentMethod = salesOrder.PaymentMethod;
                this.PlacingContactMechanism = salesOrder.PlacingContactMechanism;
                this.SalesChannel = salesOrder.SalesChannel;
                this.PlacingCustomer = salesOrder.PlacingCustomer;
                this.SalesOrderItems = salesOrder.SalesOrderItems;
                this.TakenByInternalOrganisation = salesOrder.TakenByInternalOrganisation;
                this.Quote = salesOrder.Quote;
                this.CurrentObjectState = salesOrder.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}