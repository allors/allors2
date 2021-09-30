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
        public static void BaseOnBuild(this Party @this, ObjectOnBuild method)
        {
            var session = @this.Strategy.Session;

            if (!@this.ExistPreferredCurrency)
            {
                var singleton = session.GetSingleton();
                @this.PreferredCurrency = singleton.Settings.PreferredCurrency;
            }
        }

        public static void BaseOnPreDerive(this Party @this, ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(@this) || changeSet.IsCreated(@this) || changeSet.HasChangedRoles(@this))
            {
                foreach (PartyFinancialRelationship partyFinancialRelationship in @this.PartyFinancialRelationshipsWhereParty)
                {
                    iteration.AddDependency(partyFinancialRelationship, @this);
                    iteration.Mark(partyFinancialRelationship);
                }
            }
        }

        public static void BaseOnDerive(this Party @this, ObjectOnDerive method)
        {
            @this.DerivedRoles.BillingAddress = null;
            @this.DerivedRoles.BillingInquiriesFax = null;
            @this.DerivedRoles.BillingInquiriesPhone = null;
            @this.DerivedRoles.CellPhoneNumber = null;
            @this.DerivedRoles.GeneralCorrespondence = null;
            @this.DerivedRoles.GeneralFaxNumber = null;
            @this.DerivedRoles.GeneralPhoneNumber = null;
            @this.DerivedRoles.HeadQuarter = null;
            @this.DerivedRoles.HomeAddress = null;
            @this.DerivedRoles.InternetAddress = null;
            @this.DerivedRoles.OrderAddress = null;
            @this.DerivedRoles.OrderInquiriesFax = null;
            @this.DerivedRoles.OrderInquiriesPhone = null;
            @this.DerivedRoles.PersonalEmailAddress = null;
            @this.DerivedRoles.SalesOffice = null;
            @this.DerivedRoles.ShippingAddress = null;
            @this.DerivedRoles.ShippingInquiriesFax = null;
            @this.DerivedRoles.ShippingAddress = null;

            foreach (PartyContactMechanism partyContactMechanism in @this.PartyContactMechanisms)
            {
                if (partyContactMechanism.UseAsDefault)
                {
                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).BillingAddress))
                    {
                        @this.DerivedRoles.BillingAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).BillingInquiriesFax))
                    {
                        @this.DerivedRoles.BillingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).BillingInquiriesPhone))
                    {
                        @this.DerivedRoles.BillingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).MobilePhoneNumber))
                    {
                        @this.DerivedRoles.CellPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralCorrespondence))
                    {
                        @this.DerivedRoles.GeneralCorrespondence = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralEmail))
                    {
                        @this.DerivedRoles.GeneralEmail = partyContactMechanism.ContactMechanism as EmailAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralFaxNumber))
                    {
                        @this.DerivedRoles.GeneralFaxNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).GeneralPhoneNumber))
                    {
                        @this.DerivedRoles.GeneralPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).HeadQuarters))
                    {
                        @this.DerivedRoles.HeadQuarter = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).HomeAddress))
                    {
                        @this.DerivedRoles.HomeAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).InternetAddress))
                    {
                        @this.DerivedRoles.InternetAddress = partyContactMechanism.ContactMechanism as ElectronicAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).OrderAddress))
                    {
                        @this.DerivedRoles.OrderAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).OrderInquiriesFax))
                    {
                        @this.DerivedRoles.OrderInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).OrderInquiriesPhone))
                    {
                        @this.DerivedRoles.OrderInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).PersonalEmailAddress))
                    {
                        @this.DerivedRoles.PersonalEmailAddress = partyContactMechanism.ContactMechanism as EmailAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).SalesOffice))
                    {
                        @this.DerivedRoles.SalesOffice = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).ShippingAddress))
                    {
                        @this.DerivedRoles.ShippingAddress = partyContactMechanism.ContactMechanism as PostalAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).ShippingInquiriesFax))
                    {
                        @this.DerivedRoles.ShippingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(@this.Strategy.Session).ShippingInquiriesPhone))
                    {
                        @this.DerivedRoles.ShippingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }
                }

                // Fallback
                if (!@this.ExistBillingAddress && @this.ExistGeneralCorrespondence)
                {
                    @this.DerivedRoles.BillingAddress = @this.GeneralCorrespondence;
                }

                // Fallback
                if (!@this.ExistShippingAddress && @this.GeneralCorrespondence is PostalAddress postalAddress)
                {
                    @this.DerivedRoles.ShippingAddress = postalAddress;
                }
            }

            @this.DerivedRoles.CurrentPartyContactMechanisms = @this.PartyContactMechanisms
                .Where(v => v.FromDate <= @this.Strategy.Session.Now() && (!v.ExistThroughDate || v.ThroughDate >= @this.Strategy.Session.Now()))
                .ToArray();

            @this.DerivedRoles.InactivePartyContactMechanisms = @this.PartyContactMechanisms
                .Except(@this.CurrentPartyContactMechanisms)
                .ToArray();

            var allPartyRelationshipsWhereParty = @this.PartyRelationshipsWhereParty;

            @this.DerivedRoles.CurrentPartyRelationships = allPartyRelationshipsWhereParty
                .Where(v => v.FromDate <= @this.Strategy.Session.Now() && (!v.ExistThroughDate || v.ThroughDate >= @this.Strategy.Session.Now()))
                .ToArray();

            @this.DerivedRoles.InactivePartyRelationships = allPartyRelationshipsWhereParty
                .Except(@this.CurrentPartyRelationships)
                .ToArray();
        }

        public static void BaseOnPostDerive(this Party @this, ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;

            @this.BaseOnDerivePartyFinancialRelationships(derivation);
        }

        public static bool BaseIsActiveCustomer(this Party @this, InternalOrganisation internalOrganisation, DateTime? date)
        {
            if (date == DateTime.MinValue || internalOrganisation == null)
            {
                return false;
            }

            var customerRelationships = @this.CustomerRelationshipsWhereCustomer;
            customerRelationships.Filter.AddEquals(M.CustomerRelationship.InternalOrganisation, internalOrganisation);

            return customerRelationships.Any(relationship => relationship.FromDate.Date <= date
                                                             && (!relationship.ExistThroughDate || relationship.ThroughDate >= date));
        }

        public static CustomerShipment BaseGetPendingCustomerShipmentForStore(this Party @this, PostalAddress address, Store store, ShipmentMethod shipmentMethod)
        {
            var shipments = @this.ShipmentsWhereShipToParty;
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
                if (shipment.ShipmentState.Equals(new ShipmentStates(@this.Strategy.Session).Created) ||
                    shipment.ShipmentState.Equals(new ShipmentStates(@this.Strategy.Session).Picking) ||
                    shipment.ShipmentState.Equals(new ShipmentStates(@this.Strategy.Session).Picked) ||
                    shipment.ShipmentState.Equals(new ShipmentStates(@this.Strategy.Session).OnHold) ||
                    shipment.ShipmentState.Equals(new ShipmentStates(@this.Strategy.Session).Packed))
                {
                    return shipment;
                }
            }

            return null;
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

        public static void DeriveRelationships(this Party @this)
        {
            var now = @this.Session().Now();

            ((PartyDerivedRoles)@this).CurrentPartyContactMechanisms = @this.PartyContactMechanisms
                .Where(v => v.FromDate <= now && (!v.ExistThroughDate || v.ThroughDate >= now))
                .ToArray();

            ((PartyDerivedRoles)@this).InactivePartyContactMechanisms = @this.PartyContactMechanisms
                .Except(@this.CurrentPartyContactMechanisms)
                .ToArray();

            var allPartyRelationshipsWhereParty = @this.PartyRelationshipsWhereParty;

            ((PartyDerivedRoles)@this).CurrentPartyRelationships = allPartyRelationshipsWhereParty
                .Where(v => v.FromDate <= now && (!v.ExistThroughDate || v.ThroughDate >= now))
                .ToArray();

            ((PartyDerivedRoles)@this).InactivePartyRelationships = allPartyRelationshipsWhereParty
                .Except(@this.CurrentPartyRelationships)
                .ToArray();
        }
    }
}
