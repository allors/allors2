// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyExtensions.cs" company="Allors bvba">
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
using System.Runtime.CompilerServices;

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using Meta;

    public static class PartyExtensions
    {
        public static int? PaymentNetDays(this Party party)
        {
            int? customerPaymentNetDays = null;
            foreach (Agreement agreement in party.Agreements)
            {
                foreach (AgreementTerm term in agreement.AgreementTerms)
                {
                    if (term.TermType.Equals(new InvoiceTermTypes(party.Strategy.Session).PaymentNetDays))
                    {
                        if (int.TryParse(term.TermValue, out var netDays))
                        {
                            customerPaymentNetDays = netDays;
                        }

                        return customerPaymentNetDays;
                    }
                }
            }

            return null;
        }

        public static bool AppsIsActiveCustomer(this Party party, InternalOrganisation internalOrganisation, DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var customerRelationships = party.CustomerRelationshipsWhereCustomer;
            customerRelationships.Filter.AddEquals(M.CustomerRelationship.InternalOrganisation, internalOrganisation);

            foreach (CustomerRelationship relationship in customerRelationships)
            {
                if (relationship.FromDate.Date <= date &&
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
                if (relationship.FromDate.Date <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<CustomerShipment> AppsGetPendingCustomerShipments(this Party party)
        {
            var shipments = party.ShipmentsWhereShipToParty;

            var pending = new List<CustomerShipment>();
            foreach (CustomerShipment shipment in shipments)
            {
                if (shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(party.Strategy.Session).Created) ||
                    shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(party.Strategy.Session).Picked) ||
                    shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(party.Strategy.Session).OnHold) ||
                    shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(party.Strategy.Session).Packed))
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
                if (shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(party.Strategy.Session).Created) ||
                    shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(party.Strategy.Session).Picked) ||
                    shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(party.Strategy.Session).OnHold) ||
                    shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(party.Strategy.Session).Packed))
                {
                    return shipment;
                }
            }

            return null;
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
            var session = party.Strategy.Session;

            if (!party.ExistLocale)
            {
                party.Locale = session.GetSingleton().DefaultLocale;
            }

            if (!party.ExistPreferredCurrency)
            {
                party.PreferredCurrency = session.GetSingleton().PreferredCurrency;
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
                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).BillingAddress))
                    {
                        @this.BillingAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).BillingInquiriesFax))
                    {
                        @this.BillingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).BillingInquiriesPhone))
                    {
                        @this.BillingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).MobilePhoneNumber))
                    {
                        @this.CellPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralCorrespondence))
                    {
                        @this.GeneralCorrespondence = partyContactMechanism.ContactMechanism as PostalAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralEmail))
                    {
                        @this.GeneralEmail = partyContactMechanism.ContactMechanism as EmailAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralFaxNumber))
                    {
                        @this.GeneralFaxNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralPhoneNumber))
                    {
                        @this.GeneralPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).HeadQuarters))
                    {
                        @this.HeadQuarter = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).HomeAddress))
                    {
                        @this.HomeAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).InternetAddress))
                    {
                        @this.InternetAddress = partyContactMechanism.ContactMechanism as ElectronicAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).OrderAddress))
                    {
                        @this.OrderAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).OrderInquiriesFax))
                    {
                        @this.OrderInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).OrderInquiriesPhone))
                    {
                        @this.OrderInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).PersonalEmailAddress))
                    {
                        @this.PersonalEmailAddress = partyContactMechanism.ContactMechanism as EmailAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).SalesOffice))
                    {
                        @this.SalesOffice = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).ShippingAddress))
                    {
                        @this.ShippingAddress = partyContactMechanism.ContactMechanism as PostalAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).ShippingInquiriesFax))
                    {
                        @this.ShippingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).ShippingInquiriesPhone))
                    {
                        @this.ShippingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }
                }
            }

            @this.AppsOnDeriveCurrentSalesReps(derivation);
            @this.AppsOnDeriveActiveCustomer(derivation);
            @this.AppsOnDeriveOpenOrderAmount();
            @this.AppsOnDeriveAmountDue(derivation);
            @this.AppsOnDeriveAmountOverDue(derivation);
            @this.AppsOnDeriveRevenue();
        }

        public static void AppsOnPostDerive(this Party @this, ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;

            @this.AppsOnDerivePartyFinancialRelationships(derivation);
        }

        public static void AppsOnDerivePartyFinancialRelationships(this Party @this, IDerivation derivation)
        {
            var internalOrganisations = new Organisations(@this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!internalOrganisations.Contains(@this))
            {
                foreach (Organisation internalOrganisation in internalOrganisations)
                {
                    var partyFinancial = @this.PartyFinancialRelationshipsWhereParty.FirstOrDefault(v =>
                            Equals(v.InternalOrganisation, internalOrganisation));

                    if (partyFinancial == null)
                    {
                        partyFinancial = new PartyFinancialRelationshipBuilder(@this.Strategy.Session)
                            .WithParty(@this)
                            .WithInternalOrganisation(internalOrganisation)
                            .Build();
                    }

                    if (partyFinancial.SubAccountNumber == 0)
                    {
                        partyFinancial.SubAccountNumber = internalOrganisation.NextSubAccountNumber();
                    }
                }
            }
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

        public static void AppsOnDeriveActiveCustomer(this Party party, IDerivation derivation)
        {
            foreach (CustomerRelationship customerRelationship in party.CustomerRelationshipsWhereCustomer)
            {
                if (party.AppsIsActiveCustomer(customerRelationship.InternalOrganisation, DateTime.UtcNow))
                {
                    customerRelationship.InternalOrganisation.AddActiveCustomer(party);
                }
                else
                {
                    customerRelationship.InternalOrganisation.RemoveActiveCustomer(party);
                }

            }
        }

        public static void AppsOnDeriveOpenOrderAmount(this Party @this)
        {
            foreach (PartyFinancialRelationship partyFinancial in @this.PartyFinancialRelationshipsWhereParty)
            {
                partyFinancial.OpenOrderAmount = 0;
                foreach (SalesOrder salesOrder in @this.SalesOrdersWhereBillToCustomer)
                {
                    if (Equals(salesOrder.TakenBy, partyFinancial.InternalOrganisation) &&
                        !salesOrder.SalesOrderState.Equals(new SalesOrderStates(@this.Strategy.Session).Finished) &&
                        !salesOrder.SalesOrderState.Equals(new SalesOrderStates(@this.Strategy.Session).Cancelled))
                    {
                        partyFinancial.OpenOrderAmount += salesOrder.TotalIncVat;
                    }
                }
            }
        }

        public static void AppsOnDeriveAmountDue(this Party @this, IDerivation derivation)
        {
            foreach (PartyFinancialRelationship partyFinancial in @this.PartyFinancialRelationshipsWhereParty)
            {
                partyFinancial.AmountDue = 0;

                foreach (SalesInvoice salesInvoice in @this.SalesInvoicesWhereBillToCustomer)
                {
                    if (Equals(salesInvoice.BilledFrom, partyFinancial.InternalOrganisation) &&
                        !salesInvoice.SalesInvoiceState.Equals(new SalesInvoiceStates(@this.Strategy.Session).Paid))
                    {
                        if (salesInvoice.AmountPaid > 0)
                        {
                            partyFinancial.AmountDue += salesInvoice.TotalIncVat - salesInvoice.AmountPaid;
                        }
                        else
                        {
                            foreach (SalesInvoiceItem invoiceItem in salesInvoice.InvoiceItems)
                            {
                                if (!invoiceItem.SalesInvoiceItemState.Equals(
                                    new SalesInvoiceItemStates(@this.Strategy.Session).Paid))
                                {
                                    if (invoiceItem.ExistTotalIncVat)
                                    {
                                        partyFinancial.AmountDue += invoiceItem.TotalIncVat - invoiceItem.AmountPaid;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void AppsOnDeriveAmountOverDue(this Party @this, IDerivation derivation)
        {
            foreach (PartyFinancialRelationship partyFinancial in @this.PartyFinancialRelationshipsWhereParty)
            {
                partyFinancial.AmountOverDue = 0;

                foreach (SalesInvoice salesInvoice in @this.SalesInvoicesWhereBillToCustomer)
                {
                    if (Equals(salesInvoice.BilledFrom, partyFinancial.InternalOrganisation) &&
                        !salesInvoice.SalesInvoiceState.Equals(new SalesInvoiceStates(@this.Strategy.Session).Paid))
                    {
                        var gracePeriod = salesInvoice.Store.PaymentGracePeriod;

                        if (salesInvoice.DueDate.HasValue)
                        {
                            var dueDate = salesInvoice.DueDate.Value.AddDays(gracePeriod);

                            if (DateTime.UtcNow > dueDate)
                            {
                                partyFinancial.AmountOverDue += salesInvoice.TotalIncVat - salesInvoice.AmountPaid;
                            }
                        }
                    }
                }
            }
        }

        public static void AppsOnDeriveRevenue(this Party @this)
        {
            foreach (PartyFinancialRelationship partyFinancial in @this.PartyFinancialRelationshipsWhereParty)
            {
                partyFinancial.YTDRevenue = 0;
                partyFinancial.LastYearsRevenue = 0;

                foreach (PartyRevenue partyRevenue in @this.PartyRevenuesWhereParty)
                {
                    if (Equals(partyRevenue.InternalOrganisation, partyFinancial.InternalOrganisation))
                    {
                        if (partyRevenue.Year == DateTime.UtcNow.Year)
                        {
                            partyFinancial.YTDRevenue += partyRevenue.Revenue;
                        }

                        if (partyRevenue.Year == DateTime.UtcNow.AddYears(-1).Year)
                        {
                            partyFinancial.LastYearsRevenue += partyRevenue.Revenue;
                        }
                    }
                }
            }
        }
    }
}