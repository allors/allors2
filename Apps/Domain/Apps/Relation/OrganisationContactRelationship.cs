// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationContactRelationship.cs" company="Allors bvba">
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

    public partial class OrganisationContactRelationship
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistFromDate)
            {
                this.FromDate = DateTime.UtcNow;
            }
        }
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistOrganisation)
            {
                derivation.AddDependency(this.Organisation, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.DeriveCustomerContactMemberShip(derivation);
            this.DeriveSupplierContactMemberShip(derivation);
            this.DerivePartnerContactMemberShip(derivation);

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
        }

        public void AppsOnDeriveCustomerContactMemberShip(IDerivation derivation)
        {
            if (this.ExistContact && this.ExistOrganisation && this.Organisation.ExistCustomerContactUserGroup)
            {
                if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                {
                    if (this.Organisation.IsActiveCustomer(this.FromDate))
                    {
                        if (!this.Organisation.CustomerContactUserGroup.ContainsMember(this.Contact))
                        {
                            this.Organisation.CustomerContactUserGroup.AddMember(this.Contact);
                        }
                    }
                    else
                    {
                        if (this.Organisation.CustomerContactUserGroup.ContainsMember(this.Contact))
                        {
                            this.Organisation.CustomerContactUserGroup.RemoveMember(this.Contact);
                        }
                    }
                }
                else
                {
                    if (this.Organisation.CustomerContactUserGroup.ContainsMember(this.Contact))
                    {
                        this.Organisation.CustomerContactUserGroup.RemoveMember(this.Contact);
                    }
                }                
            }
        }

        public void AppsOnDeriveSupplierContactMemberShip(IDerivation derivation)
        {
            if (this.ExistContact && this.ExistOrganisation && this.Organisation.ExistSupplierContactUserGroup)
            {
                if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                {
                    if (this.Organisation.IsActiveSupplier(this.FromDate))
                    {
                        if (!this.Organisation.SupplierContactUserGroup.ContainsMember(this.Contact))
                        {
                            this.Organisation.SupplierContactUserGroup.AddMember(this.Contact);
                        }
                    }
                    else
                    {
                        if (this.Organisation.SupplierContactUserGroup.ContainsMember(this.Contact))
                        {
                            this.Organisation.SupplierContactUserGroup.RemoveMember(this.Contact);
                        }
                    }
                }
                else
                {
                    if (this.Organisation.SupplierContactUserGroup.ContainsMember(this.Contact))
                    {
                        this.Organisation.SupplierContactUserGroup.RemoveMember(this.Contact);
                    }
                }   
            }                
        }

        public void AppsOnDerivePartnerContactMemberShip(IDerivation derivation)
        {
            if (this.ExistContact && this.ExistOrganisation && this.Organisation.ExistPartnerContactUserGroup)
            {
                if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                {
                    if (this.Organisation.IsActivePartner(this.FromDate))
                    {
                        if (!this.Organisation.PartnerContactUserGroup.ContainsMember(this.Contact))
                        {
                            this.Organisation.PartnerContactUserGroup.AddMember(this.Contact);
                        }
                    }
                    else
                    {
                        if (this.Organisation.PartnerContactUserGroup.ContainsMember(this.Contact))
                        {
                            this.Organisation.PartnerContactUserGroup.RemoveMember(this.Contact);
                        }
                    }
                }
                else
                {
                    if (this.Organisation.PartnerContactUserGroup.ContainsMember(this.Contact))
                    {
                        this.Organisation.PartnerContactUserGroup.RemoveMember(this.Contact);
                    }
                }                
            }
        }

        public bool IsActive
        {
            get { return this.ExistPartyWhereCurrentOrganisationContactRelationship;  }
        }
    }
}