namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ac18c87b-683c-4529-9171-d23e73c583d4")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("WorkEffortAssignmentRates")]
	public partial class WorkEffortAssignmentRateClass : Class
	{
		#region Allors
		[Id("3a4e3614-cb04-4014-826b-78a2b87e6a1f")]
		[AssociationId("0548e8ad-5ba7-462a-b3ef-6be2014bad65")]
		[RoleId("26ee13eb-79f5-46a1-8749-485f43a3ee8c")]
		#endregion
		[Indexed]
		[Type(typeof(RateTypeClass))]
		[Plural("RateTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType RateType;

		#region Allors
		[Id("627da684-d501-4221-97c2-81329e2b5e8c")]
		[AssociationId("4b9c1fd3-acf0-4e5b-8cb5-d32f94bff10b")]
		[RoleId("e6409680-f8e1-4c61-8bd3-b9ec42435741")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortPartyAssignmentClass))]
		[Plural("WorkEffortPartyAssignments")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType WorkEffortPartyAssignment;



		public static WorkEffortAssignmentRateClass Instance {get; internal set;}

		internal WorkEffortAssignmentRateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}