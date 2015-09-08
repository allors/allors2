// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderItem.v.cs" company="Allors bvba">
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
    public partial class SalesOrderItem
    {
        public void CalculatePurchasePrice(IDerivation derivation)
        {
            this.AppsCalculatePurchasePrice(derivation);
        }

        public void CalculateUnitPrice(IDerivation derivation)
        {
            this.AppsCalculateUnitPrice(derivation);
        }

        public void DerivePrices(IDerivation derivation, decimal quantityOrdered = 0, decimal totalBasePrice = 0)
        {
            this.AppsOnDerivePrices(derivation, quantityOrdered, totalBasePrice);
        }

        public void DeriveShipTo(IDerivation derivation)
        {
            this.AppsOnDeriveShipTo(derivation);
        }

        public void DeriveDeliveryDate(IDerivation derivation)
        {
            this.AppsOnDeriveDeliveryDate(derivation);
        }

        public void DeriveReservedFromInventoryItem(IDerivation derivation)
        {
            this.AppsOnDeriveReservedFromInventoryItem(derivation);
        }

        public void DeriveQuantities(IDerivation derivation)
        {
            this.AppsOnDeriveQuantities(derivation);
        }

        public void DeriveCurrentObjectState(IDerivation derivation)
        {
            this.AppsOnDeriveCurrentObjectState(derivation);
        }

        public void DeriveCurrentOrderStatus(IDerivation derivation)
        {
            this.AppsOnDeriveCurrentOrderStatus(derivation);
        }

        public void DeriveCurrentShipmentStatus(IDerivation derivation)
        {
            this.AppsOnDeriveCurrentShipmentStatus(derivation);
        }

        public void DeriveMarkupAndProfitMargin(IDerivation derivation)
        {
            this.AppsOnDeriveMarkupAndProfitMargin(derivation);
        }

        public void DeriveOnShip(IDerivation derivation)
        {
            this.AppsOnDeriveOnShip(derivation);
        }

        public void DeriveOnShipped(IDerivation derivation, decimal quantity)
        {
            this.AppsOnDeriveOnShipped(derivation, quantity);
        }

        public void DeriveOnPicked(IDerivation derivation, decimal quantity)
        {
            this.AppsOnDeriveOnPicked(derivation, quantity);
        }

        public void DeriveAddToShipping(IDerivation derivation, decimal quantity)
        {
            this.AppsOnDeriveAddToShipping(derivation, quantity);
        }

        public void DeriveSubtractFromShipping(IDerivation derivation, decimal quantity)
        {
            this.AppsOnDeriveSubtractFromShipping(derivation, quantity);
        }

        public void DeriveSalesRep(IDerivation derivation)
        {
            this.AppsOnDeriveSalesRep(derivation);
        }

        public void DeriveIsValidOrderItem(IDerivation derivation)
        {
            this.AppsOnDeriveIsValidOrderItem(derivation);
        }

        public void DeriveVatRate(IDerivation derivation)
        {
            this.AppsOnDeriveVatRate(derivation);
        }

        public void DeriveVatRegime(IDerivation derivation)
        {
            this.AppsOnDeriveVatRegime(derivation);
        }

        public void DeriveCurrentPaymentStatus(IDerivation derivation)
        {
            this.AppsOnDeriveCurrentPaymentStatus(derivation);
        }
    }
}