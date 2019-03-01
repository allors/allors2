namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("3a5dcec7-308f-48c7-afee-35d38415aa0b")]
    #endregion
    public partial class Organisation : InternalOrganisation, Deletable, Versioned
    {
        #region inherited properties
        public string PurchaseOrderNumberPrefix { get; set; }

        public string TransactionReferenceNumber { get; set; }

        public JournalEntryNumber[] JournalEntryNumbers { get; set; }

        public Country EuListingState { get; set; }

        public PaymentMethod[] PaymentMethods { get; set; }
        public PaymentMethod DefaultCollectionMethod { get; set; }

        public Counter PurchaseInvoiceCounter { get; set; }

        public AccountingPeriod ActualAccountingPeriod { get; set; }

        public InvoiceSequence InvoiceSequence { get; set; }

        public PaymentMethod[] ActiveCollectionMethods { get; set; }

        public decimal MaximumAllowedPaymentDifference { get; set; }

        public CostCenterSplitMethod CostCenterSplitMethod { get; set; }

        public Counter PurchaseOrderCounter { get; set; }

        public GeneralLedgerAccount SalesPaymentDifferencesAccount { get; set; }

        public string PurchaseTransactionReferenceNumber { get; set; }

        public int FiscalYearStartMonth { get; set; }

        public CostOfGoodsSoldMethod CostOfGoodsSoldMethod { get; set; }

        public bool VatDeactivated { get; set; }

        public int FiscalYearStartDay { get; set; }

        public GeneralLedgerAccount[] GeneralLedgerAccounts { get; set; }

        public Counter AccountingTransactionCounter { get; set; }

        public Counter IncomingShipmentCounter { get; set; }

        public GeneralLedgerAccount RetainedEarningsAccount { get; set; }

        public string PurchaseInvoiceNumberPrefix { get; set; }

        public GeneralLedgerAccount SalesPaymentDiscountDifferencesAccount { get; set; }

        public Counter SubAccountCounter { get; set; }

        public AccountingTransactionNumber[] AccountingTransactionNumbers { get; set; }

        public string TransactionReferenceNumberPrefix { get; set; }

        public Counter QuoteCounter { get; set; }

        public Counter RequestCounter { get; set; }

        public GeneralLedgerAccount PurchasePaymentDifferencesAccount { get; set; }

        public GeneralLedgerAccount SuspenceAccount { get; set; }

        public GeneralLedgerAccount NetIncomeAccount { get; set; }

        public bool DoAccounting { get; set; }

        public GeneralLedgerAccount PurchasePaymentDiscountDifferencesAccount { get; set; }

        public string QuoteNumberPrefix { get; set; }

        public string PurchaseTransactionReferenceNumberPrefix { get; set; }

        public GeneralLedgerAccount CalculationDifferencesAccount { get; set; }

        public string IncomingShipmentNumberPrefix { get; set; }

        public string RequestNumberPrefix { get; set; }

        public Party[] CurrentCustomers { get; set; }

        public Organisation[] CurrentSuppliers { get; set; }

        public Person[] SalesReps { get; set; }

        public GeneralLedgerAccount GlAccount { get; set; }

        public Party[] ActiveCustomers { get; set; }

        public Person[] ActiveEmployees { get; set; }

        public Party[] ActiveSuppliers { get; set; }

        public string PartyName { get; set; }

        public PostalAddress GeneralCorrespondence { get; set; }

        public TelecommunicationsNumber BillingInquiriesFax { get; set; }

        public Qualification[] Qualifications { get; set; }

        public ContactMechanism HomeAddress { get; set; }

        public ContactMechanism SalesOffice { get; set; }

        public PartyContactMechanism[] InactivePartyContactMechanisms { get; set; }

        public TelecommunicationsNumber OrderInquiriesFax { get; set; }

        public Person[] CurrentSalesReps { get; set; }

        public PartyContactMechanism[] PartyContactMechanisms { get; set; }

        public PartyRelationship[] InactivePartyRelationships { get; set; }

        public Person[] CurrentContacts { get; set; }

        public Person[] InactiveContacts { get; set; }

        public TelecommunicationsNumber ShippingInquiriesFax { get; set; }

        public TelecommunicationsNumber ShippingInquiriesPhone { get; set; }

        public BillingAccount[] BillingAccounts { get; set; }

        public TelecommunicationsNumber OrderInquiriesPhone { get; set; }

        public PartySkill[] PartySkills { get; set; }

        public PartyClassification[] PartyClassifications { get; set; }

        public BankAccount[] BankAccounts { get; set; }

        public ContactMechanism BillingAddress { get; set; }

        public EmailAddress GeneralEmail { get; set; }

        public ShipmentMethod DefaultShipmentMethod { get; set; }

        public Resume[] Resumes { get; set; }

        public ContactMechanism HeadQuarter { get; set; }

        public EmailAddress PersonalEmailAddress { get; set; }

        public TelecommunicationsNumber CellPhoneNumber { get; set; }

        public TelecommunicationsNumber BillingInquiriesPhone { get; set; }

        public ContactMechanism OrderAddress { get; set; }

        public ElectronicAddress InternetAddress { get; set; }

        public Media[] Contents { get; set; }

        public CreditCard[] CreditCards { get; set; }

        public PostalAddress ShippingAddress { get; set; }

        public TelecommunicationsNumber GeneralFaxNumber { get; set; }

        public PartyContactMechanism[] CurrentPartyContactMechanisms { get; set; }

        public PartyRelationship[] CurrentPartyRelationships { get; set; }

        public TelecommunicationsNumber GeneralPhoneNumber { get; set; }

        public Currency PreferredCurrency { get; set; }

        public VatRegime VatRegime { get; set; }

        public Agreement[] Agreements { get; set; }

        public CommunicationEvent[] CommunicationEvents { get; set; }

        public PaymentMethod DefaultPaymentMethod { get; set; }

        public Locale Locale { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Template ProductQuoteTemplate { get; set; }

        public Template SalesOrderTemplate { get; set; }

        public Template SalesInvoiceTemplate { get; set; }

        public Template WorkTaskTemplate { get; set; }

        public PartyRate[] PartyRates { get; set; }

        public Counter WorkEffortCounter { get; set; }

        public string WorkEffortPrefix { get; set; }

        public bool RequireExistingWorkEffortPartyAssignment { get; set; }

        public Media LogoImage { get; set; }
        #endregion

        #region Versioning
        #region Allors
        [Id("275CFF8F-AD72-4237-AEBD-158A72650D25")]
        [AssociationId("730A672E-41F6-4BAB-B918-9A7852B4E50E")]
        [RoleId("1E1B3E3B-C471-47EB-AB96-C01165E39FB4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public OrganisationVersion CurrentVersion { get; set; }

        #region Allors
        [Id("9BF20468-BF1D-410D-8D83-EBA561A5F066")]
        [AssociationId("B503EBD6-E4A8-4E5D-8005-1823079BEF2D")]
        [RoleId("DAD9D684-9821-48BD-9829-C9F5BE1E740A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public OrganisationVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("25E8BD32-807F-4484-8561-2AA34B425C6F")]
        [AssociationId("F2F0A84E-0EBA-437F-9F2D-431CB7FA19B1")]
        [RoleId("4C553A51-9B83-4D05-ACAF-4884796072B3")]
        #endregion
        [Required]
        [Workspace]
        public bool IsInternalOrganisation { get; set; }

        #region Allors
        [Id("8FB7635C-6C06-43E8-9B6C-A760C7205804")]
        [AssociationId("A1D28C43-6C7B-4872-878C-75E91285AB9A")]
        [RoleId("D338444E-1D92-47A2-9A27-83296C93482E")]
        #endregion
        [Required]
        [Workspace]
        public bool IsManufacturer { get; set; }

        #region Allors
        [Id("124181AE-3BB2-42E2-A27B-D9B811824282")]
        [AssociationId("E85FE7C7-0B10-4ABB-AD84-1B920DB767E2")]
        [RoleId("5E5718DC-1060-4BF1-9E76-B2D2927E028D")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Derived]
        public SecurityToken ContactsSecurityToken { get; set; }

        #region Allors
        [Id("98D13035-810F-4550-8EDE-8514FDFD275D")]
        [AssociationId("4B77E890-1061-49E0-AE29-A28EB0C6A52E")]
        [RoleId("18376FC8-224A-4F16-BBC8-006B04F8C184")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Derived]
        public AccessControl ContactsAccessControl { get; set; }

        #region Allors
        [Id("980631CB-CC72-4264-87E5-B65DC6ABBB4D")]
        [AssociationId("53B3AFB0-D926-477D-9591-D537B00CCCBD")]
        [RoleId("0CCF23A0-F65B-469B-9886-3E554A02A353")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        public UserGroup OwnerUserGroup { get; set; }

        #region Allors
        [Id("1c8bf2e3-6794-47c8-990c-f124d47653fb")]
        [AssociationId("d60f70d2-a17e-47d9-bccc-7971f5ef776d")]
        [RoleId("d0f185d6-1ae2-40bf-a95e-6fde7ae10fa9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public LegalForm LegalForm { get; set; }

        #region Allors
        [Id("2cc74901-cda5-4185-bcd8-d51c745a8437")]
        [AssociationId("896a4589-4caf-4cd2-8365-c4200b12f519")]
        [RoleId("baa30557-79ff-406d-b374-9d32519b2de7")]
        #endregion
        [Indexed]
        [Required]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("4cc8bc02-8305-4bd3-b0c7-e9b3ecaf4bd2")]
        [AssociationId("c2be4896-2eae-40fa-9300-b548741407f2")]
        [RoleId("a26de636-8efa-4df4-b56d-225ac25f31a8")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        public UserGroup ContactsUserGroup { get; set; }

        #region Allors
        [Id("813633df-c6cb-44a6-9fdf-579aa8180ebd")]
        [AssociationId("4e4c1ca5-43e1-4567-8f1e-636197ca72b7")]
        [RoleId("e5c40212-c5c5-44a1-8f18-f5d3dbeec9ca")]
        #endregion
        [Size(256)]
        [Workspace]
        public string TaxNumber { get; set; }

        #region Allors
        [Id("a5318bd4-da7d-48bd-9d41-00c3261caa09")]
        [AssociationId("baae72eb-acf3-4dce-b480-fce90e124de3")]
        [RoleId("6458017c-a4bf-4815-a486-66d654f3801a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public IndustryClassification[] IndustryClassifications { get; set; }

        #region Allors
        [Id("d0ac426e-4775-4f2f-8055-08cb84e8e9bd")]
        [AssociationId("e8677033-8927-4a52-b210-9a98558625ba")]
        [RoleId("8e2fd09a-eda9-47e2-8908-2527e947ffd1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public CustomOrganisationClassification[] CustomClassifications { get; set; }

        #region Allors
        [Id("c79070fc-2c7d-440b-80ce-f86796c59a14")]
        [AssociationId("8bb86356-0b10-4e77-bbbb-d4d33230c3a9")]
        [RoleId("8c72ca39-b408-4623-8a90-54c3b3630e6b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public OrganisationContactRelationship[] CurrentOrganisationContactRelationships { get; set; }

        #region Allors
        [Id("1bf7b758-2b58-4f82-a6a1-a8d5991d3d9d")]
        [AssociationId("240a4c51-86f3-47c7-a28d-7c8fd7b5d68e")]
        [RoleId("08655bdf-9abb-404d-a4d4-739896199bc3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public OrganisationContactRelationship[] InactiveOrganisationContactRelationships { get; set; }

        #region inherited methods

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void OnPreFinalize(){} public void OnFinalize()
        {
            
        }

        public void OnPostFinalize()
        {
            
        }

        public void Delete(){}

        public void StartNewFiscalYear() {}
        #endregion
    }
}