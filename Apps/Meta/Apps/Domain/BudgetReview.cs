namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d12719f0-2c0e-4a9d-869b-4a209fc35a56")]
	#endregion
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("BudgetReviews")]
	public partial class BudgetReviewClass : Class
	{
		#region Allors
		[Id("4396be4d-edb4-405d-a39a-ee6ff5c39ca5")]
		[AssociationId("9cbcaf98-22d1-41ed-b7d4-88a32e41de5f")]
		[RoleId("61c422a4-cfb0-4e7a-b8ee-29ecf92589ee")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ReviewDates")]
		public RelationType ReviewDate;

		#region Allors
		[Id("6d065017-6c6f-413c-bc79-1a6349180c34")]
		[AssociationId("b0f12ce4-58e3-4757-996f-3e3aca8aafbb")]
		[RoleId("eff0da0c-1ea3-40d8-8894-141d43f20a5f")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static BudgetReviewClass Instance {get; internal set;}

		internal BudgetReviewClass() : base(MetaPopulation.Instance)
        {
        }
        internal override void AppsExtend()
        {
            this.ReviewDate.RoleType.IsRequired = true;
            this.Description.RoleType.IsRequired = true;
        }

    }
}