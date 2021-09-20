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
                    .WithRule(M.Organisation.RequestNumberCounter.RoleType)
                    .WithRule(M.Organisation.QuoteNumberCounter.RoleType)
                    .WithRule(M.Organisation.PurchaseInvoiceNumberCounter.RoleType)
                    .WithRule(M.Organisation.PurchaseOrderNumberCounter.RoleType)
                    .WithRule(M.Organisation.SubAccountCounter.RoleType)
                    .WithRule(M.Organisation.IncomingShipmentNumberCounter.RoleType)
                    .WithRule(M.Organisation.WorkEffortNumberCounter.RoleType)
                    .WithRule(M.Organisation.InvoiceSequence.RoleType)
                    .WithRule(M.Organisation.ContactsUserGroup)
                    .WithRule(M.Organisation.OrganisationContactRelationshipsWhereOrganisation, organisationContactRelationshipPrefetch)
                    .WithRule(M.Organisation.PartyContactMechanisms.RoleType, partyContactMechanismePrefetch)
                    .WithRule(M.Organisation.CurrentContacts.RoleType)
                    .Build();
            }
        }

        public List<string> Roles => new List<string>() { "Internal organisation" };

        private bool IsDeletable =>
            !this.IsInternalOrganisation
            && !this.ExistExternalAccountingTransactionsWhereFromParty
            && !this.ExistExternalAccountingTransactionsWhereToParty
            && !this.ExistShipmentsWhereShipFromParty
            && !this.ExistShipmentsWhereShipToParty
            && !this.ExistPaymentsWhereReceiver
            && !this.ExistPaymentsWhereSender
            && !this.ExistPaymentsWhereSender
            && !this.ExistEmploymentsWhereEmployer
            && !this.ExistEngagementsWhereBillToParty
            && !this.ExistEngagementsWherePlacingParty
            && !this.ExistPartsWhereManufacturedBy
            && !this.ExistPartsWhereSuppliedBy
            && !this.ExistOrganisationGlAccountsWhereInternalOrganisation
            && !this.ExistOrganisationRollUpsWhereParent
            && !this.ExistPartyFixedAssetAssignmentsWhereParty
            && !this.ExistPickListsWhereShipToParty
            && !this.ExistQuotesWhereIssuer
            && !this.ExistQuotesWhereReceiver
            && !this.ExistPurchaseInvoicesWhereBilledTo
            && !this.ExistPurchaseInvoicesWhereBilledFrom
            && !this.ExistPurchaseInvoicesWhereShipToCustomer
            && !this.ExistPurchaseInvoicesWhereBillToEndCustomer
            && !this.ExistPurchaseInvoicesWhereShipToEndCustomer
            && !this.ExistPurchaseOrdersWhereTakenViaSupplier
            && !this.ExistPurchaseOrdersWhereTakenViaSubcontractor
            && !this.ExistRequestsWhereOriginator
            && !this.ExistRequestsWhereRecipient
            && !this.ExistRequirementsWhereAuthorizer
            && !this.ExistRequirementsWhereNeededFor
            && !this.ExistRequirementsWhereOriginator
            && !this.ExistRequirementsWhereServicedBy
            && !this.ExistSalesInvoicesWhereBilledFrom
            && !this.ExistSalesInvoicesWhereBillToCustomer
            && !this.ExistSalesInvoicesWhereBillToEndCustomer
            && !this.ExistSalesInvoicesWhereShipToCustomer
            && !this.ExistSalesInvoicesWhereShipToEndCustomer
            && !this.ExistSalesOrdersWhereBillToCustomer
            && !this.ExistSalesOrdersWhereBillToEndCustomer
            && !this.ExistSalesOrdersWhereShipToCustomer
            && !this.ExistSalesOrdersWhereShipToEndCustomer
            && !this.ExistSalesOrdersWherePlacingCustomer
            && !this.ExistSalesOrdersWhereTakenBy
            && !this.ExistSalesOrderItemsWhereAssignedShipToParty
            && !this.ExistSerialisedItemsWhereSuppliedBy
            && !this.ExistSerialisedItemsWhereOwnedBy
            && !this.ExistSerialisedItemsWhereRentedBy
            && !this.ExistSerialisedItemsWhereBuyer
            && !this.ExistSerialisedItemsWhereSeller
            && !this.ExistWorkEffortsWhereCustomer
            && !this.ExistWorkEffortsWhereExecutedBy
            && !this.ExistWorkEffortPartyAssignmentsWhereParty;

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (!changeSet.IsCreated(this) &&
                !changeSet.HasChangedRoles(this) &&
                changeSet.HasChangedAssociation(this, this.Meta.EmploymentsWhereEmployer) && // ActiveEmployees
                changeSet.HasChangedAssociation(this, this.Meta.EmploymentsWhereEmployer)) // ActiveCustomers
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

            this.PartyName = this.Name;

            var now = this.Session().Now();

            this.ActiveEmployees = this.EmploymentsWhereEmployer
                .Where(v => v.FromDate <= now && (!v.ExistThroughDate || v.ThroughDate >= now))
                .Select(v => v.Employee)
                .ToArray();

            this.ActiveCustomers = this.CustomerRelationshipsWhereInternalOrganisation
                .Where(v => v.FromDate <= now && (!v.ExistThroughDate || v.ThroughDate >= now))
                .Select(v => v.Customer)
                .ToArray();

            // Contacts
            if (!this.ExistContactsUserGroup)
            {
                var customerContactGroupName = $"Customer contacts at {this.Name} ({this.UniqueId})";
                this.ContactsUserGroup = new UserGroupBuilder(this.Strategy.Session).WithName(customerContactGroupName).Build();
            }

            DeriveRelationships();

            this.ContactsUserGroup.Members = this.CurrentContacts.ToArray();

            this.Sync();
        }

        public void DeriveRelationships()
        {
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
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
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

        public void BaseDelete(DeletableDelete method)
        {
            if (this.IsDeletable)
            {
                foreach (PartyFinancialRelationship deletable in this.PartyFinancialRelationshipsWhereParty)
                {
                    deletable.Delete();
                }

                foreach (PartyContactMechanism deletable in this.PartyContactMechanisms)
                {
                    var contactmechanism = deletable.ContactMechanism;

                    deletable.Delete();

                    if(!contactmechanism.ExistPartyContactMechanismsWhereContactMechanism)
                    {
                        contactmechanism.Delete();
                    }
                }

                foreach (OrganisationContactRelationship deletable in this.OrganisationContactRelationshipsWhereOrganisation)
                {
                    deletable.Contact.Delete();
                }

                foreach (PriceComponent deletable in this.PriceComponentsWherePricedBy)
                {
                    deletable.Delete();
                }

                foreach (CommunicationEvent deletable in this.CommunicationEventsWhereInvolvedParty)
                {
                    deletable.Delete();
                }

                foreach (CustomerRelationship deletable in this.CustomerRelationshipsWhereCustomer)
                {
                    deletable.Delete();
                }

                foreach (OrganisationRollUp deletable in this.OrganisationRollUpsWhereChild)
                {
                    deletable.Delete();
                }

                foreach (PartyFixedAssetAssignment deletable in this.PartyFixedAssetAssignmentsWhereParty)
                {
                    deletable.Delete();
                }

                foreach (ProfessionalServicesRelationship deletable in this.ProfessionalServicesRelationshipsWhereProfessionalServicesProvider)
                {
                    deletable.Delete();
                }

                foreach (SubContractorRelationship deletable in this.SubContractorRelationshipsWhereSubContractor)
                {
                    deletable.Delete();
                }

                foreach (SupplierRelationship deletable in this.SupplierRelationshipsWhereSupplier)
                {
                    deletable.Delete();
                }

                foreach (SupplierOffering deletable in this.SupplierOfferingsWhereSupplier)
                {
                    deletable.Delete();
                }
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
    }
}
