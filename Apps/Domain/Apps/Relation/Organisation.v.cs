// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Organisation.v.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class Organisation
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

        public bool IsActiveClient(DateTime date)
        {
            return this.AppsIsActiveClient(date);
        }

        public bool IsActiveCustomer(DateTime date)
        {
            return this.AppsIsActiveCustomer(date);
        }

        public bool IsActiveDistributor(DateTime date)
        {
            return this.AppsIsActiveDistributor(date);
        }

        public bool IsActivePartner(DateTime date)
        {
            return this.AppsIsActivePartner(date);
        }

        public bool IsActiveProfessionalServicesProvider(DateTime date)
        {
            return this.AppsIsActiveProfessionalServicesProvider(date);
        }

        public bool IsActiveProspect(DateTime date)
        {
            return this.AppsIsActiveProspect(date);
        }

        public bool IsActiveSubContractor(DateTime date)
        {
            return this.AppsIsActiveSubContractor(date);
        }

        public bool IsActiveSupplier(DateTime date)
        {
            return this.AppsIsActiveSupplier(date);
        }

        public void DeriveUserGroups(IDerivation derivation)
        {
            this.AppsOnDeriveUserGroups(derivation);
        }
    }
}