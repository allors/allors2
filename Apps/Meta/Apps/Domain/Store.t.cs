namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d8611e48-b0ba-4037-a992-09e3e26c6d5d")]
	#endregion
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Stores")]
	public partial class StoreClass : Class
	{
		#region Allors
		[Id("0a0ad3b1-afa2-4c78-8414-e657fabebb3e")]
		[AssociationId("c460c53b-1460-4bc2-8390-98e9c1492b71")]
		[RoleId("06502486-41cb-4840-856e-7d44c0038375")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("ShipmentThreshold")]
		public RelationType ShipmentThreshold;

		#region Allors
		[Id("124a58f1-f7a3-43d1-8f4d-0a068b7a2659")]
		[AssociationId("87a6b9dd-5c38-4b79-a0e4-75e6777f5207")]
		[RoleId("5725c16f-b079-4435-a347-660fe9de7223")]
		#endregion
		[Indexed]
		[Type(typeof(CounterClass))]
		[Plural("SalesOrderCounters")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesOrderCounter;

		#region Allors
		[Id("190d2363-affa-4a4b-8662-d8b566c506d6")]
		[AssociationId("76194162-1ef0-45f0-8804-2183f42e0e17")]
		[RoleId("b4d8f605-9a66-468d-b9a0-f2157e8528b7")]
		#endregion
		[Indexed]
		[Type(typeof(StringTemplateClass))]
		[Plural("SalesInvoiceTemplates")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType SalesInvoiceTemplate;

		#region Allors
		[Id("3a837bae-993a-4765-8d4f-b690bf65dc79")]
		[AssociationId("0304eacc-65bc-475d-9a82-00b0cdb233ad")]
		[RoleId("21c8c056-2997-4f75-82db-597e258dceb6")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("OutgoingShipmentNumberPrefixes")]
		public RelationType OutgoingShipmentNumberPrefix;

		#region Allors
		[Id("3e378f04-0d14-4b03-b8e2-b58da3039184")]
		[AssociationId("b4f8b63a-d4c6-4a40-a603-84c4225f02ed")]
		[RoleId("3a00ec26-a46e-4262-aed0-56cb25abf2b1")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("SalesInvoiceNumberPrefixes")]
		public RelationType SalesInvoiceNumberPrefix;

		#region Allors
		[Id("4927a65d-a9d3-4fad-afce-1ec8679d3a55")]
		[AssociationId("e2dc511c-86b0-46fe-b5cf-680dfe012f47")]
		[RoleId("f18df944-920c-474a-ac8e-2e10b460c522")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("PaymentGracePeriods")]
		public RelationType PaymentGracePeriod;

		#region Allors
		[Id("4a647ddb-9a17-4544-8cae-6204140c413a")]
		[AssociationId("d657040d-138b-4f6f-9dc7-547448a1fd11")]
		[RoleId("93cab392-5d64-4270-9e5d-ac3a62dcde4d")]
		#endregion
		[Indexed]
		[Type(typeof(MediaClass))]
		[Plural("LogoImages")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType LogoImage;

		#region Allors
		[Id("555c3b9a-7556-4fdf-a431-6d18a6ae7cbd")]
		[AssociationId("b3625e45-4568-4030-85cc-565f77ccc1a1")]
		[RoleId("5bda243d-b0c9-47bd-9d33-9fd3723512b9")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("PaymentsNetDays")]
		public RelationType PaymentNetDays;

		#region Allors
		[Id("63d433b9-8cb3-428b-b516-be25f1895673")]
		[AssociationId("273cfb27-2698-469b-91ab-24901a4df9fd")]
		[RoleId("67f48e4c-6e56-47c8-a87f-47160453ece6")]
		#endregion
		[Indexed]
		[Type(typeof(FacilityInterface))]
		[Plural("DefaultFacilities")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DefaultFacility;

		#region Allors
		[Id("6e4b701a-2540-4cec-8413-50bfb69d3a7c")]
		[AssociationId("2a1d8fe1-51af-4747-b6e4-7c2532e5fa8c")]
		[RoleId("2bcd6952-16c1-4153-a23c-6e58fae6a49c")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Name")]
		public RelationType Name;

		#region Allors
		[Id("72ea05f1-a631-4dec-a568-1307e380d41f")]
		[AssociationId("3799c587-25f3-4d18-a671-2d2301dae0df")]
		[RoleId("c6bb5b3c-c599-427d-9c42-dc1966f11ce5")]
		#endregion
		[Indexed]
		[Type(typeof(StringTemplateClass))]
		[Plural("SalesOrderTemplates")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType SalesOrderTemplate;

		#region Allors
		[Id("79244ed7-6388-48ca-86db-7b57a64fe680")]
		[AssociationId("5d145726-fa9e-46f9-b389-7f8380e0088c")]
		[RoleId("1df01d8b-9d72-495c-baae-0f5ea4c9e76c")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("CreditLimits")]
		public RelationType CreditLimit;

		#region Allors
		[Id("7c9cda07-5920-4037-b934-5b74355c4b85")]
		[AssociationId("0da06b1f-12ce-43d1-9e21-82e506ce7750")]
		[RoleId("f1c26a78-d986-4fcc-ac55-3658783790ef")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentMethodClass))]
		[Plural("DefaultShipmentMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DefaultShipmentMethod;

		#region Allors
		[Id("80670a7a-1be8-4407-917e-fa359e632519")]
		[AssociationId("dcd8b2e0-7490-40e4-ae4b-d1b0c0be0527")]
		[RoleId("72993b04-4c10-4467-9d99-064e1b39f9e2")]
		#endregion
		[Indexed]
		[Type(typeof(CarrierClass))]
		[Plural("DefaultCarriers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DefaultCarrier;

		#region Allors
		[Id("8a3d0121-e5f9-4bc9-a829-340e1b4b5402")]
		[AssociationId("d92f46c9-07aa-4be9-b0ab-36d66b24ae5e")]
		[RoleId("7d03bd42-e0fa-4053-bf9c-c45b06fcff97")]
		#endregion
		[Indexed]
		[Type(typeof(CounterClass))]
		[Plural("SalesInvoiceCounters")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesInvoiceCounter;

		#region Allors
		[Id("954d4e3c-f188-45f4-98b8-ece14ac7dabd")]
		[AssociationId("a08d71c8-6aa1-4ae3-bccc-a8078bd51071")]
		[RoleId("92f5fc7a-eabe-4710-b1fe-aa35e7fd1606")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("OrdersThreshold")]
		public RelationType OrderThreshold;

		#region Allors
		[Id("9a0dfe33-016a-4b41-979c-d17a6f87d2d2")]
		[AssociationId("a6741bcc-527d-4cbe-bd1e-fc881fd30951")]
		[RoleId("611edb41-213f-4ade-8ea5-a512b99ee9b6")]
		#endregion
		[Indexed]
		[Type(typeof(PaymentMethodInterface))]
		[Plural("DefaultPaymentMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DefaultPaymentMethod;

		#region Allors
		[Id("b64dd0b0-8f35-4c71-9e7c-f47ee7ea1097")]
		[AssociationId("6a3bac6d-f9c6-460e-9c02-29728c567109")]
		[RoleId("aaa0ce97-c540-42d7-aed3-e6b3430b1f23")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("Owners")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Owner;

		#region Allors
		[Id("bc11d48f-bcab-4880-afe8-0a52d3c11e44")]
		[AssociationId("d44420aa-80fc-4d55-8032-18b1b1c63d69")]
		[RoleId("6a37b722-41b4-411e-bc84-18918990ad14")]
		#endregion
		[Indexed]
		[Type(typeof(FiscalYearInvoiceNumberClass))]
		[Plural("FiscalYearInvoiceNumbers")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType FiscalYearInvoiceNumber;

		#region Allors
		[Id("ca82d0f8-f886-4936-80f5-a7dbb7c550b5")]
		[AssociationId("94284261-b6db-4d74-ae74-955f6481375f")]
		[RoleId("49940864-e0db-4d0b-a607-b1725e6f45c9")]
		#endregion
		[Indexed]
		[Type(typeof(PaymentMethodInterface))]
		[Plural("PaymentMethods")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType PaymentMethod;

		#region Allors
		[Id("dfc3f6be-0a95-49e0-8742-3901dbab5185")]
		[AssociationId("92072a30-fdb0-42c2-b4b6-f1fa53e83e7e")]
		[RoleId("9fc009b2-0c9c-446f-ba4f-39144cb6c90d")]
		#endregion
		[Indexed]
		[Type(typeof(CounterClass))]
		[Plural("OutgoingShipmentCounters")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType OutgoingShipmentCounter;

		#region Allors
		[Id("e00e948e-6fc3-43fd-a49b-008fc6d6133f")]
		[AssociationId("3f5cbcd9-c36b-4792-b1fc-15cf533ba6f3")]
		[RoleId("a7e750a0-2e69-485d-8208-ab04682b6efd")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("SalesOrderNumberPrefixes")]
		public RelationType SalesOrderNumberPrefix;

		#region Allors
		[Id("f024d205-2420-40bb-ab1d-71533fa25557")]
		[AssociationId("68800a34-f973-4339-b5d5-7c611b39a2b1")]
		[RoleId("e858cc6c-2a6e-4c04-be7a-e019009bf3f7")]
		#endregion
		[Indexed]
		[Type(typeof(StringTemplateClass))]
		[Plural("CustomerShipmentTemplates")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType CustomerShipmentTemplate;



		public static StoreClass Instance {get; internal set;}

		internal StoreClass() : base(MetaPopulation.Instance)
        {
        }
	}
}