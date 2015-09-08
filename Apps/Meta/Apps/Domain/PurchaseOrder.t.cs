namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("062bd939-9902-4747-a631-99ea10002156")]
	#endregion
	[Inherit(typeof(OrderInterface))]

	[Plural("PurchaseOrders")]
	public partial class PurchaseOrderClass : Class
	{
		#region Allors
		[Id("15ea478f-b71d-412f-8ee4-abe554b9a7d8")]
		[AssociationId("e48c8211-2539-41ba-9250-27a08799b31b")]
		[RoleId("6ef2d258-4291-4a9f-b7f0-9f154b789775")]
		#endregion
		[Indexed]
		[Type(typeof(PurchaseOrderItemClass))]
		[Plural("PurchaseOrderItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PurchaseOrderItem;

		#region Allors
		[Id("1638a432-3a4f-4cca-906e-660b9164838b")]
		[AssociationId("04f4151a-1adf-426a-9fb1-a0f8cc782b0e")]
		[RoleId("20131db5-50af-42a8-9ac8-fd250c1aa8b6")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("PreviousTakenViaSuppliers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PreviousTakenViaSupplier;

		#region Allors
		[Id("169ea375-cb46-44b9-a0b8-e0c9c37d6eb5")]
		[AssociationId("259aaa98-e60f-4c2e-a08d-ae3e25c44434")]
		[RoleId("c442da88-7371-4305-96e0-a2771502fefc")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderStatusClass))]
		[Plural("PaymentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PaymentStatus;

		#region Allors
		[Id("2a4ce670-53f4-4c31-9e46-96437f4a80e1")]
		[AssociationId("82e04803-a492-45ef-b7ce-9eca4f521c51")]
		[RoleId("e52f200c-d14c-442b-a4cc-1670ec47efe1")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderStatusClass))]
		[Plural("CurrentPaymentStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentPaymentStatus;

		#region Allors
		[Id("36607a9e-d411-4726-a63c-7622b928bfe8")]
		[AssociationId("a8573588-3898-4422-92a2-056448200216")]
		[RoleId("31a6a1a2-92ee-4ffd-9eb8-d69e8f2183fd")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("TakenViaSuppliers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TakenViaSupplier;

		#region Allors
		[Id("3b1f04bd-c9ad-4fca-981c-2ca243fdc292")]
		[AssociationId("e50e5e9b-f312-4520-81d1-1dbd7f856d0f")]
		[RoleId("64e08e15-4d4d-465b-9d9e-c9e7c971a56d")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("3edb1f6e-8497-4730-8144-e64f6c8d4446")]
		[AssociationId("c5628e77-b2cf-42c0-a583-930731aa8474")]
		[RoleId("4c8c8e44-84d7-450a-bfcc-158e1879b189")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderStatusClass))]
		[Plural("CurrentShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentShipmentStatus;

		#region Allors
		[Id("4830cfc5-0375-4996-8cd8-27e36c102b65")]
		[AssociationId("efa439f8-787e-43d7-bd1b-400cba7e3a62")]
		[RoleId("583bfc51-0bb7-4ea5-914c-33a5c2d64196")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("TakenViaContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TakenViaContactMechanism;

		#region Allors
		[Id("6d0f4867-3237-4c3e-b00b-96f4ad456c55")]
		[AssociationId("ac51d7e6-abd1-4d07-9b25-34888b4f830d")]
		[RoleId("e146ab05-15d4-4f29-a89c-c97b5a75d0cf")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderStatusClass))]
		[Plural("OrderStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType OrderStatus;

		#region Allors
		[Id("7eceb1b6-1395-4655-a558-6d72ad4b380e")]
		[AssociationId("b6e1159c-fcb7-47f1-822b-4ab75e5dac14")]
		[RoleId("ab3ee3c7-dc02-4acf-a34e-6b25783e11fc")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("BillToContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillToContactMechanism;

		#region Allors
		[Id("87a2439e-83e1-4b51-97b8-9c21cc743e07")]
		[AssociationId("711e264a-74eb-4229-a4d1-82ddef1bb597")]
		[RoleId("0316aafc-f5b2-41b2-bf39-7c2b7e4e0afc")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderStatusClass))]
		[Plural("ShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentStatus;

		#region Allors
		[Id("b390a733-e322-4ada-9ead-75a8c9976337")]
		[AssociationId("6082b0af-f5ed-493c-bb2b-ad4764053819")]
		[RoleId("c725b348-df8f-4a64-adc2-c3d8b3b986b5")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("ShipToBuyers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipToBuyer;

		#region Allors
		[Id("cbcf2101-3e38-4c69-b5ea-a6860ea056f0")]
		[AssociationId("72fdb437-3f79-454a-9294-883901f7ffd5")]
		[RoleId("9c2cd7fa-6279-4bab-a2d3-c2eaf6ef7cff")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseOrderStatusClass))]
		[Plural("CurrentOrderStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentOrderStatus;

		#region Allors
		[Id("ccf88515-6441-4d0f-a2e7-8f5ed7c0533e")]
		[AssociationId("ce230886-53a7-4360-b545-a20d3cf47f1f")]
		[RoleId("2f7e7d1b-6a61-41a6-a05f-375e8a5feeb2")]
		#endregion
		[Indexed]
		[Type(typeof(FacilityInterface))]
		[Plural("Facilities")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Facility;

		#region Allors
		[Id("d74bd1fd-f243-4b5d-8061-1eafe7c25beb")]
		[AssociationId("5465663b-6757-4b1d-9f91-233bfd86bc5d")]
		[RoleId("35c28c9f-852a-4ebb-bc2b-1dce9e3812fa")]
		#endregion
		[Indexed]
		[Type(typeof(PostalAddressClass))]
		[Plural("ShipToAdresses")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipToAddress;

		#region Allors
		[Id("f05e0ba5-4321-4d88-8f2c-8994de5b44b7")]
		[AssociationId("38d76559-6a9c-48c7-bde5-1a2e685b9a40")]
		[RoleId("a0b2ec91-5b7e-4abb-91fb-91836cb88490")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("BillToPurchasers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillToPurchaser;



		public static PurchaseOrderClass Instance {get; internal set;}

		internal PurchaseOrderClass() : base(MetaPopulation.Instance)
        {
        }
	}
}