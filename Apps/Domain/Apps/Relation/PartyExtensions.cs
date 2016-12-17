// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyExtensions.cs" company="Allors bvba">
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
    using Meta;

    public static class PartyExtensions
    {
        public static NumberFormatInfo AppsGetCurrencyFormat(this Party party)
        {
            var cultureInfo = new CultureInfo(party.Locale.Name, false);
            var currencyFormat = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            currencyFormat.CurrencySymbol = party.PreferredCurrency.Symbol;
            return currencyFormat;
        }

        public static List<SalesOrder> AppsGetPreOrders(this Party party)
        {
            var preOrders = new List<SalesOrder>();
            foreach (SalesOrder salesOrder in party.SalesOrdersWhereBillToCustomer)
            {
                if (salesOrder.CurrentObjectState.Equals(new SalesOrderObjectStates(party.Strategy.Session).Provisional))
                {
                    preOrders.Add(salesOrder);
                }
            }

            return preOrders;
        }

        public static Person AppsFindCurrentContactByName(this Party party, string name)
        {
            var personsFound = new List<Person>();
            name = name.ToLower();
            foreach (Person person in party.CurrentContacts)
            {
                if ((person.ExistPartyName && person.PartyName.ToLower() == name) ||
                    (person.ExistLastName && person.LastName.ToLower() == name) ||
                    (person.ExistFirstName && person.FirstName.ToLower() == name))
                {
                    personsFound.Add(person);
                }
            }

            if (personsFound.Count == 1)
            {
                return personsFound[0];
            }

            return null;
        }

        public static IEnumerable<CustomerShipment> AppsGetPendingCustomerShipments(this Party party)
        {
            var shipments = party.ShipmentsWhereShipToParty;

            var pending = new List<CustomerShipment>();
            foreach (CustomerShipment shipment in shipments)
            {
                if (shipment.CurrentObjectState.Equals(new CustomerShipmentObjectStates(party.Strategy.Session).Created) ||
                    shipment.CurrentObjectState.Equals(new CustomerShipmentObjectStates(party.Strategy.Session).Picked) ||
                    shipment.CurrentObjectState.Equals(new CustomerShipmentObjectStates(party.Strategy.Session).OnHold) ||
                    shipment.CurrentObjectState.Equals(new CustomerShipmentObjectStates(party.Strategy.Session).Packed))
                {
                    pending.Add(shipment);
                }
            }

            return pending;
        }

        public static CustomerShipment AppsGetPendingCustomerShipmentForStore(this Party party, PostalAddress address, Store store, ShipmentMethod shipmentMethod)
        {
            var shipments = party.ShipmentsWhereShipToParty;
            shipments.Filter.AddEquals(M.Shipment.ShipToAddress, address);
            shipments.Filter.AddEquals(M.Shipment.Store, store);
            shipments.Filter.AddEquals(M.Shipment.ShipmentMethod, shipmentMethod);

            foreach (CustomerShipment shipment in shipments)
            {
                if (shipment.CurrentObjectState.Equals(new CustomerShipmentObjectStates(party.Strategy.Session).Created) ||
                    shipment.CurrentObjectState.Equals(new CustomerShipmentObjectStates(party.Strategy.Session).Picked) ||
                    shipment.CurrentObjectState.Equals(new CustomerShipmentObjectStates(party.Strategy.Session).OnHold) ||
                    shipment.CurrentObjectState.Equals(new CustomerShipmentObjectStates(party.Strategy.Session).Packed))
                {
                    return shipment;
                }                
            }

            return null;
        }

        public static bool AppsIsActiveSubContractor(this Party party, DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var subContractorRelationships = party.SubContractorRelationshipsWhereSubContractor;
            foreach (SubContractorRelationship relationship in subContractorRelationships)
            {
                if (relationship.FromDate <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool AppsIsActiveProspect(this Party party, DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var prospectRelationships = party.ProspectRelationshipsWhereProspect;
            foreach (ProspectRelationship relationship in prospectRelationships)
            {
                if (relationship.FromDate <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool AppsIsActiveClient(this Party party, DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var clientRelationships = party.ClientRelationshipsWhereClient;
            foreach (ClientRelationship relationship in clientRelationships)
            {
                if (relationship.FromDate <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool AppsIsActiveCustomer(this Party party, DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var customerRelationships = party.CustomerRelationshipsWhereCustomer;
            foreach (CustomerRelationship relationship in customerRelationships)
            {
                if (relationship.FromDate <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public static void AppsOnDeriveCurrentPartyContactMechanisms(this Party party, IDerivation derivation)
        {
            party.RemoveCurrentPartyContactMechanisms();

            foreach (PartyContactMechanism partyContactMechanism in party.PartyContactMechanisms)
            {
                if (partyContactMechanism.FromDate <= DateTime.UtcNow &&
                    (!partyContactMechanism.ExistThroughDate || partyContactMechanism.ThroughDate >= DateTime.UtcNow))
                {
                    party.AddCurrentPartyContactMechanism(partyContactMechanism);
                }
            }
        }

        public static void AppsOnBuild(this Party party, ObjectOnBuild method)
        {
            if (!party.ExistLocale && Singleton.Instance(party.Strategy.Session).ExistDefaultInternalOrganisation)
            {
                party.Locale = Singleton.Instance(party.Strategy.Session).DefaultInternalOrganisation.Locale;
            }

            if (!party.ExistPreferredCurrency && Singleton.Instance(party.Strategy.Session).ExistDefaultInternalOrganisation)
            {
                party.PreferredCurrency = Singleton.Instance(party.Strategy.Session).DefaultInternalOrganisation.PreferredCurrency;
            }
        }

        public static void AppsOnDerive(this Party @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            @this.BillingAddress = null;
            @this.BillingInquiriesFax = null;
            @this.BillingInquiriesPhone = null;
            @this.CellPhoneNumber = null;
            @this.GeneralFaxNumber = null;
            @this.GeneralPhoneNumber = null;
            @this.HeadQuarter = null;
            @this.HomeAddress = null;
            @this.InternetAddress = null;
            @this.OrderAddress = null;
            @this.OrderInquiriesFax = null;
            @this.OrderInquiriesPhone = null;
            @this.PersonalEmailAddress = null;
            @this.SalesOffice = null;
            @this.ShippingAddress = null;
            @this.ShippingInquiriesFax = null;
            @this.ShippingAddress = null;

            foreach (PartyContactMechanism partyContactMechanism in @this.PartyContactMechanisms)
            {
                if (partyContactMechanism.UseAsDefault)
                {
                    if (partyContactMechanism.ContactPurpose.IsBillingAddress)
                    {
                        @this.BillingAddress = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsBillingInquiriesFax)
                    {
                        @this.BillingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsBillingInquiriesPhone)
                    {
                        @this.BillingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsCellPhoneNumber)
                    {
                        @this.CellPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsGeneralCorrespondence)
                    {
                        @this.GeneralCorrespondence = partyContactMechanism.ContactMechanism as PostalAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsGeneralEmail)
                    {
                        @this.GeneralEmail = partyContactMechanism.ContactMechanism as ElectronicAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsGeneralFaxNumber)
                    {
                        @this.GeneralFaxNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsGeneralPhoneNumber)
                    {
                        @this.GeneralPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsHeadQuarters)
                    {
                        @this.HeadQuarter = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsHomeAddress)
                    {
                        @this.HomeAddress = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsInternetAddress)
                    {
                        @this.InternetAddress = partyContactMechanism.ContactMechanism as ElectronicAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsOrderAddress)
                    {
                        @this.OrderAddress = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsOrderInquiriesFax)
                    {
                        @this.OrderInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsOrderInquiriesPhone)
                    {
                        @this.OrderInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsPersonalEmailAddress)
                    {
                        @this.PersonalEmailAddress = partyContactMechanism.ContactMechanism as ElectronicAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsSalesOffice)
                    {
                        @this.SalesOffice = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsShippingAddress)
                    {
                        @this.ShippingAddress = partyContactMechanism.ContactMechanism as Domain.PostalAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsShippingInquiriesFax)
                    {
                        @this.ShippingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsShippingInquiriesPhone)
                    {
                        @this.ShippingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }
                }
            }

            @this.AppsOnDeriveCurrentSalesReps(derivation);
            @this.AppsOnDeriveOpenOrderAmount();
            @this.AppsOnDeriveRevenue();
        }

        public static void AppsOnDeriveCurrentSalesReps(this Party party, IDerivation derivation)
        {
            party.RemoveCurrentSalesReps();

            foreach (SalesRepRelationship salesRepRelationship in party.SalesRepRelationshipsWhereCustomer)
            {
                if (salesRepRelationship.FromDate <= DateTime.UtcNow &&
                    (!salesRepRelationship.ExistThroughDate || salesRepRelationship.ThroughDate >= DateTime.UtcNow))
                {
                    party.AddCurrentSalesRep(salesRepRelationship.SalesRepresentative);
                }
            }
        }

        public static void AppsOnDeriveOpenOrderAmount(this Party party)
        {
            party.OpenOrderAmount = 0;
            foreach (SalesOrder salesOrder in party.SalesOrdersWhereBillToCustomer)
            {
                if (!salesOrder.CurrentObjectState.Equals(new SalesOrderObjectStates(party.Strategy.Session).Finished) &&
                    !salesOrder.CurrentObjectState.Equals(new SalesOrderObjectStates(party.Strategy.Session).Cancelled))
                {
                    party.OpenOrderAmount += salesOrder.TotalIncVat;
                }
            }
        }

        public static void AppsOnDeriveRevenue(this Party party)
        {
            party.YTDRevenue = 0;
            party.LastYearsRevenue = 0;

            foreach (PartyRevenue partyRevenue in party.PartyRevenuesWhereParty)
            {
                if (partyRevenue.Year == DateTime.UtcNow.Year)
                {
                    party.YTDRevenue += partyRevenue.Revenue;
                }

                if (partyRevenue.Year == DateTime.UtcNow.AddYears(-1).Year)
                {
                    party.LastYearsRevenue += partyRevenue.Revenue;
                }
            }
        }
    }
}