namespace Allors.Repository
{
    using System;
    public partial class AutomatedAgent : Party 
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

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion
        
        #region inherited methods

        #endregion
    }
}