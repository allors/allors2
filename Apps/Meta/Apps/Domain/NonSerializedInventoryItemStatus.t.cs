namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("700360b9-56be-4e51-9610-f1e5951dd765")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("NonSerializedInventoryItemStatuses")]
	public partial class NonSerializedInventoryItemStatusClass : Class
	{
		#region Allors
		[Id("590f1d9b-a805-4b0e-a2bd-8e274608fe3c")]
		[AssociationId("bcc05955-7c14-45f9-b2ea-ab36feca7287")]
		[RoleId("30a81620-9871-414c-9b09-c2fad0358bb4")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("959aa0a9-a197-4eb4-bc9e-e40da8892dd0")]
		[AssociationId("d3b7eb35-52d2-48fc-a416-a2185ae347ee")]
		[RoleId("78059f92-3345-4e5a-8d04-d30d15eee05a")]
		#endregion
		[Indexed]
		[Type(typeof(NonSerializedInventoryItemObjectStateClass))]
		[Plural("NonSerializedInventoryItemObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType NonSerializedInventoryItemObjectState;



		public static NonSerializedInventoryItemStatusClass Instance {get; internal set;}

		internal NonSerializedInventoryItemStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}