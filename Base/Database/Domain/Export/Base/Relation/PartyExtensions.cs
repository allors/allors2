// <copyright file="PartyExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Meta;

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

        public static bool BaseIsActiveCustomer(this Party party, InternalOrganisation internalOrganisation, DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var customerRelationships = party.CustomerRelationshipsWhereCustomer;
            customerRelationships.Filter.AddEquals(M.CustomerRelationship.InternalOrganisation, internalOrganisation);

            return customerRelationships.Any(relationship => relationship.FromDate.Date <= date
                                                             && (!relationship.ExistThroughDate || relationship.ThroughDate >= date));
        }

        public static CustomerShipment BaseGetPendingCustomerShipmentForStore(this Party party, PostalAddress address, Store store, ShipmentMethod shipmentMethod)
        {
            var shipments = party.ShipmentsWhereShipToParty;
            if (address != null)
            {
                shipments.Filter.AddEquals(M.Shipment.ShipToAddress, address);
            }

            if (store != null)
            {
                shipments.Filter.AddEquals(M.Shipment.Store, store);
            }

            if (shipmentMethod != null)
            {
                shipments.Filter.AddEquals(M.Shipment.ShipmentMethod, shipmentMethod);
            }

            foreach (CustomerShipment shipment in shipments)
            {
                if (shipment.ShipmentState.Equals(new ShipmentStates(party.Strategy.Session).Created) ||
                    shipment.ShipmentState.Equals(new ShipmentStates(party.Strategy.Session).Picked) ||
                    shipment.ShipmentState.Equals(new ShipmentStates(party.Strategy.Session).OnHold) ||
                    shipment.ShipmentState.Equals(new ShipmentStates(party.Strategy.Session).Packed))
                {
                    return shipment;
                }
            }

            return null;
        }

        public static void BaseOnBuild(this Party party, ObjectOnBuild method)
        {
            var session = party.Strategy.Session;

            if (!party.ExistLocale)
            {
                party.Locale = session.GetSingleton().DefaultLocale;
            }

            if (!party.ExistPreferredCurrency)
            {
                party.PreferredCurrency = session.GetSingleton().Settings.PreferredCurrency;
            }
        }

        public static void BaseOnDerive(this Party @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            @this.BillingAddress = null;
            @this.BillingInquiriesFax = null;
            @this.BillingInquiriesPhone = null;
            @this.CellPhoneNumber = null;
            @this.GeneralCorrespondence = null;
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
                        @this.GeneralCorrespondence = partyContactMechanism.ContactMechanism;
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

                // Fallback
                if (!@this.ExistBillingAddress && @this.ExistGeneralCorrespondence)
                {
                    @this.BillingAddress = @this.GeneralCorrespondence;
                }

                // Fallback
                if (!@this.ExistShippingAddress && @this.GeneralCorrespondence is PostalAddress postalAddress)
                {
                    @this.ShippingAddress = postalAddress;
                }
            }

            @this.CurrentPartyContactMechanisms = @this.PartyContactMechanisms
                .Where(v => v.FromDate <= @this.Strategy.Session.Now() && (!v.ExistThroughDate || v.ThroughDate >= @this.Strategy.Session.Now()))
                .ToArray();

            @this.InactivePartyContactMechanisms = @this.PartyContactMechanisms
                .Except(@this.CurrentPartyContactMechanisms)
                .ToArray();

            var allPartyRelationshipsWhereParty = @this.PartyRelationshipsWhereParty;

            @this.CurrentPartyRelationships = allPartyRelationshipsWhereParty
                .Where(v => v.FromDate <= @this.Strategy.Session.Now() && (!v.ExistThroughDate || v.ThroughDate >= @this.Strategy.Session.Now()))
                .ToArray();

            @this.InactivePartyRelationships = allPartyRelationshipsWhereParty
                .Except(@this.CurrentPartyRelationships)
                .ToArray();

            @this.CurrentSalesReps = @this.SalesRepRelationshipsWhereCustomer
                .Where(v => v.FromDate <= @this.Strategy.Session.Now() && (!v.ExistThroughDate || v.ThroughDate >= @this.Strategy.Session.Now()))
                .Select(v => v.SalesRepresentative)
                .ToArray();

            // @this.BaseOnDeriveActiveCustomer(derivation);
            foreach (CustomerRelationship customerRelationship in @this.CustomerRelationshipsWhereCustomer)
            {
                if (@this.BaseIsActiveCustomer(customerRelationship.InternalOrganisation, @this.Strategy.Session.Now()))
                {
                    customerRelationship.InternalOrganisation.AddActiveCustomer(@this);
                }
                else
                {
                    customerRelationship.InternalOrganisation.RemoveActiveCustomer(@this);
                }
            }

            // var allCustomerRelationships = @this.CustomerRelationshipsWhereCustomer;
            // var allInternalOrganisations = allCustomerRelationships
            //    .Select(v => v.InternalOrganisation)
            //    .Distinct()
            //    .ToArray();

            // foreach (InternalOrganisation internalOrganisation in allInternalOrganisations)
            // {
            //    var activeCustomers = allCustomerRelationships
            //        .Where(v => Equals(v.InternalOrganisation, internalOrganisation) && v.FromDate.Date <= DateTime.UtcNow &&
            //                    (!v.ExistThroughDate || v.ThroughDate >= DateTime.UtcNow))
            //        .Select(v => v.Customer)
            //        .ToArray();

            // internalOrganisation.ActiveCustomers = activeCustomers;
            // }
            foreach (PartyFinancialRelationship partyFinancial in @this.PartyFinancialRelationshipsWhereParty)
            {
                partyFinancial.AmountDue = 0;
                partyFinancial.AmountOverDue = 0;

                // Open Order Amount
                partyFinancial.OpenOrderAmount = @this.SalesOrdersWhereBillToCustomer
                    .Where(v =>
                        Equals(v.TakenBy, partyFinancial.InternalOrganisation) &&
                        !v.SalesOrderState.Equals(new SalesOrderStates(@this.Strategy.Session).Finished) &&
                        !v.SalesOrderState.Equals(new SalesOrderStates(@this.Strategy.Session).Cancelled))
                    .Sum(v => v.TotalIncVat);

                // Amount Due
                // Amount OverDue
                foreach (var salesInvoice in @this.SalesInvoicesWhereBillToCustomer.Where(v => Equals(v.BilledFrom, partyFinancial.InternalOrganisation) &&
                                                                                                        !v.SalesInvoiceState.Equals(new SalesInvoiceStates(@this.Strategy.Session).Paid)))
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

                    var gracePeriod = salesInvoice.Store.PaymentGracePeriod;

                    if (salesInvoice.DueDate.HasValue)
                    {
                        var dueDate = salesInvoice.DueDate.Value.AddDays(gracePeriod);

                        if (@this.Strategy.Session.Now() > dueDate)
                        {
                            partyFinancial.AmountOverDue += salesInvoice.TotalIncVat - salesInvoice.AmountPaid;
                        }
                    }
                }
            }
        }

        public static void BaseOnPostDerive(this Party @this, ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;

            @this.BaseOnDerivePartyFinancialRelationships(derivation);
        }

        public static void BaseOnDerivePartyFinancialRelationships(this Party @this, IDerivation derivation)
        {
            var internalOrganisations = new Organisations(@this.Strategy.Session).InternalOrganisations();

            if (!internalOrganisations.Contains(@this))
            {
                foreach (var internalOrganisation in internalOrganisations)
                {
                    var partyFinancial = @this.PartyFinancialRelationshipsWhereParty.FirstOrDefault(v => Equals(v.InternalOrganisation, internalOrganisation));

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

        public static void BaseOnDeriveActiveCustomer(this Party party, IDerivation derivation)
        {
            foreach (CustomerRelationship customerRelationship in party.CustomerRelationshipsWhereCustomer)
            {
                if (party.BaseIsActiveCustomer(customerRelationship.InternalOrganisation, party.Strategy.Session.Now()))
                {
                    customerRelationship.InternalOrganisation.AddActiveCustomer(party);
                }
                else
                {
                    customerRelationship.InternalOrganisation.RemoveActiveCustomer(party);
                }
            }
        }
    }
}
