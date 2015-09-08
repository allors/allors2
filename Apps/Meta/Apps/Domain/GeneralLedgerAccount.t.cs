namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1a0e396b-69bd-4e77-a602-3d7f7938fd74")]
	#endregion
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("GeneralLedgerAccounts")]
	public partial class GeneralLedgerAccountClass : Class
	{
		#region Allors
		[Id("0144834d-c5a9-42e7-bf22-af46ff95ee5f")]
		[AssociationId("01f0ef35-7ecd-44a9-9366-bad1c213246c")]
		[RoleId("76129210-c674-43d8-9864-39ac497f7a48")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("DefaultCostUnits")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DefaultCostUnit;

		#region Allors
		[Id("01c49e6f-087a-494d-902d-12811442470e")]
		[AssociationId("839d77a1-7307-4aeb-921b-9aa8832ef853")]
		[RoleId("6a616950-edba-4ed1-8419-7ed4ab3a8fcd")]
		#endregion
		[Indexed]
		[Type(typeof(CostCenterClass))]
		[Plural("DefaultCostCenters")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DefaultCostCenter;

		#region Allors
		[Id("08bb53f7-9b27-4079-bb9b-d8ff96f89b42")]
		[AssociationId("032959df-bd6a-4043-9703-1ed9ce1ca0ee")]
		[RoleId("671e880a-e52a-45ed-8712-c0b6280de422")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("27ba2d5b-9e0b-4b20-9b34-f007a0f2e2f2")]
		[AssociationId("b05953d9-4d24-4fc6-8ee6-eeda14d519ca")]
		[RoleId("c6cb1d95-1734-4d37-ad1c-d4cb19546b03")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountTypeClass))]
		[Plural("GeneralLedgerAccountTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType GeneralLedgerAccountType;

		#region Allors
		[Id("2e6545f8-5fcf-4129-99f6-1f41280cd02d")]
		[AssociationId("559a7346-4ec8-449c-ae3a-2e9360933196")]
		[RoleId("62f62635-b19e-4985-98f7-55c677badb26")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("CashAccounts")]
		public RelationType CashAccount;

		#region Allors
		[Id("3fc28997-124c-4e16-9c4d-128314e6395c")]
		[AssociationId("f42c835a-4325-4d25-a2e4-ea621d9bab6b")]
		[RoleId("c8224cdd-85c3-4dd7-85cc-3eee6b0ee010")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("CostCenterAccounts")]
		public RelationType CostCenterAccount;

		#region Allors
		[Id("4877e61b-443f-4bef-820f-5c93f8d42b8a")]
		[AssociationId("6f44b333-9270-4215-842e-12520d4fc5f6")]
		[RoleId("e48f4a4a-bf66-49b2-b457-76b48e3cab21")]
		#endregion
		[Indexed]
		[Type(typeof(DebitCreditConstantClass))]
		[Plural("Sides")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Side;

		#region Allors
		[Id("5f797e0d-05aa-4dfb-a826-157ac6cdb0a9")]
		[AssociationId("b7e05caa-992f-4676-ab9d-f3ba8d678032")]
		[RoleId("674a2d4f-62b2-41ec-8de8-b1fad7d05686")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("BalanceSheetAccounts")]
		public RelationType BalanceSheetAccount;

		#region Allors
		[Id("7f2e28ea-124a-45fa-9ed3-e3c2b0bb1822")]
		[AssociationId("c5ad5e4e-9437-4838-8b23-88a72a3f51f0")]
		[RoleId("1a860600-659d-410e-91c7-21682ba9002c")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("ReconciliationsAccount")]
		public RelationType ReconciliationAccount;

		#region Allors
		[Id("8616e916-a3e2-4cfe-84a4-778fd4b50d87")]
		[AssociationId("ef7a784f-0f35-4973-b8d0-e732e3a9a741")]
		[RoleId("d758a087-f381-4a5c-ab9f-e433ba786166")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("9b679f99-d678-4ec0-8ab1-e02eaabe6658")]
		[AssociationId("bd544bfa-d073-4534-8c11-de070571f5cb")]
		[RoleId("8a1191c3-3ef7-4932-b6b0-d52f2eb604fb")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("CostCenterRequireds")]
		public RelationType CostCenterRequired;

		#region Allors
		[Id("a3aa445f-2aae-41be-8024-7b4a7e0a76ed")]
		[AssociationId("eae74634-5d98-482f-bfc5-c81e9731c2e0")]
		[RoleId("5d60a6f0-eb91-4048-99fd-285c9685c555")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("CostUnitsRequired")]
		public RelationType CostUnitRequired;

		#region Allors
		[Id("aa569c0a-597d-4b75-a527-25c6ef339547")]
		[AssociationId("73a33f92-462b-46c6-83d1-41d6f170aaee")]
		[RoleId("b32aa8cb-b590-4302-8a30-66eacd2ec4f7")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountGroupClass))]
		[Plural("GeneralLedgerAccountGroups")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType GeneralLedgerAccountGroup;

		#region Allors
		[Id("beda5c75-e1a0-493a-85ec-a943214cec8d")]
		[AssociationId("319a12b6-4ee9-4bbd-9026-480b02e71255")]
		[RoleId("be03ce0d-4023-4d62-8f2e-918e46d1f097")]
		#endregion
		[Indexed]
		[Type(typeof(CostCenterClass))]
		[Plural("CostCentersAllowed")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType CostCenterAllowed;

		#region Allors
		[Id("bfe446ee-f9ff-462f-bb45-9bf52d61daa4")]
		[AssociationId("dca53c27-9157-4f61-a3e5-eac272b764cd")]
		[RoleId("e1b47d11-4212-4594-b405-3eff0ba3ef82")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("CostUnitsAccount")]
		public RelationType CostUnitAccount;

		#region Allors
		[Id("cedccf34-0386-4be3-aa77-6ec0a9032c15")]
		[AssociationId("06fdbd7c-2693-4f23-ab53-965bb40aa79c")]
		[RoleId("1ed39598-1583-401d-8a8e-4a34bf342001")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("AccountNumbers")]
		public RelationType AccountNumber;

		#region Allors
		[Id("d2078f49-9745-48e5-bdd2-7d7738f25d4e")]
		[AssociationId("276120e0-5e26-415c-bdc0-b1da8790d7f5")]
		[RoleId("42f7e330-3be1-4eb7-8649-289c3907fb8f")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("CostUnitsAllowed")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType CostUnitAllowed;

		#region Allors
		[Id("e433abed-8f41-4a23-8e5b-e597bb6a14d2")]
		[AssociationId("9d56bae8-0cf9-4a5f-81db-a9fcc6aea183")]
		[RoleId("da4c5d7b-2f0b-4c18-8f6a-850064c81b9e")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("Protecteds")]
		public RelationType Protected;



		public static GeneralLedgerAccountClass Instance {get; internal set;}

		internal GeneralLedgerAccountClass() : base(MetaPopulation.Instance)
        {
        }
	}
}