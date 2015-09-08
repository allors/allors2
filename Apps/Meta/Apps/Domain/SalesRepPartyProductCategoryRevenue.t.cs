namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("01fd14a1-c852-42c9-8d16-3243ff655b8f")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(DeletableInterface))]

	[Plural("SalesRepPartyProductCategoryRevenues")]
	public partial class SalesRepPartyProductCategoryRevenueClass : Class
	{
		#region Allors
		[Id("192f7c27-fd25-45a2-8de2-4101b7ce42f9")]
		[AssociationId("8e411184-ed58-4bb2-bb06-771cc93b8f53")]
		[RoleId("06ddf58f-379e-4c8e-9679-c6677fe124e8")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Years")]
		public RelationType Year;

		#region Allors
		[Id("25bd83fe-0ff9-4241-b3e6-d0f1d06ab4be")]
		[AssociationId("fa5dfed6-df70-4316-a2ab-f9b3499dd987")]
		[RoleId("fb2b7da7-d226-40fd-a638-d7cb906afa14")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("SalesReps")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesRep;

		#region Allors
		[Id("2a3346f4-ea7f-4be0-a69e-fcfb88cba88a")]
		[AssociationId("f6b88a6a-a906-4002-a2bf-d9007750bc0d")]
		[RoleId("3e6ee104-2371-48be-aac1-f4f7d752c2e8")]
		#endregion
		[Indexed]
		[Type(typeof(ProductCategoryClass))]
		[Plural("ProductCategories")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductCategory;

		#region Allors
		[Id("3ec97b92-b4d9-456a-8a38-6f129fa8f963")]
		[AssociationId("67b70d49-9dad-4d20-9388-1cd98e2af413")]
		[RoleId("589a1da1-1add-441b-8cfc-55f0fa0b2242")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Months")]
		public RelationType Month;

		#region Allors
		[Id("46deffbe-e2b7-46d8-9669-27ade84de02c")]
		[AssociationId("58fd5226-a4f7-4971-a3e2-54a5b41c2eb7")]
		[RoleId("ea0e2bc7-8df9-4380-abd9-588e6805e84f")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Parties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Party;

		#region Allors
		[Id("689ea9aa-082f-43e5-be65-554df3b0f8dc")]
		[AssociationId("f3a151f6-3d05-4401-9925-401aff21d437")]
		[RoleId("93a71bd3-c6e1-42a8-9425-9704698c0f1c")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;

		#region Allors
		[Id("77715cdd-a0c6-47da-8072-6a23be984cad")]
		[AssociationId("fb61b669-ebf2-46de-9178-a5ded1d03930")]
		[RoleId("c00dfdba-f353-4e2f-a875-70d16e96daaf")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("ee15e022-a420-4bbf-84f4-75b380cea7bb")]
		[AssociationId("fece3c35-cbf8-495c-820b-bad9f6dd02eb")]
		[RoleId("eac4e8b1-fd89-4d16-86bd-3be5cb1178e2")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("f378a0a3-0ffc-4761-8ccc-d906b257c2f2")]
		[AssociationId("1f9500ce-38dc-4b90-a638-c2a457978cc4")]
		[RoleId("a7b8f672-2e7e-4413-a3ff-a3a9ac7b3452")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("SalesRepNames")]
		public RelationType SalesRepName;



		public static SalesRepPartyProductCategoryRevenueClass Instance {get; internal set;}

		internal SalesRepPartyProductCategoryRevenueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}