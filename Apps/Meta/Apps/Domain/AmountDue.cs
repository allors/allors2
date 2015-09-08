namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("848053ee-18d8-4962-81c3-bd6c7837565a")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("AmountsDue")]
	public partial class AmountDueClass : Class
	{
		#region Allors
		[Id("0274d4d3-3f07-408c-89e3-f5367acd5fab")]
		[AssociationId("9c7d4eeb-36fc-47f9-8a88-ea3e9cc0ce77")]
		[RoleId("d961ac61-9dc2-4ce0-87aa-24a69cc8fbc4")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Amounts")]
		public RelationType Amount;

		#region Allors
		[Id("3856c988-32d3-455d-89d8-aa1eaa80dcce")]
		[AssociationId("896a030b-a038-4b05-8af5-982f7050c0ea")]
		[RoleId("636344f0-e795-448a-8f37-53d1ebd87e9b")]
		#endregion
		[Indexed]
		[Type(typeof(PaymentMethodInterface))]
		[Plural("PaymentMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PaymentMethod;

		#region Allors
		[Id("39d2f4f2-0c16-40f5-990b-38bad15fae99")]
		[AssociationId("cada8a73-b732-4789-aa7b-a4caeaea20e2")]
		[RoleId("341bd110-6126-476c-9d35-1069c207dc1b")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("TransactionDates")]
		public RelationType TransactionDate;

		#region Allors
		[Id("3ca978b2-8c0a-4fec-8b98-88e9ea3b2966")]
		[AssociationId("90befe3a-0821-4ec9-bac1-f580ebdaab9e")]
		[RoleId("6b0f0eed-a757-4668-97bb-9d82ed4ff983")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("BlockedForDunnings")]
		public RelationType BlockedForDunning;

		#region Allors
		[Id("43193cac-15ad-4a1a-8945-f4ecb7d93291")]
		[AssociationId("96a1c683-052a-446e-8d82-c1943caaf53f")]
		[RoleId("fc8ed608-7c2a-4eaf-9240-0d233184b5b3")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("AmountsVat")]
		public RelationType AmountVat;

		#region Allors
		[Id("5cb888fd-bcff-4eef-8ad6-efab3434364d")]
		[AssociationId("3c54dbfa-3dbd-419c-b341-4a127bd1387b")]
		[RoleId("e9635cd0-13f0-43fc-ad12-5f65b40c6923")]
		#endregion
		[Indexed]
		[Type(typeof(BankAccountClass))]
		[Plural("BankAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BankAccount;

		#region Allors
		[Id("90b4eaea-21cd-4a04-a64b-3c3dce0718d9")]
		[AssociationId("87f05475-29aa-4cb7-a5d9-865a47995cd6")]
		[RoleId("71271fdc-4a24-4650-a57a-9cbc2973cc04")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ReconciliationDates")]
		public RelationType ReconciliationDate;

		#region Allors
		[Id("953877d2-055c-4274-afa5-91fd425b5449")]
		[AssociationId("9e20dc87-aa15-4854-b0d4-1ecefb22621e")]
		[RoleId("e5a11de2-fa90-4fe0-87f3-5f4ea0629f8d")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("InvoiceNumbers")]
		public RelationType InvoiceNumber;

		#region Allors
		[Id("98ec45be-fea4-4df7-91fb-6643edf74784")]
		[AssociationId("102ff721-0f77-4e05-9252-974b0e2a1505")]
		[RoleId("e422bd7e-2998-4022-88cf-8de0a84621c4")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("DunningSteps")]
		public RelationType DunningStep;

		#region Allors
		[Id("a40ae239-df13-47e1-8fa2-cfcb4946b966")]
		[AssociationId("3fca92c6-8996-4f5b-867a-7ad209f8e44e")]
		[RoleId("8d211ac4-115f-47e4-be98-784ca9f7409a")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("SubAccountNumbers")]
		public RelationType SubAccountNumber;

		#region Allors
		[Id("a6693763-e246-4ae8-bd37-74313d32883b")]
		[AssociationId("e74e0621-56ec-4bf9-920d-919b8cebd2f9")]
		[RoleId("9761e619-7ce8-466c-b154-743590b1bc46")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("TransactionNumbers")]
		public RelationType TransactionNumber;

		#region Allors
		[Id("acedb9ed-b0de-464f-86ec-621022938ad7")]
		[AssociationId("7a8823ee-ce46-4f99-8b15-217476244273")]
		[RoleId("64f6065c-b8b1-4b91-9128-b1a17c109cc5")]
		#endregion
		[Indexed]
		[Type(typeof(DebitCreditConstantClass))]
		[Plural("Sides")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Side;

		#region Allors
		[Id("b0570264-3211-4444-a69f-1cdb2eb6e783")]
		[AssociationId("8586c543-e997-48e1-8302-a5ef7e1ad6ab")]
		[RoleId("c7da91ea-2071-45b8-a4c7-957d7bd9579b")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("d227669f-8052-4962-8ccf-a775355691f1")]
		[AssociationId("a0b14293-269d-46d3-b297-a149a5090b1d")]
		[RoleId("a6620318-c985-4e60-bdea-204993e65217")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("BlockedForPayments")]
		public RelationType BlockedForPayment;

		#region Allors
		[Id("def3c00e-f065-48e5-97a2-22497f1800b3")]
		[AssociationId("278bb3d9-da67-44ca-a78b-8c047da3b2d4")]
		[RoleId("3b867d6f-f7e7-4223-99cf-052ac43e139b")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("DatesLastReminder")]
		public RelationType DateLastReminder;

		#region Allors
		[Id("e9b2fc3f-c6ed-4e67-a634-9ee78f824ad8")]
		[AssociationId("7d7e867b-e22e-4ca9-8751-e8af845fc206")]
		[RoleId("8fd5d417-fc56-4b5c-9a9c-a156dff25006")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("YourReferences")]
		public RelationType YourReference;

		#region Allors
		[Id("edad9c25-ef5e-4326-aba6-535deb6a8a7e")]
		[AssociationId("deeaff94-26ec-4dbf-815b-c3105024459e")]
		[RoleId("8fb086f5-2dba-46fb-b86e-2b81e17fa996")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("OurReferences")]
		public RelationType OurReference;

		#region Allors
		[Id("f027183b-d8a1-4909-bedc-5d16a62d6bc2")]
		[AssociationId("bc7040ea-f230-42e5-ab35-458a0d3fc52e")]
		[RoleId("3760cfff-b831-4767-9346-9d37ce76172b")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("ReconciliationNumbers")]
		public RelationType ReconciliationNumber;

		#region Allors
		[Id("f18c665b-4f88-4e97-950c-08a38d9f0d93")]
		[AssociationId("8707951a-3285-44a9-b3d8-7c51cc9977ec")]
		[RoleId("9caf74b7-48c5-4d1b-81fb-15bc22518156")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("DueDates")]
		public RelationType DueDate;



		public static AmountDueClass Instance {get; internal set;}

		internal AmountDueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}