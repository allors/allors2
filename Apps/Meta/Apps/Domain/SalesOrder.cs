namespace Allors.Meta
{
	#region Allors
	[Id("716647bf-7589-4146-a45c-a6a3b1cee507")]
	#endregion
	[Inherit(typeof(OrderInterface))]
	public partial class SalesOrderClass : Class
	{
		#region Allors
		[Id("108a1136-feaa-45b8-a899-d455718090d1")]
		[AssociationId("a2df509a-6923-4121-9159-8d55b91fd407")]
		[RoleId("7cf7c405-f20c-4416-84f5-a4ff05412162")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("TakenByContactMechanism")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TakenByContactMechanism;

		#region Allors
		[Id("19b2f705-c5c7-4440-9b44-96783802ead0")]
		[AssociationId("9ce4d748-c7a2-445e-adf1-35ab41edbfe8")]
		[RoleId("4a78e297-5ede-41a9-a487-43509450be2f")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderStatusClass))]
		[Plural("ShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentStatus;

		#region Allors
		[Id("19dc0809-46cb-4c37-923c-6bc29a357ba8")]
		[AssociationId("bb864baf-b34d-48ab-87e4-e26d82cd4149")]
		[RoleId("2bc32ce8-1202-4091-934a-c89ef338a5fe")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderStatusClass))]
		[Plural("CurrentShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentShipmentStatus;

		#region Allors
		[Id("209f99fb-2e14-45fe-9533-a56573a8c115")]
		[AssociationId("f00c8458-3fc7-4279-b3b0-58902ff3bf1d")]
		[RoleId("49399720-aa62-4f69-a20a-1d1e0b8d3978")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderStatusClass))]
		[Plural("CurrentPaymentStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentPaymentStatus;

		#region Allors
		[Id("28359bf8-506e-41db-a86b-a1eee3d50198")]
		[AssociationId("9a1a8d51-904d-480e-869f-66f5edae0ccd")]
		[RoleId("de181822-ac8e-4a85-af28-b217aa9fcfcd")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("ShipToCustomers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipToCustomer;

		#region Allors
		[Id("2bd27b4c-37fd-4f82-bd43-4301ac704749")]
		[AssociationId("39389068-26bb-4e3b-b816-ef5730761301")]
		[RoleId("97e7045d-cf35-46ee-acfc-34f6b2572096")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("BillToCustomers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillToCustomer;

		#region Allors
		[Id("2d097a42-0cfd-43d7-a683-2ae94b9ddaf1")]
		[AssociationId("2921dfd5-e57c-4686-b95d-54da85af6604")]
		[RoleId("683dcf30-f20f-44fa-947b-e8b1901b5165")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalPurchasePrices")]
		public RelationType TotalPurchasePrice;

		#region Allors
		[Id("2ee793c8-512e-4358-b28a-f364280db93f")]
		[AssociationId("fce2bfd3-8f68-4c9f-a1a3-dce309767458")]
		[RoleId("d123ca45-1afb-4403-9b88-2a5a135d0e60")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentMethodClass))]
		[Plural("ShipmentMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipmentMethod;

		#region Allors
		[Id("30abf0e0-08a3-441a-a91e-09ab14199689")]
		[AssociationId("009552df-953c-4170-bbbf-495c8746d6c0")]
		[RoleId("403e22eb-805b-4fff-9b1f-0243d215d9fd")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalListPricesCustomerCurrency")]
		public RelationType TotalListPriceCustomerCurrency;

		#region Allors
		[Id("30ddd003-9055-4c1b-8bbb-af75a54da66d")]
		[AssociationId("aed47f4f-411d-49ee-9327-5543761d16b5")]
		[RoleId("2b2ee710-ba86-4c7b-ba0e-443e229bec23")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("MaintainedProfitMargins")]
		public RelationType MaintainedProfitMargin;

		#region Allors
		[Id("3a2be2f2-2608-46e0-b1f1-1da7e372b8f8")]
		[AssociationId("11b71189-8551-467d-9c50-07afe152bdc0")]
		[RoleId("86ca98fe-6bc1-44ce-984e-23ed2f51e9b1")]
		#endregion
		[Indexed]
		[Type(typeof(PostalAddressClass))]
		[Plural("ShipToAddresses")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipToAddress;

		#region Allors
		[Id("3d01b9c9-5f37-40a8-9305-8ee9e98cc192")]
		[AssociationId("e93969bc-b73c-4fec-aec2-aa557c57e844")]
		[RoleId("0100d1ae-c1a6-4b4d-b904-59a2f337e158")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("PreviousShipToCustomers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PreviousShipToCustomer;

		#region Allors
		[Id("469084d5-acc5-4fc9-910b-ead4d8d4d021")]
		[AssociationId("0cc79ccb-af0e-4025-a4f1-3ec5d4f16b96")]
		[RoleId("9640b6a4-f926-48d7-96a2-6c8a0e54cd6b")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("BillToContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillToContactMechanism;

		#region Allors
		[Id("4958ae32-6bc0-451d-bacc-8b7244a9dc56")]
		[AssociationId("bf8525ec-1fdf-4bae-9fd9-85bb4aa54400")]
		[RoleId("6281e7d9-d7b8-4611-83f4-e1bdb44cc5f9")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("SalesReps")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType SalesRep;

		#region Allors
		[Id("4f3cf098-b9d8-4c10-8317-ea2c05ebc4b0")]
		[AssociationId("6d3492d0-dda6-41a0-a7e4-32bbccb237f5")]
		[RoleId("4ab44f50-c591-47ec-b6ce-130b5e8791f8")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("InitialProfitMargins")]
		public RelationType InitialProfitMargin;

		#region Allors
		[Id("7c5206f5-391d-485d-a030-513450f4dd2f")]
		[AssociationId("1086a778-17dd-4984-b73b-a5629a9b8e7c")]
		[RoleId("1020ff0a-e353-418a-9111-c61a5216032d")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalListPrices")]
		public RelationType TotalListPrice;

		#region Allors
		[Id("8f27c21b-ac66-4851-90d8-e955ef31bbec")]
		[AssociationId("bfa36bda-784b-4540-a7da-813d37e24c56")]
		[RoleId("fc27baa3-1bdb-44a7-9848-431fbc8ef91e")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("PartiallyShips")]
		public RelationType PartiallyShip;

		#region Allors
		[Id("a012c48a-823a-4a4f-a251-33cfd3056ae2")]
		[AssociationId("0b2de73e-2fa1-4fa3-8df4-3c840c58babd")]
		[RoleId("a5fab0ee-f955-4b36-a914-10479ee16b82")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderStatusClass))]
		[Plural("PaymentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PaymentStatus;

		#region Allors
		[Id("a1d8e768-0a81-409d-ac13-7c7b8f5081f0")]
		[AssociationId("e2de9a21-d93a-4668-9991-1cda6dcab18e")]
		[RoleId("464890a7-d099-4d3d-9ffb-66d79858a579")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Customers")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType Customer;

		#region Allors
		[Id("a54ff0dc-5adb-4314-8081-66522431b11d")]
		[AssociationId("9af36608-dc4a-4197-a7a6-77a2cc3bdfd4")]
		[RoleId("d40d841d-a4f0-4e14-b726-3a66f3628ead")]
		#endregion
		[Indexed]
		[Type(typeof(StoreClass))]
		[Plural("Stores")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Store;

		#region Allors
		[Id("a5746883-0ad8-4efb-931c-799b8f33ce63")]
		[AssociationId("a6333d69-b7e9-4694-8c97-63742a532c28")]
		[RoleId("90d4d0ec-7a65-417b-9b63-05e0fa73070a")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("MaintainedMarkupPercentages")]
		public RelationType MaintainedMarkupPercentage;

		#region Allors
		[Id("aa416d3e-0f75-4fa5-97e0-ef0bc4327ea9")]
		[AssociationId("6bd25de8-38a0-4005-baae-fe1339c24bbd")]
		[RoleId("d99b70e4-1ad2-46a3-b362-26880a843ff8")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("BillFromContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillFromContactMechanism;

		#region Allors
		[Id("b9f315a5-22dc-4cba-a19f-fe71fe56ca49")]
		[AssociationId("9b59abe7-e3ae-4899-a233-71e9df67555a")]
		[RoleId("44f187d7-afed-47c8-b318-454a3982c8af")]
		#endregion
		[Indexed]
		[Type(typeof(PaymentMethodInterface))]
		[Plural("PaymentMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PaymentMethod;

		#region Allors
		[Id("ba592fc9-78bb-4102-b9b5-fa692210dc38")]
		[AssociationId("207a2983-64c8-41c1-a97f-eb1e8bb78919")]
		[RoleId("0a0596d8-8717-466a-9321-02fc8f3410d3")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("PlacingContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PlacingContactMechanism;

		#region Allors
		[Id("c4d3ceff-ccca-47e7-9749-cfae1c1154bf")]
		[AssociationId("7c6cff28-d2b9-434c-acae-1151d1d9dcea")]
		[RoleId("579807ac-8c19-4323-905a-068bb0bc7f9f")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderStatusClass))]
		[Plural("CurrentOrderStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentOrderStatus;

		#region Allors
		[Id("c90e107b-6b47-4337-9937-391eacd1b1c5")]
		[AssociationId("f496f8ec-b2f8-4264-96bc-6d6567b46d11")]
		[RoleId("87824813-0d60-4e7e-af9c-5ad441913820")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("PreviousBillToCustomers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PreviousBillToCustomer;

		#region Allors
		[Id("ce771472-d789-4077-80bb-25622624e1df")]
		[AssociationId("6d50f1f0-8e69-4fca-960e-31c48bddadea")]
		[RoleId("f7da8b79-e2cd-492d-a721-88d3c8fc530c")]
		#endregion
		[Indexed]
		[Type(typeof(SalesChannelClass))]
		[Plural("SalesChannels")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesChannel;

		#region Allors
		[Id("d6714c09-dce1-4182-aa2f-bbc887edc89a")]
		[AssociationId("9d679860-d975-4a0a-aef4-08975f45d855")]
		[RoleId("23fc03a8-a44c-431f-bdfa-75905691764b")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("PlacingCustomers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PlacingCustomer;

		#region Allors
		[Id("d85bdcb7-cfce-4bfd-9fd3-dfe039138be1")]
		[AssociationId("38f0331a-3a80-4f11-b19a-cfedfe89d520")]
		[RoleId("51aab220-38bd-42f9-b33b-ce384f0e4471")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderStatusClass))]
		[Plural("OrderStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType OrderStatus;

		#region Allors
		[Id("da5a63d2-33bb-4da3-a1bf-064280cac0fa")]
		[AssociationId("05da158d-3f90-4c1f-9bdf-22263b285ed1")]
		[RoleId("d2390ce6-ea3d-43da-9a48-966e9274bcc2")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesInvoiceClass))]
		[Plural("ProformaInvoices")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType ProformaInvoice;

		#region Allors
		[Id("eb5a3564-996d-4bbe-b592-6205adad93b8")]
		[AssociationId("37612ba0-d689-49ca-9005-3b3bf21cd272")]
		[RoleId("bd5d13f3-c1e7-4eea-9c33-09f4b47289f3")]
		#endregion
		[Indexed]
		[Type(typeof(SalesOrderItemClass))]
		[Plural("SalesOrderItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType SalesOrderItem;

		#region Allors
		[Id("f20ac339-0761-410b-bbb6-6fb393bcba8a")]
		[AssociationId("cf9fcfa0-a862-4cb2-88b4-c4dcd6d0034d")]
		[RoleId("6ec955c9-6d25-45b3-a4d1-5e4270d28750")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("f7b7b4d2-fd9e-4d29-99be-f69b2967cc3b")]
		[AssociationId("ae9335e4-4d72-40fc-b028-dcfd7ea67cfa")]
		[RoleId("d70334f4-c9f6-4804-a887-2969d75c8644")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("InitialMarkupPercentages")]
		public RelationType InitialMarkupPercentage;

		#region Allors
		[Id("ff972b6c-ab12-4596-a1bc-18f93127ac31")]
		[AssociationId("556771a7-0f67-4061-8c88-c8401bf0b1c1")]
		[RoleId("6d7ee57e-35b3-4a19-ad1f-4a1850c41568")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("TakenByInternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TakenByInternalOrganisation;

		public static SalesOrderClass Instance {get; internal set;}

		internal SalesOrderClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.TotalPurchasePrice.RoleType.IsRequired = true;
            this.TotalListPriceCustomerCurrency.RoleType.IsRequired = true;
            this.MaintainedProfitMargin.RoleType.IsRequired = true;
            this.InitialProfitMargin.RoleType.IsRequired = true;
            this.TotalListPrice.RoleType.IsRequired = true;
            this.MaintainedMarkupPercentage.RoleType.IsRequired = true;
            this.InitialMarkupPercentage.RoleType.IsRequired = true;

            this.CurrentObjectState.RoleType.IsRequired = true;
            this.TakenByInternalOrganisation.RoleType.IsRequired = true;
            this.Store.RoleType.IsRequired = true;
            this.PartiallyShip.RoleType.IsRequired = true;


            this.ConcreteRoleTypeByRoleType[OrderInterface.Instance.CustomerCurrency.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[OrderInterface.Instance.DeliveryDate.RoleType].IsRequiredOverride = true;
        }
    }
}