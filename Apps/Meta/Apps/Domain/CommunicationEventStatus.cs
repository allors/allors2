namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("e2c3f3fa-7b94-4315-b8dd-2f538d8e2132")]
	#endregion
	[Inherit(typeof(DeletableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("CommunicationEventStatuses")]
	public partial class CommunicationEventStatusClass : Class
	{
		#region Allors
		[Id("414fc983-3086-4362-806a-d77b09f04b24")]
		[AssociationId("0da368d4-31c3-45b3-b556-561b080a03a5")]
		[RoleId("d0e76e79-e797-44dd-8256-f88c10b1d440")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("5ad71d39-d9e2-4b08-a6ac-322c18f14be5")]
		[AssociationId("51e4f7d6-a511-493e-8207-b60343fccae6")]
		[RoleId("4d04158e-c9a4-490c-9a35-c2205a01938a")]
		#endregion
		[Indexed]
		[Type(typeof(CommunicationEventObjectStateClass))]
		[Plural("CommunicationEventObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CommunicationEventObjectState;



		public static CommunicationEventStatusClass Instance {get; internal set;}

		internal CommunicationEventStatusClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.CommunicationEventObjectState.RoleType.IsRequired = true;
        }
    }
}