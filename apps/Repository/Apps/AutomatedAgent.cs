namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("3587d2e1-c3f6-4c55-a96c-016e0501d99c")]
    #endregion
    public partial class AutomatedAgent : User, Party 
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }

        public bool UserEmailConfirmed { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string UserPasswordHash { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Locale Locale { get; set; }

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

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("4e158d75-d0b5-4cb7-ad41-e8ed3002d175")]
        [AssociationId("6f2a83eb-17e9-408e-b18b-9bb2b9a3e812")]
        [RoleId("4fac2dd3-8711-4115-96b9-a38f62e2d093")]
        #endregion
        [Indexed]
        [Size(256)]
        public string Name { get; set; }

        #region Allors
        [Id("58870c93-b066-47b7-95f7-5411a46dbc7e")]
        [AssociationId("31925ed6-e66c-4718-963f-c8a71d566fe8")]
        [RoleId("eee42775-b172-4fde-9042-a0f9b2224ec3")]
        #endregion
        [Required]
        [Size(256)]
        public string Description { get; set; }
        
        #region inherited methods

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}