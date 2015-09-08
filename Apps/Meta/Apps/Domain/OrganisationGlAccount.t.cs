namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("59f3100c-da48-4b4c-a302-1a75e37216a6")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(PeriodInterface))]

	[Plural("OrganisationGlAccounts")]
	public partial class OrganisationGlAccountClass : Class
	{
		#region Allors
		[Id("8e20ce74-a772-45c8-a76a-a8ca0d4d7ebd")]
		[AssociationId("948a2115-8780-46c6-83cf-dd4d27a1771b")]
		[RoleId("a3d68e22-e492-4d1e-8386-ea45ad67ee3a")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("Products")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Product;

		#region Allors
		[Id("9337f791-56aa-4086-b661-2043cf02c662")]
		[AssociationId("59fb9b8f-4d0a-4f97-b4d6-b3a5708de269")]
		[RoleId("f264e9da-aa7f-4d81-aa1b-741f020c2bef")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationGlAccountClass))]
		[Plural("Parents")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Parent;

		#region Allors
		[Id("9af20c76-200c-4aed-8154-99fd88907a15")]
		[AssociationId("7d9f9cad-0685-4b7d-b12d-770f046465f3")]
		[RoleId("817a7322-724b-4239-8261-a9c683f1ea4a")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Parties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Party;

		#region Allors
		[Id("a1608d47-9fa7-4dc4-9736-c59f28221842")]
		[AssociationId("61d6a380-171a-41c2-bda9-6cd8638ba442")]
		[RoleId("de2ad2c4-bf0e-4092-9611-b23c3e613429")]
		#endregion
		[Derived]
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("HasBankStatementsTransactions")]
		public RelationType HasBankStatementTransactions;

		#region Allors
		[Id("c0de2fbb-9e70-4094-8279-fb46734e920e")]
		[AssociationId("92c29de0-8454-4ae1-8bf9-ed4c5ec0d313")]
		[RoleId("8b65fb70-4905-4c1b-a1b1-51470ce58599")]
		#endregion
		[Indexed]
		[Type(typeof(ProductCategoryClass))]
		[Plural("ProductCategories")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductCategory;

		#region Allors
		[Id("d5332edf-3cc4-4f26-b0d1-da7ce1182dbc")]
		[AssociationId("1fb5ddfd-930a-4c81-8e9e-ac9d94840864")]
		[RoleId("30c0c848-5361-4bd3-9c0a-8b9aeaccdadc")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("f1d3e642-2844-4c5a-a053-4dcfce461902")]
		[AssociationId("b0706892-9a04-4e5a-8caa-bd015f3d81f9")]
		[RoleId("6cb69b76-2852-43eb-bff6-a10bc44503a3")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountClass))]
		[Plural("GeneralLedgerAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType GeneralLedgerAccount;



		public static OrganisationGlAccountClass Instance {get; internal set;}

		internal OrganisationGlAccountClass() : base(MetaPopulation.Instance)
        {
        }
	}
}