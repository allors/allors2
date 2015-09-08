namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d35c33c3-ca15-4b70-b20d-c51ed068626a")]
	#endregion
	[Inherit(typeof(DeletableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("ShipmentItems")]
	public partial class ShipmentItemClass : Class
	{
		#region Allors
		[Id("082e7e0d-190c-463f-89c8-af8e2c57c68d")]
		[AssociationId("cfbef516-6673-4496-ad91-54e772557ef5")]
		[RoleId("7e235029-4dc3-46d2-878d-58a05e68c4e1")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Quantities")]
		public RelationType Quantity;

		#region Allors
		[Id("158f104b-fa5c-425e-8b55-ee4e866820ec")]
		[AssociationId("77f01592-48e6-486f-9217-7c9cfc477267")]
		[RoleId("6dcd6646-e5fa-42c9-a54b-c95380e860a2")]
		#endregion
		[Indexed]
		[Type(typeof(PartInterface))]
		[Plural("Parts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Part;

		#region Allors
		[Id("19c691ae-f849-451e-ac7e-ea84f4a9b51a")]
		[AssociationId("9a57f102-0b43-4f10-af75-c808c718c8b7")]
		[RoleId("b18cc4e1-0be7-48d7-9e92-efc1e3a3edca")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("ContentsDescriptions")]
		public RelationType ContentsDescription;

		#region Allors
		[Id("6b3ab563-a19b-4d92-be3a-ddf3046d5b18")]
		[AssociationId("d41aeb48-bd41-40b2-bbc4-f4dd096a6c5f")]
		[RoleId("e9a936df-2165-455e-8f9c-02b3dc5d7ebb")]
		#endregion
		[Indexed]
		[Type(typeof(DocumentInterface))]
		[Plural("Documents")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType Document;

		#region Allors
		[Id("92cca1c2-4c7e-49a0-ba4b-88693b87c379")]
		[AssociationId("d7dca1d7-e678-4b52-9c25-d93f353ca25d")]
		[RoleId("50dd2c62-6d8f-45bf-a77b-c8e1f7933ad3")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("QuantitiesShipped")]
		public RelationType QuantityShipped;

		#region Allors
		[Id("b5d35e87-f741-4600-9838-4419b127681d")]
		[AssociationId("797743d0-c0e9-4a75-9180-4e05eb55423f")]
		[RoleId("d37d1290-af88-45c1-8e70-2774de0c58c2")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentItemClass))]
		[Plural("InResponseToShipmentItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType InResponseToShipmentItem;

		#region Allors
		[Id("b8ca6fae-0866-4806-9ffd-64d5d2b978f9")]
		[AssociationId("2a9e81f6-6009-4706-a0d0-cd180cb825e6")]
		[RoleId("31227051-0164-40e7-9e37-d1b31719e483")]
		#endregion
		[Indexed]
		[Type(typeof(InventoryItemInterface))]
		[Plural("InventoryItems")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType InventoryItem;

		#region Allors
		[Id("b9bfaea8-e5f0-4b0e-955f-df28ed63e8e3")]
		[AssociationId("7da8c058-92b7-4fd7-9eaf-7b7fb94f62cf")]
		[RoleId("fb45aece-26e0-42ec-8dac-ddfcf11e61d9")]
		#endregion
		[Indexed]
		[Type(typeof(ProductFeatureInterface))]
		[Plural("ProductFeatures")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType ProductFeature;

		#region Allors
		[Id("bdd8041b-b1d1-4902-980a-f085c0af166d")]
		[AssociationId("150fd46f-8768-4210-a559-740386e7c03d")]
		[RoleId("2fb644a8-bbd4-419e-8c30-ef10efeb07d7")]
		#endregion
		[Indexed]
		[Type(typeof(InvoiceItemInterface))]
		[Plural("InvoiceItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType InvoiceItem;

		#region Allors
		[Id("f5c0279e-5ce4-4f09-bb93-ecaeb4825bcf")]
		[AssociationId("59b2bb80-3e60-4958-a3d8-9b5f7242d95c")]
		[RoleId("fbac397f-52f2-4903-95bc-ee3f6ab3ae7b")]
		#endregion
		[Indexed]
		[Type(typeof(GoodClass))]
		[Plural("Goods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Good;



		public static ShipmentItemClass Instance {get; internal set;}

		internal ShipmentItemClass() : base(MetaPopulation.Instance)
        {
        }
	}
}