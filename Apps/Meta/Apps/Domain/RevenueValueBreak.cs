namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("32f8ea23-5ef9-4d2c-86d9-b6f67529c05d")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("RevenueValueBreaks")]
	public partial class RevenueValueBreakClass : Class
	{
		#region Allors
		[Id("03baba9a-c9ef-49d0-8fc8-fbdc4bfec949")]
		[AssociationId("c2746ebd-cdd4-4e22-a9fb-d8c4fbcc86da")]
		[RoleId("cec9b76a-7ab9-4c47-a8a8-635ccd374fb0")]
		#endregion
		[Indexed]
		[Type(typeof(ProductCategoryClass))]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductCategory;

		#region Allors
		[Id("96391ee1-5ba2-48db-95c9-cec6e73758b7")]
		[AssociationId("846a94f9-72cd-48a7-be94-9e8f146e245a")]
		[RoleId("8ea60dde-6149-4389-bb94-b94e7bcc81b2")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("ThroughAmounts")]
		public RelationType ThroughAmount;

		#region Allors
		[Id("cf544df2-3ccb-42b5-b009-c355fcf88ed6")]
		[AssociationId("dbdb3b16-701c-4f45-9d38-6b3e21f66ab3")]
		[RoleId("44217cbb-1f44-4c04-bb66-e8bf597df3f6")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("FromAmounts")]
		public RelationType FromAmount;



		public static RevenueValueBreakClass Instance {get; internal set;}

		internal RevenueValueBreakClass() : base(MetaPopulation.Instance)
        {
        }
	}
}