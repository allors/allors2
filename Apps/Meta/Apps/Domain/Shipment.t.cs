namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9c6f4ad8-5a4e-4b6e-96b7-876f7aabcffb")]
	#endregion
	[Plural("Shipments")]
	[Inherit(typeof(PrintableInterface))]
	[Inherit(typeof(TransitionalInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class ShipmentInterface: Interface
	{
		#region Allors
		[Id("05221b28-9c80-4d3b-933f-12a8a17bc261")]
		[AssociationId("c59ef057-da9a-433f-90d3-5ff657aa1e48")]
		[RoleId("6fe551cd-0808-466b-9ec9-833098ebad79")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentMethodClass))]
		[Plural("ShipmentMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipmentMethod;

		#region Allors
		[Id("05b0841b-d546-4fd6-9305-492b0ce20f8a")]
		[AssociationId("26be1e2b-ee3c-4c37-9ccc-07a916e6af29")]
		[RoleId("313a2875-bafc-430a-b7c4-1aa45e825233")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("BillToContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillToContactMechanism;

		#region Allors
		[Id("165b529f-df1c-45b6-bbed-d19ffcb375f2")]
		[AssociationId("c71e40be-1f55-483d-9bfa-0d2dfb26c7d9")]
		[RoleId("18a5331e-120e-45e6-8ef4-35a1f48237e0")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentPackageClass))]
		[Plural("ShipmentPackages")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentPackage;

		#region Allors
		[Id("17234c66-6b61-4ac9-a23b-4388e19f4888")]
		[AssociationId("bc2164ec-5d7e-4dff-8db6-4d1eeab970e6")]
		[RoleId("f939af72-bcb4-44bc-b47d-758c27304a7d")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("ShipmentNumbers")]
		public RelationType ShipmentNumber;

		#region Allors
		[Id("18808545-f941-4c5a-8809-0f1fb0cca2d8")]
		[AssociationId("44940303-b210-42bd-8791-906004294261")]
		[RoleId("a65dbc06-f659-4e34-bf2d-af4b4717972e")]
		#endregion
		[Indexed]
		[Type(typeof(DocumentInterface))]
		[Plural("Documents")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType Document;

		#region Allors
		[Id("50f36218-ae61-4d67-af4d-d05cc8b2266d")]
		[AssociationId("a8ff4824-3ccd-49a8-82e6-e7723ccb8348")]
		[RoleId("b7ead377-a5d4-4eab-98d9-e9527177090a")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("BillToParties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillToParty;

		#region Allors
		[Id("5891b368-89cd-4a0e-aaef-439f442909c8")]
		[AssociationId("5fef9e9f-bd3d-454a-8aa1-10b262a34a4b")]
		[RoleId("dd5e8d80-0395-413d-addb-ca66f36c50e8")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("ShipToParties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipToParty;

		#region Allors
		[Id("6a568bea-6718-414a-b822-d8304502be7b")]
		[AssociationId("499bb422-b2f0-48cf-bf09-0544e768b5de")]
		[RoleId("b8724e90-9888-4f81-b70d-1eceb93af3d3")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentItemClass))]
		[Plural("ShipmentItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentItem;

		#region Allors
		[Id("6b90885f-9421-442a-b517-b85c6fe3c60d")]
		[AssociationId("b4df53d0-3970-45bf-bcfb-251dc18ebb46")]
		[RoleId("215a7b54-93d9-455c-9979-759b116677cd")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("BillFromInternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillFromInternalOrganisation;

		#region Allors
		[Id("78e7e7a5-2d8c-4184-b917-10095dc033b1")]
		[AssociationId("f924c450-6c83-4853-9449-b34efb52cc78")]
		[RoleId("b9c80d27-7278-4883-b1f3-d01712463109")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("ReceiverContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ReceiverContactMechanism;

		#region Allors
		[Id("7e1325e0-a072-46da-adb5-b997dde9980a")]
		[AssociationId("f73c3f6d-cc9c-4bda-a4c6-ef4f406a491d")]
		[RoleId("14f6385d-4e20-4ffe-89e7-f7a261eda78e")]
		#endregion
		[Indexed]
		[Type(typeof(PostalAddressClass))]
		[Plural("ShipToAddresses")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipToAddress;

		#region Allors
		[Id("894ecdf3-1322-4513-bf94-63882c5c29bf")]
		[AssociationId("da1adb58-e2be-4018-97b0-fb2ef107a661")]
		[RoleId("7e28940e-6039-4698-b1f5-b31769aa7bbb")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("EstimatedShipCosts")]
		public RelationType EstimatedShipCost;

		#region Allors
		[Id("97788e21-ec31-4fb2-9ef7-0b7b5a7367a1")]
		[AssociationId("227f8e47-58af-44be-bcaf-0da60e2c13d4")]
		[RoleId("338e2be0-6eb5-42ad-b51c-83dd9b7f0194")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EstimatedShipDates")]
		public RelationType EstimatedShipDate;

		#region Allors
		[Id("a74391e5-bd03-4247-93b8-e7081d939823")]
		[AssociationId("41060c75-fb34-4391-96f3-d0d267344ba3")]
		[RoleId("eb3f084c-9d59-4fff-9fc3-186d7b9a19b3")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("LatestCancelDates")]
		public RelationType LatestCancelDate;

		#region Allors
		[Id("b37c7c90-0287-4f12-b000-025e2505499c")]
		[AssociationId("13e8d5af-43ff-431b-85d8-5e7706dc2f75")]
		[RoleId("81367cbd-4713-46bd-8f4d-0df30c3daf96")]
		#endregion
		[Indexed]
		[Type(typeof(CarrierClass))]
		[Plural("Carriers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Carrier;

		#region Allors
		[Id("b5dabbcc-508a-4998-a21a-6b86d7193688")]
		[AssociationId("43d9bbc8-319c-4971-a651-11f246fafa97")]
		[RoleId("ebf2b41f-a922-4689-83d4-51569a3d85d3")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("InquireAboutContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InquireAboutContactMechanism;

		#region Allors
		[Id("b69c6812-bdc4-4a06-a782-fa8ff4a71aca")]
		[AssociationId("988cafce-2323-4c0d-b1cd-026045764ba4")]
		[RoleId("cd02effa-d176-4f6e-8407-ec12d23b9f2a")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EstimatedReadyDates")]
		public RelationType EstimatedReadyDate;

		#region Allors
		[Id("c8b0eff8-4dff-449c-9d44-a7235cd24928")]
		[AssociationId("556c0ae6-045e-4f35-8f63-ffb41f57dc44")]
		[RoleId("5c34f5ee-5f25-42dd-97a8-7aa3aeb9973e")]
		#endregion
		[Indexed]
		[Type(typeof(PostalAddressClass))]
		[Plural("ShipFromAddresses")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipFromAddress;

		#region Allors
		[Id("ea57219b-217e-444d-9741-c1c4e7aee9f7")]
		[AssociationId("2d0935d0-cdb5-4c3e-9726-e27ea731c43b")]
		[RoleId("d7184821-3b9c-4800-874f-32d7cd9b72e3")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("BillFromContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillFromContactMechanism;

		#region Allors
		[Id("ee49c6ca-bb03-40d3-97f1-004cc5a31132")]
		[AssociationId("167b541c-d2dd-4d9b-9fe1-6cd8d1a5f727")]
		[RoleId("39a0ed41-436e-44bd-afc7-5d848397433b")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("HandlingInstructions")]
		public RelationType HandlingInstruction;

		#region Allors
		[Id("f1059139-6664-43d5-801f-79a4cc4288a6")]
		[AssociationId("92807e93-ed03-4dbc-9296-c508c879705b")]
		[RoleId("3f2699b9-9652-4af4-98d7-2ff803677692")]
		#endregion
		[Indexed]
		[Type(typeof(StoreClass))]
		[Plural("Stores")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Store;

		#region Allors
		[Id("f1e92d31-db63-419c-8ed7-49f5db66c63d")]
		[AssociationId("fffbc8b5-a541-402d-8df6-3134cc52b306")]
		[RoleId("566b9c3a-3fec-455f-a40d-b23338d3508c")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("ShipFromParties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipFromParty;

		#region Allors
		[Id("f403ab39-cc81-4e09-8794-a45db9ef178f")]
		[AssociationId("78c8d202-0277-4c3a-9e24-74041cc56872")]
		[RoleId("8086c3d5-1577-4c32-bf73-abe72aac725c")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentRouteSegmentClass))]
		[Plural("ShipmentRouteSegments")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentRouteSegment;

		#region Allors
		[Id("fdac3beb-edf8-4d1b-80d4-21b643ef43ce")]
		[AssociationId("63d8adfc-6afb-499f-bd27-2f1d3f78bee6")]
		[RoleId("8f56ce24-500e-4db9-abce-c7a301c38fe6")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EstimatedArrivalDates")]
		public RelationType EstimatedArrivalDate;



		public static ShipmentInterface Instance {get; internal set;}

		internal ShipmentInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}