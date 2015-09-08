// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Organisation.cs" company="Allors bvba">
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

using System.Collections.Generic;

namespace Allors.Domain
{
    using System;

    public partial class Organisation
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            

            if (!this.ExistLocale)
            {
                this.Locale = Singleton.Instance(this.Strategy.Session).DefaultLocale;
            }
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            this.RemoveSecurityTokens();
            this.AddSecurityToken(this.OwnerSecurityToken);
            this.AddSecurityToken(Singleton.Instance(this.Strategy.Session).AdministratorSecurityToken);

            foreach (CustomerRelationship customerRelationship in this.CustomerRelationshipsWhereCustomer)
            {
                this.AddSecurityToken(customerRelationship.InternalOrganisation.OwnerSecurityToken);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.PartyName = this.Name;

            if (!this.ExistOwnerSecurityToken)
            {
                var securityToken = new SecurityTokenBuilder(this.Strategy.Session).Build();
                this.OwnerSecurityToken = securityToken;

                this.AddSecurityToken(this.OwnerSecurityToken);
            }

            this.BillingAddress = null;
            this.BillingInquiriesFax = null;
            this.BillingInquiriesPhone = null;
            this.CellPhoneNumber = null;
            this.GeneralFaxNumber = null;
            this.GeneralPhoneNumber = null;
            this.HeadQuarter = null;
            this.HomeAddress = null;
            this.InternetAddress = null;
            this.OrderAddress = null;
            this.OrderInquiriesFax = null;
            this.OrderInquiriesPhone = null;
            this.PersonalEmailAddress = null;
            this.SalesOffice = null;
            this.ShippingAddress = null;
            this.ShippingInquiriesFax = null;
            this.ShippingAddress = null;

            foreach (PartyContactMechanism partyContactMechanism in this.PartyContactMechanisms)
            {
                if (partyContactMechanism.UseAsDefault && partyContactMechanism.ExistContactPurpose)
                {
                    if (partyContactMechanism.ContactPurpose.IsBillingAddress)
                    {
                        this.BillingAddress = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsBillingInquiriesFax)
                    {
                        this.BillingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsBillingInquiriesPhone)
                    {
                        this.BillingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsCellPhoneNumber)
                    {
                        this.CellPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsGeneralCorrespondence)
                    {
                        this.GeneralCorrespondence = partyContactMechanism.ContactMechanism as PostalAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsGeneralEmail)
                    {
                        this.GeneralEmail = partyContactMechanism.ContactMechanism as ElectronicAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsGeneralFaxNumber)
                    {
                        this.GeneralFaxNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsGeneralPhoneNumber)
                    {
                        this.GeneralPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsHeadQuarters)
                    {
                        this.HeadQuarter = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsHomeAddress)
                    {
                        this.HomeAddress = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsInternetAddress)
                    {
                        this.InternetAddress = partyContactMechanism.ContactMechanism as ElectronicAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsOrderAddress)
                    {
                        this.OrderAddress = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsOrderInquiriesFax)
                    {
                        this.OrderInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsOrderInquiriesPhone)
                    {
                        this.OrderInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsPersonalEmailAddress)
                    {
                        this.PersonalEmailAddress = partyContactMechanism.ContactMechanism as ElectronicAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsSalesOffice)
                    {
                        this.SalesOffice = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsShippingAddress)
                    {
                        this.ShippingAddress = partyContactMechanism.ContactMechanism as PostalAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsShippingInquiriesFax)
                    {
                        this.ShippingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsShippingInquiriesPhone)
                    {
                        this.ShippingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }
                }
            }

            this.DeriveUserGroups(derivation);
            this.AppsOnDeriveCurrentContacts(derivation);
            this.AppsOnDeriveInactiveContacts(derivation);
            this.AppsOnDeriveCurrentOrganisationContactRelationships(derivation);
            this.AppsOnDeriveInactiveOrganisationContactRelationships(derivation);
            this.AppsOnDeriveCurrentPartyContactMechanisms(derivation);
            this.AppsOnDeriveInactivePartyContactMechanisms(derivation);
        }
        
