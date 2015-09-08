namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("00be6409-1ca0-491e-b0a1-3d53e17005f6")]
	#endregion
	[Inherit(typeof(DeletableInterface))]

	[Plural("OrderShipments")]
	public partial class OrderShipmentClass : Class
	{
		#region Allors
		[Id("1494758b-f763-48e5-a5a9-cd5c83a8af95")]
		[AssociationId("5aa8e3aa-cc9c-4b12-9126-5ab6f160d661")]
		[RoleId("d49541c8-7cf6-439f-84e0-c8a1d73e5f3c")]
		#endregion
		[Indexed]
		[Type(typeof(SalesOrderItemClass))]
		[Plural("SalesOrderItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesOrderItem;

		#region Allors
		[Id("261a25f4-672a-44a0-ad2d-1c62ba383006")]
		[AssociationId("cfaa2021-233c-4b55-b33d-65b9344adb67")]
		[RoleId("69f35130-996e-4a55-b6be-90199a2548d0")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("Pickeds")]
		public RelationType Picked;

		#region Allors
		[Id("b55bbdb8-af05-4008-a6a7-b4eea78096bd")]
		[AssociationId("a4d6f79e-c204-44ca-b7db-3a0a3eacff69")]
		[RoleId("a6a0d0ac-15c6-489f-ab15-197314f4f52c")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentItemClass))]
		[Plural("ShipmentItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipmentItem;

		#region Allors
		[Id("d4725e9c-b72c-4cdf-95f9-70f9c4b57b11")]
		[AssociationId("4f4c74fc-44d8-445e-aa2e-1e79c2fd6b87")]
		[RoleId("c9ce4f17-3bef-4b0b-a5e0-4fc38641f8ed")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Quantities")]
		public RelationType Quantity;

		#region Allors
		[Id("d6c35df9-dad3-4e4c-b66e-5ccda26093d5")]
		[AssociationId("b8ea1ed0-ba19-44c3-9e8d-5228734b3bc4")]
		[RoleId("78bddfef-0bbe-4185-8a5a-78e5a3ba42a0")]
		#endregion
		[Indexed]
		[Type(typeof(PurchaseOrderItemClass))]
		[Plural("PurchaseOrderItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchaseOrderItem;



		public static OrderShipmentClass Instance {get; internal set;}

		internal OrderShipmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}