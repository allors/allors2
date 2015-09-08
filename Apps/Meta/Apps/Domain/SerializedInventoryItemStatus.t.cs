namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1da3e549-47cb-4896-94ec-3f8a263bb559")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("SerializedInventoryItemStatuses")]
	public partial class SerializedInventoryItemStatusClass : Class
	{
		#region Allors
		[Id("aabb931a-38ee-4568-af8c-5f8ed98ed7b9")]
		[AssociationId("85598163-c71c-4bdc-942b-5ad461943e01")]
		[RoleId("87e945cc-864b-42b6-ad9b-c3d447d96073")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("d2c2fff8-73ec-4748-9c8f-29304abbdb0d")]
		[AssociationId("ee25cfd7-7389-4db7-9bb2-ee388e57f6d1")]
		[RoleId("584017a5-99b5-414a-b32e-c64f7f2a0d4e")]
		#endregion
		[Indexed]
		[Type(typeof(SerializedInventoryItemObjectStateClass))]
		[Plural("SerializedInventoryItemObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SerializedInventoryItemObjectState;



		public static SerializedInventoryItemStatusClass Instance {get; internal set;}

		internal SerializedInventoryItemStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}