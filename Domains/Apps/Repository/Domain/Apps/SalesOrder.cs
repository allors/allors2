namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("716647bf-7589-4146-a45c-a6a3b1cee507")]
    #endregion
    public partial class SalesOrder : Order, ISalesOrder 
    {
        #region inherited properties

        public string InternalComment { get; set; }
        public Currency CustomerCurrency { get; set; }
        public decimal TotalBasePriceCustomerCurrency { get; set; }
        public decimal TotalIncVatCustomerCurrency { get; set; }
        public decimal TotalDiscountCustomerCurrency { get; set; }
        public string CustomerReference { get; set; }
        public Fee Fee { get; set; }
        public decimal TotalExVat { get; set; }
        public OrderTerm[] OrderTerms { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalSurcharge { get; set; }
        public OrderItem[] ValidOrderItems { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalVatCustomerCurrency { get; set; }
        public decimal TotalDiscount { get; set; }
        public string Message { get; set; }
        public decimal TotalShippingAndHandlingCustomerCurrency { get; set; }
        public DateTime EntryDate { get; set; }
        public DiscountAdjustment DiscountAdjustment { get; set; }
        public OrderKind OrderKind { get; set; }
        public decimal TotalIncVat { get; set; }
        public decimal TotalSurchargeCustomerCurrency { get; set; }
        public VatRegime VatRegime { get; set; }
        public decimal TotalFeeCustomerCurrency { get; set; }
        public decimal TotalShippingAndHandling { get; set; }
        public ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalExVatCustomerCurrency { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TotalBasePrice { get; set; }
        public decimal TotalFee { get; set; }
        public SurchargeAdjustment SurchargeAdjustment { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public Guid UniqueId { get; set; }
        public string PrintContent { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public string Comment { get; set; }
        public Locale Locale { get; set; }
        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public ContactMechanism TakenByContactMechanism { get; set; }
        public Party ShipToCustomer { get; set; }
        public Party BillToCustomer { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public ShipmentMethod ShipmentMethod { get; set; }
        public decimal TotalListPriceCustomerCurrency { get; set; }
        public decimal MaintainedProfitMargin { get; set; }
        public PostalAddress ShipToAddress { get; set; }
        public Party PreviousShipToCustomer { get; set; }
        public ContactMechanism BillToContactMechanism { get; set; }
        public Person[] SalesReps { get; set; }
        public decimal InitialProfitMargin { get; set; }
        public decimal TotalListPrice { get; set; }
        public bool PartiallyShip { get; set; }
        public Party[] Customers { get; set; }
        public Store Store { get; set; }
        public decimal MaintainedMarkupPercentage { get; set; }
        public ContactMechanism BillFromContactMechanism { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ContactMechanism PlacingContactMechanism { get; set; }
        public Party PreviousBillToCustomer { get; set; }
        public SalesChannel SalesChannel { get; set; }
        public Party PlacingCustomer { get; set; }
        public SalesInvoice ProformaInvoice { get; set; }
        public SalesOrderItem[] SalesOrderItems { get; set; }
        public SalesOrderObjectState CurrentObjectState { get; set; }
        public decimal InitialMarkupPercentage { get; set; }
        public InternalOrganisation TakenByInternalOrganisation { get; set; }
        public ProductQuote Quote { get; set; }

        #endregion

        #region Allors
        [Id("3CE4119B-6653-482D-9EEC-27BBDC56472F")]
        [AssociationId("FE2BFC2B-B71B-4827-94DD-7598A9F1E327")]
        [RoleId("9C2B70A6-1E31-4587-8C44-C2500A0D3982")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderVersion CurrentVersion { get; set; }

        #region Allors
        [Id("B9FB9215-71AA-48D0-896D-92DAFFCF03C3")]
        [AssociationId("997DCCFE-00CC-44DB-9C56-D5796F1D6AD9")]
        [RoleId("5D20F859-19E8-4CEB-A032-41847128F1B8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderVersion PreviousVersion { get; set; }

        #region Allors
        [Id("9C6B7601-4B01-4A2D-8B43-E08703B66A0C")]
        [AssociationId("D6FB1FB7-6DFE-4ECE-97C5-26FC475AF5B2")]
        [RoleId("35D43EF4-FF87-4CEF-982E-014407C79B3E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderVersion[] AllVersions { get; set; }

        #region Allors
        [Id("BF0D2494-DA58-4B34-A033-E879835D2E78")]
        [AssociationId("93D987E4-C203-4B6F-9517-253C80ABC6C2")]
        [RoleId("19725B93-D295-4A6F-8F08-0E4704C5C24C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("F8E2AF6E-E223-4628-AC9A-C7C76E82D208")]
        [AssociationId("63DFB23E-52C6-4B29-A84F-95435CC58F37")]
        [RoleId("8BE82ED4-7097-48D4-99B8-4E5B5D059D77")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderVersion[] AllStateVersions { get; set; }

        #region Allors
        [Id("CDA26CC7-96C4-4374-83B1-6C97E39BA62F")]
        [AssociationId("D1BB7ED5-776B-45E5-9770-EC2D909E5008")]
        [RoleId("B6FEE1C8-6FA5-48EF-9B9F-D4CEE1A1C68C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderVersion CurrentPaymentStateVersion { get; set; }

        #region Allors
        [Id("77B8B8EC-DBB7-46A1-BBF9-AC9071CA0719")]
        [AssociationId("713C7391-36F3-4288-BA4A-87D65CD251FA")]
        [RoleId("568489D2-291A-4C73-B512-8337BAF97690")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderVersion[] AllPaymentStateVersions { get; set; }

        #region Allors
        [Id("62D1C455-C8DC-437D-BDD1-D9644F7107D0")]
        [AssociationId("848B207E-8C7B-42E0-B4BD-A79290CD37C2")]
        [RoleId("2209BD95-112D-4AF6-BE91-39613596E4D2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderVersion CurrentShipmentStateVersion { get; set; }

        #region Allors
        [Id("786E2666-59DB-44CF-872A-68A43B5A1CD0")]
        [AssociationId("6DDDF481-403D-4FAB-96B6-7641092747A8")]
        [RoleId("4099B76B-768E-4C02-8C56-81B8744B441F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderVersion[] AllShipmentStateVersions { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Approve(){}

        public void Reject(){}

        public void Hold(){}

        public void Continue(){}

        public void Confirm(){}

        public void Cancel(){}

        public void Complete(){}

        public void Finish(){}
        public void AddNewOrderItem() { }

        #endregion
    }
}