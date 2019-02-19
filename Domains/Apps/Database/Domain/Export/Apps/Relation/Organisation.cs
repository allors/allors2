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
using System.Linq;
using Allors.Meta;

namespace Allors.Domain
{
    using System;

    public partial class Organisation
    {
        public override string ToString()
        {
            return $"Organisation: {this.Id} {this.Name}";
        }

        private bool IsDeletable => !this.ExistCurrentContacts;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistLocale)
            {
                this.Locale = this.Strategy.Session.GetSingleton().DefaultLocale;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var validation = derivation.Validation;

            this.Strategy.Session.Prefetch(this.PrefetchPolicy);

            if (this.IsInternalOrganisation)
            {
                if (!this.ExistRequestCounter)
                {
                    this.RequestCounter = new CounterBuilder(this.strategy.Session).Build();
                }

                if (!this.ExistQuoteCounter)
                {
                    this.QuoteCounter = new CounterBuilder(this.strategy.Session).Build();
                }
                
                if (!this.ExistPurchaseInvoiceCounter)
                {
                    this.PurchaseInvoiceCounter = new CounterBuilder(this.strategy.Session).Build();
                }

                if (!this.ExistPurchaseOrderCounter)
                {
                    this.PurchaseOrderCounter = new CounterBuilder(this.strategy.Session).Build();
                }

                if (!this.ExistSubAccountCounter)
                {
                    this.SubAccountCounter = new CounterBuilder(this.strategy.Session).Build();
                }

                if (!this.ExistIncomingShipmentCounter)
                {
                    this.IncomingShipmentCounter = new CounterBuilder(this.strategy.Session).Build();
                }

                if (!this.ExistWorkEffortCounter)
                {
                    this.WorkEffortCounter = new CounterBuilder(this.strategy.Session).Build();
                    this.WorkEffortPrefix = "W";
                }

                if (!this.ExistInvoiceSequence)
                {
                    this.InvoiceSequence = new InvoiceSequenceBuilder(this.strategy.Session).Build();
                }

                if (this.DoAccounting && !this.ExistFiscalYearStartMonth)
                {
                    this.FiscalYearStartMonth = 1;
                }

                if (this.DoAccounting && !this.ExistFiscalYearStartDay)
                {
                    this.FiscalYearStartDay = 1;
                }
            }

            this.PartyName = this.Name;

            // Contacts
            if (!this.ExistContactsUserGroup)
            {
                var customerContactGroupName = $"Customer contacts at {this.Name} ({this.UniqueId})";
                this.ContactsUserGroup = new UserGroupBuilder(this.strategy.Session).WithName(customerContactGroupName).Build();
            }

            var allContactRelationships = this.OrganisationContactRelationshipsWhereOrganisation.ToArray();
            var allContacts = allContactRelationships.Select(v => v.Contact);

            this.CurrentOrganisationContactRelationships = allContactRelationships
                .Where(v => v.FromDate <= DateTime.UtcNow && (!v.ExistThroughDate || v.ThroughDate >= DateTime.UtcNow))
                .ToArray();

            this.InactiveOrganisationContactRelationships = allContactRelationships
                .Except(this.CurrentOrganisationContactRelationships)
                .ToArray();

            this.CurrentContacts = this.CurrentOrganisationContactRelationships
                .Select(v => v.Contact).ToArray();

            this.InactiveContacts = this.InactiveOrganisationContactRelationships
                .Select(v => v.Contact)
                .ToArray();

            this.ContactsUserGroup.Members = this.CurrentContacts.ToArray();

            this.Sync();

            var deletePermission = new Permissions(this.strategy.Session).Get(this.Meta.ObjectType, this.Meta.Delete, Operations.Execute);
            if (this.IsDeletable)
            {
                this.RemoveDeniedPermission(deletePermission);
            }
            else
            {
                this.AddDeniedPermission(deletePermission);
            }
        }

        public void Sync()
        {
            var partyContactMechanisms = this.PartyContactMechanisms.ToArray();
            foreach (OrganisationContactRelationship organisationContactRelationship in this.OrganisationContactRelationshipsWhereOrganisation)
            {
                organisationContactRelationship.Contact.Sync(partyContactMechanisms);
            }
        }

        public PrefetchPolicy PrefetchPolicy
        {
            get
            {
                var organisationContactRelationshipPrefetch = new PrefetchPolicyBuilder()
                    .WithRule(M.OrganisationContactRelationship.Contact)
                    .Build();

                var partyContactMechanismePrefetch = new PrefetchPolicyBuilder()
                    .WithRule(M.PartyContactMechanism.ContactMechanism)
                    .Build();

                return new PrefetchPolicyBuilder()
                    .WithRule(M.Organisation.RequestCounter.RoleType)
                    .WithRule(M.Organisation.QuoteCounter.RoleType)
                    .WithRule(M.Organisation.PurchaseInvoiceCounter.RoleType)
                    .WithRule(M.Organisation.PurchaseOrderCounter.RoleType)
                    .WithRule(M.Organisation.SubAccountCounter.RoleType)
                    .WithRule(M.Organisation.IncomingShipmentCounter.RoleType)
                    .WithRule(M.Organisation.WorkEffortCounter.RoleType)
                    .WithRule(M.Organisation.InvoiceSequence.RoleType)
                    .WithRule(M.Organisation.ContactsUserGroup)
                    .WithRule(M.Organisation.OrganisationContactRelationshipsWhereOrganisation, organisationContactRelationshipPrefetch)
                    .WithRule(M.Organisation.PartyContactMechanisms.RoleType, partyContactMechanismePrefetch)
                    .WithRule(M.Organisation.CurrentContacts.RoleType)
                    .Build();
            }
        }

        public List<string> Roles => new List<string>() { "Internal organisation" };

        public bool AppsIsActiveProfessionalServicesProvider(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return this.ExistProfessionalServicesRelationshipsWhereProfessionalServicesProvider 
                   && this.ProfessionalServicesRelationshipsWhereProfessionalServicesProvider
                       .Any(v => v.FromDate.Date <= date && (!v.ExistThroughDate || v.ThroughDate >= date));
           
        }

        public bool AppsIsActiveSubContractor(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return this.ExistSubContractorRelationshipsWhereContractor 
                   && this.SubContractorRelationshipsWhereContractor
                        .Any(v => v.FromDate.Date <= date && (!v.ExistThroughDate || v.ThroughDate >= date));
        }

        public bool AppsIsActiveSupplier(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return this.ExistSupplierRelationshipsWhereSupplier && this.SupplierRelationshipsWhereSupplier
                       .Any(v => v.FromDate <= date && (!v.ExistThroughDate || v.ThroughDate >= date));
        }
        
        public void AppsDelete(DeletableDelete method)
        {
            if (this.IsDeletable)
            {
                foreach (PartyFinancialRelationship partyFinancialRelationship in this.PartyFinancialRelationshipsWhereParty)
                {
                    partyFinancialRelationship.Delete();
                }

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
    }
}