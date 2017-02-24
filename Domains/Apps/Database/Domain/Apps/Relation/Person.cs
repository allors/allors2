// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public partial class Person
    {
        public bool IsPerson => true;

        public bool IsOrganisation => false;

        public bool AppsIsActiveEmployee(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var employments = this.EmploymentsWhereEmployee;
            foreach (Employment relationship in employments)
            {
                if (relationship.FromDate.Date <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public bool AppsIsActiveOrganisationContact(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var organisationContactRelationships = this.OrganisationContactRelationshipsWhereContact;
            foreach (OrganisationContactRelationship relationship in organisationContactRelationships)
            {
                if (relationship.FromDate.Date<= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public bool AppsIsActiveSalesRep(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            var salesRepRelationships = this.SalesRepRelationshipsWhereSalesRepresentative;
            foreach (SalesRepRelationship relationship in salesRepRelationships)
            {
                if (relationship.FromDate.Date <= date &&
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

                if (this.AppsIsActiveClient(DateTime.UtcNow.Date))
                {
                    roles.Add("Client");
                }

                if (this.AppsIsActiveCustomer(DateTime.UtcNow.Date))
                {
                    roles.Add("Customer");
                }

                if (this.AppsIsActiveEmployee(DateTime.UtcNow.Date))
                {
                    roles.Add("Employee");
                }

                if (this.AppsIsActiveOrganisationContact(DateTime.UtcNow.Date))
                {
                    roles.Add("Organisation contact");
                }

                if (this.AppsIsActiveSalesRep(DateTime.UtcNow.Date))
                {
                    roles.Add("Sales representative");
                }

                if (this.AppsIsActiveProspect(DateTime.UtcNow.Date))
                {
                    roles.Add("Prospect");
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

            // var contactRelationships = this.OrganisationContactRelationshipsWhereContact;
            // contactRelationships.Filter.AddLessThan(OrganisationContactRelationships.Meta.FromDate, date.AddDays(1));
            // var or1 = contactRelationships.Filter.AddOr();
            // or1.AddNot().AddExists(PartyRelationships.Meta.ThroughDate);
            // or1.AddGreaterThan(PartyRelationships.Meta.ThroughDate, date.AddDays(-1));

            // foreach (OrganisationContactRelationship contactRelationship in contactRelationships)
            // {
            // var customerRelationships = contactRelationship.Organisation.CustomerRelationshipsWhereCustomer;
            // customerRelationships.Filter.AddLessThan(M.CustomerRelationship.FromDate, date.AddDays(1));
            // var or2 = contactRelationships.Filter.AddOr();
            // or2.AddNot().AddExists(PartyRelationships.Meta.ThroughDate);
            // or2.AddGreaterThan(PartyRelationships.Meta.ThroughDate, date.AddDays(-1));

            // if (customerRelationships.Count > 0)
            // {
            // return true;
            // }
            // }
            var contactRelationships = this.OrganisationContactRelationshipsWhereContact;
            foreach (OrganisationContactRelationship relationship in contactRelationships)
            {
                if (relationship.FromDate <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
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

            this.PartyName = this.DerivePartyName();
            this.AppsOnDeriveCurrentEmployment(derivation);
            this.AppsOnDeriveCurrentContacts(derivation);
            this.AppsOnDeriveInactiveContacts(derivation);
            this.AppsOnDeriveCurrentOrganisationContactRelationships(derivation);
            this.AppsOnDeriveInactiveOrganisationContactRelationships(derivation);
            this.AppsOnDeriveCurrentPartyContactMechanisms(derivation);
            this.AppsOnDeriveInactivePartyContactMechanisms(derivation);
            this.AppsOnDeriveCommission();
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

        public bool IsDeletable => !this.IsActiveContact(DateTime.UtcNow);

        public void AppsDelete(DeletableDelete method)
        {
            if (this.IsDeletable)
            {
                foreach (PartyContactMechanism partyContactMechanism in this.PartyContactMechanisms)
                {
                    partyContactMechanism.ContactMechanism.Delete();
                    partyContactMechanism.Delete();
                }

                if (this.ExistOwnerAccessControl)
                {
                    this.OwnerAccessControl.Delete();
                }

                if (this.ExistOwnerSecurityToken)
                {
                    this.OwnerSecurityToken.Delete();
                }
            }
        }

        public void AppsOnDeriveCurrentEmployment(IDerivation derivation)
        {
            InternalOrganisation previousEmployer = null;

            if (this.ExistCurrentEmployment)
            {
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
        }
    }
}