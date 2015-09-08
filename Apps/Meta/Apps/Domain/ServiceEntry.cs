namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4a4a0548-b75f-4a79-89aa-f5c242121f11")]
	#endregion
	[Plural("ServiceEntries")]
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class ServiceEntryInterface: Interface
	{
		#region Allors
		[Id("385eac5a-a588-4f30-b4df-a4b07be43d88")]
		[AssociationId("36477b8a-7c51-4fe6-bd6f-44e6205fb1bd")]
		[RoleId("ed9b3483-c2a2-4572-9346-35ed621500b9")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ThroughDateTimes")]
		public RelationType ThroughDateTime;

		#region Allors
		[Id("74fc8f9b-62f3-4921-bce1-ca10eed33204")]
		[AssociationId("987c6fb3-b512-4797-933d-28424500649e")]
		[RoleId("1bbf98fb-fb84-45e7-b3f3-c6d5bb9b155c")]
		#endregion
		[Indexed]
		[Type(typeof(EngagementItemInterface))]
		[Plural("EngagementItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType EngagementItem;

		#region Allors
		[Id("9b04b715-376f-4c39-b78b-f92af6b4ffc1")]
		[AssociationId("2c25dc8f-c253-471e-87fb-fe6934cf2b15")]
		[RoleId("b80138a0-0a0b-4a3a-8fbb-5bca2dc8c84c")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("AreBillable")]
		public RelationType IsBillable;

		#region Allors
		[Id("a4246c12-e77c-41e0-9f00-995fab17c13c")]
		[AssociationId("eef2f215-f262-4f7e-b87b-a8229b1d5d4b")]
		[RoleId("f1ff8c32-0f88-49b9-83c1-b0754d65700e")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("FromDateTimes")]
		public RelationType FromDateTime;

		#region Allors
		[Id("a6ae42bd-babf-44e1-bdc0-cc403e56e43e")]
		[AssociationId("47acb5ae-b805-494e-9a44-10e2ddccec80")]
		[RoleId("04df18b1-b92d-437d-a666-852c85e64330")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("b9bb6409-c6b9-4a4b-9d46-02c62b4b3304")]
		[AssociationId("c4b7a55c-d0d9-429f-9577-d32de5b6f0cd")]
		[RoleId("f624973f-1a6a-4cd6-930f-ecfb4d3772ec")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortInterface))]
		[Plural("WorkEfforts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType WorkEffort;



		public static ServiceEntryInterface Instance {get; internal set;}

		internal ServiceEntryInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}