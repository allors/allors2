namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("3a5dcec7-308f-48c7-afee-35d38415aa0b")]
    #endregion
    public partial class Organisation : Party, Deletable 
    {
        #region inherited properties

        public PostalAddress GeneralCorrespondence { get; set; }

        public decimal YTDRevenue { get; set; }

        public decimal LastYearsRevenue { get; set; }

        public TelecommunicationsNumber BillingInquiriesFax { get; set; }

        public Qualification[] Qualifications { get; set; }

        public ContactMechanism HomeAddress { get; set; }

        public OrganisationContactRelationship[] InactiveOrganisationContactRelationships { get; set; }

        public ContactMechanism SalesOffice { get; set; }

        public Person[] InactiveContacts { get; set; }

        public PartyContactMechanism[] InactivePartyContactMechanisms { get; set; }

        public TelecommunicationsNumber OrderInquiriesFax { get; set; }

        public Person[] CurrentSalesReps { get; set; }

        public PartyContactMechanism[] PartyContactMechanisms { get; set; }

        public TelecommunicationsNumber ShippingInquiriesFax { get; set; }

        public TelecommunicationsNumber ShippingInquiriesPhone { get; set; }

        public BillingAccount[] BillingAccounts { get; set; }

        public TelecommunicationsNumber OrderInquiriesPhone { get; set; }

        public PartySkill[] PartySkills { get; set; }

        public PartyClassification[] PartyClassifications { get; set; }

        public bool ExcludeFromDunning { get; set; }

        public BankAccount[] BankAccounts { get; set; }

        public Person[] CurrentContacts { get; set; }

        public ContactMechanism BillingAddress { get; set; }

        public ElectronicAddress GeneralEmail { get; set; }

        public ShipmentMethod DefaultShipmentMethod { get; set; }

        public Resume[] Resumes { get; set; }

        public ContactMechanism HeadQuarter { get; set; }

        public ElectronicAddress PersonalEmailAddress { get; set; }

        public TelecommunicationsNumber CellPhoneNumber { get; set; }

        public TelecommunicationsNumber BillingInquiriesPhone { get; set; }

        public string PartyName { get; set; }

        public ContactMechanism OrderAddress { get; set; }

        public ElectronicAddress InternetAddress { get; set; }

        public Media[] Contents { get; set; }

        public CreditCard[] CreditCards { get; set; }

        public PostalAddress ShippingAddress { get; set; }

        public OrganisationContactRelationship[] CurrentOrganisationContactRelationships { get; set; }

        public decimal OpenOrderAmount { get; set; }

        public TelecommunicationsNumber GeneralFaxNumber { get; set; }

        public PaymentMethod DefaultPaymentMethod { get; set; }

        public PartyContactMechanism[] CurrentPartyContactMechanisms { get; set; }

        public TelecommunicationsNumber GeneralPhoneNumber { get; set; }

        public Currency PreferredCurrency { get; set; }

        public VatRegime VatRegime { get; set; }

        public Locale Locale { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Allors
        [Id("6B9E0BC1-5CE5-48D3-BDDB-E364E8566AAA")]
        [AssociationId("35A4477A-C742-417C-9167-7A1306B01373")]
        [RoleId("CCEDB2E8-4428-4FB6-B67B-0443ED7AC3A4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Required]
        [Indexed]
        [Workspace]
        public OrganisationRole[] OrganisationRoles { get; set; }

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
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        #endregion
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
        [Id("786a74b0-015a-47db-8d3a-c790b326cc7d")]
        [AssociationId("6f7363d4-46c5-4bcb-b19c-314733af9e9e")]
        [RoleId("1c339b5d-6f97-41bd-952a-3706d383c3d8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Media LogoImage { get; set; }

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
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public IndustryClassification IndustryClassification { get; set; }

        #region Allors
        [Id("d0ac426e-4775-4f2f-8055-08cb84e8e9bd")]
        [AssociationId("e8677033-8927-4a52-b210-9a98558625ba")]
        [RoleId("8e2fd09a-eda9-47e2-8908-2527e947ffd1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public OrganisationClassification[] OrganisationClassifications { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}







        public void Delete(){}
        #endregion

    }
}