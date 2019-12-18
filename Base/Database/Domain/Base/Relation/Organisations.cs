// <copyright file="Organisations.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.IO;

    using Allors.Meta;

    public partial class Organisations
    {
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
            decimal? purchaseOrderApprovalThresholdLevel2)
        {
            var postalAddress1 = new PostalAddressBuilder(session)
                    .WithAddress1(address)
                    .WithPostalCode(postalCode)
                    .WithLocality(locality)
                    .WithCountry(country)
                    .Build();

            var email = new EmailAddressBuilder(session)
                .WithElectronicAddressString(emailAddress)
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
                .WithInvoiceSequence(new InvoiceSequences(session).EnforcedSequence)
                .WithFiscalYearStartMonth(01)
                .WithFiscalYearStartDay(01)
                .WithDoAccounting(false)
                .WithRequestNumberPrefix(requestNumberPrefix)
                .WithQuoteNumberPrefix(quoteNumberPrefix)
                .WithWorkEffortPrefix(workEffortPrefix)
                .WithPurchaseOrderNumberPrefix(purchaseOrderNumberPrefix)
                .WithPurchaseInvoiceNumberPrefix(purchaseInvoiceNumberPrefix)
                .WithPurchaseOrderNeedsApproval(purchaseOrderNeedsApproval)
                .WithPurchaseOrderApprovalThresholdLevel1(purchaseOrderApprovalThresholdLevel1)
                .WithPurchaseOrderApprovalThresholdLevel2(purchaseOrderApprovalThresholdLevel2)
                .Build();

            if (purchaseOrderCounterValue != null)
            {
                internalOrganisation.PurchaseOrderCounter = new CounterBuilder(session).WithValue(purchaseOrderCounterValue).Build();
            }

            if (purchaseInvoiceCounterValue != null)
            {
                internalOrganisation.PurchaseInvoiceCounter = new CounterBuilder(session).WithValue(purchaseInvoiceCounterValue).Build();
            }

            OwnBankAccount defaultCollectionMethod = null;
            if (bankAccount != null)
            {
                internalOrganisation.AddBankAccount(bankAccount);
                defaultCollectionMethod = new OwnBankAccountBuilder(session).WithBankAccount(bankAccount).WithDescription("Huisbank").Build();
                internalOrganisation.DefaultCollectionMethod = defaultCollectionMethod;
            }

            if (requestCounterValue != null)
            {
                internalOrganisation.RequestCounter = new CounterBuilder(session).WithValue(requestCounterValue).Build();
            }

            if (quoteCounterValue != null)
            {
                internalOrganisation.QuoteCounter = new CounterBuilder(session).WithValue(quoteCounterValue).Build();
            }

            internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                .WithUseAsDefault(true)
                .WithContactMechanism(email)
                .WithContactPurpose(new ContactMechanismPurposes(session).GeneralEmail)
                .Build());
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

            if (File.Exists(logo))
            {
                internalOrganisation.LogoImage = FileReader.CreateMedia(session, logo);
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
                .WithOutgoingShipmentNumberPrefix(outgoingShipmentNumberPrefix)
                .WithSalesInvoiceNumberPrefix(salesInvoiceNumberPrefix)
                .WithSalesOrderNumberPrefix(salesOrderNumberPrefix)
                .WithDefaultCollectionMethod(defaultCollectionMethod)
                .WithCreditNoteNumberPrefix(creditNoteNumberPrefix)
                .WithDefaultShipmentMethod(new ShipmentMethods(session).Ground)
                .WithDefaultCarrier(new Carriers(session).Fedex)
                .WithBillingProcess(billingProcess)
                .WithSalesInvoiceCounter(new CounterBuilder(session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build())
                .WithIsImmediatelyPicked(isImmediatelyPicked)
                .WithAutoGenerateShipmentPackage(autoGenerateShipmentPackage)
                .WithIsImmediatelyPacked(isImmediatelyPacked)
                .WithIsAutomaticallyShipped(isAutomaticallyShipped)
                .WithAutoGenerateCustomerShipment(autoGenerateCustomerShipment)
                .WithIsAutomaticallyReceived(isAutomaticallyReceived)
                .WithAutoGeneratePurchaseShipment(autoGeneratePurchaseShipment)
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

            if (orderCounterValue != null)
            {
                store.SalesOrderCounter = new CounterBuilder(session).WithValue(orderCounterValue).Build();
            }

            if (invoiceCounterValue != null)
            {
                store.SalesInvoiceCounter = new CounterBuilder(session).WithValue(invoiceCounterValue).Build();
            }

            return internalOrganisation;
        }

        /// <summary>
        /// Returns an array of Organisations.
        /// </summary>
        /// <returns></returns>
        public Organisation[] InternalOrganisations()
        {
            var internalOrganisations = this.Extent();
            internalOrganisations.Filter.AddEquals(M.Organisation.IsInternalOrganisation, true);

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
