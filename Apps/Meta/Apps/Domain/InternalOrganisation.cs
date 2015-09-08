namespace Allors.Meta
{
	#region Allors
	[Id("c81441c8-9ac9-440e-a926-c96230b2701f")]
	#endregion
	[Inherit(typeof(PartyInterface))]
	public partial class InternalOrganisationClass : Class
	{
		#region Allors
		[Id("00bf781c-c874-44fe-ae60-d6609075b1c0")]
		[AssociationId("3b99af1e-e6c3-498b-aabd-78d6e82c8819")]
		[RoleId("b9a508e2-2931-4ddc-ab34-947d19c2d742")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("PurchaseOrderNumberPrefixes")]
		public RelationType PurchaseOrderNumberPrefix;

		#region Allors
		[Id("01d4f5d8-da57-4524-b35f-69a1a4adfa1c")]
		[AssociationId("84ff4f9a-b1d3-4e2c-aff6-52a9f75e874a")]
		[RoleId("ee1b0251-ba57-490a-bdc9-c4e8fd6142ce")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("TransactionReferenceNumbers")]
		public RelationType TransactionReferenceNumber;

		#region Allors
		[Id("0994b73e-8d4c-4fa4-aca2-287449b22ca7")]
		[AssociationId("17a9138e-76c8-42e1-85b8-7af73b551a22")]
		[RoleId("09fbb64d-c32e-4734-8df9-6e741a5070a5")]
		#endregion
		[Indexed]
		[Type(typeof(JournalEntryNumberClass))]
		[Plural("JournalEntryNumbers")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType JournalEntryNumber;

		#region Allors
		[Id("1a2533cb-9b75-4597-83ab-9bbfc49e0103")]
		[AssociationId("b4cb12ba-0ea1-41e5-945a-030503bf2c7b")]
		[RoleId("031ba0aa-32e4-470c-9a79-fae65cace2f2")]
		#endregion
		[Indexed]
		[Type(typeof(CountryClass))]
		[Plural("EuListingState")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType EuListingState;

		#region Allors
		[Id("1a986cbf-b7db-4850-af06-d96e1339beb7")]
		[AssociationId("fc69f819-2ca0-4ac4-b36e-cfff791679a1")]
		[RoleId("d6e816ae-50c5-41c7-8877-9f4c30208d47")]
		#endregion
		[Indexed]
		[Type(typeof(CounterClass))]
		[Plural("PurchaseInvoiceCounters")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchaseInvoiceCounter;

		#region Allors
		[Id("219a1d97-9615-47c5-bc4d-20a7d37313bd")]
		[AssociationId("4fd6a9e2-174c-41db-b519-44c317de0f96")]
		[RoleId("45f4d069-5faf-45cf-b097-21f58fab4097")]
		#endregion
		[Indexed]
		[Type(typeof(AccountingPeriodClass))]
		[Plural("ActualAccountingPeriods")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType ActualAccountingPeriod;

		#region Allors
		[Id("23aee857-9cea-481c-a4a3-72dd8b808d71")]
		[AssociationId("61c64e66-4647-439d-9efa-28500319e8ca")]
		[RoleId("4d68b2fe-d5b4-45f8-9e68-377d75f3401d")]
		#endregion
		[Indexed]
		[Type(typeof(InvoiceSequenceClass))]
		[Plural("InvoiceSequences")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InvoiceSequence;

		#region Allors
		[Id("293758d7-cc0a-4f1c-b122-84f609a828c2")]
		[AssociationId("afe9d80b-2984-4e46-b45c-e5a25af3bccd")]
		[RoleId("97aa32a6-69c0-4cc5-9e42-98907ff6c45f")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PaymentMethodInterface))]
		[Plural("ActivePaymentMethods")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ActivePaymentMethod;

		#region Allors
		[Id("33a47048-277a-40e8-bfe0-c8090eb179b2")]
		[AssociationId("81bc7c99-b328-4b81-9391-fe7659146924")]
		[RoleId("403008e6-8c8a-437e-8b09-02573198e319")]
		#endregion
		[Indexed]
		[Type(typeof(StringTemplateClass))]
		[Plural("PurchaseShipmentTemplates")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType PurchaseShipmentTemplate;

		#region Allors
		[Id("37b4bf2c-5b09-42b0-84d9-59b57793cf37")]
		[AssociationId("22aff7e0-1b45-4f06-b281-19cbf0d1c511")]
		[RoleId("dcd31b64-e449-4833-91cb-8237bdb71b78")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("MaximumAllowedPaymentDifferences")]
		public RelationType MaximumAllowedPaymentDifference;

		#region Allors
		[Id("39a09487-dcf4-4bc8-8494-859d7a8cc3dd")]
		[AssociationId("da98358b-fb19-4b40-ad32-ffc0b48583fe")]
		[RoleId("9b20db3b-ac0e-4374-8262-3ee22f8067ee")]
		#endregion
		[Indexed]
		[Type(typeof(MediaClass))]
		[Plural("LogoImages")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType LogoImage;

		#region Allors
		[Id("3b32c442-9cbc-41d8-8eb2-2ae41beca2c4")]
		[AssociationId("eda352cd-00cc-4d04-99cf-f7ad667cb20a")]
		[RoleId("b8211575-f4c9-44c8-89b2-bd122704f098")]
		#endregion
		[Indexed]
		[Type(typeof(CostCenterSplitMethodClass))]
		[Plural("CostCenterSplitMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CostCenterSplitMethod;

		#region Allors
		[Id("44f165ed-a6ca-4979-9046-a0f7391bef7d")]
		[AssociationId("5e581649-0b45-498f-b274-fdcc05bb3894")]
		[RoleId("1069343b-d6d5-4418-abf9-688b9dcec6d0")]
		#endregion
		[Indexed]
		[Type(typeof(CounterClass))]
		[Plural("PurchaseOrderCounters")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchaseOrderCounter;

		#region Allors
		[Id("496f6d33-2259-442e-924f-636d73cec52f")]
		[AssociationId("4ac05596-41d8-402a-8308-d9f458d604e0")]
		[RoleId("0739dac1-6507-4306-95c9-78f10532a78e")]
		#endregion
		[Indexed]
		[Type(typeof(LegalFormClass))]
		[Plural("LegalForms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType LegalForm;

		#region Allors
		[Id("49b087e2-9f55-463e-8e77-2500149ad771")]
		[AssociationId("1da60a2b-5359-4688-b925-edee515a2427")]
		[RoleId("a0e7966c-1096-4491-a712-1dc38b58b67c")]
		#endregion
		[Indexed]
		[Type(typeof(AccountingPeriodClass))]
		[Plural("AccountingPeriods")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType AccountingPeriod;

		#region Allors
		[Id("4fc741ef-fe95-49a8-8bcd-8ff43092db88")]
		[AssociationId("0e58cd98-94e6-42ab-8501-394f8b8d3624")]
		[RoleId("d5096d5d-a443-4667-95dd-7184f348e55c")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountClass))]
		[Plural("SalesPaymentDifferencesAccounts")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType SalesPaymentDifferencesAccount;

		#region Allors
		[Id("538fc59e-42da-471a-96a4-d8a93b2de229")]
		[AssociationId("d0f6dc1f-a056-4a8c-b138-d68a5cf10247")]
		[RoleId("948f1ebc-a637-4c1b-ae35-dcc7462c95d0")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("5b64038f-5ad9-46a6-9af6-b95819ac9830")]
		[AssociationId("753f6fa9-ff10-402c-9812-d2c738d35dbb")]
		[RoleId("8411c910-14d4-4629-aa39-c58602a799d4")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("PurchaseTransactionReferenceNumbers")]
		public RelationType PurchaseTransactionReferenceNumber;

		#region Allors
		[Id("5b64cf9d-e990-491e-b009-3481d73db67e")]
		[AssociationId("40296302-a559-4014-ba68-929d4238f4d8")]
		[RoleId("5b966f3d-cc80-44f4-b255-87182aa796d4")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("FiscalYearStartMonths")]
		public RelationType FiscalYearStartMonth;

		#region Allors
		[Id("5ca9eda8-278b-4466-99f3-8c61d0383ef4")]
		[AssociationId("5dd8f968-1ebf-4b94-be2d-11005d24aeb5")]
		[RoleId("9e72477e-4729-4371-8f6b-80ff618705f7")]
		#endregion
		[Indexed]
		[Type(typeof(StringTemplateClass))]
		[Plural("PurchaseOrderTemplates")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType PurchaseOrderTemplate;

		#region Allors
		[Id("63c9ceb1-d583-41e1-a9a9-0c2576e9adfc")]
		[AssociationId("fd1ac2ff-6869-44bd-9b4e-05a3b14fbad9")]
		[RoleId("41058049-5c44-47b7-bbb9-34ab0bdcfbcb")]
		#endregion
		[Indexed]
		[Type(typeof(CostOfGoodsSoldMethodClass))]
		[Plural("CostOfGoodsSoldMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CostOfGoodsSoldMethod;

		#region Allors
		[Id("76acc9c6-0aa1-4b30-8cca-4629fdd56b91")]
		[AssociationId("b9f585f4-a612-451a-ac0e-4a2584982385")]
		[RoleId("e9e6013f-9458-4d27-89a2-bced57a2b15f")]
		#endregion
		[Indexed]
		[Type(typeof(RoleClass))]
		[Plural("EmployeeRoles")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType EmployeeRole;

		#region Allors
		[Id("77ae5145-791a-4ef0-94cc-6c9683b02f13")]
		[AssociationId("0da6cc82-b538-4346-bb48-19b02223a566")]
		[RoleId("bef210d2-7af6-4653-872b-a5eebba2af87")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("VatsDeactivated")]
		public RelationType VatDeactivated;

		#region Allors
		[Id("7e210c5e-a68b-4ea0-b019-1dd452d8e407")]
		[AssociationId("b9c41192-1666-44c0-9365-b24df29a2cdf")]
		[RoleId("dfe3042c-babb-416b-a414-5dda0a2958c0")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("FiscalYearStartDays")]
		public RelationType FiscalYearStartDay;

		#region Allors
		[Id("848f3098-ce8b-400c-9775-85c00ac68f28")]
		[AssociationId("44649646-ccf8-48b7-9ebf-a09df75d23fc")]
		[RoleId("3d5a74b3-cb62-4803-8115-fde43a648af5")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(GeneralLedgerAccountClass))]
		[Plural("GeneralLedgerAccounts")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType GeneralLedgerAccount;

		#region Allors
		[Id("859d95c2-7321-4408-bcd1-405dc0b31efc")]
		[AssociationId("47674006-74f4-497e-b9b6-1d5cff1ada0f")]
		[RoleId("0dc922be-86d0-469e-a9d2-5bf19a910994")]
		#endregion
		[Indexed]
		[Type(typeof(CounterClass))]
		[Plural("AccountingTransactionCounters")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType AccountingTransactionCounter;

		#region Allors
		[Id("86dd32e2-74e7-4ced-bfbd-4e1fdc723588")]
		[AssociationId("ab8ace70-5d23-4ba0-83f7-471a8dbefeb6")]
		[RoleId("fa1a381c-a895-44d2-a028-860486b1d1af")]
		#endregion
		[Indexed]
		[Type(typeof(CounterClass))]
		[Plural("IncomingShipmentCounters")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType IncomingShipmentCounter;

		#region Allors
		[Id("89f4907d-4a10-428d-9e6b-ef9fb045c019")]
		[AssociationId("a0fd3167-5d4e-400f-b593-1497fce5d024")]
		[RoleId("0ab03957-8140-4e1c-8a13-a9647f6c9e47")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountClass))]
		[Plural("RetainedEarningsAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType RetainedEarningsAccount;

		#region Allors
		[Id("9306706d-c35c-4e7d-af31-60ff13c348bd")]
		[AssociationId("7299e411-7fd5-4500-8e25-a44f0339806a")]
		[RoleId("1975353f-30b6-4c4a-ab41-94886c8f7f5a")]
		#endregion
		[Indexed]
		[Type(typeof(StringTemplateClass))]
		[Plural("PackagingSlipTemplates")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType PackagingSlipTemplate;

		#region Allors
		[Id("9a2ab89e-c3bc-4b6b-a82d-417dc21c8f9e")]
		[AssociationId("fcc6e653-3787-44a0-8a3a-35e80e232a02")]
		[RoleId("31549cf9-6418-4d19-96b0-5813cc964491")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Customers")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType Customer;

		#region Allors
		[Id("9d6aaa81-9f97-427e-9f46-1f1e93748248")]
		[AssociationId("b251ce10-b4ac-45da-bc57-0a42e75f3660")]
		[RoleId("6ffd36cd-37de-444b-93b6-4128de34254f")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("PurchaseInvoiceNumberPrefixes")]
		public RelationType PurchaseInvoiceNumberPrefix;

		#region Allors
		[Id("a8d09b7d-5f55-4adb-897b-73b2c107932f")]
		[AssociationId("82e7ff09-f69c-4f22-97ad-80cae5730f19")]
		[RoleId("9309b4cb-0552-4f9a-bf50-77bb686d8b8e")]
		#endregion
		[Indexed]
		[Type(typeof(StringTemplateClass))]
		[Plural("PickListTemplates")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType PickListTemplate;

		#region Allors
		[Id("ab2004c1-fd91-4298-87cd-532a6fe5efb0")]
		[AssociationId("b53b80a4-92f5-4e08-a9fb-86bf2bd9572e")]
		[RoleId("7fe218f4-975d-40f4-82b6-b43cb308aff4")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountClass))]
		[Plural("SalesPaymentDiscountDifferencesAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesPaymentDiscountDifferencesAccount;

		#region Allors
		[Id("ad7c8532-59d2-4668-bd9f-6c67ddc4e4bc")]
		[AssociationId("7028e722-f29d-42f6-b7c5-d73c2e99c907")]
		[RoleId("48454368-06c6-4e64-876b-b5e4268407a4")]
		#endregion
		[Indexed]
		[Type(typeof(CounterClass))]
		[Plural("SubAccountCounters")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SubAccountCounter;

		#region Allors
		[Id("afbaffe6-b03c-463e-b074-08b32641b482")]
		[AssociationId("4a02dff3-4bd3-4453-b2fa-e6e79f1b18b0")]
		[RoleId("334cb83a-410e-4abb-8b51-9dd19e2fc21b")]
		#endregion
		[Indexed]
		[Type(typeof(AccountingTransactionNumberClass))]
		[Plural("AccountingTransactionNumbers")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType AccountingTransactionNumber;

		#region Allors
		[Id("b79d1af6-f14a-4466-aba2-893bf554dbc1")]
		[AssociationId("e24f08c4-a880-42f7-907d-d844d48fa152")]
		[RoleId("78650a41-e592-4889-8c62-2f16a2e7fa01")]
		#endregion
		[Indexed]
		[Type(typeof(StringTemplateClass))]
		[Plural("QuoteTemplates")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType QuoteTemplate;

		#region Allors
		[Id("b8af8dce-d0e8-4e16-8d72-e56b920a04b4")]
		[AssociationId("57168fae-0b31-4370-afff-ab5a02c9a8ee")]
		[RoleId("d4c44bcf-c38b-46b9-86f6-462a48714389")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("TransactionReferenceNumberPrefixes")]
		public RelationType TransactionReferenceNumberPrefix;

		#region Allors
		[Id("ba00c0d2-6067-4584-bdc4-e6c72be77232")]
		[AssociationId("687afb94-445e-41e2-bf22-deaac5b40e5a")]
		[RoleId("9fa5512e-098a-40fd-a821-38a5f5f280f5")]
		#endregion
		[Indexed]
		[Type(typeof(CounterClass))]
		[Plural("QuoteCounters")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType QuoteCounter;

		#region Allors
		[Id("bce0d7d9-cfc9-4092-99a1-93ff5c0b94dd")]
		[AssociationId("5c4cfec8-5bc0-48d2-ac92-132c0538e614")]
		[RoleId("bece103b-dde6-4319-938b-e08d23d9f99e")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("PreviousCurrencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PreviousCurrency;

		#region Allors
		[Id("cd40057a-5211-4289-a4ef-c30aa4049957")]
		[AssociationId("5dd54980-fa14-434c-80fc-64dec203fd8b")]
		[RoleId("ca87441c-6ce7-4041-bdd4-ca83f3b19289")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Employees")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType Employee;

		#region Allors
		[Id("d0ebaa65-260a-4511-a137-89f25016f12c")]
		[AssociationId("70e0b1a9-4c82-4c47-a6b8-df7a62423a08")]
		[RoleId("1413c3f9-a226-4356-8bcc-6f8ae1963a6e")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountClass))]
		[Plural("PurchasePaymentDifferencesAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchasePaymentDifferencesAccount;

		#region Allors
		[Id("d2ad57d5-de30-4bc0-90a7-9aea7a9da8c7")]
		[AssociationId("dbd28e59-d5d6-4d95-aef1-6881e0fe2d48")]
		[RoleId("2e28606f-5708-4c0f-bdfa-04019a0e97d9")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountClass))]
		[Plural("SuspenceAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SuspenceAccount;

		#region Allors
		[Id("d48ef8bb-064b-4360-8162-a138fb601761")]
		[AssociationId("91252cf4-eca3-4b9d-b06d-5df795c3709c")]
		[RoleId("3ef1f904-5b02-4f00-8486-2e452668be67")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountClass))]
		[Plural("NetIncomeAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType NetIncomeAccount;

		#region Allors
		[Id("d5645df8-2b10-435d-8e47-57b5d268541a")]
		[AssociationId("4d145acb-007a-46b9-98a8-a86888221e28")]
		[RoleId("c2fe67e4-a100-4d96-b4ff-df1ec73db5fe")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("DosAccounting")]
		public RelationType DoAccounting;

		#region Allors
		[Id("dcf24d2f-7bf2-43fd-82b4-bd30fd545022")]
		[AssociationId("b4b8b2e6-141c-416f-89a8-746c72c26e5c")]
		[RoleId("dc7127c4-c4ca-49a3-95ae-970de554d4f3")]
		#endregion
		[Indexed]
		[Type(typeof(FacilityInterface))]
		[Plural("DefaultFacilities")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DefaultFacility;

		#region Allors
		[Id("dd008dfe-a219-42ab-bc08-d091da3f8ea4")]
		[AssociationId("77ce8418-a00b-46d0-ab7b-4a782b7387da")]
		[RoleId("a5ac9bc1-5323-41ec-a791-d4ecc7d0eee8")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountClass))]
		[Plural("PurchasePaymentDiscountDifferencesAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchasePaymentDiscountDifferencesAccount;

		#region Allors
		[Id("e09976e8-dc99-4539-9b0b-0bbe98cc5404")]
		[AssociationId("0d828c12-82bd-4b37-96c8-68997a7c2f48")]
		[RoleId("3d362cd1-d49c-422f-9722-7276a6ee07c4")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Suppliers")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType Supplier;

		#region Allors
		[Id("e9af1ca5-d24f-4af2-8687-833744941b24")]
		[AssociationId("ca041d3d-7adf-43da-ac42-c749633bd9b8")]
		[RoleId("4b50a77e-4343-415b-ad45-6cd074b681b5")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("QuoteNumberPrefixes")]
		public RelationType QuoteNumberPrefix;

		#region Allors
		[Id("ec8e7400-0088-4237-af32-a687e1c45d77")]
		[AssociationId("3ee2e0b2-6835-490b-937a-a853c85dd3e4")]
		[RoleId("c70fd771-87a6-4e97-98ea-34c2c0265450")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("PurchaseTransactionReferenceNumberPrefixes")]
		public RelationType PurchaseTransactionReferenceNumberPrefix;

		#region Allors
		[Id("f14f1865-7820-4ed5-8ca9-dffcbeb6b1ec")]
		[AssociationId("4b8a7531-3996-4af2-a921-3b5653dc46ba")]
		[RoleId("0b574be0-2369-41df-9f64-71235c0b9e9a")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("TaxNumbers")]
		public RelationType TaxNumber;

		#region Allors
		[Id("f353e7ef-d24d-4a27-8ec9-e930ef936240")]
		[AssociationId("cf556dfd-4b43-48e4-8243-df2650d8ce97")]
		[RoleId("6b7bda76-5fe7-4526-8c79-b42324fd4090")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountClass))]
		[Plural("CalculationDifferencesAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CalculationDifferencesAccount;

		#region Allors
		[Id("f7ad4cfe-fc31-412c-8df7-2a514783e2ed")]
		[AssociationId("4145ddcd-726a-405f-b4cb-2e85b0bd60a2")]
		[RoleId("7940e9e6-e6f6-4eac-bfa4-e14f63315276")]
		#endregion
		[Indexed]
		[Type(typeof(PaymentMethodInterface))]
		[Plural("PaymentMethods")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PaymentMethod;

		#region Allors
		[Id("fe96e14b-9dbd-4497-935f-f605abd2ada7")]
		[AssociationId("68f9e5fd-7398-456b-bcb8-27b23dbce3f1")]
		[RoleId("3fc3517d-0d36-4401-a56f-ac3a83f1f892")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("IncomingShipmentNumberPrefixes")]
		public RelationType IncomingShipmentNumberPrefix;

		public static InternalOrganisationClass Instance {get; internal set;}

		internal InternalOrganisationClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.PurchaseInvoiceCounter.RoleType.IsRequired = true;
            this.QuoteCounter.RoleType.IsRequired = true;
            this.PurchaseOrderCounter.RoleType.IsRequired = true;
            this.IncomingShipmentCounter.RoleType.IsRequired = true;
            this.SubAccountCounter.RoleType.IsRequired = true;
            this.Name.RoleType.IsRequired = true;
            this.FiscalYearStartMonth.RoleType.IsRequired = true;
            this.FiscalYearStartDay.RoleType.IsRequired = true;
            this.DoAccounting.RoleType.IsRequired = true;
            this.InvoiceSequence.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[LocalisedInterface.Instance.Locale.RoleType].IsRequiredOverride = true;

            this.ConcreteRoleTypeByRoleType[PartyInterface.Instance.DefaultPaymentMethod.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[PartyInterface.Instance.PreferredCurrency.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[PartyInterface.Instance.BillingAddress.RoleType].IsRequiredOverride = true;
        }
    }
}