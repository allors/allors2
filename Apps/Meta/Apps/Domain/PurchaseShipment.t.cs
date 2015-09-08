namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2bf859c6-de64-476f-a437-5eb57a778262")]
	#endregion
	[Inherit(typeof(ShipmentInterface))]

	[Plural("PurchaseShipments")]
	public partial class PurchaseShipmentClass : Class
	{
		#region Allors
		[Id("400d42cd-bb10-406d-a448-9d9d53ccb5ca")]
		[AssociationId("9133d8c2-1988-42ef-a006-d02693e322e4")]
		[RoleId("2f25f5a9-d1ef-45e3-8317-8bbcc65ff2ff")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseShipmentObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("40277d59-6ab8-40b0-acee-c95ba759e2c8")]
		[AssociationId("d7feb989-dd2d-4619-b079-8a059129f8ed")]
		[RoleId("068d5263-18d7-40e4-80c1-4f9a8e88d10a")]
		#endregion
		[Indexed]
		[Type(typeof(FacilityInterface))]
		[Plural("Facilities")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Facility;

		#region Allors
		[Id("8be328c9-0688-469b-901d-c9c290b30e88")]
		[AssociationId("df4fe9a9-3043-44a6-a4da-96465e63ba07")]
		[RoleId("3726d5dd-a927-48ac-83a5-04d5c2e46b85")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseShipmentStatusClass))]
		[Plural("ShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentStatus;

		#region Allors
		[Id("944c8d81-db22-469b-beb3-d31c045b5af0")]
		[AssociationId("e070f627-4b02-4f43-998b-5b4d8ccbfe80")]
		[RoleId("9e7f1f5d-ed7f-43d8-bb16-45b7f9adc43e")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseShipmentStatusClass))]
		[Plural("CurrentShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentShipmentStatus;

		#region Allors
		[Id("ef34543c-6194-4f27-87d7-a54285bc0a15")]
		[AssociationId("33b1069f-7be2-4f41-b502-8689256706d9")]
		[RoleId("33dce90e-2a2a-482b-bee5-fcea55e59160")]
		#endregion
		[Indexed]
		[Type(typeof(PurchaseOrderClass))]
		[Plural("PurchaseOrders")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchaseOrder;



		public static PurchaseShipmentClass Instance {get; internal set;}

		internal PurchaseShipmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}