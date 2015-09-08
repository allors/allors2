namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("33e9355b-b3db-43e0-a250-8ebc576e6221")]
	#endregion
	[Inherit(typeof(PeriodInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(DeletableInterface))]

	[Plural("WorkEffortAssignments")]
	public partial class WorkEffortAssignmentClass : Class
	{
		#region Allors
		[Id("54bbdb5d-74b9-4ac7-b638-b1ef4a210b6e")]
		[AssociationId("91713efa-721d-43b7-99dd-ec7681456781")]
		[RoleId("dafa5322-4905-40fa-ae14-ae5ee80f0f1c")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Professionals")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Professional;

		#region Allors
		[Id("93cb0818-2599-4652-addd-4a1032d5dde9")]
		[AssociationId("2d5c955f-4bd5-43d2-a8f4-3df03ef6b78b")]
		[RoleId("c42be8db-6e5a-459c-afbc-39bcac3e1eb2")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortInterface))]
		[Plural("Assignments")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Assignment;



		public static WorkEffortAssignmentClass Instance {get; internal set;}

		internal WorkEffortAssignmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}