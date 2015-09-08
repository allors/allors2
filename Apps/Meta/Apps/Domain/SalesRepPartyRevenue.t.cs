namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("7b0e5009-eef2-4043-8794-b94663397053")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(DeletableInterface))]

	[Plural("SalesRepPartyRevenues")]
	public partial class SalesRepPartyRevenueClass : Class
	{
		#region Allors
		[Id("1671b689-b431-48e3-aa9c-04b67d35645d")]
		[AssociationId("0bd0f873-b179-4d95-b1c9-f9b32db5a58e")]
		[RoleId("673851a4-435d-441e-8bc5-9ebae678a9a2")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;

		#region Allors
		[Id("2f2c05ed-09b0-4058-9dec-69d2e7b0bdce")]
		[AssociationId("3de0784e-21d8-40cc-ad93-699c67b1d996")]
		[RoleId("96a797ef-2830-448d-b08f-1fb54b3a76f2")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Years")]
		public RelationType Year;

		#region Allors
		[Id("44124d06-9bbd-4c55-85f6-7036b49ffbcd")]
		[AssociationId("b7e31e2e-2e76-4049-870b-ff5848dc4ebc")]
		[RoleId("37984d70-d086-461f-8879-710ecef10778")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("SalesReps")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesRep;

		#region Allors
		[Id("85d4036b-dfaf-42e7-935a-cff6858b4b57")]
		[AssociationId("a9d39829-f661-4f34-90f4-c6e6f8a81cda")]
		[RoleId("a40e4494-9fcf-42bf-b5f9-5977dc3d3dd7")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("SalesRepNames")]
		public RelationType SalesRepName;

		#region Allors
		[Id("9ebbb60b-b198-404b-b16b-1f6848f07c65")]
		[AssociationId("ce0e6e38-d0a5-4363-9184-8a14ee71c4e9")]
		[RoleId("827927b3-8d15-473a-8cad-b61b97e3322a")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("a9a0868b-e96d-4da4-9fd8-4cc19fdf4bc1")]
		[AssociationId("ab409a8b-dca5-446a-ad27-d255b946555c")]
		[RoleId("3af187a4-a5ce-45e5-9e47-00e66d2e6791")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Parties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Party;

		#region Allors
		[Id("cab7ab56-2068-4090-bdae-b4e42a68ec36")]
		[AssociationId("e5f5eefd-0276-450b-815e-4bde07fee1d6")]
		[RoleId("99d895a8-12c9-468c-9081-f600f64a9117")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("d2c46b6c-5aef-4172-be33-1fe4ea7fdce0")]
		[AssociationId("f9917c36-de1e-4a42-ae37-6883372873c9")]
		[RoleId("f298e963-b2b1-4cc2-b7aa-2d07fb4e1662")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Months")]
		public RelationType Month;



		public static SalesRepPartyRevenueClass Instance {get; internal set;}

		internal SalesRepPartyRevenueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}