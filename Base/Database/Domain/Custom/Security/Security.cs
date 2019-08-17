// <copyright file="Security.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using Allors.Meta;

    public partial class Security
    {
        public void Grantemployee(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.EmployeeId, objectType, operations);

        private void CustomOnPreSetup()
        {
            // Default access policy
            var security = new Security(this.session);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            foreach (ObjectType @class in this.session.Database.MetaPopulation.Classes)
            {
                security.GrantAdministrator(@class, full);
                security.GrantLocalAdministrator(@class, full);
                security.GrantCreator(@class, full);
                security.GrantProductQuoteApprover(@class, full);
                security.GrantPurchaseOrderApproverLevel1(@class, full);
                security.GrantPurchaseOrderApproverLevel2(@class, full);

                if (@class.Equals(M.WorkTask.ObjectType) ||
                    @class.Equals(M.TimeEntry.ObjectType) ||
                    @class.Equals(M.InventoryItemTransaction.ObjectType))
                {
                    security.GrantBlueCollarWorker(@class, full);
                }
                else if (@class.Equals(M.WorkEffortInventoryAssignment.ObjectType))
                {
                    var excepts = new HashSet<OperandType>
                    {
                        M.WorkEffortInventoryAssignment.BillableQuantity,
                        M.WorkEffortInventoryAssignment.UnitSellingPrice,
                        M.WorkEffortInventoryAssignment.AssignedUnitSellingPrice,
                        M.WorkEffortInventoryAssignment.UnitPurchasePrice,
                    };

                    security.GrantExceptBlueCollarWorker(@class, excepts, full);
                }
                else if (@class.Equals(M.Bank.ObjectType) ||
                         @class.Equals(M.BankAccount.ObjectType) ||
                         @class.Equals(M.Benefit.ObjectType) ||
                         @class.Equals(M.BillingAccount.ObjectType) ||
                         @class.Equals(M.ClientAgreement.ObjectType) ||
                         @class.Equals(M.CommunicationEventPurpose.ObjectType) ||
                         @class.Equals(M.ContactMechanismPurpose.ObjectType) ||
                         @class.Equals(M.ContactMechanismType.ObjectType) ||
                         @class.Equals(M.Country.ObjectType) ||
                         @class.Equals(M.County.ObjectType) ||
                         @class.Equals(M.CreditCard.ObjectType) ||
                         @class.Equals(M.CreditCardCompany.ObjectType) ||
                         @class.Equals(M.CustomOrganisationClassification.ObjectType) ||
                         @class.Equals(M.CustomerRelationship.ObjectType) ||
                         @class.Equals(M.Document.ObjectType) ||
                         @class.Equals(M.EmailAddress.ObjectType) ||
                         @class.Equals(M.EmailCommunication.ObjectType) ||
                         @class.Equals(M.Employment.ObjectType) ||
                         @class.Equals(M.EmploymentAgreement.ObjectType) ||
                         @class.Equals(M.EuSalesListType.ObjectType) ||
                         @class.Equals(M.Event.ObjectType) ||
                         @class.Equals(M.EventRegistration.ObjectType) ||
                         @class.Equals(M.FaceToFaceCommunication.ObjectType) ||
                         @class.Equals(M.FaxCommunication.ObjectType) ||
                         @class.Equals(M.FinancialTerm.ObjectType) ||
                         @class.Equals(M.GenderType.ObjectType) ||
                         @class.Equals(M.Hobby.ObjectType) ||
                         @class.Equals(M.Incentive.ObjectType) ||
                         @class.Equals(M.IndustryClassification.ObjectType) ||
                         @class.Equals(M.IncoTerm.ObjectType) ||
                         @class.Equals(M.LegalForm.ObjectType) ||
                         @class.Equals(M.LegalTerm.ObjectType) ||
                         @class.Equals(M.LetterCorrespondence.ObjectType) ||
                         @class.Equals(M.MaritalStatus.ObjectType) ||
                         @class.Equals(M.Organisation.ObjectType) ||
                         @class.Equals(M.OrganisationClassification.ObjectType) ||
                         @class.Equals(M.OrganisationContactKind.ObjectType) ||
                         @class.Equals(M.OrganisationContactRelationship.ObjectType) ||
                         @class.Equals(M.OrganisationRole.ObjectType) ||
                         @class.Equals(M.OrganisationRollUp.ObjectType) ||
                         @class.Equals(M.OrganisationUnit.ObjectType) ||
                         @class.Equals(M.OwnBankAccount.ObjectType) ||
                         @class.Equals(M.OwnCreditCard.ObjectType) ||
                         @class.Equals(M.PartyBenefit.ObjectType) ||
                         @class.Equals(M.PartyContactMechanism.ObjectType) ||
                         @class.Equals(M.PartyFinancialRelationship.ObjectType) ||
                         @class.Equals(M.Passport.ObjectType) ||
                         @class.Equals(M.Person.ObjectType) ||
                         @class.Equals(M.PersonalTitle.ObjectType) ||
                         @class.Equals(M.PersonClassification.ObjectType) ||
                         @class.Equals(M.PersonRole.ObjectType) ||
                         @class.Equals(M.PhoneCommunication.ObjectType) ||
                         @class.Equals(M.PostalAddress.ObjectType) ||
                         @class.Equals(M.PostalCode.ObjectType) ||
                         @class.Equals(M.ProfessionalAssignment.ObjectType) ||
                         @class.Equals(M.ProfessionalPlacement.ObjectType) ||
                         @class.Equals(M.ProfessionalServicesRelationship.ObjectType) ||
                         @class.Equals(M.Province.ObjectType) ||
                         @class.Equals(M.PurchaseAgreement.ObjectType) ||
                         @class.Equals(M.QuoteTerm.ObjectType) ||
                         @class.Equals(M.Region.ObjectType) ||
                         @class.Equals(M.SalesAgreement.ObjectType) ||
                         @class.Equals(M.SalesChannel.ObjectType) ||
                         @class.Equals(M.SalesRepRelationship.ObjectType) ||
                         @class.Equals(M.SalesTerritory.ObjectType) ||
                         @class.Equals(M.Salutation.ObjectType) ||
                         @class.Equals(M.ServiceTerritory.ObjectType) ||
                         @class.Equals(M.State.ObjectType) ||
                         @class.Equals(M.SubContractorAgreement.ObjectType) ||
                         @class.Equals(M.SubContractorRelationship.ObjectType) ||
                         @class.Equals(M.SupplierRelationship.ObjectType) ||
                         @class.Equals(M.TelecommunicationsNumber.ObjectType) ||
                         @class.Equals(M.Territory.ObjectType) ||
                         @class.Equals(M.Threshold.ObjectType) ||
                         @class.Equals(M.WebAddress.ObjectType) ||
                         @class.Equals(M.WebSiteCommunication.ObjectType))
                {
                    security.GrantSalesAccountManager(@class, full);
                    security.Grantemployee(@class, Operations.Read);
                }
                else if (@class.Equals(M.RequestForInformation.ObjectType) ||
                         @class.Equals(M.RequestForProposal.ObjectType) ||
                         @class.Equals(M.RequestForQuote.ObjectType) ||
                         @class.Equals(M.ProductQuote.ObjectType) ||
                         @class.Equals(M.Proposal.ObjectType) ||
                         @class.Equals(M.StatementOfWork.ObjectType) ||
                         @class.Equals(M.SalesInvoice.ObjectType) ||
                         @class.Equals(M.SalesOrder.ObjectType))
                {
                    security.GrantSalesAccountManager(@class, full);
                }
                else if (@class.Equals(M.AccountAdjustment.ObjectType) ||
                         @class.Equals(M.AccountingPeriod.ObjectType) ||
                         @class.Equals(M.AccountingTransactionNumber.ObjectType) ||
                         @class.Equals(M.CapitalBudget.ObjectType) ||
                         @class.Equals(M.ChartOfAccounts.ObjectType) ||
                         @class.Equals(M.CustomerReturn.ObjectType) ||
                         @class.Equals(M.CustomerShipment.ObjectType) ||
                         @class.Equals(M.DropShipment.ObjectType) ||
                         @class.Equals(M.Journal.ObjectType) ||
                         @class.Equals(M.JournalEntry.ObjectType) ||
                         @class.Equals(M.JournalEntryNumber.ObjectType) ||
                         @class.Equals(M.OperatingBudget.ObjectType) ||
                         @class.Equals(M.PurchaseInvoice.ObjectType) ||
                         @class.Equals(M.PurchaseOrder.ObjectType) ||
                         @class.Equals(M.PurchaseReturn.ObjectType) ||
                         @class.Equals(M.PurchaseShipment.ObjectType) ||
                         @class.Equals(M.Transfer.ObjectType) ||
                         @class.Equals(M.WorkTask.ObjectType))
                {
                    // Do not grant read permission to employee
                }
                else
                {
                    security.Grantemployee(@class, Operations.Read);
                }
            }
        }

        private void CustomOnPostSetup()
        {
        }
    }
}
