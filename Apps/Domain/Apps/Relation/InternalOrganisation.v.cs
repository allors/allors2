// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalOrganisation.v.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.Globalization;

    public partial class InternalOrganisation
    {
        public NumberFormatInfo CurrencyFormat
        {
            get
            {
                return this.AppsGetCurrencyFormat();
            }
        }

        public List<SalesOrder> PreOrders
        {
            get
            {
                return this.AppsGetPreOrders();
            }
        }

        public IEnumerable<CustomerShipment> PendingCustomerShipments
        {
            get
            {
                return this.AppsGetPendingCustomerShipments();
            }
        }

        public void DeriveCurrentSalesReps(IDerivation derivation)
        {
            this.AppsOnDeriveCurrentSalesReps(derivation);
        }

        public void DeriveOpenOrderAmount()
        {
            this.AppsOnDeriveOpenOrderAmount();
        }

        public void DeriveRevenue()
        {
            this.AppsOnDeriveRevenue();
        }

        public CustomerShipment GetPendingCustomerShipmentForStore(PostalAddress address, Store store, ShipmentMethod shipmentMethod)
        {
            return this.AppsGetPendingCustomerShipmentForStore(address, store, shipmentMethod);
        }

        public void StartNewFiscalYear()
        {
            this.AppsStartNewFiscalYear();
        }

        private void DeriveEmployeeUserGroups(IDerivation derivation)
        {
            this.AppsOnDeriveEmployeeUserGroups(derivation);
        }
    }
}