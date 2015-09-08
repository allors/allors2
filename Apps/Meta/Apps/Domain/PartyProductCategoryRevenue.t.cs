namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3f2c4c17-ec80-44ad-b452-76cf694f3d6a")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(DeletableInterface))]

	[Plural("PartyProductCategoryRevenues")]
	public partial class PartyProductCategoryRevenueClass : Class
	{
		#region Allors
		[Id("05de234d-6f00-49b2-802d-fcc590cc1aec")]
		[AssociationId("93a67619-2960-49e8-97e1-813614df8a32")]
		[RoleId("fba6ebaf-efbd-465d-b142-b754c34af161")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Parties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Party;

		#region Allors
		[Id("068c3c16-2827-4574-9fe6-323e74634db0")]
		[AssociationId("2948c509-a13f-4f3b-9cad-b39ce9bdb3c7")]
		[RoleId("337d69c4-0565-42bc-a0cf-3c76dde9115f")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;

		#region Allors
		[Id("1f41a4c2-a9af-42e1-92e4-a9069bc024b1")]
		[AssociationId("42f9808e-94c3-4fda-b822-4a02e4a7648c")]
		[RoleId("6a6ca0a9-f262-4891-98c5-f6abb1adf93f")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Months")]
		public RelationType Month;

		#region Allors
		[Id("4584c4f9-12a5-4435-a6b8-2b2e6bb932b9")]
		[AssociationId("852fe55f-9f03-4542-90f4-ef4fceb560a3")]
		[RoleId("013068f6-131a-4d0c-9847-d772ba9d3596")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("4fff87cc-3232-4174-9a91-9d9ee0192360")]
		[AssociationId("57a27625-0f61-4026-aabf-9a7de257d133")]
		[RoleId("38fd22de-76e6-4887-9148-f99015e4816b")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Years")]
		public RelationType Year;

		#region Allors
		[Id("5b77f9ce-15c5-4fb3-95d6-11d2cc1aca95")]
		[AssociationId("5609dc7f-0354-4ed0-a6cf-120fd41d3eb9")]
		[RoleId("3d418a0d-f737-4bf8-856e-a94f8f7af774")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("PartyProductCategoryNames")]
		public RelationType PartyProductCategoryName;

		#region Allors
		[Id("7f6972f9-16dc-4795-9e4b-7095738e80ed")]
		[AssociationId("20ecc315-ee8c-4bd2-9320-f16d258db9bc")]
		[RoleId("9cf8972a-71d1-465e-bf08-6713c695a29a")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("8e0338d5-5fab-4024-9e37-05afd05aa514")]
		[AssociationId("a9c15cc8-1066-4c43-bd85-a2a306a2b5d1")]
		[RoleId("e4393c39-4047-46ba-8335-68644a69413b")]
		#endregion
		[Indexed]
		[Type(typeof(ProductCategoryClass))]
		[Plural("ProductCategories")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductCategory;

		#region Allors
		[Id("b0b50eec-0a88-41e8-895a-270517240b7b")]
		[AssociationId("0af942bf-8bd7-4297-93ff-11971dbd12ce")]
		[RoleId("53055a2e-dbf8-40a9-a1d4-50297bfe38c1")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Quantities")]
		public RelationType Quantity;



		public static PartyProductCategoryRevenueClass Instance {get; internal set;}

		internal PartyProductCategoryRevenueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}