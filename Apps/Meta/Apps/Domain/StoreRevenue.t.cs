namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("282e0f1a-fda0-4287-a043-65dcc1853d95")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(DeletableInterface))]

	[Plural("StoreRevenues")]
	public partial class StoreRevenueClass : Class
	{
		#region Allors
		[Id("0074524a-afb6-4acd-b8d7-34dcc988a34a")]
		[AssociationId("dfaa2eeb-c42f-44e2-89ba-69ab1e484093")]
		[RoleId("dc9a6bd0-9250-4e2a-91bf-22ae91d992c0")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("0bb50dce-d7dc-49a7-8af6-f9a75232e1f4")]
		[AssociationId("1fe6427d-19fa-44d9-a116-92b3cc9ebca5")]
		[RoleId("02c4a390-69d3-4133-a29b-82779566d79e")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Months")]
		public RelationType Month;

		#region Allors
		[Id("4686c8fa-0637-4159-8cb6-ff738f686eb0")]
		[AssociationId("b1557f78-f07c-417b-b0b0-c4f26f08574e")]
		[RoleId("3daa636c-e6bc-461d-9db7-7a890622e506")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("620bebb6-89b9-4c37-b9c6-a80605dab7dd")]
		[AssociationId("bcaa6464-f7b9-4cec-91c1-8a35b7f5889c")]
		[RoleId("5bcdf844-f377-4439-84f3-191a79346fcb")]
		#endregion
		[Indexed]
		[Type(typeof(StoreClass))]
		[Plural("Stores")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Store;

		#region Allors
		[Id("870630fe-7bc0-4442-befe-10a013cb4dcd")]
		[AssociationId("dd9cc663-1f43-44be-8942-63ba5740ffaf")]
		[RoleId("452d7d3f-5028-4526-b7cc-17cc24457dd7")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("StoreNames")]
		public RelationType StoreName;

		#region Allors
		[Id("b1305cde-30aa-4573-a227-d333eed87713")]
		[AssociationId("f51e254a-62a8-4670-9a39-2ecaa09cad53")]
		[RoleId("2b002d48-54b3-453d-bbdd-5debeaebce55")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;

		#region Allors
		[Id("e20a98fc-50d9-4bb4-97a4-6754886fd0a2")]
		[AssociationId("3b527ed6-a340-47ee-b294-76b50b122c16")]
		[RoleId("bd11a8a5-be1c-4c87-8c90-8cac07e6b8df")]
		#endregion
		[Indexed]
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Years")]
		public RelationType Year;



		public static StoreRevenueClass Instance {get; internal set;}

		internal StoreRevenueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}