namespace Allors.Meta
{
	#region Allors
	[Id("90df16ba-ab97-4ec1-9db3-ab20314122bc")]
	#endregion
	[Inherit(typeof(DeletableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Plural("WorkEffortStatuses")]
	public partial class WorkEffortStatusClass : Class
	{
		#region Allors
		[Id("5dd27f4b-032d-4b45-86ad-ba288c26fa7c")]
		[AssociationId("2743e797-731b-404f-866a-5b9249309f60")]
		[RoleId("99022ef3-d4f2-4635-b27a-c02b554ad8ae")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("f9e60388-f0da-45d9-94c2-5fe2d5ff581a")]
		[AssociationId("9bb24455-11ed-4dba-820c-fa6b03aae9a6")]
		[RoleId("6d2efe75-9b3f-449b-95f7-cbc552a2ca3c")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortObjectStateClass))]
		[Plural("WorkEffortObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType WorkEffortObjectState;
        
		public static WorkEffortStatusClass Instance {get; internal set;}

		internal WorkEffortStatusClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.WorkEffortObjectState.RoleType.IsRequired = true;
        }
    }
}