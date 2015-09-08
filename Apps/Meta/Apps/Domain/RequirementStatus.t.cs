namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a79b3e89-e878-45a5-9c9f-7911d259fc33")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("RequirementStatuses")]
	public partial class RequirementStatusClass : Class
	{
		#region Allors
		[Id("03542f1b-23ac-4bfc-add5-bad028295b4e")]
		[AssociationId("9000b234-a6cf-4707-8a33-b90f6ee7b869")]
		[RoleId("7baebeac-8013-4acc-bef7-508eed0eb1c3")]
		#endregion
		[Indexed]
		[Type(typeof(RequirementObjectStateClass))]
		[Plural("RequirementObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType RequirementObjectState;

		#region Allors
		[Id("49e2a03a-aeba-4ae2-9f75-47639334bde6")]
		[AssociationId("a3ac3a93-90ed-4bfb-8a8c-e1fbd8fe743e")]
		[RoleId("265a7954-c3be-420b-98b5-ff70076cefdf")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;



		public static RequirementStatusClass Instance {get; internal set;}

		internal RequirementStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}