namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("7f13c77f-1ef1-446d-928d-1c96f9fc8b05")]
	#endregion
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("OrderKinds")]
	public partial class OrderKindClass : Class
	{
		#region Allors
		[Id("cb4c5cfa-5c2c-4cdf-898b-9afcd28229c4")]
		[AssociationId("6b9b043f-c629-439d-be92-e825177d8c29")]
		[RoleId("8f85bba3-1eaa-4352-bfd4-68f29ce4f71c")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("e35c295c-a4a8-4441-af9a-bd2d3e96bab3")]
		[AssociationId("c2158f51-489a-4618-b289-dff18a05afb5")]
		[RoleId("c07c051a-e204-46dd-bac2-1ce957c8c6d9")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("ScheduleManuallies")]
		public RelationType ScheduleManually;



		public static OrderKindClass Instance {get; internal set;}

		internal OrderKindClass() : base(MetaPopulation.Instance)
        {
        }
	}
}