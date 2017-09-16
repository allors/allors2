namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6173fc23-115f-4356-a0ce-867872c151ac")]
    #endregion
    public partial class SalesInvoice : Invoice, ISalesInvoice 
    {
        #region inherited properties
        public string InternalComment { get; set; }
        public decimal TotalShippingAndHandlingCustomerCurrency { get; set; }
        public Currency CustomerCurrency { get; set; }
        public string Description { get; set; }
        public ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }
        public decimal TotalFeeCustomerCurrency { get; set; }
        public Fee Fee { get; set; }
        public decimal TotalExVatCustomerCurrency { get; set; }
        public string CustomerReference { get; set; }
        public DiscountAdjustment DiscountAdjustment { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TotalDiscount { get; set; }
        public BillingAccount BillingAccount { get; set; }
        public decimal TotalIncVat { get; set; }
        public decimal TotalSurcharge { get; set; }
        public decimal TotalBasePrice { get; set; }
        public decimal TotalVatCustomerCurrency { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal TotalIncVatCustomerCurrency { get; set; }
        public decimal TotalShippingAndHandling { get; set; }
        public decimal TotalBasePriceCustomerCurrency { get; set; }
        public SurchargeAdjustment SurchargeAdjustment { get; set; }
        public decimal TotalExVat { get; set; }
        public InvoiceTerm[] InvoiceTerms { get; set; }
        public decimal TotalSurchargeCustomerCurrency { get; set; }
        public string InvoiceNumber { get; set; }
        public string Message { get; set; }
        public VatRegime VatRegime { get; set; }
        public decimal TotalDiscountCustomerCurrency { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalFee { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public Locale Locale { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public string Comment { get; set; }
        public Guid UniqueId { get; set; }
        public string PrintContent { get; set; }
        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public SalesInvoiceObjectState CurrentObjectState { get; set; }
        public decimal TotalListPrice { get; set; }
        public InternalOrganisation BilledFromInternalOrganisation { get; set; }
        public ContactMechanism BillToContactMechanism { get; set; }
        public Party PreviousBillToCustomer { get; set; }
        public SalesInvoiceType SalesInvoiceType { get; set; }
        public decimal InitialProfitMargin { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public decimal InitialMarkupPercentage { get; set; }
        public decimal MaintainedMarkupPercentage { get; set; }
        public Person[] SalesReps { get; set; }
        public Shipment Shipment { get; set; }
        public decimal MaintainedProfitMargin { get; set; }
        public Party PreviousShipToCustomer { get; set; }
        public Party BillToCustomer { get; set; }
        public SalesInvoiceItem[] SalesInvoiceItems { get; set; }
        public decimal TotalListPriceCustomerCurrency { get; set; }
        public Party ShipToCustomer { get; set; }
        public ContactMechanism BilledFromContactMechanism { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public SalesChannel SalesChannel { get; set; }
        public Party[] Customers { get; set; }
        public PostalAddress ShipToAddress { get; set; }
        public Store Store { get; set; }
        #endregion

        #region Allors
        [Id("FD3391B6-1B75-43F6-ADDB-A97F5E8F3BC6")]
        [AssociationId("FE12E157-9CC4-4C6B-88C0-7777406E67DA")]
        [RoleId("000F6B7C-FE00-4748-84C1-77C48F44B006")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesInvoiceVersion CurrentVersion { get; set; }

        #region Allors
        [Id("942BCFF3-509E-44D4-BA61-B8AE1DAA04CF")]
        [AssociationId("2EE321FF-A4C8-43CE-ADFA-EDDE7AD061D0")]
        [RoleId("30BC809D-C7A2-4418-9F55-D2B8DDD9B5CC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesInvoiceVersion PreviousVersion { get; set; }

        #region Allors
        [Id("EF051A68-7FB9-4461-B16D-34F1B99F34C4")]
        [AssociationId("2C4596DA-0CD4-4F06-BDE9-0433A34C38AA")]
        [RoleId("1126B3E9-D287-4EA3-9D33-37B7BF06CDB4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesInvoiceVersion[] AllVersions { get; set; }

        #region Allors
        [Id("99DB81A9-7775-4AB0-85DE-08D3CCD0A261")]
        [AssociationId("E7FF268A-7DCC-4941-AB75-D63A41B752E4")]
        [RoleId("E58CC8D9-50C9-4A33-BEC0-FDD4E4EC0FE7")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesInvoiceVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("0CA9E47D-5575-44B1-BE9F-5587A3C8EFD5")]
        [AssociationId("747FD89A-90F4-4025-A1C2-F986F5499AFF")]
        [RoleId("FA4BC473-75BB-4AFA-BAF4-9B0DF62D6B3B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesInvoiceVersion[] AllStateVersions { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}
        public void AddNewInvoiceItem() { }

        #endregion

        #region Allors
        [Id("55A60B80-2052-47E6-BD41-2AF414ABB885")]
        #endregion
        [Workspace]
        public void Send() { }

        #region Allors
        [Id("96AF8F69-F1A4-420A-8D9D-AF61EB061620")]
        #endregion
        [Workspace]
        public void CancelInvoice() { }

        #region Allors
        [Id("F6EC229C-288C-4830-9DE5-8D5236DE4781")]
        #endregion
        [Workspace]
        public void WriteOff() { }

    }
}