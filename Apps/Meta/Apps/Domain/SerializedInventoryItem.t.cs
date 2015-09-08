namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4a70cbb3-6e23-4118-a07d-d611de9297de")]
	#endregion
	[Inherit(typeof(InventoryItemInterface))]

	[Plural("SerializedInventoryItems")]
	public partial class SerializedInventoryItemClass : Class
	{
		#region Allors
		[Id("a07e8bbb-7bf3-42e1-bcc2-d922a180f5e0")]
		[AssociationId("035a8f39-9b2f-403c-ae64-c43299d59ac2")]
		[RoleId("e53e4d41-6518-4008-a419-522145e712af")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SerializedInventoryItemStatusClass))]
		[Plural("InventoryItemStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType InventoryItemStatus;

		#region Allors
		[Id("de9caf09-6ae7-412e-b9bc-19ece66724da")]
		[AssociationId("ba630eb8-3087-43c6-9082-650094a0226e")]
		[RoleId("c0ada954-d86e-46c3-9a99-09209fb812a5")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("SerialNumbers")]
		public RelationType SerialNumber;

		#region Allors
		[Id("e0fe2033-85a9-428d-9918-f543fbcf3ed7")]
		[AssociationId("49e8ccb2-8a3f-4846-8067-9f68d005e44f")]
		[RoleId("9d19f214-3ed9-4e2d-a924-2d513ca01934")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SerializedInventoryItemObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("fdc2607c-1081-4836-8aa5-1efb96e38da4")]
		[AssociationId("dc285060-57aa-4941-9335-c1b6e273f162")]
		[RoleId("82b912e8-34f9-4a11-a33b-4fdeb7e54ffc")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(SerializedInventoryItemStatusClass))]
		[Plural("CurrentInventoryItemStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentInventoryItemStatus;



		public static SerializedInventoryItemClass Instance {get; internal set;}

		internal SerializedInventoryItemClass() : base(MetaPopulation.Instance)
        {
        }
	}
}