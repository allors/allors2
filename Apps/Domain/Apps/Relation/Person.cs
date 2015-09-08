    // --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="Allors bvba">
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
    using System.Text;

    public partial class Person
    {
        public bool IsPerson
        {
            get
            {
                return true;
            }
        }

        public bool IsOrganisation
        {
            get
            {
                return false;
            }
        }

        private bool AppsIsActiveClient(DateTime? date)
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

        private bool AppsIsActiveCustomer(DateTime? date)
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

        private bool AppsIsActiveEmployee(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var employments = this.EmploymentsWhereEmployee;
            foreach (Employment relationship in employments)
            {
                if (relationship.FromDate <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        private bool AppsIsActiveOrganisationContact(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var organisationContactRelationships = this.OrganisationContactRelationshipsWhereContact;
            foreach (OrganisationContactRelationship relationship in organisationContactRelationships)
            {
                if (relationship.FromDate <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        private bool AppsIsActiveSalesRep(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var salesRepRelationships = this.SalesRepRelationshipsWhereSalesRepresentative;
            foreach (SalesRepRelationship relationship in salesRepRelationships)
            {
                if (relationship.FromDate <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        private bool AppsIsActiveProspect(DateTime? date)
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

        private bool AppsIsActiveSubContractor(DateTime? date)
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

                if (IsActiveEmployee(DateTime.UtcNow.Date))
                {
                    roles.Add("Employee");
                }

                if (IsActiveOrganisationContact(DateTime.UtcNow.Date))
                {
                    roles.Add("Organisation contact");
                }

                if (IsActiveSalesRep(DateTime.UtcNow.Date))
                {
                    roles.Add("Sales representative");
                }

                if (IsActiveProspect(DateTime.UtcNow.Date))
                {
                    roles.Add("Prospect");
                }

                if (IsActiveSubContractor(DateTime.UtcNow.Date))
                {
                    roles.Add("Subcontractor");
                }

                return roles;
            }            
        }

        public bool IsActiveContact(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var contactRelationships = this.OrganisationContactRelationshipsWhereContact;
            contactRelationships.Filter.AddLessThan(OrganisationContactRelationships.Meta.FromDate, date.AddDays(1));
            var or1 = contactRelationships.Filter.AddOr();
            or1.AddNot().AddExists(PartyRelationships.Meta.ThroughDate);
            or1.AddGreaterThan(PartyRelationships.Meta.ThroughDate, date.AddDays(-1));

            foreach (OrganisationContactRelationship contactRelationship in contactRelationships)
            {
                var customerRelationships = contactRelationship.Organisation.CustomerRelationshipsWhereCustomer;
                customerRelationships.Filter.AddLessThan(CustomerRelationships.Meta.FromDate, date.AddDays(1));
                var or2 = contactRelationships.Filter.AddOr();
                or2.AddNot().AddExists(PartyRelationships.Meta.ThroughDate);
                or2.AddGreaterThan(PartyRelationships.Meta.ThroughDate, date.AddDays(-1));

                if (customerRelationships.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistClientRelationshipsWhereClient)
                {
                    foreach (ClientRelationship relationship in this.ClientRelationshipsWhereClient)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (CustomerRelationship relationship in this.CustomerRelationshipsWhereCustomer)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (Employment relationship in this.EmploymentsWhereEmployee)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (OrganisationContactRelationship relationship in this.OrganisationContactRelationshipsWhereContact)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (ProfessionalServicesRelationship relationship in this.ProfessionalServicesRelationshipsWhereProfessional)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (SalesRepRelationship relationship in this.SalesRepRelationshipsWhereSalesRepresentative)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (SubContractorRelationship relationship in this.SubContractorRelationshipsWhereContractor)
                    {
                        derivation.AddDependency(relationship, this);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

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
                if (partyContactMechanism.UseAsDefault)
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
                        this.ShippingAddress = partyContactMechanism.ContactMechanism as Domain.PostalAddress;
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
                        continue;
                    }
                }
            }

            this.PartyName = this.DerivePartyName();
            this.DeriveCurrentEmployment(derivation);
            this.AppsOnDeriveCurrentContacts(derivation);
            this.AppsOnDeriveInactiveContacts(derivation);
            this.AppsOnDeriveCurrentOrganisationContactRelationships(derivation);
            this.AppsOnDeriveInactiveOrganisationContactRelationships(derivation);
            this.AppsOnDeriveCurrentPartyContactMechanisms(derivation);
            this.AppsOnDeriveInactivePartyContactMechanisms(derivation);
            this.DeriveCommission();
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            this.RemoveSecurityTokens();
            this.AddSecurityToken(this.OwnerSecurityToken);
            this.AddSecurityToken(Singleton.Instance(this.Strategy.Session).AdministratorSecurityToken);

            foreach (Organisation organisation in this.PartiesWhereCurrentContact)
            {
                this.AddSecurityToken(organisation.OwnerSecurityToken);
            }

            if (this.ExistCurrentEmployment)
            {
                this.AddSecurityToken(this.CurrentEmployment.Employer.OwnerSecurityToken);
            }

            if (this.ExistOrganisationContactRelationshipsWhereContact)
            {
                foreach (OrganisationContactRelationship organisationContactRelationship in OrganisationContactRelationshipsWhereContact)
                {
                    if (organisationContactRelationship.ExistOrganisation)
                    {
                        foreach (CustomerRelationship customerRelationship in organisationContactRelationship.Organisation.CustomerRelationshipsWhereCustomer)
                        {
                            this.AddSecurityToken(customerRelationship.InternalOrganisation.OwnerSecurityToken);
                        }
                    }
                }
            }

            foreach (CustomerRelationship customerRelationship in this.CustomerRelationshipsWhereCustomer)
            {
                this.AddSecurityToken(customerRelationship.InternalOrganisation.OwnerSecurityToken);
            }
        }

        public void AppsOnDeriveCurrentContacts(IDerivation derivation)
        {
            this.RemoveCurrentContacts();
        }

        public void AppsOnDeriveInactiveContacts(IDerivation derivation)
        {
            this.RemoveInactiveContacts();
        }

        public void AppsOnDeriveCurrentOrganisationContactRelationships(IDerivation derivation)
        {
            this.RemoveCurrentOrganisationContactRelationships();
        }

        public void AppsOnDeriveInactiveOrganisationContactRelationships(IDerivation derivation)
        {
            this.RemoveInactiveOrganisationContactRelationships();
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

        public void AppsOnDeriveCommission()
        {
            this.YTDCommission = 0;
            this.LastYearsCommission = 0;

            foreach (SalesRepCommission salesRepCommission in this.SalesRepCommissionsWhereSalesRep)
            {
                if (salesRepCommission.Commission.HasValue)
                {
                    if (salesRepCommission.Year == DateTime.UtcNow.Year)
                    {
                        this.YTDCommission += salesRepCommission.Commission.Value;
                    }

                    if (salesRepCommission.Year == DateTime.UtcNow.AddYears(-1).Year)
                    {
                        this.LastYearsCommission += salesRepCommission.Commission.Value;
                    }
                }
            }
        }

        private string DerivePartyName()
        {
            var partyName = new StringBuilder();

            if (this.ExistFirstName)
            {
                partyName.Append(this.FirstName);
            }

            if (this.ExistMiddleName)
            {
                if (partyName.Length > 0)
                {
                    partyName.Append(" ");
                }

                partyName.Append(this.MiddleName);
            }

            if (this.ExistLastName)
            {
                if (partyName.Length > 0)
                {
                    partyName.Append(" ");
                }

                partyName.Append(this.LastName);
            }

            return partyName.ToString(); 
        }

        public void AppsBuildOwnerSecurityToken()
        {
            if (!this.ExistOwnerSecurityToken)
            {
                var mySecurityToken = new SecurityTokenBuilder(this.Strategy.Session).Build();
                this.OwnerSecurityToken = mySecurityToken;

                //// In case person does not belong to any group (i.e. private person placing an order) this person should be granted customer permissions.
                if (!this.ExistAccessControlsWhereSubject)
                {
                    var customerRole = new Roles(this.Strategy.Session).Customer;
                    new AccessControlBuilder(this.Strategy.Session)
                        .WithRole(customerRole)
                        .WithSubject(this)
                        .WithObject(this.OwnerSecurityToken)
                        .Build();

                    var ownerRole = new Roles(this.Strategy.Session).Owner;
                    new AccessControlBuilder(this.Strategy.Session)
                        .WithRole(ownerRole)
                        .WithSubject(this)
                        .WithObject(this.OwnerSecurityToken)
                        .Build();
                }
            }
        }

        public void AppsDelete(DeletableDelete method)
        {
            foreach (Employment employment in this.EmploymentsWhereEmployee)
            {
                employment.Delete();
            }

            if (this.ExistOwnerSecurityToken)
            {
                foreach (AccessControl acl in this.OwnerSecurityToken.AccessControlsWhereObject)
                {
                    acl.Delete();
                }

                this.OwnerSecurityToken.Delete();
            }
        }

        public void AppsOnDeriveCurrentEmployment(IDerivation derivation)
        {
            var usergroupsWhereMember = new List<UserGroup>();
            InternalOrganisation previousEmployer = null;
            if (this.ExistCurrentEmployment)
            {
                foreach (UserGroup userGroup in this.CurrentEmployment.Employer.UserGroupsWhereParty)
                {
                    if (userGroup.ContainsMember(this))
                    {
                        usergroupsWhereMember.Add(userGroup);
                    }
                }

                previousEmployer = this.CurrentEmployment.Employer;
                this.RemoveCurrentEmployment();
            }

            var employments = this.EmploymentsWhereEmployee;
            foreach (Employment employment in employments)
            {
                if (employment.ExistEmployer &&
                    employment.FromDate <= DateTime.UtcNow && (!employment.ExistThroughDate || employment.ThroughDate >= DateTime.UtcNow))
                {
                    this.CurrentEmployment = employment;
                }
            }

            if (!this.ExistCurrentEmployment || (previousEmployer != null && !this.CurrentEmployment.Employer.Equals(previousEmployer)))
            {
                foreach (var userGroup in usergroupsWhereMember)
                {
                    userGroup.RemoveMember(this);
                }
            }
        }
    }
}