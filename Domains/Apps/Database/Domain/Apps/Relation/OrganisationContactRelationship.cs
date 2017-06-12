// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationContactRelationship.cs" company="Allors bvba">
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

    public partial class OrganisationContactRelationship
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistOrganisation)
            {
                derivation.AddDependency(this, this.Organisation);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.AppsOnDeriveContactMembership(derivation);

            ////Before deriving this.Contact
            if (this.ExistOrganisation)
            {
                this.Organisation.AppsOnDeriveCurrentContacts(derivation);
            }

            ////After deriving this.Organisation
            if (this.ExistContact)
            {
                this.Contact.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Parties = new Party[] { this.Contact, this.Organisation };

            if (!this.ExistContact || !this.ExistOrganisation)
            {
                this.Delete();
            }
        }

        public void AppsOnDeriveContactMembership(IDerivation derivation)
        {
            if (this.ExistContact && this.ExistOrganisation && this.Organisation.ExistContactsUserGroup)
            {
                this.Organisation.ContactsUserGroup.RemoveMember(this.Contact);
                if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                {
                    if (this.Organisation.AppsIsActiveCustomer(this.FromDate) ||
                        this.Organisation.AppsIsActiveSupplier(this.FromDate) ||
                        this.Organisation.AppsIsActiveProfessionalServicesProvider(this.FromDate) ||
                        this.Organisation.AppsIsActiveSubContractor(this.FromDate))
                    {
                        if (!this.Organisation.ContactsUserGroup.Members.Contains(this.Contact))
                        {
                            this.Organisation.ContactsUserGroup.AddMember(this.Contact);
                        }
                    }
                }
            }
        }

        public bool IsActive => this.ExistPartyWhereCurrentOrganisationContactRelationship;
    }
}