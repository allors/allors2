namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("E1AFA103-7032-416B-AC7B-274A7E35381A")]
    #endregion
    public partial class OrganisationVersion : PartyVersion
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public string Comment { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string PartyName { get; set; }

        public PostalAddress GeneralCorrespondence { get; set; }

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

        public ContactMechanism OrderAddress { get; set; }

        public ElectronicAddress InternetAddress { get; set; }

        public Media[] Contents { get; set; }

        public CreditCard[] CreditCards { get; set; }

        public PostalAddress ShippingAddress { get; set; }

        public OrganisationContactRelationship[] CurrentOrganisationContactRelationships { get; set; }

        public TelecommunicationsNumber GeneralFaxNumber { get; set; }

        public PartyContactMechanism[] CurrentPartyContactMechanisms { get; set; }

        public TelecommunicationsNumber GeneralPhoneNumber { get; set; }

        public Currency PreferredCurrency { get; set; }

        public VatRegime VatRegime { get; set; }

        public Agreement[] Agreements { get; set; }

        public CommunicationEvent[] CommunicationEvents { get; set; }

        public PartyRate CurrentPartyRate { get; set; }

        public PartyRate[] AllPartyRates { get; set; }
        #endregion

        #region Allors
        [Id("45CDF666-F891-4F5D-8583-0CB5D489C918")]
        [AssociationId("32AC1395-3787-4510-8169-A705917101A0")]
        [RoleId("F0ABA345-88FC-4693-B6F4-9AFFDC11B22C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        public SecurityToken ContactsSecurityToken { get; set; }

        #region Allors
        [Id("7BE2E3DF-E4B8-4D4F-BA1C-89E8E123D79B")]
        [AssociationId("5834413A-BDC2-44DC-8E4D-053A775503F9")]
        [RoleId("38658FBD-CB20-4DC2-9572-02EFBD654517")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        public AccessControl ContactsAccessControl { get; set; }

        #region Allors
        [Id("3A40A81A-049D-40E9-93AA-6DDA5CDCC450")]
        [AssociationId("12766F67-D7DF-4097-A6B6-D8D40DC0C0E8")]
        [RoleId("2D9764F7-27CC-448A-9BAB-27613F49CDBC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        public UserGroup OwnerUserGroup { get; set; }

        #region Allors
        [Id("FB2498BD-0CC8-4D3A-BA00-1F30B71A856B")]
        [AssociationId("9068C9D0-0C50-4AE4-8197-6346F08FCFF5")]
        [RoleId("034289B4-C4C5-4911-90A1-A3E675551DEA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public LegalForm LegalForm { get; set; }

        #region Allors
        [Id("B5552663-3D07-4EEA-BA8A-DEB40D264D48")]
        [AssociationId("0AB1485D-FEF4-4418-81AB-B974C54B7FDB")]
        [RoleId("13C0A269-41B1-4F7E-A3DB-6CCD1BD33572")]
        #endregion
        [Indexed]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("F0038F19-2997-4A5D-AF28-6019638E9264")]
        [AssociationId("C4113A30-9FDD-472A-A346-90BC773A4E57")]
        [RoleId("77E775B8-AB98-4FFD-B472-AFE39D9D825D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        public UserGroup ContactsUserGroup { get; set; }

        #region Allors
        [Id("A6D98EB2-0911-436E-81FD-0ABC723346B4")]
        [AssociationId("CA6FBB7A-15C7-4287-B753-BB57DCA9ABD2")]
        [RoleId("78AF90B5-EB4E-4C34-900E-C481E80C7FC9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Media LogoImage { get; set; }

        #region Allors
        [Id("5910ADAC-B422-4599-9F6D-38489BA7023B")]
        [AssociationId("A088AF86-A994-49ED-8BE7-4C980D5FF558")]
        [RoleId("10353E3E-4278-4A6B-8737-344DE9ACABB0")]
        #endregion
        [Size(256)]
        [Workspace]
        public string TaxNumber { get; set; }

        #region Allors
        [Id("9D37EDBB-ED85-4966-8B14-D26BC8A8BF52")]
        [AssociationId("44B4A589-579D-4D8E-8161-F8FA995001DB")]
        [RoleId("6F176997-3676-47F6-A699-F7EFD2CEC413")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public IndustryClassification[] IndustryClassifications { get; set; }

        #region Allors
        [Id("E6C1CB71-592C-4BFD-8B26-4ABD4EB03A72")]
        [AssociationId("C332CCDC-A5BB-47EF-87B5-C8957DE74938")]
        [RoleId("FAEAAA5F-09B5-41C4-B54E-7BE11F46C7B8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public OrganisationClassification[] OrganisationClassifications { get; set; }

        #region inherited methods

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}