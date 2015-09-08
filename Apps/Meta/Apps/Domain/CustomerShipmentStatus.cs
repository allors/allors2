namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f13976d4-b1f4-4b78-a720-beab1e0a7e4c")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("CustomerShipmentStatuses")]
	public partial class CustomerShipmentStatusClass : Class
	{
		#region Allors
		[Id("591d3237-220b-4765-8001-4bc18ecd2d8c")]
		[AssociationId("2a2704d6-44f6-4e86-a8c9-407842b7eb83")]
		[RoleId("fd56e773-b27b-4336-b1be-e262d1d26b41")]
		#endregion
		[Indexed]
		[Type(typeof(CustomerShipmentObjectStateClass))]
		[Plural("CustomerShipmentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CustomerShipmentObjectState;

		#region Allors
		[Id("74e826e5-75d0-4e7d-b2fe-73a7c58e30ef")]
		[AssociationId("261ca695-c146-493d-b059-3836913268c4")]
		[RoleId("eb029966-6353-401b-b24b-190460f0c035")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;



		public static CustomerShipmentStatusClass Instance {get; internal set;}

		internal CustomerShipmentStatusClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.CustomerShipmentObjectState.RoleType.IsRequired = true;
        }
    }
}