namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d149dd80-1cdc-4a29-bb0b-b88823d718bc")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("EngineeringChangeStatuses")]
	public partial class EngineeringChangeStatusClass : Class
	{
		#region Allors
		[Id("0a6c34f7-b37b-4abc-b12e-05ef14a8d986")]
		[AssociationId("66d0b0a8-b39a-4654-9ce6-3b8a8e9bbf4a")]
		[RoleId("765d186a-a5b5-4895-ae27-93bfe6ef98f2")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("6a7695dc-4343-4645-b4f1-78348d6873c3")]
		[AssociationId("7a1f031f-29ca-4b1c-95c0-1bdc35856412")]
		[RoleId("09cf51ab-77a3-4188-97ad-590b6bca6a97")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(EngineeringChangeObjectStateClass))]
		[Plural("EngineeringChangeObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType EngineeringChangeObjectState;



		public static EngineeringChangeStatusClass Instance {get; internal set;}

		internal EngineeringChangeStatusClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.EngineeringChangeObjectState.RoleType.IsRequired = true;
        }
    }
}