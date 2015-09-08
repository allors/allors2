namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("962e5149-546b-4b18-ab09-e4de59b709ff")]
	#endregion
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PerformanceReviewItems")]
	public partial class PerformanceReviewItemClass : Class
	{
		#region Allors
		[Id("6d7bb4b2-885d-4f7b-9d31-d517c3d03ac2")]
		[AssociationId("4c8cd6fe-acea-43ae-90e9-41ae1b84f269")]
		[RoleId("1a5977eb-1914-4b02-a0d3-feaad843465d")]
		#endregion
		[Indexed]
		[Type(typeof(RatingTypeClass))]
		[Plural("RatingTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType RatingType;

		#region Allors
		[Id("d62d7236-458f-4e30-8df4-27eb877d0931")]
		[AssociationId("7056f19c-c67e-4b54-a08c-c49155326a5e")]
		[RoleId("cac7ce59-1969-43b8-99a9-a90af638558d")]
		#endregion
		[Indexed]
		[Type(typeof(PerformanceReviewItemTypeClass))]
		[Plural("PerformanceReviewItemTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PerformanceReviewItemType;



		public static PerformanceReviewItemClass Instance {get; internal set;}

		internal PerformanceReviewItemClass() : base(MetaPopulation.Instance)
        {
        }
	}
}