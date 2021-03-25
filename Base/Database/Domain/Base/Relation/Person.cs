// <copyright file="Person.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using System.Text;
    using Allors.Meta;

    public partial class Person
    {
        public PrefetchPolicy PrefetchPolicy => new PrefetchPolicyBuilder()
            .WithRule(M.Person.OrganisationContactRelationshipsWhereContact)
            .WithRule(M.Person.PartyContactMechanisms.RoleType)
            .WithRule(M.Person.TimeSheetWhereWorker)
            .WithRule(M.Person.EmploymentsWhereEmployee)
            .Build();

        private bool IsDeletable =>
            (!this.ExistTimeSheetWhereWorker || !this.TimeSheetWhereWorker.ExistTimeEntries)
            && !this.ExistExternalAccountingTransactionsWhereFromParty
            && !this.ExistExternalAccountingTransactionsWhereToParty
            && !this.ExistShipmentsWhereShipFromParty
            && !this.ExistShipmentsWhereShipToParty
            && !this.ExistPaymentsWhereReceiver
            && !this.ExistPaymentsWhereSender
            && !this.ExistPaymentsWhereSender
            && !this.ExistEngagementsWhereBillToParty
            && !this.ExistEngagementsWherePlacingParty
            && !this.ExistPartsWhereManufacturedBy
            && !this.ExistPartsWhereSuppliedBy
            && !this.ExistPartyFixedAssetAssignmentsWhereParty
            && !this.ExistPickListsWhereShipToParty
            && !this.ExistQuotesWhereReceiver
            && !this.ExistPurchaseInvoicesWhereBilledFrom
            && !this.ExistPurchaseInvoicesWhereShipToCustomer
            && !this.ExistPurchaseInvoicesWhereBillToEndCustomer
            && !this.ExistPurchaseInvoicesWhereShipToEndCustomer
            && !this.ExistPurchaseOrdersWhereTakenViaSupplier
            && !this.ExistPurchaseOrdersWhereTakenViaSubcontractor
            && !this.ExistRequestsWhereOriginator
            && !this.ExistRequirementsWhereAuthorizer
            && !this.ExistRequirementsWhereNeededFor
            && !this.ExistRequirementsWhereOriginator
            && !this.ExistRequirementsWhereServicedBy
            && !this.ExistSalesInvoicesWhereBillToCustomer
            && !this.ExistSalesInvoicesWhereBillToEndCustomer
            && !this.ExistSalesInvoicesWhereShipToCustomer
            && !this.ExistSalesInvoicesWhereShipToEndCustomer
            && !this.ExistSalesOrdersWhereBillToCustomer
            && !this.ExistSalesOrdersWhereBillToEndCustomer
            && !this.ExistSalesOrdersWhereShipToCustomer
            && !this.ExistSalesOrdersWhereShipToEndCustomer
            && !this.ExistSalesOrdersWherePlacingCustomer
            && !this.ExistSalesOrderItemsWhereAssignedShipToParty
            && !this.ExistSerialisedItemsWhereSuppliedBy
            && !this.ExistSerialisedItemsWhereOwnedBy
            && !this.ExistSerialisedItemsWhereRentedBy
            && !this.ExistWorkEffortsWhereCustomer
            && !this.ExistWorkEffortPartyAssignmentsWhereParty
            && !this.ExistCashesWherePersonResponsible
            && !this.ExistCommunicationEventsWhereOwner
            && !this.ExistEngagementItemsWhereCurrentAssignedProfessional
            && !this.ExistEmploymentsWhereEmployee
            && !this.ExistEngineeringChangesWhereAuthorizer
            && !this.ExistEngineeringChangesWhereDesigner
            && !this.ExistEngineeringChangesWhereRequestor
            && !this.ExistEngineeringChangesWhereTester
            && !this.ExistEventRegistrationsWherePerson
            && !this.ExistOwnCreditCardsWhereOwner
            && !this.ExistPerformanceNotesWhereEmployee
            && !this.ExistPerformanceNotesWhereGivenByManager
            && !this.ExistPerformanceReviewsWhereEmployee
            && !this.ExistPerformanceReviewsWhereManager
            && !this.ExistPickListsWherePicker
            && !this.ExistPositionFulfillmentsWherePerson
            && !this.ExistProfessionalAssignmentsWhereProfessional;

        public bool BaseIsActiveEmployee(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return this.ExistEmploymentsWhereEmployee
                   && this.EmploymentsWhereEmployee
                       .Any(v => v.FromDate.Date <= date && (!v.ExistThroughDate || v.ThroughDate >= date));
        }

        public bool BaseIsActiveContact(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return this.ExistOrganisationContactRelationshipsWhereContact
                   && this.OrganisationContactRelationshipsWhereContact
                       .Any(v => v.FromDate.Date <= date && (!v.ExistThroughDate || v.ThroughDate >= date));
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var now = this.Session().Now();

            this.Strategy.Session.Prefetch(this.PrefetchPolicy);

            if (this.ExistSalutation
                && (this.Salutation.Equals(new Salutations(this.Session()).Mr)
                    || this.Salutation.Equals(new Salutations(this.Session()).Dr)))
            {
                this.Gender = new GenderTypes(this.Session()).Male;
            }

            if (this.ExistSalutation
                && (this.Salutation.Equals(new Salutations(this.Session()).Mrs)
                    || this.Salutation.Equals(new Salutations(this.Session()).Ms)
                    || this.Salutation.Equals(new Salutations(this.Session()).Mme)))
            {
                this.Gender = new GenderTypes(this.Session()).Female;
            }

            this.PartyName = this.DerivePartyName();

            DeriveRelationships();

            if (!this.ExistTimeSheetWhereWorker && (this.BaseIsActiveEmployee(now) || this.CurrentOrganisationContactRelationships.Count > 0))
            {
                new TimeSheetBuilder(this.Strategy.Session).WithWorker(this).Build();
            }
        }

        public void DeriveRelationships()
        {
            var now = this.Session().Now();
            var allOrganisationContactRelationships = this.OrganisationContactRelationshipsWhereContact;

            this.CurrentOrganisationContactRelationships = allOrganisationContactRelationships
                .Where(v => v.FromDate <= now && (!v.ExistThroughDate || v.ThroughDate >= now))
                .ToArray();

            this.InactiveOrganisationContactRelationships = allOrganisationContactRelationships
                .Except(this.CurrentOrganisationContactRelationships)
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

        public void BaseDelete(DeletableDelete method)
        {
            if (!this.IsDeletable)
            {
                return;
            }

            foreach (OrganisationContactRelationship deletable in this.OrganisationContactRelationshipsWhereContact)
            {
                deletable.Delete();
            }

            foreach (ProfessionalServicesRelationship deletable in this.ProfessionalServicesRelationshipsWhereProfessional)
            {
                deletable.Delete();
            }

            foreach (PartyFinancialRelationship deletable in this.PartyFinancialRelationshipsWhereParty)
            {
                deletable.Delete();
            }

            foreach (PartyContactMechanism deletable in this.PartyContactMechanisms)
            {
                var contactmechanism = deletable.ContactMechanism;

                deletable.Delete();

                if (!contactmechanism.ExistPartyContactMechanismsWhereContactMechanism)
                {
                    contactmechanism.Delete();
                }
            }

            foreach (CommunicationEvent deletable in this.CommunicationEventsWhereInvolvedParty)
            {
                deletable.Delete();
            }

            foreach (OrganisationContactRelationship deletable in this.OrganisationContactRelationshipsWhereContact)
            {
                deletable.Delete();
            }

            if (this.ExistTimeSheetWhereWorker)
            {
                this.TimeSheetWhereWorker.Delete();
            }

            if (this.ExistOwnerAccessControl)
            {
                this.OwnerAccessControl.Delete();
            }

            if (this.ExistOwnerSecurityToken)
            {
                this.OwnerSecurityToken.Delete();
            }
        }

        public void Sync(PartyContactMechanism[] organisationContactMechanisms)
        {
            foreach (var partyContactMechanism in organisationContactMechanisms)
            {
                this.RemoveCurrentOrganisationContactMechanism(partyContactMechanism.ContactMechanism);

                if (partyContactMechanism.FromDate <= this.Session().Now() &&
                    (!partyContactMechanism.ExistThroughDate || partyContactMechanism.ThroughDate >= this.Session().Now()))
                {
                    this.AddCurrentOrganisationContactMechanism(partyContactMechanism.ContactMechanism);
                }
            }
        }

        private string DerivePartyName()
        {
            var partyName = new StringBuilder();

            if (this.ExistFirstName)
            {
                partyName.Append(this.FirstName);
            }

            if (this.ExistMiddleName)
            {
                if (partyName.Length > 0)
                {
                    partyName.Append(" ");
                }

                partyName.Append(this.MiddleName);
            }

            if (this.ExistLastName)
            {
                if (partyName.Length > 0)
                {
                    partyName.Append(" ");
                }

                partyName.Append(this.LastName);
            }

            if (partyName.Length == 0)
            {
                partyName.Append($"[{this.UserName}]");
            }

            return partyName.ToString();
        }
    }
}
