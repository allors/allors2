namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c0b015e0-57a4-4fe3-984b-12e8bda25db7")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("CaseStatuses")]
	public partial class CaseStatusClass : Class
	{
		#region Allors
		[Id("28ef5fa2-7e2a-4ebb-b5a2-fe8cf7f18d04")]
		[AssociationId("9a1d40d3-0c58-4088-9a62-d7a35b787bf6")]
		[RoleId("1acf2d5a-2631-48da-8e9a-222ff1293e83")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("332b3322-ef2e-4457-8503-045aa99061c9")]
		[AssociationId("76b6d6d5-e406-43aa-be3f-90a685a3f8dc")]
		[RoleId("8fbd70e2-fc8c-4584-ac6c-82bc432f9326")]
		#endregion
		[Indexed]
		[Type(typeof(CaseObjectStateClass))]
		[Plural("CaseObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CaseObjectState;



		public static CaseStatusClass Instance {get; internal set;}

		internal CaseStatusClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.CaseObjectState.RoleType.IsRequired = true;
        }
    }
}