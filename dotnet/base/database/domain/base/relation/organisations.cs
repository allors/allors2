// <copyright file="Organisations.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.IO;
    using System.Linq;
    using Allors.Meta;

    public partial class Organisations
    {
        public static void Daily(ISession session)
        {
            var organisations = new Organisations(session).Extent();

            foreach (Organisation organisation in organisations)
            {
                organisation.DeriveRelationships();
            }
        }

        public static Organisation CreateInternalOrganisation(
            ISession session,
            string name,
            string address,
            string postalCode,
            string locality,
            Country country,
            string phone1CountryCode,
            string phone1,
            ContactMechanismPurpose phone1Purpose,
            string phone2CountryCode,
            string phone2,
            ContactMechanismPurpose phone2Purpose,
            string emailAddress,
            string websiteAddress,
            string taxNumber,
            string bankName,
            string facilityName,
            string bic,
            string iban,
            Currency currency,
            string logo,
            string storeName,
            BillingProcess billingProcess,
            string outgoingShipmentNumberPrefix,
            string salesInvoiceNumberPrefix,
            string salesOrderNumberPrefix,
            string purchaseOrderNumberPrefix,
            string purchaseInvoiceNumberPrefix,
            string requestNumberPrefix,
            string quoteNumberPrefix,
            string productNumberPrefix,
            string workEffortPrefix,
            string creditNoteNumberPrefix,
            bool isImmediatelyPicked,
            bool autoGenerateShipmentPackage,
            bool isImmediatelyPacked,
            bool isAutomaticallyShipped,
            bool autoGenerateCustomerShipment,
            bool isAutomaticallyReceived,
            bool autoGeneratePurchaseShipment,
            bool useCreditNoteSequence,
            int? requestCounterValue,
            int? quoteCounterValue,
            int? orderCounterValue,
            int? purchaseOrderCounterValue,
            int? invoiceCounterValue,
            int? purchaseInvoiceCounterValue,
            bool purchaseOrderNeedsApproval,
            decimal? purchaseOrderApprovalThresholdLevel1,
            decimal? purchaseOrderApprovalThresholdLevel2,
            SerialisedItemSoldOn[] serialisedItemSoldOns,
            bool collectiveWorkEffortInvoice,
            InvoiceSequence invoiceSequence,
            RequestSequence requestSequence,
            QuoteSequence quoteSequence,
            CustomerShipmentSequence customerShipmentSequence,
            PurchaseShipmentSequence purchaseShipmentSequence,
            WorkEffortSequence workEffortSequence)
        {
            var postalAddress1 = new PostalAddressBuilder(session)
                    .WithAddress1(address)
                    .WithPostalCode(postalCode)
                    .WithLocality(locality)
                    .WithCountry(country)
                    .Build();

            var webSite = new WebAddressBuilder(session)
                .WithElectronicAddressString(websiteAddress)
                .Build();

            BankAccount bankAccount = null;
            if (!string.IsNullOrEmpty(bic) && !string.IsNullOrEmpty(iban))
            {
                var bank = new BankBuilder(session).WithName(bankName).WithBic(bic).WithCountry(country).Build();
                bankAccount = new BankAccountBuilder(session)
                    .WithBank(bank)
                    .WithIban(iban)
                    .WithNameOnAccount(name)
                    .WithCurrency(currency)
                    .Build();
            }

            var internalOrganisation = new OrganisationBuilder(session)
                .WithIsInternalOrganisation(true)
                .WithTaxNumber(taxNumber)
                .WithName(name)
                .WithPreferredCurrency(new Currencies(session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithInvoiceSequence(invoiceSequence)
                .WithRequestSequence(requestSequence)
                .WithQuoteSequence(quoteSequence)
                .WithCustomerShipmentSequence(customerShipmentSequence)
                .WithPurchaseShipmentSequence(purchaseShipmentSequence)
                .WithWorkEffortSequence(workEffortSequence)
                .WithFiscalYearStartMonth(01)
                .WithFiscalYearStartDay(01)
                .WithDoAccounting(false)
                .WithPurchaseOrderNeedsApproval(purchaseOrderNeedsApproval)
                .WithPurchaseOrderApprovalThresholdLevel1(purchaseOrderApprovalThresholdLevel1)
                .WithPurchaseOrderApprovalThresholdLevel2(purchaseOrderApprovalThresholdLevel2)
                .WithAutoGeneratePurchaseShipment(autoGeneratePurchaseShipment)
                .WithIsAutomaticallyReceived(isAutomaticallyReceived)
                .WithCollectiveWorkEffortInvoice(collectiveWorkEffortInvoice)
                .WithCountry(country)
                .Build();

            internalOrganisation.SerialisedItemSoldOns = serialisedItemSoldOns;

            if (invoiceSequence == new InvoiceSequences(session).RestartOnFiscalYear)
            {
                var sequenceNumbers = internalOrganisation.FiscalYearsInternalOrganisationSequenceNumbers.FirstOrDefault(v => v.FiscalYear == session.Now().Year);
                if (sequenceNumbers == null)
                {
                    sequenceNumbers = new FiscalYearInternalOrganisationSequenceNumbersBuilder(session).WithFiscalYear(session.Now().Year).Build();
                    internalOrganisation.AddFiscalYearsInternalOrganisationSequenceNumber(sequenceNumbers);
                }

                sequenceNumbers.PurchaseOrderNumberPrefix = purchaseOrderNumberPrefix;
                sequenceNumbers.PurchaseInvoiceNumberPrefix = purchaseInvoiceNumberPrefix;

                if (purchaseOrderCounterValue != null)
                {
                    sequenceNumbers.PurchaseOrderNumberCounter = new CounterBuilder(session).WithValue(purchaseOrderCounterValue).Build();
                }

                if (purchaseInvoiceCounterValue != null)
                {
                    sequenceNumbers.PurchaseInvoiceNumberCounter = new CounterBuilder(session).WithValue(purchaseInvoiceCounterValue).Build();
                }
            }
            else
            {
                internalOrganisation.PurchaseOrderNumberPrefix = purchaseOrderNumberPrefix;
                internalOrganisation.PurchaseInvoiceNumberPrefix = purchaseInvoiceNumberPrefix;

                if (purchaseOrderCounterValue != null)
                {
                    internalOrganisation.PurchaseOrderNumberCounter = new CounterBuilder(session).WithValue(purchaseOrderCounterValue).Build();
                }

                if (purchaseInvoiceCounterValue != null)
                {
                    internalOrganisation.PurchaseInvoiceNumberCounter = new CounterBuilder(session).WithValue(purchaseInvoiceCounterValue).Build();
                }
            }

            if (requestSequence == new RequestSequences(session).RestartOnFiscalYear)
            {
                var sequenceNumbers = internalOrganisation.FiscalYearsInternalOrganisationSequenceNumbers.FirstOrDefault(v => v.FiscalYear == session.Now().Year);
                if (sequenceNumbers == null)
                {
                    sequenceNumbers = new FiscalYearInternalOrganisationSequenceNumbersBuilder(session).WithFiscalYear(session.Now().Year).Build();
                    internalOrganisation.AddFiscalYearsInternalOrganisationSequenceNumber(sequenceNumbers);
                }

                sequenceNumbers.RequestNumberPrefix = requestNumberPrefix;

                if (requestCounterValue != null)
                {
                    sequenceNumbers.RequestNumberCounter = new CounterBuilder(session).WithValue(requestCounterValue).Build();
                }
            }
            else
            {
                internalOrganisation.RequestNumberPrefix = requestNumberPrefix;

                if (requestCounterValue != null)
                {
                    internalOrganisation.RequestNumberCounter = new CounterBuilder(session).WithValue(requestCounterValue).Build();
                }
            }

            if (quoteSequence == new QuoteSequences(session).RestartOnFiscalYear)
            {
                var sequenceNumbers = internalOrganisation.FiscalYearsInternalOrganisationSequenceNumbers.FirstOrDefault(v => v.FiscalYear == session.Now().Year);
                if (sequenceNumbers == null)
                {
                    sequenceNumbers = new FiscalYearInternalOrganisationSequenceNumbersBuilder(session).WithFiscalYear(session.Now().Year).Build();
                    internalOrganisation.AddFiscalYearsInternalOrganisationSequenceNumber(sequenceNumbers);
                }

                sequenceNumbers.QuoteNumberPrefix = quoteNumberPrefix;

                if (quoteCounterValue != null)
                {
                    sequenceNumbers.QuoteNumberCounter = new CounterBuilder(session).WithValue(quoteCounterValue).Build();
                }
            }
            else
            {
                internalOrganisation.QuoteNumberPrefix = quoteNumberPrefix;

                if (quoteCounterValue != null)
                {
                    internalOrganisation.QuoteNumberCounter = new CounterBuilder(session).WithValue(quoteCounterValue).Build();
                }
            }

            if (workEffortSequence == new WorkEffortSequences(session).RestartOnFiscalYear)
            {
                var sequenceNumbers = internalOrganisation.FiscalYearsInternalOrganisationSequenceNumbers.FirstOrDefault(v => v.FiscalYear == session.Now().Year);
                if (sequenceNumbers == null)
                {
                    sequenceNumbers = new FiscalYearInternalOrganisationSequenceNumbersBuilder(session).WithFiscalYear(session.Now().Year).Build();
                    internalOrganisation.AddFiscalYearsInternalOrganisationSequenceNumber(sequenceNumbers);
                }

                sequenceNumbers.WorkEffortNumberPrefix = workEffortPrefix;
            }
            else
            {
                internalOrganisation.WorkEffortNumberPrefix = workEffortPrefix;
            }

            OwnBankAccount defaultCollectionMethod = null;
            if (bankAccount != null)
            {
                internalOrganisation.AddBankAccount(bankAccount);
                defaultCollectionMethod = new OwnBankAccountBuilder(session).WithBankAccount(bankAccount).WithDescription("Huisbank").Build();
                internalOrganisation.DefaultCollectionMethod = defaultCollectionMethod;
            }

            if (!string.IsNullOrEmpty(emailAddress))
            {
                var email = new EmailAddressBuilder(session)
                    .WithElectronicAddressString(emailAddress)
                    .Build();

                internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(email)
                    .WithContactPurpose(new ContactMechanismPurposes(session).GeneralEmail)
                    .Build());
            }

            internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                .WithUseAsDefault(true)
                .WithContactMechanism(postalAddress1)
                .WithContactPurpose(new ContactMechanismPurposes(session).RegisteredOffice)
                .WithContactPurpose(new ContactMechanismPurposes(session).GeneralCorrespondence)
                .WithContactPurpose(new ContactMechanismPurposes(session).BillingAddress)
                .WithContactPurpose(new ContactMechanismPurposes(session).ShippingAddress)
                .Build());

            internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                .WithUseAsDefault(true)
                .WithContactMechanism(webSite)
                .WithContactPurpose(new ContactMechanismPurposes(session).InternetAddress)
                .Build());

            TelecommunicationsNumber phoneNumber1 = null;
            if (!string.IsNullOrEmpty(phone1))
            {
                phoneNumber1 = new TelecommunicationsNumberBuilder(session).WithContactNumber(phone1).Build();
                if (!string.IsNullOrEmpty(phone1CountryCode))
                {
                    phoneNumber1.CountryCode = phone1CountryCode;
                }
            }

            if (phoneNumber1 != null)
            {
                internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(phoneNumber1)
                    .WithContactPurpose(phone1Purpose)
                    .Build());
            }

            TelecommunicationsNumber phoneNumber2 = null;
            if (!string.IsNullOrEmpty(phone2))
            {
                phoneNumber2 = new TelecommunicationsNumberBuilder(session).WithContactNumber(phone2).Build();
                if (!string.IsNullOrEmpty(phone2CountryCode))
                {
                    phoneNumber2.CountryCode = phone2CountryCode;
                }
            }

            if (phoneNumber2 != null)
            {
                internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(phoneNumber2)
                    .WithContactPurpose(phone2Purpose)
                    .Build());
            }

            if (!string.IsNullOrWhiteSpace(logo))
            {
                var singleton = session.GetSingleton();
                internalOrganisation.LogoImage = new MediaBuilder(session).WithInFileName(logo).WithInData(singleton.GetResourceBytes(logo)).Build();
            }

            Facility facility = null;
            if (facilityName != null)
            {
                facility = new FacilityBuilder(session)
                    .WithName(facilityName)
                    .WithFacilityType(new FacilityTypes(session).Warehouse)
                    .WithOwner(internalOrganisation)
                    .Build();
            }

            var store = new StoreBuilder(session)
                .WithName(storeName)
                .WithDefaultCollectionMethod(defaultCollectionMethod)
                .WithDefaultShipmentMethod(new ShipmentMethods(session).Ground)
                .WithDefaultCarrier(new Carriers(session).Fedex)
                .WithBillingProcess(billingProcess)
                .WithIsImmediatelyPicked(isImmediatelyPicked)
                .WithAutoGenerateShipmentPackage(autoGenerateShipmentPackage)
                .WithIsImmediatelyPacked(isImmediatelyPacked)
                .WithIsAutomaticallyShipped(isAutomaticallyShipped)
                .WithAutoGenerateCustomerShipment(autoGenerateCustomerShipment)
                .WithUseCreditNoteSequence(useCreditNoteSequence)
                .WithInternalOrganisation(internalOrganisation)
                .Build();

            if (defaultCollectionMethod == null)
            {
                store.DefaultCollectionMethod = new CashBuilder(session).Build();
            }
            else
            {
                store.DefaultCollectionMethod = defaultCollectionMethod;
            }

            if (facility != null)
            {
                store.DefaultFacility = facility;
            }

            if (invoiceSequence == new InvoiceSequences(session).RestartOnFiscalYear)
            {
                var sequenceNumbers = new FiscalYearStoreSequenceNumbersBuilder(session).WithFiscalYear(session.Now().Year).Build();

                sequenceNumbers.CreditNoteNumberPrefix = creditNoteNumberPrefix;
                sequenceNumbers.OutgoingShipmentNumberPrefix = outgoingShipmentNumberPrefix;
                sequenceNumbers.SalesInvoiceNumberPrefix = salesInvoiceNumberPrefix;
                sequenceNumbers.SalesOrderNumberPrefix = salesOrderNumberPrefix;

                if (orderCounterValue != null)
                {
                    sequenceNumbers.SalesOrderNumberCounter = new CounterBuilder(session).WithValue(orderCounterValue).Build();
                }

                if (invoiceCounterValue != null)
                {
                    sequenceNumbers.SalesInvoiceNumberCounter = new CounterBuilder(session).WithValue(invoiceCounterValue).Build();
                }

                store.AddFiscalYearsStoreSequenceNumber(sequenceNumbers);
            }
            else
            {
                store.CreditNoteNumberPrefix = creditNoteNumberPrefix;
                store.OutgoingShipmentNumberPrefix = outgoingShipmentNumberPrefix;
                store.SalesInvoiceNumberPrefix = salesInvoiceNumberPrefix;
                store.SalesOrderNumberPrefix = salesOrderNumberPrefix;

                if (orderCounterValue != null)
                {
                    store.SalesOrderNumberCounter = new CounterBuilder(session).WithValue(orderCounterValue).Build();
                }

                if (invoiceCounterValue != null)
                {
                    store.SalesInvoiceNumberCounter = new CounterBuilder(session).WithValue(invoiceCounterValue).Build();
                }
            }

            return internalOrganisation;
        }

        // TODO: Remove Extent
        public Organisation[] InternalOrganisations()
        {
            var internalOrganisations = this.Extent();
            internalOrganisations.Filter.AddEquals(M.InternalOrganisation.IsInternalOrganisation, true);

            return internalOrganisations.ToArray();
        }

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.Meta.ObjectType, M.InventoryStrategy);
            setup.AddDependency(this.Meta.ObjectType, M.Role);
            setup.AddDependency(this.Meta.ObjectType, M.OrganisationRole);
            setup.AddDependency(this.Meta.ObjectType, M.InvoiceSequence);
            setup.AddDependency(this.Meta.ObjectType, M.Singleton);
            setup.AddDependency(this.Meta.ObjectType, M.FacilityType);
        }
    }
}
