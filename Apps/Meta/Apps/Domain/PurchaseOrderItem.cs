namespace Allors.Meta
{
	#region Allors
	[Id("ab648bd0-6e31-4ab0-a9ee-cf4a6f02033d")]
	#endregion
	[Inherit(typeof(OrderItemInterface))]
	public partial class PurchaseOrderItemClass : Class
	{
        #region Allors
        [Id("3F65C670-B891-4979-B664-D47D45833AF5")]
        #endregion
        public MethodType Complete;

        #region Allors
        [Id("0d6cc324-fa0e-4a8c-8afd-802a6301a6c7")]
		[AssociationId("68ad7777-1d14-4635-8f36-1c1e68bd1989")]
		[RoleId("ddbd34f4-264a-4465-b57c-a3f9c76e6a52")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderItemStatusClass))]
		[Plural("OrderItemStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType OrderItemStatus;

		#region Allors
		[Id("43035995-bea3-488b-9e81-e85e929faa57")]
		[AssociationId("f9d773a8-772b-4981-a360-944f14a5ef94")]
		[RoleId("f7034bc1-6cc0-4e03-ab3c-da64d427df9b")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderItemObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("47af92f0-f773-40e2-b0ed-4b3677eddbb7")]
		[AssociationId("6eb5977f-2a79-49e1-ac87-16a53de7e40b")]
		[RoleId("e2ee216b-ae28-4ddf-b354-aa7a75f4cc4e")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderItemStatusClass))]
		[Plural("ShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentStatus;

		#region Allors
		[Id("5e2f5c1a-99e7-4906-8cdd-e78ac4f4bce0")]
		[AssociationId("de791292-84df-4297-959f-d3bc61a2e137")]
		[RoleId("5f9865d9-b7b2-42e3-b13d-013b8945e843")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderItemStatusClass))]
		[Plural("PaymentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PaymentStatus;

		#region Allors
		[Id("64e30c56-a77d-4ecf-b21e-e480dd5a25d8")]
		[AssociationId("448695c9-c23b-4ae0-98d7-802a8ae4e9f8")]
		[RoleId("9586b58f-8ae0-4b26-81b6-085a9e28aa77")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("QuantitiesReceived")]
		public RelationType QuantityReceived;

		#region Allors
		[Id("6c187e2c-d7ab-4d3d-b8d9-732af7310e7e")]
		[AssociationId("50d321f7-fa51-4d08-a12d-e7b8702d2c33")]
		[RoleId("0aab6049-05b6-494a-ac11-df251374f8f4")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderItemStatusClass))]
		[Plural("CurrentShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentShipmentStatus;

		#region Allors
		[Id("adfe14e7-fbf6-465f-b6e5-1eb3e8583179")]
		[AssociationId("682538a3-d3e7-432b-9264-38197462cee1")]
		[RoleId("fecc85a0-871b-4846-b8f1-c2a5728fbbd2")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("Products")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Product;

		#region Allors
		[Id("bbe10173-c24c-4514-86ec-96bd0741efa6")]
		[AssociationId("d12015c4-7462-4dec-95b6-2c233cbb8607")]
		[RoleId("75c95f93-74c1-47b9-9bcc-457edc48a4b3")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderItemStatusClass))]
		[Plural("CurrentOrderItemStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentOrderItemStatus;

		#region Allors
		[Id("cca92fe0-8711-46fd-b08d-bf313cc585a6")]
		[AssociationId("db50db5b-59d8-46b9-9c59-d1b9a93fec11")]
		[RoleId("425b29d1-4001-46e0-821c-6da18051d3ee")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderItemStatusClass))]
		[Plural("CurrentPaymentStatus")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentPaymentStatus;

		#region Allors
		[Id("e2dc0027-220b-4935-bc5a-cb2e2b6be248")]
		[AssociationId("3d24da0d-fdd6-46e3-909b-7710e84e2d68")]
		[RoleId("76ed288c-be72-44e2-8b83-0a0f5a616e52")]
		#endregion
		[Indexed]
		[Type(typeof(PartInterface))]
		[Plural("Parts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Part;

		public static PurchaseOrderItemClass Instance {get; internal set;}

		internal PurchaseOrderItemClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.QuantityReceived.RoleType.IsRequired = true;

            this.CurrentObjectState.RoleType.IsRequired = true;
        }
    }
}