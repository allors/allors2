// <copyright file="InternalOrganisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("c81441c8-9ac9-440e-a926-c96230b2701f")]
    #endregion
    public partial interface InternalOrganisation : Party
    {
        #region inherited properties

        #endregion

        #region Allors
        [Id("E6BBEC1C-5855-4D22-97D2-BF62B853DC7E")]
        [AssociationId("FE5822F4-4E05-4059-B4EB-56AAB4356384")]
        [RoleId("AF8BE64A-A610-4917-9B21-D277419756F9")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToMany)]
        PaymentMethod[] PaymentMethods { get; set; }

        #region Allors
        [Id("356044B9-47FA-4EEB-955E-B75C2A21EA2E")]
        [AssociationId("DCAC5A59-8918-482C-B453-6340A7EFAC7A")]
        [RoleId("77F0A7ED-F548-4F62-AD53-3CE616B2476C")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        PaymentMethod DefaultCollectionMethod { get; set; }

        #region Allors
        [Id("1a986cbf-b7db-4850-af06-d96e1339beb7")]
        [AssociationId("fc69f819-2ca0-4ac4-b36e-cfff791679a1")]
        [RoleId("d6e816ae-50c5-41c7-8877-9f4c30208d47")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Counter PurchaseInvoiceCounter { get; set; }

        #region Allors
        [Id("23aee857-9cea-481c-a4a3-72dd8b808d71")]
        [AssociationId("61c64e66-4647-439d-9efa-28500319e8ca")]
        [RoleId("4d68b2fe-d5b4-45f8-9e68-377d75f3401d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        InvoiceSequence InvoiceSequence { get; set; }

        #region Allors
        [Id("44f165ed-a6ca-4979-9046-a0f7391bef7d")]
        [AssociationId("5e581649-0b45-498f-b274-fdcc05bb3894")]
        [RoleId("1069343b-d6d5-4418-abf9-688b9dcec6d0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Counter PurchaseOrderCounter { get; set; }

        #region Allors
        [Id("7e210c5e-a68b-4ea0-b019-1dd452d8e407")]
        [AssociationId("b9c41192-1666-44c0-9365-b24df29a2cdf")]
        [RoleId("dfe3042c-babb-416b-a414-5dda0a2958c0")]
        #endregion
        [Workspace]
        int FiscalYearStartDay { get; set; }

        #region Allors
        [Id("ad7c8532-59d2-4668-bd9f-6c67ddc4e4bc")]
        [AssociationId("7028e722-f29d-42f6-b7c5-d73c2e99c907")]
        [RoleId("48454368-06c6-4e64-876b-b5e4268407a4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Counter SubAccountCounter { get; set; }

        #region Allors
        [Id("A49663B5-A432-41FA-BBCA-8368D1B9D53D")]
        [AssociationId("27E7D099-3D94-4DCF-AFD2-9143A4815ECC")]
        [RoleId("A82306DA-FFA4-48D5-81EC-B657E2CB99C1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Counter RequestCounter { get; set; }

        #region Allors
        [Id("5b64cf9d-e990-491e-b009-3481d73db67e")]
        [AssociationId("40296302-a559-4014-ba68-929d4238f4d8")]
        [RoleId("5b966f3d-cc80-44f4-b255-87182aa796d4")]
        #endregion
        [Workspace]
        int FiscalYearStartMonth { get; set; }

        #region Allors
        [Id("86dd32e2-74e7-4ced-bfbd-4e1fdc723588")]
        [AssociationId("ab8ace70-5d23-4ba0-83f7-471a8dbefeb6")]
        [RoleId("fa1a381c-a895-44d2-a028-860486b1d1af")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Counter IncomingShipmentCounter { get; set; }

        #region Allors
        [Id("ba00c0d2-6067-4584-bdc4-e6c72be77232")]
        [AssociationId("687afb94-445e-41e2-bf22-deaac5b40e5a")]
        [RoleId("9fa5512e-098a-40fd-a821-38a5f5f280f5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Counter QuoteCounter { get; set; }

        #region Allors
        [Id("d5645df8-2b10-435d-8e47-57b5d268541a")]
        [AssociationId("4d145acb-007a-46b9-98a8-a86888221e28")]
        [RoleId("c2fe67e4-a100-4d96-b4ff-df1ec73db5fe")]
        #endregion
        [Required]
        [Workspace]
        bool DoAccounting { get; set; }

        #region Allors
        [Id("00bf781c-c874-44fe-ae60-d6609075b1c0")]
        [AssociationId("3b99af1e-e6c3-498b-aabd-78d6e82c8819")]
        [RoleId("b9a508e2-2931-4ddc-ab34-947d19c2d742")]
        #endregion
        [Size(256)]
        [Workspace]
        string PurchaseOrderNumberPrefix { get; set; }

        #region Allors
        [Id("01d4f5d8-da57-4524-b35f-69a1a4adfa1c")]
        [AssociationId("84ff4f9a-b1d3-4e2c-aff6-52a9f75e874a")]
        [RoleId("ee1b0251-ba57-490a-bdc9-c4e8fd6142ce")]
        #endregion
        [Workspace]
        [Size(256)]
        string TransactionReferenceNumber { get; set; }

        #region Allors
        [Id("0994b73e-8d4c-4fa4-aca2-287449b22ca7")]
        [AssociationId("17a9138e-76c8-42e1-85b8-7af73b551a22")]
        [RoleId("09fbb64d-c32e-4734-8df9-6e741a5070a5")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Indexed]
        JournalEntryNumber[] JournalEntryNumbers { get; set; }

        #region Allors
        [Id("1a2533cb-9b75-4597-83ab-9bbfc49e0103")]
        [AssociationId("b4cb12ba-0ea1-41e5-945a-030503bf2c7b")]
        [RoleId("031ba0aa-32e4-470c-9a79-fae65cace2f2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        Country EuListingState { get; set; }

        #region Allors
        [Id("219a1d97-9615-47c5-bc4d-20a7d37313bd")]
        [AssociationId("4fd6a9e2-174c-41db-b519-44c317de0f96")]
        [RoleId("45f4d069-5faf-45cf-b097-21f58fab4097")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        AccountingPeriod ActualAccountingPeriod { get; set; }

        #region Allors
        [Id("293758d7-cc0a-4f1c-b122-84f609a828c2")]
        [AssociationId("afe9d80b-2984-4e46-b45c-e5a25af3bccd")]
        [RoleId("97aa32a6-69c0-4cc5-9e42-98907ff6c45f")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        PaymentMethod[] ActiveCollectionMethods { get; set; }

        #region Allors
        [Id("37b4bf2c-5b09-42b0-84d9-59b57793cf37")]
        [AssociationId("22aff7e0-1b45-4f06-b281-19cbf0d1c511")]
        [RoleId("dcd31b64-e449-4833-91cb-8237bdb71b78")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal MaximumAllowedPaymentDifference { get; set; }

        #region Allors
        [Id("3b32c442-9cbc-41d8-8eb2-2ae41beca2c4")]
        [AssociationId("eda352cd-00cc-4d04-99cf-f7ad667cb20a")]
        [RoleId("b8211575-f4c9-44c8-89b2-bd122704f098")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        CostCenterSplitMethod CostCenterSplitMethod { get; set; }

        #region Allors
        [Id("4fc741ef-fe95-49a8-8bcd-8ff43092db88")]
        [AssociationId("0e58cd98-94e6-42ab-8501-394f8b8d3624")]
        [RoleId("d5096d5d-a443-4667-95dd-7184f348e55c")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        GeneralLedgerAccount SalesPaymentDifferencesAccount { get; set; }

        #region Allors
        [Id("5b64038f-5ad9-46a6-9af6-b95819ac9830")]
        [AssociationId("753f6fa9-ff10-402c-9812-d2c738d35dbb")]
        [RoleId("8411c910-14d4-4629-aa39-c58602a799d4")]
        #endregion
        [Size(256)]
        [Workspace]
        string PurchaseTransactionReferenceNumber { get; set; }

        #region Allors
        [Id("63c9ceb1-d583-41e1-a9a9-0c2576e9adfc")]
        [AssociationId("fd1ac2ff-6869-44bd-9b4e-05a3b14fbad9")]
        [RoleId("41058049-5c44-47b7-bbb9-34ab0bdcfbcb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        CostOfGoodsSoldMethod CostOfGoodsSoldMethod { get; set; }

        #region Allors
        [Id("77ae5145-791a-4ef0-94cc-6c9683b02f13")]
        [AssociationId("0da6cc82-b538-4346-bb48-19b02223a566")]
        [RoleId("bef210d2-7af6-4653-872b-a5eebba2af87")]
        #endregion
        [Workspace]
        bool VatDeactivated { get; set; }

        #region Allors
        [Id("848f3098-ce8b-400c-9775-85c00ac68f28")]
        [AssociationId("44649646-ccf8-48b7-9ebf-a09df75d23fc")]
        [RoleId("3d5a74b3-cb62-4803-8115-fde43a648af5")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        GeneralLedgerAccount[] GeneralLedgerAccounts { get; set; }

        #region Allors
        [Id("859d95c2-7321-4408-bcd1-405dc0b31efc")]
        [AssociationId("47674006-74f4-497e-b9b6-1d5cff1ada0f")]
        [RoleId("0dc922be-86d0-469e-a9d2-5bf19a910994")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Counter AccountingTransactionCounter { get; set; }

        #region Allors
        [Id("89f4907d-4a10-428d-9e6b-ef9fb045c019")]
        [AssociationId("a0fd3167-5d4e-400f-b593-1497fce5d024")]
        [RoleId("0ab03957-8140-4e1c-8a13-a9647f6c9e47")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        GeneralLedgerAccount RetainedEarningsAccount { get; set; }

        #region Allors
        [Id("9d6aaa81-9f97-427e-9f46-1f1e93748248")]
        [AssociationId("b251ce10-b4ac-45da-bc57-0a42e75f3660")]
        [RoleId("6ffd36cd-37de-444b-93b6-4128de34254f")]
        #endregion
        [Size(256)]
        [Workspace]
        string PurchaseInvoiceNumberPrefix { get; set; }

        #region Allors
        [Id("ab2004c1-fd91-4298-87cd-532a6fe5efb0")]
        [AssociationId("b53b80a4-92f5-4e08-a9fb-86bf2bd9572e")]
        [RoleId("7fe218f4-975d-40f4-82b6-b43cb308aff4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        GeneralLedgerAccount SalesPaymentDiscountDifferencesAccount { get; set; }

        #region Allors
        [Id("afbaffe6-b03c-463e-b074-08b32641b482")]
        [AssociationId("4a02dff3-4bd3-4453-b2fa-e6e79f1b18b0")]
        [RoleId("334cb83a-410e-4abb-8b51-9dd19e2fc21b")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        AccountingTransactionNumber[] AccountingTransactionNumbers { get; set; }

        #region Allors
        [Id("b8af8dce-d0e8-4e16-8d72-e56b920a04b4")]
        [AssociationId("57168fae-0b31-4370-afff-ab5a02c9a8ee")]
        [RoleId("d4c44bcf-c38b-46b9-86f6-462a48714389")]
        #endregion
        [Size(256)]
        [Workspace]
        string TransactionReferenceNumberPrefix { get; set; }

        #region Allors
        [Id("d0ebaa65-260a-4511-a137-89f25016f12c")]
        [AssociationId("70e0b1a9-4c82-4c47-a6b8-df7a62423a08")]
        [RoleId("1413c3f9-a226-4356-8bcc-6f8ae1963a6e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        GeneralLedgerAccount PurchasePaymentDifferencesAccount { get; set; }

        #region Allors
        [Id("d2ad57d5-de30-4bc0-90a7-9aea7a9da8c7")]
        [AssociationId("dbd28e59-d5d6-4d95-aef1-6881e0fe2d48")]
        [RoleId("2e28606f-5708-4c0f-bdfa-04019a0e97d9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        GeneralLedgerAccount SuspenceAccount { get; set; }

        #region Allors
        [Id("d48ef8bb-064b-4360-8162-a138fb601761")]
        [AssociationId("91252cf4-eca3-4b9d-b06d-5df795c3709c")]
        [RoleId("3ef1f904-5b02-4f00-8486-2e452668be67")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        GeneralLedgerAccount NetIncomeAccount { get; set; }

        #region Allors
        [Id("dd008dfe-a219-42ab-bc08-d091da3f8ea4")]
        [AssociationId("77ce8418-a00b-46d0-ab7b-4a782b7387da")]
        [RoleId("a5ac9bc1-5323-41ec-a791-d4ecc7d0eee8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        GeneralLedgerAccount PurchasePaymentDiscountDifferencesAccount { get; set; }

        #region Allors
        [Id("e9af1ca5-d24f-4af2-8687-833744941b24")]
        [AssociationId("ca041d3d-7adf-43da-ac42-c749633bd9b8")]
        [RoleId("4b50a77e-4343-415b-ad45-6cd074b681b5")]
        #endregion
        [Size(256)]
        [Workspace]
        string QuoteNumberPrefix { get; set; }

        #region Allors
        [Id("ec8e7400-0088-4237-af32-a687e1c45d77")]
        [AssociationId("3ee2e0b2-6835-490b-937a-a853c85dd3e4")]
        [RoleId("c70fd771-87a6-4e97-98ea-34c2c0265450")]
        #endregion
        [Size(256)]
        [Workspace]
        string PurchaseTransactionReferenceNumberPrefix { get; set; }

        #region Allors
        [Id("f353e7ef-d24d-4a27-8ec9-e930ef936240")]
        [AssociationId("cf556dfd-4b43-48e4-8243-df2650d8ce97")]
        [RoleId("6b7bda76-5fe7-4526-8c79-b42324fd4090")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        GeneralLedgerAccount CalculationDifferencesAccount { get; set; }

        #region Allors
        [Id("fe96e14b-9dbd-4497-935f-f605abd2ada7")]
        [AssociationId("68f9e5fd-7398-456b-bcb8-27b23dbce3f1")]
        [RoleId("3fc3517d-0d36-4401-a56f-ac3a83f1f892")]
        #endregion
        [Workspace]
        [Size(256)]
        string IncomingShipmentNumberPrefix { get; set; }

        #region Allors
        [Id("06FABCD6-EFA4-45DD-B76C-8F791A0E10EF")]
        [AssociationId("83D3C799-B76E-481D-90D4-FCC5DF9FABCE")]
        [RoleId("BF7CB693-CCE3-4BDA-9A7F-C3165AD5134D")]
        #endregion
        [Size(256)]
        [Workspace]
        string RequestNumberPrefix { get; set; }

        #region Allors
        [Id("224518F0-2014-4BF4-B10A-406821A0FD39")]
        [AssociationId("36FDEA56-5918-4F68-895C-65449FE4C579")]
        [RoleId("8DC69D34-FB39-4979-8EEA-CBF09C0996F0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        Party[] CurrentCustomers { get; set; }

        #region Allors
        [Id("D4B532F9-12F0-4B51-AF8E-0E2160A2488E")]
        [AssociationId("CF65C981-5A4D-4FD5-947F-C82EF07C4634")]
        [RoleId("86D03D25-89C9-48D6-90B9-962B566741C3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        Organisation[] CurrentSuppliers { get; set; }

        #region Allors
        [Id("0ac44c21-6a2c-4162-9d77-fe1b16b60b73")]
        [AssociationId("4d61b711-7aab-4162-bb31-74db09f666fe")]
        [RoleId("0da9c561-a6fc-4fea-aee3-5c24a2b08aea")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        GeneralLedgerAccount GlAccount { get; set; }

        #region Allors
        [Id("9a2ab89e-c3bc-4b6b-a82d-417dc21c8f9e")]
        [AssociationId("fcc6e653-3787-44a0-8a3a-35e80e232a02")]
        [RoleId("31549cf9-6418-4d19-96b0-5813cc964491")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Party[] ActiveCustomers { get; set; }

        #region Allors
        [Id("cd40057a-5211-4289-a4ef-c30aa4049957")]
        [AssociationId("5dd54980-fa14-434c-80fc-64dec203fd8b")]
        [RoleId("ca87441c-6ce7-4041-bdd4-ca83f3b19289")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Derived]
        [Indexed]
        Person[] ActiveEmployees { get; set; }

        #region Allors
        [Id("e09976e8-dc99-4539-9b0b-0bbe98cc5404")]
        [AssociationId("0d828c12-82bd-4b37-96c8-68997a7c2f48")]
        [RoleId("3d362cd1-d49c-422f-9722-7276a6ee07c4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Derived]
        [Indexed]
        Party[] ActiveSuppliers { get; set; }

        #region Allors
        [Id("02076C50-183B-4657-BE2F-9CF4872E9989")]
        [AssociationId("1C6A6309-836E-4DD5-9F4A-B9EEFAC1D683")]
        [RoleId("808ABBA5-5635-428D-B4EA-C7140AA3717C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        Template ProductQuoteTemplate { get; set; }

        #region Allors
        [Id("7FD61BF5-AC01-405C-A100-5DA3F2861B81")]
        [AssociationId("A5156B1A-C7CB-4AB9-91A6-FCFD129A8C4A")]
        [RoleId("D5C6F295-6A4A-426B-A233-AE7F41080FD6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        Template SalesOrderTemplate { get; set; }

        #region Allors
        [Id("8DF822D1-B7B8-4211-B941-69664FAA3537")]
        [AssociationId("63C40172-6402-4411-905F-56E0CD2DFC74")]
        [RoleId("E3B9B581-657D-4311-8723-17AA6224F327")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        Template PurchaseOrderTemplate { get; set; }

        #region Allors
        [Id("EE600466-BF26-4155-9FF1-0B86BA136AD1")]
        [AssociationId("335979F7-A675-4AE5-AA74-F7C5B051893B")]
        [RoleId("2F3484ED-F709-493E-960E-F630043611D1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        Template PurchaseInvoiceTemplate { get; set; }

        #region Allors
        [Id("A8D44A4A-9C82-44A7-BBBA-117A4F7D261B")]
        [AssociationId("3B55BFB3-5531-4659-A7E1-20D39E68FD59")]
        [RoleId("F9C61F15-C32C-48D1-85E0-490A9F540EB6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        Template SalesInvoiceTemplate { get; set; }

        #region Allors
        [Id("9505E487-973D-4556-9C79-3538E7FE1C8B")]
        [AssociationId("F4F9D9FF-6DF0-4B75-9022-DE4DF200A1F4")]
        [RoleId("CBBD4779-D96D-4DCD-A2C3-8E5365B9AEA0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        Template WorkTaskTemplate { get; set; }

        /// <summary>
        /// Gets or sets the WorkEffortCounter to be used to populate the WorkEfforNumber for WorkEffort objects.
        /// </summary>
        #region Allors
        [Id("72BA7C7A-9EA5-4327-86AF-ED77041E19AE")]
        [AssociationId("8D657926-5B1E-455F-9010-FD04ABD9192F")]
        [RoleId("7CEBA57D-3E60-47A8-9806-11B62C700C57")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Counter WorkEffortCounter { get; set; }

        /// <summary>
        /// Gets or sets the WorkEffortPrefix to be used before the WorkEfforNumber for WorkEffort objects.
        /// </summary>
        #region Allors
        [Id("DFA0E963-7F13-41C0-B1CC-3BEBE1F951F1")]
        [AssociationId("BC581E81-852C-4C41-91C7-78E195BDE869")]
        [RoleId("9E71EAAC-7973-4C65-AC43-0DCD2D9AEF83")]
        #endregion
        [Size(256)]
        [Workspace]
        string WorkEffortPrefix { get; set; }

        /// <summary>
        /// Gets or sets a flag to indicate if this InternalOrganisation Requires Existing WorkEffortPartyAssignment
        /// objects to exist before allowing TimeEntry objects to attach to WorkEffort objects.
        /// </summary>
        #region Allors
        [Id("37FEB213-9427-4A41-8050-DBFE9A33D03F")]
        [AssociationId("F7746FC5-4767-4356-9F6F-BA0009668C05")]
        [RoleId("83B1D233-C1C7-449F-98CF-45DFF2DDF2C3")]
        #endregion
        [Workspace]
        bool RequireExistingWorkEffortPartyAssignment { get; set; }

        #region Allors
        [Id("786a74b0-015a-47db-8d3a-c790b326cc7d")]
        [AssociationId("6f7363d4-46c5-4bcb-b19c-314733af9e9e")]
        [RoleId("1c339b5d-6f97-41bd-952a-3706d383c3d8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Media LogoImage { get; set; }

        #region Allors
        [Id("7E926007-DED3-4B53-B9C5-4B4D25EC9AF7")]
        [AssociationId("56A50C32-AB99-40A1-9393-99CBFBD61B6E")]
        [RoleId("18E2BEB9-343B-4C82-9D39-DB013749DB1F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        Person[] SalesReps { get; set; }

        #region Allors
        [Id("0C328E5C-E3A8-44B9-BD4D-0DDABBFC9728")]
        #endregion
        void StartNewFiscalYear();

        #region Allors
        [Id("C5BC0A23-6E54-4EEE-A721-92B3D4F90459")]
        [AssociationId("5B5CA4C7-D642-4AF0-ADB5-A9C6611B7158")]
        [RoleId("6B687A56-E279-4004-A743-2565D598CC5B")]
        #endregion
        [Required]
        [Workspace]
        bool PurchaseOrderNeedsApproval { get; set; }

        #region Allors
        [Id("CE7E4BAA-2324-4E5D-A8A7-F05DF34CFF91")]
        [AssociationId("A411F97A-56FE-4C8B-9CB7-216484C7C5EA")]
        [RoleId("8683265F-33D4-4D03-99EC-4F356F73FE98")]
        #endregion
        [Workspace]
        decimal PurchaseOrderApprovalThresholdLevel1 { get; set; }

        #region Allors
        [Id("350C37ED-D2DD-4293-A4E6-BAB9C0D95841")]
        [AssociationId("20F750C3-3973-499A-BC67-69CC8C02E16C")]
        [RoleId("A158C47E-62E2-4B56-A12B-1B837071F08E")]
        #endregion
        [Workspace]
        decimal PurchaseOrderApprovalThresholdLevel2 { get; set; }

        #region inherited methods

        #endregion
    }
}
