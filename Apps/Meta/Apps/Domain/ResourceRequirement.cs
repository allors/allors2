namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("b3753d18-0b7e-4177-92c3-81ae8ce35a8f")]
	#endregion
	[Inherit(typeof(RequirementInterface))]

	[Plural("ResourceRequirements")]
	public partial class ResourceRequirementClass : Class
	{
		#region Allors
		[Id("0655305f-5658-45de-b901-a908a4887a0f")]
		[AssociationId("10db5470-e9cb-464a-bcd4-65dd4434b6fe")]
		[RoleId("b1a4dec7-156a-4c16-94b7-966dba1faef1")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("DutiesPlural")]
		public RelationType Duties;

		#region Allors
		[Id("46f13bbb-430e-47e3-a8e2-edf3ae190417")]
		[AssociationId("48167ab3-671a-4a2c-b5c9-e8964f35448b")]
		[RoleId("841659d8-deb9-4902-855b-22b9c9822331")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("NumbersOfPositions")]
		public RelationType NumberOfPositions;

		#region Allors
		[Id("a0a42e5c-3106-4709-aa7b-c916a0ba8508")]
		[AssociationId("dc0b4ab7-19d1-4d15-938a-07ce77ba3b23")]
		[RoleId("eeec43e2-dcb4-4ae1-aceb-afbb9da25f68")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("RequiredStartDates")]
		public RelationType RequiredStartDate;

		#region Allors
		[Id("d1c048c9-eb05-4cd2-a06c-a8dacf993ab2")]
		[AssociationId("88727cd2-46fd-44c0-a5bf-08698256592d")]
		[RoleId("602edcd9-9446-4bbd-9133-1fbb45f423db")]
		#endregion
		[Indexed]
		[Type(typeof(NeededSkillClass))]
		[Plural("NeededSkills")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType NeededSkill;

		#region Allors
		[Id("ffd07bff-38a2-4284-958d-18b1296f6112")]
		[AssociationId("ff6383a4-8502-4f12-8337-6a6ead2f3f0f")]
		[RoleId("1aea6980-6914-4587-98ad-93d9164ebd63")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("RequiredEndDates")]
		public RelationType RequiredEndDate;



		public static ResourceRequirementClass Instance {get; internal set;}

		internal ResourceRequirementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}