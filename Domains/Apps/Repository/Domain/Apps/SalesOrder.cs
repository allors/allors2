namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("716647bf-7589-4146-a45c-a6a3b1cee507")]
    #endregion
    public partial class SalesOrder : Order, Versioned
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
        #endregion

        #region Allors
        [Id("108a1136-feaa-45b8-a899-d455718090d1")]
        [AssociationId("a2df509a-6923-4121-9159-8d55b91fd407")]
        [RoleId("7cf7c405-f20c-4416-84f5-a4ff05412162")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public ContactMechanism TakenByContactMechanism { get; set; }

        #region Allors
        [Id("28359bf8-506e-41db-a86b-a1eee3d50198")]
        [AssociationId("9a1a8d51-904d-480e-869f-66f5edae0ccd")]
        [RoleId("de181822-ac8e-4a85-af28-b217aa9fcfcd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party ShipToCustomer { get; set; }

        #region Allors
        [Id("2bd27b4c-37fd-4f82-bd43-4301ac704749")]
        [AssociationId("39389068-26bb-4e3b-b816-ef5730761301")]
        [RoleId("97e7045d-cf35-46ee-acfc-34f6b2572096")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public Party BillToCustomer { get; set; }

        #region Allors
        [Id("2d097a42-0cfd-43d7-a683-2ae94b9ddaf1")]
        [AssociationId("2921dfd5-e57c-4686-b95d-54da85af6604")]
        [RoleId("683dcf30-f20f-44fa-947b-e8b1901b5165")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalPurchasePrice { get; set; }

        #region Allors
        [Id("2ee793c8-512e-4358-b28a-f364280db93f")]
        [AssociationId("fce2bfd3-8f68-4c9f-a1a3-dce309767458")]
        [RoleId("d123ca45-1afb-4403-9b88-2a5a135d0e60")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ShipmentMethod ShipmentMethod { get; set; }

        #region Allors
        [Id("30abf0e0-08a3-441a-a91e-09ab14199689")]
        [AssociationId("009552df-953c-4170-bbbf-495c8746d6c0")]
        [RoleId("403e22eb-805b-4fff-9b1f-0243d215d9fd")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalListPriceCustomerCurrency { get; set; }

        #region Allors
        [Id("30ddd003-9055-4c1b-8bbb-af75a54da66d")]
        [AssociationId("aed47f4f-411d-49ee-9327-5543761d16b5")]
        [RoleId("2b2ee710-ba86-4c7b-ba0e-443e229bec23")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal MaintainedProfitMargin { get; set; }

        #region Allors
        [Id("3a2be2f2-2608-46e0-b1f1-1da7e372b8f8")]
        [AssociationId("11b71189-8551-467d-9c50-07afe152bdc0")]
        [RoleId("86ca98fe-6bc1-44ce-984e-23ed2f51e9b1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("3d01b9c9-5f37-40a8-9305-8ee9e98cc192")]
        [AssociationId("e93969bc-b73c-4fec-aec2-aa557c57e844")]
        [RoleId("0100d1ae-c1a6-4b4d-b904-59a2f337e158")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Party PreviousShipToCustomer { get; set; }

        #region Allors
        [Id("469084d5-acc5-4fc9-910b-ead4d8d4d021")]
        [AssociationId("0cc79ccb-af0e-4025-a4f1-3ec5d4f16b96")]
        [RoleId("9640b6a4-f926-48d7-96a2-6c8a0e54cd6b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism BillToContactMechanism { get; set; }

        #region Allors
        [Id("4958ae32-6bc0-451d-bacc-8b7244a9dc56")]
        [AssociationId("bf8525ec-1fdf-4bae-9fd9-85bb4aa54400")]
        [RoleId("6281e7d9-d7b8-4611-83f4-e1bdb44cc5f9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Derived]
        [Indexed]
        public Person[] SalesReps { get; set; }

        #region Allors
        [Id("4f3cf098-b9d8-4c10-8317-ea2c05ebc4b0")]
        [AssociationId("6d3492d0-dda6-41a0-a7e4-32bbccb237f5")]
        [RoleId("4ab44f50-c591-47ec-b6ce-130b5e8791f8")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal InitialProfitMargin { get; set; }

        #region Allors
        [Id("7c5206f5-391d-485d-a030-513450f4dd2f")]
        [AssociationId("1086a778-17dd-4984-b73b-a5629a9b8e7c")]
        [RoleId("1020ff0a-e353-418a-9111-c61a5216032d")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalListPrice { get; set; }

        #region Allors
        [Id("8f27c21b-ac66-4851-90d8-e955ef31bbec")]
        [AssociationId("bfa36bda-784b-4540-a7da-813d37e24c56")]
        [RoleId("fc27baa3-1bdb-44a7-9848-431fbc8ef91e")]
        #endregion
        [Required]
        public bool PartiallyShip { get; set; }

        #region Allors
        [Id("a1d8e768-0a81-409d-ac13-7c7b8f5081f0")]
        [AssociationId("e2de9a21-d93a-4668-9991-1cda6dcab18e")]
        [RoleId("464890a7-d099-4d3d-9ffb-66d79858a579")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        public Party[] Customers { get; set; }

        #region Allors
        [Id("a54ff0dc-5adb-4314-8081-66522431b11d")]
        [AssociationId("9af36608-dc4a-4197-a7a6-77a2cc3bdfd4")]
        [RoleId("d40d841d-a4f0-4e14-b726-3a66f3628ead")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public Store Store { get; set; }

        #region Allors
        [Id("a5746883-0ad8-4efb-931c-799b8f33ce63")]
        [AssociationId("a6333d69-b7e9-4694-8c97-63742a532c28")]
        [RoleId("90d4d0ec-7a65-417b-9b63-05e0fa73070a")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal MaintainedMarkupPercentage { get; set; }

        #region Allors
        [Id("aa416d3e-0f75-4fa5-97e0-ef0bc4327ea9")]
        [AssociationId("6bd25de8-38a0-4005-baae-fe1339c24bbd")]
        [RoleId("d99b70e4-1ad2-46a3-b362-26880a843ff8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public ContactMechanism BillFromContactMechanism { get; set; }

        #region Allors
        [Id("b9f315a5-22dc-4cba-a19f-fe71fe56ca49")]
        [AssociationId("9b59abe7-e3ae-4899-a233-71e9df67555a")]
        [RoleId("44f187d7-afed-47c8-b318-454a3982c8af")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PaymentMethod PaymentMethod { get; set; }

        #region Allors
        [Id("ba592fc9-78bb-4102-b9b5-fa692210dc38")]
        [AssociationId("207a2983-64c8-41c1-a97f-eb1e8bb78919")]
        [RoleId("0a0596d8-8717-466a-9321-02fc8f3410d3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism PlacingContactMechanism { get; set; }

        #region Allors
        [Id("c90e107b-6b47-4337-9937-391eacd1b1c5")]
        [AssociationId("f496f8ec-b2f8-4264-96bc-6d6567b46d11")]
        [RoleId("87824813-0d60-4e7e-af9c-5ad441913820")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Party PreviousBillToCustomer { get; set; }

        #region Allors
        [Id("ce771472-d789-4077-80bb-25622624e1df")]
        [AssociationId("6d50f1f0-8e69-4fca-960e-31c48bddadea")]
        [RoleId("f7da8b79-e2cd-492d-a721-88d3c8fc530c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public SalesChannel SalesChannel { get; set; }

        #region Allors
        [Id("d6714c09-dce1-4182-aa2f-bbc887edc89a")]
        [AssociationId("9d679860-d975-4a0a-aef4-08975f45d855")]
        [RoleId("23fc03a8-a44c-431f-bdfa-75905691764b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party PlacingCustomer { get; set; }

        #region Allors
        [Id("da5a63d2-33bb-4da3-a1bf-064280cac0fa")]
        [AssociationId("05da158d-3f90-4c1f-9bdf-22263b285ed1")]
        [RoleId("d2390ce6-ea3d-43da-9a48-966e9274bcc2")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        public SalesInvoice ProformaInvoice { get; set; }

        #region Allors
        [Id("eb5a3564-996d-4bbe-b592-6205adad93b8")]
        [AssociationId("37612ba0-d689-49ca-9005-3b3bf21cd272")]
        [RoleId("bd5d13f3-c1e7-4eea-9c33-09f4b47289f3")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public SalesOrderItem[] SalesOrderItems { get; set; }

        #region Allors
        [Id("f20ac339-0761-410b-bbb6-6fb393bcba8a")]
        [AssociationId("cf9fcfa0-a862-4cb2-88b4-c4dcd6d0034d")]
        [RoleId("6ec955c9-6d25-45b3-a4d1-5e4270d28750")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public SalesOrderObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("f7b7b4d2-fd9e-4d29-99be-f69b2967cc3b")]
        [AssociationId("ae9335e4-4d72-40fc-b028-dcfd7ea67cfa")]
        [RoleId("d70334f4-c9f6-4804-a887-2969d75c8644")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal InitialMarkupPercentage { get; set; }

        #region Allors
        [Id("ff972b6c-ab12-4596-a1bc-18f93127ac31")]
        [AssociationId("556771a7-0f67-4061-8c88-c8401bf0b1c1")]
        [RoleId("6d7ee57e-35b3-4a19-ad1f-4a1850c41568")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public InternalOrganisation TakenByInternalOrganisation { get; set; }

        #region Allors
        [Id("7788542E-5095-4D18-8F52-0732CBB599EA")]
        [AssociationId("159960F5-A8BE-455B-A402-9D3730C9D335")]
        [RoleId("4344C29C-ACA6-47FC-9B68-85488A755903")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProductQuote Quote { get; set; }

        #region Versioning
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
        [Id("9C6B7601-4B01-4A2D-8B43-E08703B66A0C")]
        [AssociationId("D6FB1FB7-6DFE-4ECE-97C5-26FC475AF5B2")]
        [RoleId("35D43EF4-FF87-4CEF-982E-014407C79B3E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderVersion[] AllVersions { get; set; }
        #endregion

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
        
        #endregion
    }
}