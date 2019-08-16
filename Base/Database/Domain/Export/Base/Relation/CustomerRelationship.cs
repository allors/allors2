
// <copyright file="CustomerRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Meta;

namespace Allors.Domain
{
    using System;
    using System.Linq;

    public partial class CustomerRelationship
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistCustomer)
            {
                derivation.AddDependency(this.Customer, this);

                if (this.Customer is Organisation customer)
                {
                    foreach (OrganisationContactRelationship contactRelationship in customer.OrganisationContactRelationshipsWhereOrganisation)
                    {
                        derivation.AddDependency(contactRelationship, this);
                    }
                }
            }

            if (this.ExistInternalOrganisation)
            {
                derivation.AddDependency(this.InternalOrganisation, this);
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

                    if (this.FromDate <= this.strategy.Session.Now() && (!this.ExistThroughDate || this.ThroughDate >= this.strategy.Session.Now()))
                    {
                        foreach (Person currentContact in customerOrganisation.CurrentContacts)
                        {
                            customerOrganisation.ContactsUserGroup.AddMember(currentContact);
                        }
                    }
                }

                if (this.FromDate <= this.strategy.Session.Now() && (!this.ExistThroughDate || this.ThroughDate >= this.strategy.Session.Now()))
                {
                    this.InternalOrganisation.AddActiveCustomer(this.Customer);
                }

                if (this.FromDate > this.strategy.Session.Now() || (this.ExistThroughDate && this.ThroughDate < this.strategy.Session.Now()))
                {
                    this.InternalOrganisation.RemoveActiveCustomer(this.Customer);
                }
            }

            this.Parties = new Party[] { this.Customer, this.InternalOrganisation };
        }
    }
}
