namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d79f734d-4434-4710-a7ea-7d6306f3064f")]
	#endregion
	[Plural("InvoiceItems")]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(TransitionalInterface))]

  	public partial class InvoiceItemInterface: Interface
	{
		#region Allors
		[Id("0599c28d-11f3-4ccb-b78c-2d6c8748b952")]
		[AssociationId("a8520b8c-37c2-4b4b-a31d-649ba867f9b8")]
		[RoleId("04cdcfac-28ee-4c0d-9e6f-aaa2b5297eea")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalIncVatsCustomerCurrency")]
		public RelationType TotalIncVatCustomerCurrency;

		#region Allors
		[Id("067674d0-6d9b-4a7e-b0c6-62c24f3a4815")]
		[AssociationId("72cdddb8-711d-491c-9965-cef190a10913")]
		[RoleId("5f894db7-f9ed-47d0-a438-c2e00446edbf")]
		#endregion
		[Indexed]
		[Type(typeof(AgreementTermInterface))]
		[Plural("InvoiceTerms")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType InvoiceTerm;

		#region Allors
		[Id("0805a468-9d72-4199-a88e-402b84fbe3e6")]
		[AssociationId("3e586376-57fc-45e3-930a-49ac79c66431")]
		[RoleId("6dbb9536-45c2-4ce4-b215-9ffac8f96450")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalVatsCustomerCurrency")]
		public RelationType TotalVatCustomerCurrency;

		#region Allors
		[Id("11bfeaf2-f89e-433c-aef2-7154a5e1fa9a")]
		[AssociationId("647c6c5f-c2b8-4e39-b5cb-5860dda100b2")]
		[RoleId("c2804f56-758d-4677-83f7-dac3671fa0b7")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalBasePrices")]
		public RelationType TotalBasePrice;

		#region Allors
		[Id("1b42d64f-db5e-4c28-8234-53458f269c0a")]
		[AssociationId("11e16623-b6d5-40cf-9ea0-c4dd6b189105")]
		[RoleId("ff7a63e0-0805-4ed5-a358-8bb87c418829")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalSurcharges")]
		public RelationType TotalSurcharge;

		#region Allors
		[Id("1f92aed8-8a8f-4eb6-8102-83a6395788d6")]
		[AssociationId("b65ecb61-b074-47fc-aac7-74119295c827")]
		[RoleId("e8c62a38-a856-4db6-a971-575d7971689c")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalInvoiceAdjustments")]
		public RelationType TotalInvoiceAdjustment;

		#region Allors
		[Id("2fe594e6-1dfe-4be5-9842-98e7e669a8c4")]
		[AssociationId("d8846b2b-476b-4396-89f3-273e6fb5c01e")]
		[RoleId("ea7e4254-fdbb-4427-805a-0025245051ec")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalExVatsCustomerCurrency")]
		public RelationType TotalExVatCustomerCurrency;

		#region Allors
		[Id("32587de5-c4e7-4048-b21d-d3770bda87b0")]
		[AssociationId("6d099688-2cf7-463d-bce6-44f171e6d375")]
		[RoleId("bb0476d5-0953-43dc-a461-f19a1ae4d7c4")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalDiscounts")]
		public RelationType TotalDiscount;

		#region Allors
		[Id("33caab05-ec61-4cf9-b903-b5d5a8d7eef9")]
		[AssociationId("77489b35-b46a-4540-8359-005adbd9d1f9")]
		[RoleId("cf9b4f4a-b867-4a47-919e-2cb90be72980")]
		#endregion
		[Indexed]
		[Type(typeof(InvoiceVatRateItemClass))]
		[Plural("InvoiceVatRateItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType InvoiceVatRateItem;

		#region Allors
		[Id("374fd832-986e-44fb-b010-5db6ecbdc29a")]
		[AssociationId("5d08d863-b624-4a39-984d-63e58a3c39e6")]
		[RoleId("aca753e1-37e1-44a3-b709-41171ae38b9a")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalDiscountsAsPercentage")]
		public RelationType TotalDiscountAsPercentage;

		#region Allors
		[Id("42c5114f-7963-477d-82cf-09bfa0b194bb")]
		[AssociationId("26a8e75e-bc48-45e1-a957-fd41c813bfe3")]
		[RoleId("033c9d78-7940-4485-844c-dedd7e40bd62")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("CalculatedUnitPrices")]
		public RelationType CalculatedUnitPrice;

		#region Allors
		[Id("43eeabd8-99c6-4f35-a804-0723f695db87")]
		[AssociationId("ca00444f-ae9e-4688-942c-017f46227615")]
		[RoleId("65ffee00-11cc-44b0-8f0e-91dd4007a865")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("UnitDiscounts")]
		public RelationType UnitDiscount;

		#region Allors
		[Id("448b5a4a-f876-48d9-9bae-c770a908997f")]
		[AssociationId("0060c405-1ed9-4f3c-be5c-bd12469ab019")]
		[RoleId("9bd2aa77-b22e-43dc-be67-fddeb648d6c4")]
		#endregion
		[Indexed]
		[Type(typeof(VatRegimeClass))]
		[Plural("AssignedVatRegimes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType AssignedVatRegime;

		#region Allors
		[Id("45289923-9da5-4d4a-b07c-78ee71d30e31")]
		[AssociationId("0dc30627-9f98-4f9e-9d2c-4e09e5994c2d")]
		[RoleId("5a7a7b07-882e-4f3f-bde7-f09c719892ec")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalIncVats")]
		public RelationType TotalIncVat;

		#region Allors
		[Id("475d7a79-27a1-4d5a-90c1-3896fa2e892e")]
		[AssociationId("ad65733c-6d3d-4e90-97d5-ca91bc4505d9")]
		[RoleId("651b29f8-644d-4588-ac56-0d51f2068ebd")]
		#endregion
		[Indexed]
		[Type(typeof(InvoiceItemInterface))]
		[Plural("AdjustmentFors")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType AdjustmentFor;

		#region Allors
		[Id("5f355cb5-5156-4f76-97bc-ec153a41e9ef")]
		[AssociationId("3233b6d6-5bca-401a-ae03-77706efda65b")]
		[RoleId("15d66771-3550-44e6-919b-2bbfbdf8b66d")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("UnitBasePrices")]
		public RelationType UnitBasePrice;

		#region Allors
		[Id("6718d6f1-e62f-4e1f-8368-813ad6fe4417")]
		[AssociationId("83c2ab80-c364-445f-a2dc-33cbc8d88d97")]
		[RoleId("e5b9ea20-d51b-4621-979c-238f43ce2f90")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalSurchargesCustomerCurrency")]
		public RelationType TotalSurchargeCustomerCurrency;

		#region Allors
		[Id("6df95cf4-115f-4f43-aaea-52313c47d824")]
		[AssociationId("93ba1265-4050-41c1-aaf8-d09786889245")]
		[RoleId("0abd9811-a8ac-42bf-9113-4f9760cfe9eb")]
		#endregion
		[Indexed]
		[Type(typeof(SerializedInventoryItemClass))]
		[Plural("SerializedInventoryItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SerializedInventoryItem;

		#region Allors
		[Id("6dfc7def-6790-4841-90db-c37a431593ec")]
		[AssociationId("27f066ba-c894-4ced-9c3f-8af137d0ffb4")]
		[RoleId("88d55469-1e0c-4dc8-83a7-ce0a48d9c9d9")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PriceComponentInterface))]
		[Plural("CurrentPriceComponents")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType CurrentPriceComponent;

		#region Allors
		[Id("741d7629-aa2f-45f9-b66c-4ab8abf07518")]
		[AssociationId("c2f98a95-6577-403a-bb7e-5e3f158fe11b")]
		[RoleId("90e76224-e139-436d-a998-0dc24709e52e")]
		#endregion
		[Indexed]
		[Type(typeof(DiscountAdjustmentClass))]
		[Plural("DiscountAdjustments")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType DiscountAdjustment;

		#region Allors
		[Id("7d3a259d-c27f-45d4-96f1-3a43a0e5043f")]
		[AssociationId("cafd1700-a59e-4278-90a5-cb387f413b8f")]
		[RoleId("721f302f-94eb-4ccd-b688-d13383d21571")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("ActualUnitPrices")]
		public RelationType ActualUnitPrice;

		#region Allors
		[Id("7eed800d-c2b5-4837-a288-150803578b27")]
		[AssociationId("9dbf4d82-0d36-42a0-81a7-49f59e5cd226")]
		[RoleId("f3b11549-8cf9-4ade-8465-111536b00171")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("Messages")]
		public RelationType Message;

		#region Allors
		[Id("8696b970-07e7-4d04-aec5-d42bcd47ce72")]
		[AssociationId("ad3bd0d4-12c1-4994-abaa-c47dc0d17a7a")]
		[RoleId("245ad362-41b4-4fa9-91a6-b52329215d13")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalInvoiceAdjustmentsCustomerCurrency")]
		public RelationType TotalInvoiceAdjustmentCustomerCurrency;

		#region Allors
		[Id("8fd19791-85ed-44c9-8580-a6768578ca3a")]
		[AssociationId("72e1379d-a9c3-41d5-8ae4-a9a82c88ad01")]
		[RoleId("1ca3573f-812f-41ca-a5e8-ec13ea6168aa")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("AmountPaids")]
		public RelationType AmountPaid;

		#region Allors
		[Id("93e0e781-4755-4cd4-aeff-bff905d6e99b")]
		[AssociationId("6f45d5e3-ef25-45ea-9f31-11fc6a480c6c")]
		[RoleId("0f3e4887-e43f-4b51-86ee-c4f474de2d7a")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(VatRateClass))]
		[Plural("DerivedVatRates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DerivedVatRate;

		#region Allors
		[Id("b114b874-9e68-4451-b3f1-c5aa7a139a02")]
		[AssociationId("4f22e0be-bdcb-496e-b7e5-77c21d1c90df")]
		[RoleId("5bfd82d4-0d28-4382-8447-a72fec2fa8a2")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalDiscountsCustomerCurrency")]
		public RelationType TotalDiscountCustomerCurrency;

		#region Allors
		[Id("b20c5190-65f8-4d71-a3f1-30da9b41173a")]
		[AssociationId("5fc0c0b2-4fcc-4f94-853e-eef98f404c28")]
		[RoleId("c85bb656-2979-4144-b56a-cb97005e6de9")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("UnitSurcharges")]
		public RelationType UnitSurcharge;

		#region Allors
		[Id("b3f7be6a-2374-40e3-98e1-095b0117847e")]
		[AssociationId("6c6cffda-dc95-437e-90dc-f98ce86e4fdc")]
		[RoleId("c056df57-dd5b-40f2-8032-ef58f4ec3f7d")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalExVats")]
		public RelationType TotalExVat;

		#region Allors
		[Id("ba90acfe-0d55-4854-a046-35279f872e0b")]
		[AssociationId("d231d38a-2e1e-4e21-8622-d5b30199f857")]
		[RoleId("b525a1c4-5f1f-402f-9f40-105e711bf45d")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Quantities")]
		public RelationType Quantity;

		#region Allors
		[Id("c540c7fe-924d-4616-a49a-515ac65c4cf7")]
		[AssociationId("08276c02-e869-40e9-a99d-63448f6c94fb")]
		[RoleId("71bd0f31-0867-4391-a45d-a8684d50d772")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalSurchargesAsPercentage")]
		public RelationType TotalSurchargeAsPercentage;

		#region Allors
		[Id("cccc995c-1478-4145-ba90-ace3ae7ba184")]
		[AssociationId("d479cadc-2f62-4d56-8e82-96c2b3166b4e")]
		[RoleId("9258d1fc-7788-46f2-baf0-5eff2443fd53")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(VatRegimeClass))]
		[Plural("VatRegimes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType VatRegime;

		#region Allors
		[Id("d4144a78-f466-44e9-a62f-d84bcdf22b0f")]
		[AssociationId("ac68b612-25dc-4ab9-81a2-06c36cc42bed")]
		[RoleId("67073f7a-6888-40e0-adf5-c4938a966a06")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalBasePricesCustomerCurrency")]
		public RelationType TotalBasePriceCustomerCurrency;

		#region Allors
		[Id("e30596f0-7c79-4d13-b1d6-26e4fd1f55f2")]
		[AssociationId("74658b67-6e2d-4c80-b745-35af0f3f8654")]
		[RoleId("00bae3ee-3825-414b-ad25-595b9ed469f9")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalVats")]
		public RelationType TotalVat;

		#region Allors
		[Id("e52e9b0a-4772-465a-bab7-c79372d7000a")]
		[AssociationId("6e45a92d-491d-491d-9a28-ee1421a87aaa")]
		[RoleId("4b9e3dd2-09a5-47a3-be48-8bdbe5a5ca7f")]
		#endregion
		[Indexed]
		[Type(typeof(SurchargeAdjustmentClass))]
		[Plural("SurchargeAdjustments")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType SurchargeAdjustment;

		#region Allors
		[Id("e7a24fa8-f664-4cf0-a392-8b7827a7f537")]
		[AssociationId("ffc4548d-098f-4aa2-8343-309c39583875")]
		[RoleId("8bcba64b-9745-4d98-8f60-f3ce548acd03")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("UnitVats")]
		public RelationType UnitVat;

		#region Allors
		[Id("fb202916-1a87-439e-b2d8-b3f3ed4f681a")]
		[AssociationId("13dda3fd-6011-4876-9860-158d86024dbd")]
		[RoleId("50ab8ac2-daca-4e66-861d-4134fcaa0e98")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static InvoiceItemInterface Instance {get; internal set;}

		internal InvoiceItemInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}