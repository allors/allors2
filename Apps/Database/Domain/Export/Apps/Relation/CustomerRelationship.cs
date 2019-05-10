// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerRelationship.cs" company="Allors bvba">
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

using Allors.Meta;

namespace Allors.Domain
{
    using System;
    using System.Linq;

    public partial class CustomerRelationship
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
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

        public void AppsOnDerive(ObjectOnDerive method)
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