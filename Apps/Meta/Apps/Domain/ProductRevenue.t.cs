namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a34ca9ef-63e5-48c0-8a62-c8f43ad2d9d9")]
	#endregion
	[Inherit(typeof(DeletableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("ProductRevenues")]
	public partial class ProductRevenueClass : Class
	{
		#region Allors
		[Id("1ae30294-1cb3-4d85-a587-4d347ea6eac3")]
		[AssociationId("dbb2dab9-e5a7-49a8-8479-a9190f536b72")]
		[RoleId("5cae7798-695c-4618-b5b0-088bf2333b56")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;

		#region Allors
		[Id("2e046fac-7803-49ba-a477-ee2cc889e3f1")]
		[AssociationId("da9d4c62-184a-4cc1-bd5c-4708a6daf71b")]
		[RoleId("799d36ff-3411-401a-bdd5-60ff0b7f210a")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("ProductNames")]
		public RelationType ProductName;

		#region Allors
		[Id("5f87a322-6943-4b20-b667-11bdf2f29244")]
		[AssociationId("e4cc94a8-3282-4a52-9e7e-1291105c3269")]
		[RoleId("4e6ed630-8d5d-4b1c-8934-4332d15426ad")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("8ee78120-6ec6-4470-835a-fdff01d3625e")]
		[AssociationId("7e4511b3-bd4e-4e4b-9f6c-c4254c47d258")]
		[RoleId("ca9e8bf5-56c0-4795-a79d-9189fcc7ebcc")]
		#endregion
		[Indexed]
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Years")]
		public RelationType Year;

		#region Allors
		[Id("c2570065-9bd8-403e-be43-884d31d4ccce")]
		[AssociationId("9d6ab03e-c52b-431b-aaee-f8116cf3a64c")]
		[RoleId("d84acfc6-0df5-4d50-89bf-b36e2a0e5e9c")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("Products")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Product;

		#region Allors
		[Id("ce487eb1-8a85-4999-a322-def96f9134d8")]
		[AssociationId("ebb2943e-4a43-4271-8348-486f74761826")]
		[RoleId("d6135f1f-464a-4e00-914c-7185b281d86d")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Months")]
		public RelationType Month;

		#region Allors
		[Id("e33476e4-a420-4fa5-a367-72ff75c4d5c4")]
		[AssociationId("e13c2b60-30b8-48d4-b0d7-443ac8e493e5")]
		[RoleId("15aa589a-9169-4644-8256-b8a644a6b5cf")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;



		public static ProductRevenueClass Instance {get; internal set;}

		internal ProductRevenueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}