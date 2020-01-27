// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;

    public partial class Organisation
    {
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

        private bool IsDeletable => !this.ExistCurrentContacts;

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (!changeSet.IsCreated(this) && !changeSet.HasChangedRoles(this) && changeSet.HasChangedAssociation(this, this.Meta.EmploymentsWhereEmployer))
            {
                iteration.Mark(this);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var session = this.Strategy.Session;
            var singleton = session.GetSingleton();

            session.Prefetch(this.PrefetchPolicy);

            if (this.IsInternalOrganisation)
            {
                if (!this.ExistRequestCounter)
                {
                    this.RequestCounter = new CounterBuilder(session).Build();
                }

                if (!this.ExistQuoteCounter)
                {
                    this.QuoteCounter = new CounterBuilder(session).Build();
                }

                if (!this.ExistPurchaseInvoiceCounter)
                {
                    this.PurchaseInvoiceCounter = new CounterBuilder(session).Build();
                }

                if (!this.ExistPurchaseOrderCounter)
                {
                    this.PurchaseOrderCounter = new CounterBuilder(session).Build();
                }

                if (!this.ExistSubAccountCounter)
                {
                    this.SubAccountCounter = new CounterBuilder(session).Build();
                }

                if (!this.ExistIncomingShipmentCounter)
                {
                    this.IncomingShipmentCounter = new CounterBuilder(session).Build();
                }

                if (!this.ExistWorkEffortCounter)
                {
                    this.WorkEffortCounter = new CounterBuilder(session).Build();
                }

                if (!this.ExistInvoiceSequence)
                {
                    this.InvoiceSequence = new InvoiceSequenceBuilder(session).Build();
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

            var now = this.Session().Now();

            this.ActiveEmployees = this.EmploymentsWhereEmployer
                .Where(v => v.FromDate <= now && (!v.ExistThroughDate || v.ThroughDate >= now))
                .Select(v => v.Employee)
                .ToArray();

            this.PartyName = this.Name;

            // Contacts
            if (!this.ExistContactsUserGroup)
            {
                var customerContactGroupName = $"Customer contacts at {this.Name} ({this.UniqueId})";
                this.ContactsUserGroup = new UserGroupBuilder(this.Strategy.Session).WithName(customerContactGroupName).Build();
            }

            this.CurrentSuppliers = this.SupplierRelationshipsWhereInternalOrganisation
                .Where(v => v.FromDate <= this.Strategy.Session.Now() && (!v.ExistThroughDate || v.ThroughDate >= this.Strategy.Session.Now()))
                .Select(v => v.Supplier)
                .ToArray();

            this.CurrentCustomers = this.CustomerRelationshipsWhereCustomer
                .Where(v => v.FromDate <= this.Strategy.Session.Now() && (!v.ExistThroughDate || v.ThroughDate >= this.Strategy.Session.Now()))
                .Select(v => v.Customer)
                .ToArray();

            var allContactRelationships = this.OrganisationContactRelationshipsWhereOrganisation.ToArray();
            var allContacts = allContactRelationships.Select(v => v.Contact);

            this.CurrentOrganisationContactRelationships = allContactRelationships
                .Where(v => v.FromDate <= this.Session().Now() && (!v.ExistThroughDate || v.ThroughDate >= this.Session().Now()))
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

            var deletePermission = new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.Delete, Operations.Execute);
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

        public bool BaseIsActiveProfessionalServicesProvider(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return this.ExistProfessionalServicesRelationshipsWhereProfessionalServicesProvider
                   && this.ProfessionalServicesRelationshipsWhereProfessionalServicesProvider
                       .Any(v => v.FromDate.Date <= date && (!v.ExistThroughDate || v.ThroughDate >= date));
        }

        public bool BaseIsActiveSubContractor(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return this.ExistSubContractorRelationshipsWhereContractor
                   && this.SubContractorRelationshipsWhereContractor
                        .Any(v => v.FromDate.Date <= date && (!v.ExistThroughDate || v.ThroughDate >= date));
        }

        public bool BaseIsActiveSupplier(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return this.ExistSupplierRelationshipsWhereSupplier && this.SupplierRelationshipsWhereSupplier
                       .Any(v => v.FromDate <= date && (!v.ExistThroughDate || v.ThroughDate >= date));
        }

        public void BaseDelete(DeletableDelete method)
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
