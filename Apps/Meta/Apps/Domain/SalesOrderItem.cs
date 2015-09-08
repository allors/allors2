namespace Allors.Meta
{
	#region Allors
	[Id("80de925c-04cc-412c-83a5-60405b0e63e6")]
	#endregion
	[Inherit(typeof(OrderItemInterface))]
	public partial class SalesOrderItemClass : Class
	{
        #region Allors
        [Id("F04381CD-3B28-4DD5-BBE8-873C5A56AEE2")]
        #endregion
        public MethodType Continue;

        #region Allors
        [Id("0229942b-e102-4e97-af8d-97ee8383203e")]
		[AssociationId("e1e640d4-4096-42df-9c12-6bf54e5314db")]
		[RoleId("16e9993a-6604-41e9-9ed0-053480d45d46")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("InitialProfitMargins")]
		public RelationType InitialProfitMargin;

		#region Allors
		[Id("14d596e8-adec-46bc-b260-af37d24a1035")]
		[AssociationId("4f846efe-5546-4709-8f31-2470a3e3650e")]
		[RoleId("ad606374-6b77-4ddc-b907-e190faf7da0e")]
		#endregion
		[Indexed]
		[Type(typeof(SalesOrderItemStatusClass))]
		[Plural("CurrentPaymentStatuses")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentPaymentStatus;

		#region Allors
		[Id("1e1ed439-ae25-4446-83e6-295d8627a7b5")]
		[AssociationId("67bc37d9-0d6f-4227-81c9-8f03a1e0da47")]
		[RoleId("d8ab230a-92d2-44cb-8e45-502285dd9a5e")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("QuantitiesShortFalled")]
		public RelationType QuantityShortFalled;

		#region Allors
		[Id("1ea02a2c-280a-4a48-9ffb-1517789c56f1")]
		[AssociationId("851f33e4-6c43-468d-ab0d-0f5f83bdb179")]
		[RoleId("213d2b36-dbfd-4e2d-a854-82ba271f0d94")]
		#endregion
		[Indexed]
		[Type(typeof(OrderItemInterface))]
		[Plural("OrderedWithFeatures")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType OrderedWithFeature;

		#region Allors
		[Id("1edd9008-537a-43ba-b4a1-56d3c3211c36")]
		[AssociationId("db9987b4-b71c-4ece-b4c1-53bb27a02dff")]
		[RoleId("ae3bd8e0-1f58-45e1-a6dc-191d7668e358")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("MaintainedProfitMargins")]
		public RelationType MaintainedProfitMargin;

		#region Allors
		[Id("2bd8163c-b2cd-49bc-922a-b8c859c24031")]
		[AssociationId("3f66929d-a2f1-4e9b-a701-4364e3a25e1d")]
		[RoleId("1fbf819e-b7fe-4ce3-86af-efea369db2fa")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("RequiredProfitMargins")]
		public RelationType RequiredProfitMargin;

		#region Allors
		[Id("3072d12f-e8de-43ba-a63c-f557744a1d5d")]
		[AssociationId("9b642b30-ec44-4877-a191-974704f6d8df")]
		[RoleId("1eb0ad30-68f6-4d63-9e07-96f7e90005ee")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderItemStatusClass))]
		[Plural("OrderItemStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType OrderItemStatus;

		#region Allors
		[Id("3dbd9a9b-8cda-4cf3-890d-2e6af4e47018")]
		[AssociationId("fba228cb-4b3f-4f50-8b6a-f16572ba3977")]
		[RoleId("9d69d9af-7c23-4a1d-a4f2-09ef58881ce8")]
		#endregion
		[Indexed]
		[Type(typeof(SalesOrderItemStatusClass))]
		[Plural("CurrentShipmentStatuses")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentShipmentStatus;

		#region Allors
		[Id("3e798309-d5d5-4860-87ec-ba3766e96c9e")]
		[AssociationId("4626b586-07e1-468c-877a-d1a8f1b196c5")]
		[RoleId("b2aef5ac-45f7-41aa-8e1b-f2d79d3d9fad")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(NonSerializedInventoryItemClass))]
		[Plural("PreviousReservedFromInventoryItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PreviousReservedFromInventoryItem;

		#region Allors
		[Id("40111efb-f609-4726-85c1-a9dd7160df72")]
		[AssociationId("1f0c1c76-4e49-4c31-9b35-cfa5d842039b")]
		[RoleId("4e3983f7-09db-4e06-bf47-a2a4dbcdfa40")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("QuantitiesShipNow")]
		public RelationType QuantityShipNow;

		#region Allors
		[Id("48e40504-bb22-4b11-949d-569b3a556416")]
		[AssociationId("979f6fa2-29f4-43c0-86d7-761509719112")]
		[RoleId("6b6dab9b-1583-4f3d-8d97-1a8a53af9e75")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("RequiredMarkupPercentages")]
		public RelationType RequiredMarkupPercentage;

		#region Allors
		[Id("545eb094-63d8-4d25-a069-7c3e91f26eb7")]
		[AssociationId("686d5956-c2dc-46d5-b812-52020d392f0f")]
		[RoleId("3a8adaf6-82e6-45a6-bd5f-61860125d77b")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("QuantitiesShipped")]
		public RelationType QuantityShipped;

		#region Allors
		[Id("5bf138bd-27c1-4291-91da-b543170bf160")]
		[AssociationId("c4fab99d-b408-437a-aea3-05cf32afa5d4")]
		[RoleId("76f3c438-e027-492d-bae4-932d81f455df")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderItemStatusClass))]
		[Plural("CurrentOrderItemStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentOrderItemStatus;

		#region Allors
		[Id("5cc50f26-361b-46d7-a8e6-a9f53f7d2722")]
		[AssociationId("0d8906e9-3bfd-4d9b-8b24-8526fdfb2e33")]
		[RoleId("000b641f-00be-4b9c-84aa-a8c968024ece")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PostalAddressClass))]
		[Plural("ShipToAddresses")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipToAddress;

		#region Allors
		[Id("64edd3e6-0b78-4b34-8a11-aa9c0a1b1f35")]
		[AssociationId("667a1304-05ae-410a-94a1-5e87a40dc53b")]
		[RoleId("d15456ab-66c1-4ecc-bfc5-910b7d9c4869")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("QuantitiesPicked")]
		public RelationType QuantityPicked;

		#region Allors
		[Id("6826e05e-eb9a-4dc4-a653-0230dec934a9")]
		[AssociationId("aa2b8b0a-672c-423b-9ca8-2fd40f8d1306")]
		[RoleId("793f4946-ed12-49ca-9764-8df534941cca")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("PreviousProducts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PreviousProduct;

		#region Allors
		[Id("710e0b05-01d1-4592-b652-f0fada3dfa45")]
		[AssociationId("9ab86597-c31b-46d7-b546-89ebfd1411cd")]
		[RoleId("9533bbb6-359f-49a3-959b-98fcdd5cc2a7")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderItemObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("75a13fdc-90b2-4550-9b2f-fc0a9387d569")]
		[AssociationId("94922e2d-1570-4667-af8f-5d4415fd6d78")]
		[RoleId("39d62b69-520c-456d-b3f1-6ca640ffc4cb")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("UnitPurchasePrices")]
		public RelationType UnitPurchasePrice;

		#region Allors
		[Id("7a8255f5-4283-4803-9f96-60a9adc2743b")]
		[AssociationId("2c9b2182-7b93-46c9-86ac-d13add6d52b5")]
		[RoleId("7596f471-e54c-4491-8af6-02f0e8d7d015")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("ShipToParties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipToParty;

		#region Allors
		[Id("7ae1b939-b387-4e6e-9da2-bc0364e04f7b")]
		[AssociationId("808f88ba-3866-4785-812c-c062c5f268a4")]
		[RoleId("64639736-a7d0-47cb-8afb-fa751a19670d")]
		#endregion
		[Indexed]
		[Type(typeof(PostalAddressClass))]
		[Plural("AssignedShipToAddresses")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType AssignedShipToAddress;

		#region Allors
		[Id("8145fbd3-a30f-44a0-9520-6b72ac20a82d")]
		[AssociationId("59383e9d-690e-46aa-9cc0-1dd39db14f60")]
		[RoleId("31087f2f-10e8-4558-9e0a-a5dbceb3204a")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("QuantitiesReturned")]
		public RelationType QuantityReturned;

		#region Allors
		[Id("85d11891-5ffe-488f-9f23-5b2c7bc1c480")]
		[AssociationId("283cdb9a-e7e3-4486-92da-5e94653505a8")]
		[RoleId("fd06dd18-c1d4-40c7-b62e-273a8522f580")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("QuantitiesReserved")]
		public RelationType QuantityReserved;

		#region Allors
		[Id("911abda0-2eb0-477e-80be-e9e7d358205e")]
		[AssociationId("23af5657-ed05-43c2-aeed-d268204528d2")]
		[RoleId("42a88fb9-84bc-4e35-83ff-6cb5c0cf3c96")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("SalesReps")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesRep;

		#region Allors
		[Id("ae30b852-d1d9-4966-a65a-6f16120652f6")]
		[AssociationId("c3a3e068-8683-44b7-a255-47e49a63c453")]
		[RoleId("7a4a9d1b-2cff-4f9c-8b7c-94f08fb68c46")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderItemStatusClass))]
		[Plural("ShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentStatus;

		#region Allors
		[Id("b2d2645e-0d3f-473e-b277-6f890b9b911e")]
		[AssociationId("68281397-74f8-4356-b9fc-014f792ab914")]
		[RoleId("1292e876-1c61-42cb-8f01-8b3eb6cf0fa0")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("AssignedShipToParties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType AssignedShipToParty;

		#region Allors
		[Id("b2f7dabb-8b87-41bc-8903-166d77bba1c5")]
		[AssociationId("ad7dfb12-d00d-4a93-a011-7cb09c1e9aa9")]
		[RoleId("ba9a9c6c-4df0-4488-b5fa-6181e45c6f18")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("QuantitiesPendingShipment")]
		public RelationType QuantityPendingShipment;

		#region Allors
		[Id("b8d116ca-dbab-4119-84ca-c0e196d9c018")]
		[AssociationId("3f2cc31e-84e9-4e49-bfbe-0a436e2236be")]
		[RoleId("7cee282f-ff61-42fc-9a2e-54164e8b6390")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("MaintainedMarkupPercentages")]
		public RelationType MaintainedMarkupPercentage;

		#region Allors
		[Id("d5639e07-37b8-46db-9e35-fa98301d31dd")]
		[AssociationId("43ee44b6-2e51-4ade-8bb7-9b10a780ba2e")]
		[RoleId("38e21291-b24a-4331-b781-f7950df3f501")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("InitialMarkupPercentages")]
		public RelationType InitialMarkupPercentage;

		#region Allors
		[Id("d7c25b48-d82f-4250-b09d-1e935eab665b")]
		[AssociationId("67e9a9d9-74ff-4b04-9aa1-dd08c5348a3e")]
		[RoleId("4bfc1720-a2f6-4204-974b-42ca42c0d2e1")]
		#endregion
		[Indexed]
		[Type(typeof(NonSerializedInventoryItemClass))]
		[Plural("ReservedFromInventoryItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ReservedFromInventoryItem;

		#region Allors
		[Id("e8980105-2c4d-41de-bd67-802a8c0720f1")]
		[AssociationId("8b747457-bf7a-4274-b245-d04607b2a5ba")]
		[RoleId("90d69cb4-d485-418f-9608-44063f116ff4")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("Products")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Product;

		#region Allors
		[Id("ed586b2f-c687-4d97-9416-52f7156b7b11")]
		[AssociationId("cb5c31c4-2daa-405b-8dc9-5ea6c87f66b3")]
		[RoleId("c5b07ead-1a71-407e-91f8-4ec39853888a")]
		#endregion
		[Indexed]
		[Type(typeof(ProductFeatureInterface))]
		[Plural("ProductFeatures")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductFeature;

		#region Allors
		[Id("f148e660-1e09-4e76-97fb-de62a7ee7482")]
		[AssociationId("0105885d-f722-44bd-9f57-6231c38191b5")]
		[RoleId("9132a260-1b35-4b5a-b14c-8dceb6383581")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("QuantitiesRequestsShipping")]
		public RelationType QuantityRequestsShipping;

		#region Allors
		[Id("f2ccd5d6-95e3-4d72-938b-9f430f36ae59")]
		[AssociationId("77472c36-b500-4788-b1b3-22741adec0c0")]
		[RoleId("748df737-edab-476d-a2f2-0f362828c0e7")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SalesOrderItemStatusClass))]
		[Plural("PaymentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PaymentStatus;
        
		public static SalesOrderItemClass Instance {get; internal set;}

		internal SalesOrderItemClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.UnitPurchasePrice.RoleType.IsRequired = true;
            this.InitialMarkupPercentage.RoleType.IsRequired = true;
            this.InitialProfitMargin.RoleType.IsRequired = true;
            this.MaintainedProfitMargin.RoleType.IsRequired = true;
            this.MaintainedMarkupPercentage.RoleType.IsRequired = true;

            this.QuantityShortFalled.RoleType.IsRequired = true;
            this.QuantityShipped.RoleType.IsRequired = true;
            this.QuantityPicked.RoleType.IsRequired = true;
            this.QuantityReserved.RoleType.IsRequired = true;
            this.QuantityPendingShipment.RoleType.IsRequired = true;
            this.QuantityRequestsShipping.RoleType.IsRequired = true;
            this.QuantityReturned.RoleType.IsRequired = true;
            this.QuantityShipped.RoleType.IsRequired = true;

            this.CurrentObjectState.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[OrderItemInterface.Instance.QuantityOrdered.RoleType].IsRequiredOverride = true;
        }
    }
}