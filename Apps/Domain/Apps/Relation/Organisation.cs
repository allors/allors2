// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Organisation.cs" company="Allors bvba">
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
using System.Collections.Generic;

namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class Organisation
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistLocale)
            {
                this.Locale = Singleton.Instance(this.Strategy.Session).DefaultLocale;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.PartyName = this.Name;

            this.AppsOnDeriveCurrentContacts(derivation);
            this.AppsOnDeriveInactiveContacts(derivation);
            this.AppsOnDeriveCurrentOrganisationContactRelationships(derivation);
            this.AppsOnDeriveInactiveOrganisationContactRelationships(derivation);
            this.AppsOnDeriveCurrentPartyContactMechanisms(derivation);
            this.AppsOnDeriveInactivePartyContactMechanisms(derivation);
        }

        public ClientRelationship ClientRelationShip(InternalOrganisation internalOrganisation)
        {
            var relationships = this.ClientRelationshipsWhereClient;
            relationships.Filter.AddEquals(M.ClientRelationship.InternalOrganisation, internalOrganisation);

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

        public CustomerRelationship CustomerRelationship(InternalOrganisation internalOrganisation)
        {
            var relationships = this.CustomerRelationshipsWhereCustomer;
            relationships.Filter.AddEquals(M.CustomerRelationship.InternalOrganisation, internalOrganisation);

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
                if (relationship.FromDate.Date <= date &&
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
            relationships.Filter.AddEquals(M.DistributionChannelRelationship.InternalOrganisation, internalOrganisation);

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
                if (partnership.FromDate.Date <= date &&
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
            relationships.Filter.AddEquals(M.Partnership.InternalOrganisation, internalOrganisation);

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
                if (relationship.FromDate.Date <= date &&
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
            relationships.Filter.AddEquals(M.ProspectRelationship.InternalOrganisation, internalOrganisation);

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
                if (relationship.FromDate.Date <= date &&
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
            relationships.Filter.AddEquals(M.SupplierRelationship.InternalOrganisation, internalOrganisation);

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

        public bool IsPerson => false;

        public bool IsOrganisation => true;

        public bool IsDeletable => this.CurrentContacts.Count == 0;

        public void AppsDelete(DeletableDelete method)
        {
            if (this.IsDeletable)
            {
                foreach (PartyContactMechanism partyContactMechanism in this.PartyContactMechanisms)
                {
                    partyContactMechanism.ContactMechanism.Delete();
                    partyContactMechanism.Delete();
                }

                foreach (OrganisationContactRelationship organisationContactRelationship in this.OrganisationContactRelationshipsWhereOrganisation)
                {
                    organisationContactRelationship.Contact.Delete();
                }
            }
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

                if (this.AppsIsActiveDistributor(DateTime.UtcNow.Date))
                {
                    roles.Add("Distributor");
                }

                if (this.AppsIsActivePartner(DateTime.UtcNow.Date))
                {
                    roles.Add("Partner");
                }

                if (this.AppsIsActiveProfessionalServicesProvider(DateTime.UtcNow.Date))
                {
                    roles.Add("Professional Service Provider");
                }

                if (this.AppsIsActiveProspect(DateTime.UtcNow.Date))
                {
                    roles.Add("Prospect");
                }

                if (this.AppsIsActiveSubContractor(DateTime.UtcNow.Date))
                {
                    roles.Add("Subcontractor");
                }

                if (this.AppsIsActiveSupplier(DateTime.UtcNow.Date))
                {
                    roles.Add("Supplier");
                }

                return roles;
            }            
        }
    }
}