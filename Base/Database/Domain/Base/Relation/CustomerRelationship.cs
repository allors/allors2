// <copyright file="CustomerRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public partial class CustomerRelationship
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (this.ExistCustomer)
                {
                    iteration.AddDependency(this.Customer, this);
                    iteration.Mark(this.Customer);

                    if (this.Customer is Organisation customer)
                    {
                        foreach (OrganisationContactRelationship contactRelationship in customer.OrganisationContactRelationshipsWhereOrganisation)
                        {
                            iteration.AddDependency(contactRelationship, this);
                            iteration.Mark(contactRelationship);
                        }
                    }
                }

                if (this.ExistInternalOrganisation)
                {
                    iteration.AddDependency(this.InternalOrganisation, this);
                    iteration.Mark(this.InternalOrganisation);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistInternalOrganisation)
            {
                var internalOrganisations = new Organisations(this.Strategy.Session).InternalOrganisations();

                if (internalOrganisations.Count() == 1)
                {
                    this.InternalOrganisation = internalOrganisations.Single();
                }
            }

            if (this.ExistCustomer)
            {
                if (this.Customer is Organisation customerOrganisation && customerOrganisation.ExistContactsUserGroup)
                {
                    foreach (Person contact in customerOrganisation.ContactsUserGroup.Members)
                    {
                        customerOrganisation.ContactsUserGroup.RemoveMember(contact);
                    }

                    if (this.FromDate <= this.Session().Now() && (!this.ExistThroughDate || this.ThroughDate >= this.Session().Now()))
                    {
                        foreach (Person currentContact in customerOrganisation.CurrentContacts)
                        {
                            customerOrganisation.ContactsUserGroup.AddMember(currentContact);
                        }
                    }
                }

                // HACK: DerivedRoles
                var internalOrganisationDerivedRoles = (OrganisationDerivedRoles)this.InternalOrganisation;

                if (this.FromDate <= this.Session().Now() && (!this.ExistThroughDate || this.ThroughDate >= this.Session().Now()))
                {
                    internalOrganisationDerivedRoles.AddActiveCustomer(this.Customer);
                }

                if (this.FromDate > this.Session().Now() || (this.ExistThroughDate && this.ThroughDate < this.Session().Now()))
                {
                    internalOrganisationDerivedRoles.RemoveActiveCustomer(this.Customer);
                }
            }

            this.Parties = new Party[] { this.Customer, this.InternalOrganisation };
        }
    }
}
