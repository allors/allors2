namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("fd411b2a-0121-4f1f-b1db-86c187e8a089")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(DeletableInterface))]

	[Plural("SalesRepProductCategoryRevenues")]
	public partial class SalesRepProductCategoryRevenueClass : Class
	{
		#region Allors
		[Id("30a0a0de-0c74-494c-bf21-ebf13238dd61")]
		[AssociationId("e4e3c7ad-970d-4439-ba86-81be13a05dd8")]
		[RoleId("ab9e97c7-3c5c-4fdb-a36e-637875cb714d")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Months")]
		public RelationType Month;

		#region Allors
		[Id("59d7cb27-e752-405b-9515-7db04aa37da7")]
		[AssociationId("348f4a59-ef85-47b1-af43-05ed9982c594")]
		[RoleId("61a193c4-66ae-48ff-b810-437b9812e77f")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("SalesRepNames")]
		public RelationType SalesRepName;

		#region Allors
		[Id("6ee43a8c-8f42-491d-a0ab-3ea5d4352dc8")]
		[AssociationId("9a6c58ab-5119-47f6-974d-fa01fbb3d320")]
		[RoleId("4d85870c-75ec-4153-894a-30a6cb253060")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("7f39eefb-9210-4796-9b91-dc4d5e0b4ea1")]
		[AssociationId("f35dfa6d-eca3-49b9-8b83-cd152b9be673")]
		[RoleId("db1bd6a7-a47c-45a3-8e6d-767a4a8e06a5")]
		#endregion
		[Indexed]
		[Type(typeof(ProductCategoryClass))]
		[Plural("ProductCategories")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductCategory;

		#region Allors
		[Id("92e8bbd9-fe5a-4053-aef3-593dbb13eac0")]
		[AssociationId("6e9761f4-fc3d-485a-9b45-32f60f993f3b")]
		[RoleId("b57c833b-6083-4fc5-a6d5-1350caab9a22")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("a875a617-4d86-4d53-bcb5-bc4b40963cb4")]
		[AssociationId("32f41de8-6ea6-4a34-a36c-bfc0b3e5ca77")]
		[RoleId("4fc24ed9-01b7-4475-9309-fe3702031b63")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;

		#region Allors
		[Id("e291d604-4afc-4d73-b28e-04a723a4747b")]
		[AssociationId("9a6dcfe5-c728-4c33-a859-d0f5538790c3")]
		[RoleId("be3e8061-a5b1-482f-9d0b-5a1b4774cc80")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Years")]
		public RelationType Year;

		#region Allors
		[Id("f978075e-05b8-4204-aa2f-97ab416fd3e8")]
		[AssociationId("0695bc15-2d76-4464-b61f-7627cf885ad3")]
		[RoleId("52cfc399-8014-4b2e-94f0-bb67fec26e65")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("SalesReps")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesRep;



		public static SalesRepProductCategoryRevenueClass Instance {get; internal set;}

		internal SalesRepProductCategoryRevenueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}