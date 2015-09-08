// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrder.v.cs" company="Allors bvba">
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
    public partial class PurchaseOrder
    {
        public void DeriveCurrentShipmentStatus(IDerivation derivation)
        {
            this.AppsOnDeriveCurrentShipmentStatus(derivation);
        }

        public void DeriveCurrentPaymentStatus(IDerivation derivation)
        {
            this.AppsOnDeriveCurrentPaymentStatus(derivation);
        }

        public void DeriveCurrentOrderStatus(IDerivation derivation)
        {
            this.AppsOnDeriveCurrentOrderStatus(derivation);
        }

        public void DeriveLocale(IDerivation derivation)
        {
            this.AppsOnDeriveLocale(derivation);
        }

        public void DeriveTemplate(IDerivation derivation)
        {
            this.AppsOnDeriveTemplate(derivation);
        }

        public void DeriveOrderTotals(IDerivation derivation)
        {
            this.AppsOnDeriveOrderTotals(derivation);
        }

        public void DeriveOrderItems(IDerivation derivation)
        {
            this.AppsOnDeriveOrderItems(derivation);
        }
    }
}