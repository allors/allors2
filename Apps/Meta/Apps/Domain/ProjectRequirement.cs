namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("51d0b6f6-221b-44d5-9a0b-9a880620b1ad")]
	#endregion
	[Inherit(typeof(RequirementInterface))]

	[Plural("ProjectRequirements")]
	public partial class ProjectRequirementClass : Class
	{
		#region Allors
		[Id("75d89129-9aa9-491c-894b-feb86b33bf52")]
		[AssociationId("e83f19ff-1441-4d6e-912f-ca56301e3621")]
		[RoleId("80b74f53-e962-4988-a1c1-2860a08ca6b3")]
		#endregion
		[Indexed]
		[Type(typeof(DeliverableClass))]
		[Plural("NeededDeliverables")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType NeededDeliverable;



		public static ProjectRequirementClass Instance {get; internal set;}

		internal ProjectRequirementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}