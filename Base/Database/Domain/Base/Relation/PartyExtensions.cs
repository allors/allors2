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
            if (date == DateTime.MinValue || internalOrganisation == null)
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
                    shipment.ShipmentState.Equals(new ShipmentStates(party.Strategy.Session).Picking) ||
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

            if (!party.ExistPreferredCurrency)
            {
                var singleton = session.GetSingleton();
                party.PreferredCurrency = singleton.Settings.PreferredCurrency;
            }
        }

        public static void BaseOnDerive(this Party @this, ObjectOnDerive method)
        {
            var derivedRoles = (PartyDerivedRoles)@this;

            derivedRoles.BillingAddress = null;
            derivedRoles.BillingInquiriesFax = null;
            derivedRoles.BillingInquiriesPhone = null;
            derivedRoles.CellPhoneNumber = null;
            derivedRoles.GeneralCorrespondence = null;
            derivedRoles.GeneralFaxNumber = null;
            derivedRoles.GeneralPhoneNumber = null;
            derivedRoles.HeadQuarter = null;
            derivedRoles.HomeAddress = null;
            derivedRoles.InternetAddress = null;
            derivedRoles.OrderAddress = null;
            derivedRoles.OrderInquiriesFax = null;
            derivedRoles.OrderInquiriesPhone = null;
            derivedRoles.PersonalEmailAddress = null;
            derivedRoles.SalesOffice = null;
            derivedRoles.ShippingAddress = null;
            derivedRoles.ShippingInquiriesFax = null;
            derivedRoles.ShippingAddress = null;

            foreach (PartyContactMechanism partyContactMechanism in @this.PartyContactMechanisms)
            {
                if (partyContactMechanism.UseAsDefault)
                {
                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).BillingAddress))
                    {
                        derivedRoles.BillingAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).BillingInquiriesFax))
                    {
                        derivedRoles.BillingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).BillingInquiriesPhone))
                    {
                        derivedRoles.BillingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).MobilePhoneNumber))
                    {
                        derivedRoles.CellPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralCorrespondence))
                    {
                        derivedRoles.GeneralCorrespondence = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralEmail))
                    {
                        derivedRoles.GeneralEmail = partyContactMechanism.ContactMechanism as EmailAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralFaxNumber))
                    {
                        derivedRoles.GeneralFaxNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralPhoneNumber))
                    {
                        derivedRoles.GeneralPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).HeadQuarters))
                    {
                        derivedRoles.HeadQuarter = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).HomeAddress))
                    {
                        derivedRoles.HomeAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).InternetAddress))
                    {
                        derivedRoles.InternetAddress = partyContactMechanism.ContactMechanism as ElectronicAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).OrderAddress))
                    {
                        derivedRoles.OrderAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).OrderInquiriesFax))
                    {
                        derivedRoles.OrderInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).OrderInquiriesPhone))
                    {
                        derivedRoles.OrderInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).PersonalEmailAddress))
                    {
                        derivedRoles.PersonalEmailAddress = partyContactMechanism.ContactMechanism as EmailAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).SalesOffice))
                    {
                        derivedRoles.SalesOffice = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).ShippingAddress))
                    {
                        derivedRoles.ShippingAddress = partyContactMechanism.ContactMechanism as PostalAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).ShippingInquiriesFax))
                    {
                        derivedRoles.ShippingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).ShippingInquiriesPhone))
                    {
                        derivedRoles.ShippingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }
                }

                // Fallback
                if (!@this.ExistBillingAddress && @this.ExistGeneralCorrespondence)
                {
                    derivedRoles.BillingAddress = @this.GeneralCorrespondence;
                }

                // Fallback
                if (!@this.ExistShippingAddress && @this.GeneralCorrespondence is PostalAddress postalAddress)
                {
                    derivedRoles.ShippingAddress = postalAddress;
                }
            }

            derivedRoles.CurrentPartyContactMechanisms = @this.PartyContactMechanisms
                .Where(v => v.FromDate <= @this.Strategy.Session.Now() && (!v.ExistThroughDate || v.ThroughDate >= @this.Strategy.Session.Now()))
                .ToArray();

            derivedRoles.InactivePartyContactMechanisms = @this.PartyContactMechanisms
                .Except(@this.CurrentPartyContactMechanisms)
                .ToArray();

            var allPartyRelationshipsWhereParty = @this.PartyRelationshipsWhereParty;

            derivedRoles.CurrentPartyRelationships = allPartyRelationshipsWhereParty
                .Where(v => v.FromDate <= @this.Strategy.Session.Now() && (!v.ExistThroughDate || v.ThroughDate >= @this.Strategy.Session.Now()))
                .ToArray();

            derivedRoles.InactivePartyRelationships = allPartyRelationshipsWhereParty
                .Except(@this.CurrentPartyRelationships)
                .ToArray();

            derivedRoles.CurrentSalesReps = @this.SalesRepRelationshipsWhereCustomer
                .Where(v => v.FromDate <= @this.Strategy.Session.Now() && (!v.ExistThroughDate || v.ThroughDate >= @this.Strategy.Session.Now()))
                .Select(v => v.SalesRepresentative)
                .ToArray();

            foreach (CustomerRelationship customerRelationship in @this.CustomerRelationshipsWhereCustomer)
            {
                // HACK: DerivedRoles
                var internalOrganisationDerivedRoles = (OrganisationDerivedRoles)customerRelationship.InternalOrganisation;
                
                if (@this.BaseIsActiveCustomer(customerRelationship.InternalOrganisation, @this.Strategy.Session.Now()))
                {
                    internalOrganisationDerivedRoles.AddActiveCustomer(@this);
                }
                else
                {
                    internalOrganisationDerivedRoles.RemoveActiveCustomer(@this);
                }
            }

            foreach (PartyFinancialRelationship partyFinancial in @this.PartyFinancialRelationshipsWhereParty)
            {
                // HACK: DerivedRoles
                var partyFinancialDerivedRoles = (PartyFinancialRelationshipDerivedRoles)partyFinancial;

                partyFinancialDerivedRoles.AmountDue = 0;
                partyFinancial.AmountOverDue = 0;

                // Open Order Amount
                partyFinancialDerivedRoles.OpenOrderAmount = @this.SalesOrdersWhereBillToCustomer
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
                        partyFinancialDerivedRoles.AmountDue += salesInvoice.TotalIncVat - salesInvoice.AmountPaid;
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
                                    partyFinancialDerivedRoles.AmountDue += invoiceItem.TotalIncVat - invoiceItem.AmountPaid;
                                }
                            }
                        }
                    }

                    var gracePeriod = salesInvoice.Store?.PaymentGracePeriod;

                    if (salesInvoice.DueDate.HasValue)
                    {
                        var dueDate = salesInvoice.DueDate.Value;

                        if (gracePeriod.HasValue)
                        {
                            dueDate = salesInvoice.DueDate.Value.AddDays(gracePeriod.Value);
                        }

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
                    // HACK: DerivedRoles
                    var internalOrganisationDerivedRoles = (OrganisationDerivedRoles)customerRelationship.InternalOrganisation;

                    internalOrganisationDerivedRoles.AddActiveCustomer(party);
                }
                else
                {
                    // HACK: DerivedRoles
                    var internalOrganisationDerivedRoles = (OrganisationDerivedRoles)customerRelationship.InternalOrganisation;
                    
                    internalOrganisationDerivedRoles.RemoveActiveCustomer(party);
                }
            }
        }
    }
}
