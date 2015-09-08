namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6c8503ec-3796-4861-af47-b1aa4e911292")]
	#endregion
	[Inherit(typeof(DeletableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("ProductCategoryRevenues")]
	public partial class ProductCategoryRevenueClass : Class
	{
		#region Allors
		[Id("3e748cda-d69d-43f9-be75-c942bd432bc7")]
		[AssociationId("164d084d-339d-4d3c-8b64-5a985b7b12f1")]
		[RoleId("89301283-a980-4a46-b113-c3b45f6ef3a3")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("ProductCategoryNames")]
		public RelationType ProductCategoryName;

		#region Allors
		[Id("458c4900-00d6-4ad8-a8bc-45a61364ca3d")]
		[AssociationId("5d066c0f-4cab-47ce-aaf5-8b3557ba11f2")]
		[RoleId("032207f1-426c-4022-92ac-4042f18cce0c")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Months")]
		public RelationType Month;

		#region Allors
		[Id("558ed9e0-81a0-4e6c-abd3-4e27e665deee")]
		[AssociationId("e1e94fa4-ac8f-44c2-9982-a05ad1eb3f8e")]
		[RoleId("125f6b43-2204-43cb-819b-4b2b940630cc")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("6127124a-c07d-49b7-8ecd-fb42d50c4c69")]
		[AssociationId("6e095e71-bcdf-4b94-8880-b6a888eec2bf")]
		[RoleId("51cfff06-36f5-4b1f-9c01-2ea2bfb015f1")]
		#endregion
		[Indexed]
		[Type(typeof(ProductCategoryClass))]
		[Plural("ProductCategories")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductCategory;

		#region Allors
		[Id("a0fef77b-3d7d-4338-b095-1a69a8cbfda4")]
		[AssociationId("0a2754a2-5492-4bd7-acf9-65a424e2d870")]
		[RoleId("fad7d065-5545-4d85-b2f9-259708822626")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;

		#region Allors
		[Id("e97efb8e-a61e-4710-a576-75e540f2ec1f")]
		[AssociationId("b5a6f45b-ee80-4661-ba07-aafaf8676794")]
		[RoleId("a763b52d-022b-472d-89ca-982e573053c9")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("f2f39bba-8f9e-4dac-ba98-542eb19aebab")]
		[AssociationId("590fa663-383d-42d7-8a4c-37ba7e9c6030")]
		[RoleId("76e05eb3-fa65-4efd-8f9a-330b811e5dfe")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Years")]
		public RelationType Year;



		public static ProductCategoryRevenueClass Instance {get; internal set;}

		internal ProductCategoryRevenueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}