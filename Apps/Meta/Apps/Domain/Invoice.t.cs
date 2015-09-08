namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a6f4eedb-b0b5-491d-bcc0-09d2bc109e86")]
	#endregion
	[Plural("Invoices")]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(LocalisedInterface))]
	[Inherit(typeof(TransitionalInterface))]
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(PrintableInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]

  	public partial class InvoiceInterface: Interface
	{
		#region Allors
		[Id("19019399-963f-4075-8754-16e1e5a4c496")]
		[AssociationId("43580c50-7831-48c7-afdf-5c4e23bcec93")]
		[RoleId("371d3a45-0283-4c8d-850f-acc9c2395464")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalShippingAndHandlingsCustomerCurrency")]
		public RelationType TotalShippingAndHandlingCustomerCurrency;

		#region Allors
		[Id("1c535b3f-bb97-43a8-bd29-29c4dc267814")]
		[AssociationId("d3155310-1267-4780-b69d-4dd47ef15e73")]
		[RoleId("2603b50f-78b9-429c-be30-38949bdec59a")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("CustomerCurrencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CustomerCurrency;

		#region Allors
		[Id("2d82521d-30bd-4185-84c7-4dfe08b5ddef")]
		[AssociationId("aa6230a9-7a9e-4d42-a14a-49b1c3b382ab")]
		[RoleId("5c1fbd73-39e2-4a4a-b58b-2e6c7a110755")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("2f7d19a8-75e1-4c95-b60f-60343f2dd4bc")]
		[AssociationId("31c04533-c845-4b71-bf88-79e4c3ad8ec4")]
		[RoleId("6bdaa705-8ea0-4df2-936a-0b392556a21d")]
		#endregion
		[Indexed]
		[Type(typeof(ShippingAndHandlingChargeClass))]
		[Plural("ShippingAndHandlingCharges")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType ShippingAndHandlingCharge;

		#region Allors
		[Id("3aa6c5be-ae74-401e-8d03-420230c4ea42")]
		[AssociationId("3d5e1fcc-a9d7-4c30-9e3b-90091f5ec63e")]
		[RoleId("133d6dcb-5429-42aa-b788-2eccd2633139")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalFeesCustomerCurrency")]
		public RelationType TotalFeeCustomerCurrency;

		#region Allors
		[Id("3b1a0c47-dd3e-406c-a1e7-bc88f7a10794")]
		[AssociationId("7ce35340-fbb1-4689-a4e5-2a7f17455d37")]
		[RoleId("72db7683-8659-441f-ae23-0407e4e11c11")]
		#endregion
		[Indexed]
		[Type(typeof(FeeClass))]
		[Plural("Fees")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType Fee;

		#region Allors
		[Id("3b6e0ea5-d9b9-4673-89e9-1491b8e6a691")]
		[AssociationId("cd0ba793-77fb-44b4-bf1c-4bf32a98d254")]
		[RoleId("d677c967-568a-4c32-a822-caba4ea5990c")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalExVatsCustomerCurrency")]
		public RelationType TotalExVatCustomerCurrency;

		#region Allors
		[Id("4b2eedbb-ec59-4e18-949f-f467e41f6401")]
		[AssociationId("b41474a8-482f-458f-b70d-b11e97129ea0")]
		[RoleId("5bab4dea-3566-4421-96c5-27b774b6542a")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("CustomerReferences")]
		public RelationType CustomerReference;

		#region Allors
		[Id("4b3a3ad0-d624-46f1-a53c-f79980b50793")]
		[AssociationId("72a0c734-9199-45fd-8264-a80721a016f2")]
		[RoleId("95e67307-5e1b-451a-ab4a-c93079b25c76")]
		#endregion
		[Indexed]
		[Type(typeof(DiscountAdjustmentClass))]
		[Plural("DiscountAdjustments")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType DiscountAdjustment;

		#region Allors
		[Id("4d3f69a0-6e9d-4ba3-acd8-e5dab2a7f401")]
		[AssociationId("4ac19707-3c95-4b7d-b281-2f9d86c3eeb9")]
		[RoleId("15779f7b-07ce-4373-a9cd-1ee5690ddbfc")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("AmountsPaid")]
		public RelationType AmountPaid;

		#region Allors
		[Id("6b474ddd-c2fd-4db1-bf18-44c86a309d53")]
		[AssociationId("01576aed-1f77-47db-bf04-40aa5dcae63a")]
		[RoleId("f0bd433a-f5cc-4a6d-be5f-8f09594aa566")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalDiscounts")]
		public RelationType TotalDiscount;

		#region Allors
		[Id("6ea961d5-89fc-4526-922a-80538ecb5654")]
		[AssociationId("66c5cfdd-6af4-4d75-b826-843be3b01bca")]
		[RoleId("f560bb3d-f855-4ec3-a5e7-4bd6c4da2595")]
		#endregion
		[Indexed]
		[Type(typeof(BillingAccountClass))]
		[Plural("BillingAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillingAccount;

		#region Allors
		[Id("7b6ab1ed-845d-4671-bda2-43ad2327ea53")]
		[AssociationId("d0994e3f-4741-4f9e-9f4f-8923ed3afdf3")]
		[RoleId("4b4902c0-780d-4de2-97ff-54a5f3bdc521")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalsIncVat")]
		public RelationType TotalIncVat;

		#region Allors
		[Id("7e8de8bd-f1c0-4fa5-a629-34d9d5f71b85")]
		[AssociationId("483b0b71-a4a8-4606-a432-d98d8bd262a2")]
		[RoleId("32e8201c-9e71-48e7-ae20-972e14ea4aeb")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalSurcharges")]
		public RelationType TotalSurcharge;

		#region Allors
		[Id("7fda150d-44c8-45a9-8048-dfe38d936c3e")]
		[AssociationId("e2199200-562f-474d-a822-094fba167dc6")]
		[RoleId("09cba9f7-d85f-4c54-a857-c28f22f0eaae")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalBasePrices")]
		public RelationType TotalBasePrice;

		#region Allors
		[Id("8247ec6f-e4b2-424c-922b-a6a5d6b05654")]
		[AssociationId("5dd3f140-ff9d-4f87-930c-94b135dd9b7f")]
		[RoleId("d8cbb429-edc1-4d2c-90d3-99b7a4caabb8")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalVatsCustomerCurrency")]
		public RelationType TotalVatCustomerCurrency;

		#region Allors
		[Id("82541f62-bf0e-4e33-9971-15a5a4fa4469")]
		[AssociationId("b3579af4-1c8e-46c5-bc1c-a9d7711a4a48")]
		[RoleId("d54fdbf9-c580-4a49-b058-28aab77d81e0")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("InvoiceDates")]
		public RelationType InvoiceDate;

		#region Allors
		[Id("8798a760-de3d-4210-bd22-165582728f36")]
		[AssociationId("d0d6a00a-2d79-4798-b51f-7e6dfb8551d5")]
		[RoleId("c1f88c71-2415-4928-ae3b-16c7f85af30c")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EntryDates")]
		public RelationType EntryDate;

		#region Allors
		[Id("8e1b6a86-eceb-412e-a246-bd95b564c87b")]
		[AssociationId("8e08972a-f735-4a67-b941-7136e29434be")]
		[RoleId("bd40de52-c9c7-48e0-9129-b1d471a03097")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalIncVatsCustomerCurrency")]
		public RelationType TotalIncVatCustomerCurrency;

		#region Allors
		[Id("94029787-f838-47bb-9617-807a8514a350")]
		[AssociationId("92badbf6-7d16-46b2-b214-1ea26855970d")]
		[RoleId("2dc528f7-451e-4570-922b-649d9448ed11")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalsShippingAndHandling")]
		public RelationType TotalShippingAndHandling;

		#region Allors
		[Id("96baf5df-43b2-4f4b-aca9-bdd2432318a7")]
		[AssociationId("52eba17e-91db-4a52-8b4e-98a5eadbde84")]
		[RoleId("1461fa7e-cc65-4006-a8be-cc6220291b7d")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalBasePricesCustomerCurrency")]
		public RelationType TotalBasePriceCustomerCurrency;

		#region Allors
		[Id("982949e0-87ac-400c-8830-a779b75e10ad")]
		[AssociationId("0892c266-1b04-4d66-b344-1e29ddf09bd4")]
		[RoleId("f1fb8739-1cb1-4080-ac63-b78512218d3a")]
		#endregion
		[Indexed]
		[Type(typeof(SurchargeAdjustmentClass))]
		[Plural("SurchargeAdjustments")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType SurchargeAdjustment;

		#region Allors
		[Id("9eec85a4-e41a-4ca2-82fa-2dc0aa45c9d5")]
		[AssociationId("26c9285b-4c0e-443e-914b-ceb95d37a8fe")]
		[RoleId("4d9bb0e9-23b1-429e-bf61-2fa3b9afb2b8")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalsExVat")]
		public RelationType TotalExVat;

		#region Allors
		[Id("9ff2d65b-0478-41cc-b70b-0df90cdbe190")]
		[AssociationId("38654202-df58-4f2a-9c8d-094fb511a19a")]
		[RoleId("a12bdf85-5c6d-43e4-92b2-8f2fefc03e3e")]
		#endregion
		[Indexed]
		[Type(typeof(InvoiceTermClass))]
		[Plural("InvoiceTerms")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType InvoiceTerm;

		#region Allors
		[Id("a9504981-4b3e-406c-9fc8-64204efb1deb")]
		[AssociationId("409180f3-1b30-457d-86dd-44d4ee834b3e")]
		[RoleId("740953b9-578e-4d0e-916d-5e283a1825be")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalSurchargesCustomerCurrency")]
		public RelationType TotalSurchargeCustomerCurrency;

		#region Allors
		[Id("ab342937-1e58-4cd7-99b5-c8a5e7afe317")]
		[AssociationId("0cd0981d-d26b-42e4-a50d-9747a1171b12")]
		[RoleId("431bbc5d-4de6-4cee-aa2d-f1f5c6e7e745")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("InvoiceNumbers")]
		public RelationType InvoiceNumber;

		#region Allors
		[Id("b298c12c-620b-4cf2-b47e-df17afc65552")]
		[AssociationId("4eff42a0-dfe5-440c-a2d2-7612ece8ff11")]
		[RoleId("92365fb1-d257-4fbd-81e4-097ef6d2405e")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("Messages")]
		public RelationType Message;

		#region Allors
		[Id("c2ecfd15-7662-45b4-99bd-9093ca108d23")]
		[AssociationId("32efeb84-a275-4b14-ba1f-aa99ba1bc776")]
		[RoleId("4e4351e1-7174-4337-b448-bd3f79e3aaa4")]
		#endregion
		[Indexed]
		[Type(typeof(VatRegimeClass))]
		[Plural("VatRegimes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType VatRegime;

		#region Allors
		[Id("c6a896be-d9e1-40b1-9f85-52dbf2886a58")]
		[AssociationId("a6369011-976f-49eb-bd82-69f82ab580f0")]
		[RoleId("7032574d-2cb1-4e08-8d4d-1eb102502c63")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalDiscountsCustomerCurrency")]
		public RelationType TotalDiscountCustomerCurrency;

		#region Allors
		[Id("c7350047-9282-41c8-8d82-4e1f86369e9c")]
		[AssociationId("0468ccd7-0e03-4bff-8812-ee1f979a6a3f")]
		[RoleId("09a4e368-3d7e-4dd5-8708-fa9ff5bddc4b")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalsVat")]
		public RelationType TotalVat;

		#region Allors
		[Id("fa826458-5423-43dd-b02f-fe2673a2d0f3")]
		[AssociationId("ac559656-d5c1-4325-a267-9775136a25af")]
		[RoleId("837d36ee-f23f-45bc-87a9-9760d08f29c4")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("TotalsFee")]
		public RelationType TotalFee;



		public static InvoiceInterface Instance {get; internal set;}

		internal InvoiceInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}