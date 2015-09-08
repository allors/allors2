// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SupplierRelationship.cs" company="Allors bvba">
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

    public partial class SupplierRelationship
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            

            if (!this.ExistFromDate)
            {
                this.FromDate = DateTime.UtcNow;
            }

            if (!this.ExistInternalOrganisation)
            {
                this.InternalOrganisation = Domain.Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }

            if (!this.ExistSubAccountNumber)
            {
                this.SubAccountNumber = this.InternalOrganisation.DeriveNextSubAccountNumber();
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistSupplier)
            {
                derivation.AddDerivable(this.Supplier);

                foreach (OrganisationContactRelationship contactRelationship in this.Supplier.OrganisationContactRelationshipsWhereOrganisation)
                {
                    derivation.AddDerivable(contactRelationship);
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.DeriveMembership(derivation);
            this.DeriveInternalOrganisationSupplier(derivation);

            var supplierRelationships = this.InternalOrganisation.SupplierRelationshipsWhereInternalOrganisation;
            supplierRelationships.Filter.AddEquals(SupplierRelationships.Meta.SubAccountNumber, this.SubAccountNumber);
            if (supplierRelationships.Count == 1)
            {
                if (!supplierRelationships[0].Equals(this))
                {
                    derivation.Log.AddError(new DerivationErrorUnique(derivation.Log, this, SupplierRelationships.Meta.SubAccountNumber));
                }
            }
            else if (supplierRelationships.Count > 1)
            {
                derivation.Log.AddError(new DerivationErrorUnique(derivation.Log, this, SupplierRelationships.Meta.SubAccountNumber));
            }
        }

        public void AppsOnDeriveInternalOrganisationSupplier(IDerivation derivation)
        {
            if (this.ExistSupplier && this.ExistInternalOrganisation)
            {
                if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                {
                    if (!this.Supplier.ExistInternalOrganisationWhereSupplier)
                    {
                        this.InternalOrganisation.AddSupplier(this.Supplier);
                    }
                }

                if (this.FromDate > DateTime.UtcNow || (this.ExistThroughDate && this.ThroughDate < DateTime.UtcNow))
                {
                    if (this.Supplier.ExistInternalOrganisationWhereSupplier)
                    {
                        this.InternalOrganisation.RemoveSupplier(this.Supplier);
                    }
                }
            }
        }

        public void AppsOnDeriveMembership(IDerivation derivation)
        {
            if (this.ExistSupplier && this.ExistInternalOrganisation)
            {
                if (this.Supplier.SupplierContactUserGroup != null)
                {
                    foreach (OrganisationContactRelationship contactRelationship in this.Supplier.OrganisationContactRelationshipsWhereOrganisation)
                    {
                        if (this.FromDate <= DateTime.UtcNow &&
                            (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                        {
                            if (!this.Supplier.SupplierContactUserGroup.ContainsMember(contactRelationship.Contact))
                            {
                                this.Supplier.SupplierContactUserGroup.AddMember(contactRelationship.Contact);
                            }
                        }
                        else
                        {
                            if (this.Supplier.SupplierContactUserGroup.ContainsMember(contactRelationship.Contact))
                            {
                                this.Supplier.SupplierContactUserGroup.RemoveMember(contactRelationship.Contact);
                            }
                        }
                    }
                }
            }
        }
    }
}