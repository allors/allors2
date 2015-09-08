namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ce394ad6-1229-4621-8506-5f0347cd8c92")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("RevenueQuantityBreaks")]
	public partial class RevenueQuantityBreakClass : Class
	{
		#region Allors
		[Id("047e27db-873d-404e-a07f-0e32f2e19dbe")]
		[AssociationId("fe0b216f-4002-4663-966d-bc9baeceea80")]
		[RoleId("39043e7b-42e7-471a-8033-c9a928a7939d")]
		#endregion
		[Indexed]
		[Type(typeof(ProductCategoryClass))]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductCategory;

		#region Allors
		[Id("7bca5fd8-016d-43d8-a38e-a811f3bd77ab")]
		[AssociationId("f312afd5-a46c-4fb6-88a4-e945b30098da")]
		[RoleId("087a6ce6-9a53-4ab1-8d9a-b8d00107c533")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Throughs")]
		public RelationType Through;

		#region Allors
		[Id("bae5ce9b-1bc0-46e4-89d9-26e8de81f54a")]
		[AssociationId("c1b72b95-f55f-4aaa-86a8-aba006489ec5")]
		[RoleId("03cc5c67-caf6-4d1f-8a46-edd1c0f76fa9")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Froms")]
		public RelationType From;



		public static RevenueQuantityBreakClass Instance {get; internal set;}

		internal RevenueQuantityBreakClass() : base(MetaPopulation.Instance)
        {
        }
	}
}