        public bool AppsIsActiveClient(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var clientRelationships = this.ClientRelationshipsWhereClient;
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

        public ClientRelationship ClientRelationShip(InternalOrganisation internalOrganisation)
        {
            var relationships = this.ClientRelationshipsWhereClient;
            relationships.Filter.AddEquals(ClientRelationships.Meta.InternalOrganisation, internalOrganisation);

            foreach (ClientRelationship relationship in relationships)
            {
                if (relationship.FromDate <= DateTime.Now &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= DateTime.Now))
                {
                    return relationship;
                }
            }

            return null;
        }

        public bool AppsIsActiveCustomer(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var customerRelationships = this.CustomerRelationshipsWhereCustomer;
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

        public CustomerRelationship CustomerRelationship(InternalOrganisation internalOrganisation)
        {
            var relationships = this.CustomerRelationshipsWhereCustomer;
            relationships.Filter.AddEquals(CustomerRelationships.Meta.InternalOrganisation, internalOrganisation);

            foreach (CustomerRelationship relationship in relationships)
            {
                if (relationship.FromDate <= DateTime.Now &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= DateTime.Now))
                {
                    return relationship;
                }
            }

            return null;
        }

        public bool AppsIsActiveDistributor(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var distributorRelationships = this.DistributionChannelRelationshipsWhereDistributor;
            foreach (DistributionChannelRelationship relationship in distributorRelationships)
            {
                if (relationship.FromDate <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public DistributionChannelRelationship DistributionChannelRelationship(InternalOrganisation internalOrganisation)
        {
            var relationships = this.DistributionChannelRelationshipsWhereDistributor;
            relationships.Filter.AddEquals(DistributionChannelRelationships.Meta.InternalOrganisation, internalOrganisation);

            foreach (DistributionChannelRelationship relationship in relationships)
            {
                if (relationship.FromDate <= DateTime.Now &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= DateTime.Now))
                {
                    return relationship;
                }
            }

            return null;
        }

        public bool AppsIsActivePartner(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var partnerships = this.PartnershipsWherePartner;
            foreach (Partnership partnership in partnerships)
            {
                if (partnership.FromDate <= date &&
                    (!partnership.ExistThroughDate || partnership.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public Partnership Partnership(InternalOrganisation internalOrganisation)
        {
            var relationships = this.PartnershipsWherePartner;
            relationships.Filter.AddEquals(Partnerships.Meta.InternalOrganisation, internalOrganisation);

            foreach (Partnership relationship in relationships)
            {
                if (relationship.FromDate <= DateTime.Now &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= DateTime.Now))
                {
                    return relationship;
                }
            }

            return null;
        }

        public bool AppsIsActiveProfessionalServicesProvider(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var professionalServicesRelationships = this.ProfessionalServicesRelationshipsWhereProfessionalServicesProvider;
            foreach (ProfessionalServicesRelationship relationship in professionalServicesRelationships)
            {
                if (relationship.FromDate <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public bool AppsIsActiveProspect(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var prospectRelationships = this.ProspectRelationshipsWhereProspect;
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

        public ProspectRelationship ProspectRelationship(InternalOrganisation internalOrganisation)
        {
            var relationships = this.ProspectRelationshipsWhereProspect;
            relationships.Filter.AddEquals(ProspectRelationships.Meta.InternalOrganisation, internalOrganisation);

            foreach (ProspectRelationship relationship in relationships)
            {
                if (relationship.FromDate <= DateTime.Now &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= DateTime.Now))
                {
                    return relationship;
                }
            }

            return null;
        }

        public bool AppsIsActiveSubContractor(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var subContractorRelationships = this.SubContractorRelationshipsWhereSubContractor;
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

        public bool AppsIsActiveSupplier(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var supplierRelationships = this.SupplierRelationshipsWhereSupplier;
            foreach (SupplierRelationship relationship in supplierRelationships)
            {
                if (relationship.FromDate <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public SupplierRelationship SupplierRelationship(InternalOrganisation internalOrganisation)
        {
            var relationships = this.SupplierRelationshipsWhereSupplier;
            relationships.Filter.AddEquals(SupplierRelationships.Meta.InternalOrganisation, internalOrganisation);

            foreach (SupplierRelationship relationship in relationships)
            {
                if (relationship.FromDate <= DateTime.Now &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= DateTime.Now))
                {
                    return relationship;
                }
            }

            return null;
        }

        public void AppsOnDeriveUserGroups(IDerivation derivation)
        {
            var customerContactGroupName = string.Format("Customer contacts at {0} ({1})", this.Name, this.UniqueId);
            var supplierContactGroupName = string.Format("Supplier contacts at {0} ({1})", this.Name, this.UniqueId);
            var partnerContactGroupName = string.Format("Partner contacts at {0} ({1})", this.Name, this.UniqueId);

            var customerContactGroupFound = false;
            var supplierContactGroupFound = false;
            var partnerContactGroupFound = false;

            foreach (UserGroup userGroup in this.UserGroupsWhereParty)
            {
                if (userGroup.Name == customerContactGroupName)
                {
                    customerContactGroupFound = true;
                }

                if (userGroup.Name == supplierContactGroupName)
                {
                    supplierContactGroupFound = true;
                }

                if (userGroup.Name == partnerContactGroupName)
                {
                    partnerContactGroupFound = true;
                }
            }

            if (!customerContactGroupFound)
            {
                this.CustomerContactUserGroup = new UserGroupBuilder(this.Strategy.Session)
                    .WithName(customerContactGroupName)
                    .WithParty(this)
                    .WithParent(new UserGroups(this.Strategy.Session).Customers)
                    .Build();

                new AccessControlBuilder(this.Strategy.Session).WithRole(new Roles(this.Strategy.Session).Customer).WithSubjectGroup(this.CustomerContactUserGroup).WithObject(this.OwnerSecurityToken).Build();
            }

            if (!supplierContactGroupFound)
            {
                this.SupplierContactUserGroup = new UserGroupBuilder(this.Strategy.Session)
                    .WithName(supplierContactGroupName)
                    .WithParty(this)
                    .WithParent(new UserGroups(this.Strategy.Session).Suppliers)
                    .Build();

                new AccessControlBuilder(this.Strategy.Session).WithRole(new Roles(this.Strategy.Session).Supplier).WithSubjectGroup(this.SupplierContactUserGroup).WithObject(this.OwnerSecurityToken).Build();
            }

            if (!partnerContactGroupFound)
            {
                this.PartnerContactUserGroup = new UserGroupBuilder(this.Strategy.Session)
                    .WithName(partnerContactGroupName)
                    .WithParty(this)
                    .WithParent(new UserGroups(this.Strategy.Session).Partners)
                    .Build();

                new AccessControlBuilder(this.Strategy.Session).WithRole(new Roles(this.Strategy.Session).Partner).WithSubjectGroup(this.PartnerContactUserGroup).WithObject(this.OwnerSecurityToken).Build();
            }
        }

        public void AppsOnDeriveCurrentContacts(IDerivation derivation)
        {
            this.RemoveCurrentContacts();

            var contactRelationships = this.OrganisationContactRelationshipsWhereOrganisation;
            foreach (OrganisationContactRelationship contactRelationship in contactRelationships)
            {
                if (contactRelationship.FromDate <= DateTime.UtcNow &&
                    (!contactRelationship.ExistThroughDate || contactRelationship.ThroughDate >= DateTime.UtcNow))
                {
                    this.AddCurrentContact(contactRelationship.Contact);
                }
            }
        }

        public void AppsOnDeriveInactiveContacts(IDerivation derivation)
        {
            this.RemoveInactiveContacts();

            var contactRelationships = this.OrganisationContactRelationshipsWhereOrganisation;
            foreach (OrganisationContactRelationship contactRelationship in contactRelationships)
            {
                if (contactRelationship.FromDate > DateTime.UtcNow ||
                    (contactRelationship.ExistThroughDate && contactRelationship.ThroughDate < DateTime.UtcNow))
                {
                    this.AddInactiveContact(contactRelationship.Contact);
                }
            }
        }

        public void AppsOnDeriveCurrentOrganisationContactRelationships(IDerivation derivation)
        {
            this.RemoveCurrentOrganisationContactRelationships();

            foreach (OrganisationContactRelationship organisationContactRelationship in this.OrganisationContactRelationshipsWhereOrganisation)
            {
                if (organisationContactRelationship.FromDate <= DateTime.UtcNow &&
                    (!organisationContactRelationship.ExistThroughDate || organisationContactRelationship.ThroughDate >= DateTime.UtcNow))
                {
                    this.AddCurrentOrganisationContactRelationship(organisationContactRelationship);
                }
            }
        }

        public void AppsOnDeriveInactiveOrganisationContactRelationships(IDerivation derivation)
        {
            this.RemoveInactiveOrganisationContactRelationships();

            foreach (OrganisationContactRelationship organisationContactRelationship in this.OrganisationContactRelationshipsWhereOrganisation)
            {
                if (organisationContactRelationship.FromDate > DateTime.UtcNow ||
                    (organisationContactRelationship.ExistThroughDate && organisationContactRelationship.ThroughDate < DateTime.UtcNow))
                {
                    this.AddInactiveOrganisationContactRelationship(organisationContactRelationship);
                }
            }
        }

        public void AppsOnDeriveCurrentPartyContactMechanisms(IDerivation derivation)
        {
            this.RemoveCurrentPartyContactMechanisms();

            foreach (PartyContactMechanism partyContactMechanism in this.PartyContactMechanisms)
            {
                if (partyContactMechanism.FromDate <= DateTime.UtcNow &&
                    (!partyContactMechanism.ExistThroughDate || partyContactMechanism.ThroughDate >= DateTime.UtcNow))
                {
                    this.AddCurrentPartyContactMechanism(partyContactMechanism);
                }
            }
        }

        public void AppsOnDeriveInactivePartyContactMechanisms(IDerivation derivation)
        {
            this.RemoveInactivePartyContactMechanisms();

            foreach (PartyContactMechanism partyContactMechanism in this.PartyContactMechanisms)
            {
                if (partyContactMechanism.FromDate > DateTime.UtcNow ||
                    (partyContactMechanism.ExistThroughDate && partyContactMechanism.ThroughDate < DateTime.UtcNow))
                {
                    this.AddInactivePartyContactMechanism(partyContactMechanism);
                }
            }
        }

        public bool IsPerson {
            get
            {
                return false;
            }
        }

        public bool IsOrganisation {
            get
            {
                return true;
            }
        }

        public List<string> Roles
        {
            get
            {
                var roles = new List<string>();

                if (IsActiveClient(DateTime.UtcNow.Date))
                {
                    roles.Add("Client");
                }

                if (IsActiveCustomer(DateTime.UtcNow.Date))
                {
                    roles.Add("Customer");
                }

                if (IsActiveDistributor(DateTime.UtcNow.Date))
                {
                    roles.Add("Distributor");
                }

                if (IsActivePartner(DateTime.UtcNow.Date))
                {
                    roles.Add("Partner");
                }

                if (IsActiveProfessionalServicesProvider(DateTime.UtcNow.Date))
                {
                    roles.Add("Professional Service Provider");
                }

                if (IsActiveProspect(DateTime.UtcNow.Date))
                {
                    roles.Add("Prospect");
                }

                if (IsActiveSubContractor(DateTime.UtcNow.Date))
                {
                    roles.Add("Subcontractor");
                }

                if (IsActiveSupplier(DateTime.UtcNow.Date))
                {
                    roles.Add("Supplier");
                }

                return roles;
            }            
        }
    }
}