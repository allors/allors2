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

using System.Linq;

namespace Allors.Domain
{
    using System;

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
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistInternalOrganisation && internalOrganisations.Count() == 1)
            {
                this.InternalOrganisation = internalOrganisations.First();
            }

            if (this.ExistCustomer)
            {
                var customerOrganisation = this.Customer as Organisation;
                if (customerOrganisation != null && customerOrganisation.ExistContactsUserGroup)
                {
                    foreach (Person contact in customerOrganisation.ContactsUserGroup.Members)
                    {
                        customerOrganisation.ContactsUserGroup.RemoveMember(contact);
                    }

                    if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                    {
                        foreach (Person currentContact in customerOrganisation.CurrentContacts)
                        {
                            customerOrganisation.ContactsUserGroup.AddMember(currentContact);
                        }
                    }
                }

                if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                {
                    this.InternalOrganisation.AddActiveCustomer(this.Customer);
                }

                if (this.FromDate > DateTime.UtcNow || (this.ExistThroughDate && this.ThroughDate < DateTime.UtcNow))
                {
                    this.InternalOrganisation.RemoveActiveCustomer(this.Customer);
                }
            }

            this.Parties = new Party[] { this.Customer, this.InternalOrganisation };
        }
    }
}