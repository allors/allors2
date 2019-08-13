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

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public string InternalComment { get; set; }
        public Currency Currency { get; set; }
        
        
        
        public string CustomerReference { get; set; }
        public Fee Fee { get; set; }
        public decimal TotalExVat { get; set; }
        public SalesTerm[] SalesTerms { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalSurcharge { get; set; }
        public OrderItem[] ValidOrderItems { get; set; }
        public string OrderNumber { get; set; }
        
        public decimal TotalDiscount { get; set; }
        public string Message { get; set; }

        public string Description { get; set; }

        
        public DateTime EntryDate { get; set; }
        public DiscountAdjustment DiscountAdjustment { get; set; }
        public OrderKind OrderKind { get; set; }
        public decimal TotalIncVat { get; set; }
        
        public VatRegime VatRegime { get; set; }
        
        public decimal TotalShippingAndHandling { get; set; }
        public ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }
        public DateTime OrderDate { get; set; }
        
        public DateTime DeliveryDate { get; set; }
        public decimal TotalBasePrice { get; set; }
        public decimal TotalFee { get; set; }
        public SurchargeAdjustment SurchargeAdjustment { get; set; }
        public TransportInitiator TransportInitiatedBy { get; set; }

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public PrintDocument PrintDocument { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Locale Locale { get; set; }
        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region ObjectStates
        #region SalesOrderState
        #region Allors
        [Id("13476417-0F9F-48AD-A197-D8FE897345E6")]
        [AssociationId("F2BF85B3-5B5C-4B86-8247-A43EBFAF9DCF")]
        [RoleId("91316F6B-A3E2-44CE-994B-B2EFFC87B92B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderState PreviousSalesOrderState { get; set; }

        #region Allors
        [Id("ABD5310C-D5CF-4584-B0E5-D76CB2A7174E")]
        [AssociationId("E095EECA-8A7B-4765-BF5E-8CA022AA88CE")]
        [RoleId("994DADF6-89A2-44CC-A911-506A24B250BD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderState LastSalesOrderState { get; set; }

        #region Allors
        [Id("822493E4-B80B-4506-BBF6-C3CD73DE0C4A")]
        [AssociationId("79E89773-D98B-4588-8EB3-4718A353A9C4")]
        [RoleId("721DE38B-AE08-4B14-B758-7BC717A40BC2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesOrderState SalesOrderState { get; set; }
        #endregion

        #region SalesOrderPaymentState
        #region Allors
        [Id("5548C20D-6F31-4D7C-8A80-7D9CE6187B76")]
        [AssociationId("DBC25417-8C33-4578-A387-54675CE397BD")]
        [RoleId("6011C0CA-1A90-419C-8778-809D763A6FF6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderPaymentState PreviousSalesOrderPaymentState { get; set; }

        #region Allors
        [Id("6D34483F-9A30-4C37-8B5E-22AA9EA03381")]
        [AssociationId("82740E30-E45A-495F-A0B5-4A76228C50A6")]
        [RoleId("1E9CE3B9-5AEE-4F00-9E52-6BEEAF1EBDF4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderPaymentState LastSalesOrderPaymentState { get; set; }

        #region Allors
        [Id("5B79CBD9-D450-4AF3-9338-A66025345011")]
        [AssociationId("C1730172-6F91-4F4E-9DFA-8CD0509B9947")]
        [RoleId("0015E8F0-8434-42EF-B938-B009390EB7D2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Derived]
        public SalesOrderPaymentState SalesOrderPaymentState { get; set; }
        #endregion

        #region SalesOrderInvoiceState
        #region Allors
        [Id("B1B53EB4-1FB6-476B-899F-FFAF5AE8ED28")]
        [AssociationId("D3AE6736-5806-484B-938E-739309388515")]
        [RoleId("0781024D-A8D2-4BC1-90B7-3A390A500B7E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderInvoiceState PreviousSalesOrderInvoiceState { get; set; }

        #region Allors
        [Id("82EF3FDC-41CA-462F-B50E-35FEE72866BE")]
        [AssociationId("D50E61FC-1A2D-4153-A7DA-C54EDE93C03A")]
        [RoleId("33F5CF8D-FE69-47DD-B013-2BE7467C8625")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderInvoiceState LastSalesOrderInvoiceState { get; set; }

        #region Allors
        [Id("FE413572-DFEB-4EB5-BDB0-003A2600946B")]
        [AssociationId("25D775C2-9BC3-4BFA-AA0A-F130C2E0D746")]
        [RoleId("2D99AF67-00B7-4A11-AF38-CDE145523E73")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public SalesOrderInvoiceState SalesOrderInvoiceState { get; set; }
        #endregion

        #region SalesOrderShipmentState
        #region Allors
        [Id("0C064509-FBCE-466B-A67E-C4EDD465E926")]
        [AssociationId("9CD025A1-91A4-4F77-8DBA-9A3548590514")]
        [RoleId("312ADC9E-512C-4BEC-9A98-B0E7A7D57CFA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderShipmentState PreviousSalesOrderShipmentState { get; set; }

        #region Allors
        [Id("3B64B544-C2EB-48AE-8DBC-32F5B31C21D2")]
        [AssociationId("7426C205-3C34-4885-A299-7A87B067CBBA")]
        [RoleId("4B908495-DAD0-41AD-B3CE-720FD4FE9EB5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderShipmentState LastSalesOrderShipmentState { get; set; }

        #region Allors
        [Id("23F8B966-A5E2-42B3-BD84-029D31FC073C")]
        [AssociationId("293EB265-AFC4-4976-BD63-3992232BB489")]
        [RoleId("9943DC63-BDB9-4A27-918F-4F0217A2099E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public SalesOrderShipmentState SalesOrderShipmentState { get; set; }
        #endregion
        #endregion

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

        #region Allors
        [Id("9E20E5E5-1838-48F6-A9E6-B56ED4F44BA3")]
        [AssociationId("C5343757-FD4B-42DF-A474-71085F304278")]
        [RoleId("F0464A62-211B-4BB8-885B-2A11221C0046")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InternalOrganisation TakenBy { get; set; }

        #region Allors
        [Id("108a1136-feaa-45b8-a899-d455718090d1")]
        [AssociationId("a2df509a-6923-4121-9159-8d55b91fd407")]
        [RoleId("7cf7c405-f20c-4416-84f5-a4ff05412162")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        [Indexed]
        public ContactMechanism TakenByContactMechanism { get; set; }

        #region Allors
        [Id("F43697A3-157B-487C-86D1-42B9A469AE88")]
        [AssociationId("F29328EA-CBE6-4341-BC3F-057232B59716")]
        [RoleId("A61589A4-7E6A-4D49-8D6E-A5C305C056E9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person TakenByContactPerson { get; set; }

        #region Allors
        [Id("28359bf8-506e-41db-a86b-a1eee3d50198")]
        [AssociationId("9a1a8d51-904d-480e-869f-66f5edae0ccd")]
        [RoleId("de181822-ac8e-4a85-af28-b217aa9fcfcd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Indexed]
        [Workspace]
        public Party ShipToCustomer { get; set; }

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
        [Id("848B1B7B-C91E-460E-B702-342CDCC58238")]
        [AssociationId("5ACB2265-3C10-4526-862B-6AD440E32095")]
        [RoleId("9744CBB5-F9FC-428C-86B0-1BE05174312D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress ShipFromAddress { get; set; }

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
        [Id("09469930-2854-4B18-99F4-65A02F4332A1")]
        [AssociationId("047DCB20-5EAC-4EEE-B68F-5DFC31DE9F7A")]
        [RoleId("71AB57A7-82C4-4C9F-91BD-34C0372E296C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToContactPerson { get; set; }

        #region Allors
        [Id("2bd27b4c-37fd-4f82-bd43-4301ac704749")]
        [AssociationId("39389068-26bb-4e3b-b816-ef5730761301")]
        [RoleId("97e7045d-cf35-46ee-acfc-34f6b2572096")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        [Indexed]
        public Party BillToCustomer { get; set; }

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
        [Id("2171BB3B-A09C-499E-A654-E1A6A615FF0B")]
        [AssociationId("7C831AB3-D751-45E4-A824-15A29EC4AF31")]
        [RoleId("35E3C29A-F359-4C27-B594-58F61D7B56B1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism BillToContactMechanism { get; set; }

        #region Allors
        [Id("FB1749C7-FB4E-4E85-9440-01941885EE31")]
        [AssociationId("455032FB-E6C4-4CF2-A353-9A956556F3BE")]
        [RoleId("3B865962-3C69-439E-881F-AF5A43CBA5D3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BillToContactPerson { get; set; }

        #region Allors
        [Id("501673A2-E15F-44BB-9CA2-BCCC1F0E2E66")]
        [AssociationId("3C0DC238-CEA6-4123-8445-BFE6139C5155")]
        [RoleId("9287C0DF-0696-4D20-83FB-DE5D906DBF86")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party BillToEndCustomer { get; set; }

        #region Allors
        [Id("469084d5-acc5-4fc9-910b-ead4d8d4d021")]
        [AssociationId("0cc79ccb-af0e-4025-a4f1-3ec5d4f16b96")]
        [RoleId("9640b6a4-f926-48d7-96a2-6c8a0e54cd6b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism BillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("F49326D6-77B3-4F46-BC93-5DC0340796FB")]
        [AssociationId("7CB93DA8-86DD-4320-8014-0A056F79ABE1")]
        [RoleId("0F95D953-7FBB-4364-A551-8CEAAA9C54F8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BillToEndCustomerContactPerson { get; set; }

        #region Allors
        [Id("D5B46F18-77AB-4535-A3DA-027489CBA9D1")]
        [AssociationId("B63B43F8-C7C3-4E8D-915D-7634B4337C90")]
        [RoleId("13F7C9E8-5789-4DAF-9BE0-249F93459FEF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ShipToEndCustomer { get; set; }

        #region Allors
        [Id("D6216599-925F-4BEA-B6EB-C0B6CCE05617")]
        [AssociationId("23044C65-5A84-41A6-894C-7963DF55532C")]
        [RoleId("3FA215D8-FE93-489F-8AD2-683C2BFAC8AE")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress ShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("D1E8AAE3-FBA0-4693-8954-CC29AD2D042C")]
        [AssociationId("1EC2B5F6-6629-4455-8361-DA09B99408E5")]
        [RoleId("8AD426B1-F4FD-419B-BB58-BDE26084D40A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToEndCustomerContactPerson { get; set; }

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
        [Id("ba592fc9-78bb-4102-b9b5-fa692210dc38")]
        [AssociationId("207a2983-64c8-41c1-a97f-eb1e8bb78919")]
        [RoleId("0a0596d8-8717-466a-9321-02fc8f3410d3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism PlacingCustomerContactMechanism { get; set; }

        #region Allors
        [Id("3963B50C-5EEA-4068-955F-85914C57F938")]
        [AssociationId("DA8487AA-08FE-49C6-82AA-FBB110CEE9F6")]
        [RoleId("790C3BE7-DF54-400A-B83E-7526CC59ACE6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person PlacingCustomerContactPerson { get; set; }

        #region Allors
        [Id("81560BAA-D8E1-4688-A191-3081C0CE3B01")]
        [AssociationId("A127E9CB-DF58-484F-BB9D-41B22BBBACC5")]
        [RoleId("ADDE584C-6ED0-4A3F-87DA-F76EF2069D49")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public Facility OriginFacility{ get; set; }

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
        [Workspace]
        public Store Store { get; set; }
        
        #region Allors
        [Id("b9f315a5-22dc-4cba-a19f-fe71fe56ca49")]
        [AssociationId("9b59abe7-e3ae-4899-a233-71e9df67555a")]
        [RoleId("44f187d7-afed-47c8-b318-454a3982c8af")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Indexed]
        [Workspace]
        public PaymentMethod PaymentMethod { get; set; }

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
        [Id("7788542E-5095-4D18-8F52-0732CBB599EA")]
        [AssociationId("159960F5-A8BE-455B-A402-9D3730C9D335")]
        [RoleId("4344C29C-ACA6-47FC-9B68-85488A755903")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProductQuote Quote { get; set; }

        #region Allors
        [Id("5308B55C-A248-44C1-9438-951B1BCDB48C")]
        [AssociationId("E5A38939-FC2C-447F-82CB-BED3E18957F2")]
        [RoleId("EA7C07AA-BA50-4279-97B6-559643788CBC")]
        #endregion
        [Derived]
        [Required]
        [Workspace]
        public bool CanShip { get; set; }

        #region Allors
        [Id("5B00CA2E-3F97-445E-813E-AA315C588AAC")]
        [AssociationId("5AB68B1A-2367-4D62-9504-D80A83AB0DFB")]
        [RoleId("9619BA05-1768-4196-861E-28B2F7C69F25")]
        #endregion
        [Derived]
        [Required]
        [Workspace]
        public bool CanInvoice { get; set; }

        #region Allors
        [Id("1FC9526F-D1C5-41E3-93D9-FDA9308CD7B4")]
        [AssociationId("19712037-D36D-4449-B2D5-1694E0357E48")]
        [RoleId("0E6910E4-7466-4B48-801B-2CFF4C830D11")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public VatClause VatClause { get; set; }

        #region Allors
        [Id("E822B75C-3A37-480A-A469-B18A060EC560")]
        #endregion
        [Workspace]
        public void Ship() { }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
            
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Approve() { }

        public void Reject() { }

        public void Hold() { }

        public void Continue() { }

        public void Confirm() { }

        public void Cancel() { }

        public void Complete() { }
        public void Invoice() { }
        public void Reopen() { }

        public void Print() { }

        #endregion
    }
}