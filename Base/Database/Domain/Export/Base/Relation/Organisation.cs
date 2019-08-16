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
        private bool IsDeletable => !this.ExistCurrentContacts;

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
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

                #region Security

                var groupName = $"{this.Name} ProductQuote approvers";

                if (!this.ExistProductQuoteApproverSecurityToken)
                {
                    this.ProductQuoteApproverSecurityToken = new SecurityTokenBuilder(session).Build();
                }

                if (!this.ExistProductQuoteApproverUserGroup)
                {
                    this.ProductQuoteApproverUserGroup = new UserGroups(session).FindBy(M.UserGroup.Name, groupName)
                                               ?? new UserGroupBuilder(session).WithName(groupName).Build();
                }

                if (!this.ExistProductQuoteApproverAccessControl)
                {
                    var role = new Roles(session).ProductQuoteApprover;

                    this.ProductQuoteApproverAccessControl =
                        new AccessControlBuilder(session).WithRole(role)
                            .WithSubjectGroup(this.ProductQuoteApproverUserGroup)
                            .Build();

                    this.ProductQuoteApproverSecurityToken.AddAccessControl(this.ProductQuoteApproverAccessControl);
                }

                groupName = $"{this.Name} PurchaseOrder approvers level 1";

                if (!this.ExistPurchaseOrderApproverLevel1SecurityToken)
                {
                    this.PurchaseOrderApproverLevel1SecurityToken = new SecurityTokenBuilder(session).Build();
                }

                if (!this.ExistPurchaseOrderApproverLevel1UserGroup)
                {
                    this.PurchaseOrderApproverLevel1UserGroup = new UserGroups(session).FindBy(M.UserGroup.Name, groupName)
                                                         ?? new UserGroupBuilder(session).WithName(groupName).Build();
                }

                if (!this.ExistPurchaseOrderApproverLevel1AccessControl)
                {
                    var role = new Roles(session).PurchaseOrderApproverLevel1;

                    this.PurchaseOrderApproverLevel1AccessControl =
                        new AccessControlBuilder(session).WithRole(role)
                            .WithSubjectGroup(this.PurchaseOrderApproverLevel1UserGroup)
                            .Build();

                    this.PurchaseOrderApproverLevel1SecurityToken.AddAccessControl(this.PurchaseOrderApproverLevel1AccessControl);
                }

                groupName = $"{this.Name} PurchaseOrder approvers level 2";

                if (!this.ExistPurchaseOrderApproverLevel2SecurityToken)
                {
                    this.PurchaseOrderApproverLevel2SecurityToken = new SecurityTokenBuilder(session).Build();
                }

                if (!this.ExistPurchaseOrderApproverLevel2UserGroup)
                {
                    this.PurchaseOrderApproverLevel2UserGroup = new UserGroups(session).FindBy(M.UserGroup.Name, groupName)
                                                                ?? new UserGroupBuilder(session).WithName(groupName).Build();
                }

                if (!this.ExistPurchaseOrderApproverLevel2AccessControl)
                {
                    var role = new Roles(session).PurchaseOrderApproverLevel2;

                    this.PurchaseOrderApproverLevel2AccessControl =
                        new AccessControlBuilder(session).WithRole(role)
                            .WithSubjectGroup(this.PurchaseOrderApproverLevel2UserGroup)
                            .Build();

                    this.PurchaseOrderApproverLevel2SecurityToken.AddAccessControl(this.PurchaseOrderApproverLevel2AccessControl);
                }

                groupName = $"{this.Name} PurchaseInvoice approvers";

                if (!this.ExistPurchaseInvoiceApproverSecurityToken)
                {
                    this.PurchaseInvoiceApproverSecurityToken = new SecurityTokenBuilder(session).Build();
                }

                if (!this.ExistPurchaseInvoiceApproverUserGroup)
                {
                    this.PurchaseInvoiceApproverUserGroup = new UserGroups(session).FindBy(M.UserGroup.Name, groupName)
                                                                ?? new UserGroupBuilder(session).WithName(groupName).Build();
                }

                if (!this.ExistPurchaseInvoiceApproverAccessControl)
                {
                    var role = new Roles(session).PurchaseInvoiceApprover;

                    this.PurchaseInvoiceApproverAccessControl =
                        new AccessControlBuilder(session).WithRole(role)
                            .WithSubjectGroup(this.PurchaseInvoiceApproverUserGroup)
                            .Build();

                    this.PurchaseInvoiceApproverSecurityToken.AddAccessControl(this.PurchaseInvoiceApproverAccessControl);
                }

                groupName = $"{this.Name} Blue-collar workers";

                if (!this.ExistBlueCollarWorkerSecurityToken)
                {
                    this.BlueCollarWorkerSecurityToken = new SecurityTokenBuilder(session).Build();
                }

                if (!this.ExistBlueCollarWorkerUserGroup)
                {
                    this.BlueCollarWorkerUserGroup = new UserGroups(session).FindBy(M.UserGroup.Name, groupName)
                                                         ?? new UserGroupBuilder(session).WithName(groupName).Build();
                }

                if (!this.ExistBlueCollarWorkerAccessControl)
                {
                    var role = new Roles(session).BlueCollarWorker;

                    this.BlueCollarWorkerAccessControl =
                        new AccessControlBuilder(session).WithRole(role)
                            .WithSubjectGroup(this.BlueCollarWorkerUserGroup)
                            .Build();

                    this.BlueCollarWorkerSecurityToken.AddAccessControl(this.BlueCollarWorkerAccessControl);
                }

                groupName = $"{this.Name} Local administrators";

                if (!this.ExistLocalAdministratorSecurityToken)
                {
                    this.LocalAdministratorSecurityToken = new SecurityTokenBuilder(session).Build();
                }

                if (!this.ExistLocalAdministratorUserGroup)
                {
                    this.LocalAdministratorUserGroup = new UserGroups(session).FindBy(M.UserGroup.Name, groupName)
                                                     ?? new UserGroupBuilder(session).WithName(groupName).Build();
                }

                if (!this.ExistLocalAdministratorAccessControl)
                {
                    var role = new Roles(session).LocalAdministrator;

                    this.LocalAdministratorAccessControl =
                        new AccessControlBuilder(session).WithRole(role)
                            .WithSubjectGroup(this.LocalAdministratorUserGroup)
                            .Build();

                    this.LocalAdministratorSecurityToken.AddAccessControl(this.LocalAdministratorAccessControl);
                }

                this.ProductQuoteApproverUserGroup.Members = this.ProductQuoteApprovers.ToArray();
                this.PurchaseOrderApproverLevel1UserGroup.Members = this.PurchaseOrderApproversLevel1.ToArray();
                this.PurchaseOrderApproverLevel2UserGroup.Members = this.PurchaseOrderApproversLevel2.ToArray();
                this.PurchaseInvoiceApproverUserGroup.Members = this.PurchaseInvoiceApprovers.ToArray();
                this.BlueCollarWorkerUserGroup.Members = this.BlueCollarWorkers.ToArray();
                this.LocalAdministratorUserGroup.Members = this.LocalAdministrators.ToArray();

                #endregion
            }

            this.PartyName = this.Name;

            // Contacts
            if (!this.ExistContactsUserGroup)
            {
                var customerContactGroupName = $"Customer contacts at {this.Name} ({this.UniqueId})";
                this.ContactsUserGroup = new UserGroupBuilder(this.Strategy.Session).WithName(customerContactGroupName).Build();
            }

            var allContactRelationships = this.OrganisationContactRelationshipsWhereOrganisation.ToArray();
            var allContacts = allContactRelationships.Select(v => v.Contact);

            this.CurrentOrganisationContactRelationships = allContactRelationships
                .Where(v => v.FromDate <= this.strategy.Session.Now() && (!v.ExistThroughDate || v.ThroughDate >= this.strategy.Session.Now()))
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
