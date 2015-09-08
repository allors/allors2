namespace Allors.Meta
{

	#region Allors
	[Id("383589fb-f410-4d22-ade6-aa5126fdef18")]
	#endregion
	[Inherit(typeof(PeriodInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CommentableInterface))]
  	public partial class PriceComponentInterface: Interface
	{
		#region Allors
		[Id("18cda5a7-6720-4133-a71b-ce23e9ebc1bb")]
		[AssociationId("bfdd0a69-e69f-49e0-b756-e6b3307a2bd2")]
		[RoleId("5934c3f4-3f64-4f5a-b455-70590ae02328")]
		#endregion
		[Indexed]
		[Type(typeof(GeographicBoundaryInterface))]
		[Plural("GeographicBoundaries")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType GeographicBoundary;

		#region Allors
		[Id("1cddef96-0be9-487a-bdb3-df024656214a")]
		[AssociationId("deb9575e-c8a2-48e3-9930-33a5c2afad2d")]
		[RoleId("3189b81e-100e-45cf-b308-d1f5e34a3c16")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Rates")]
		public RelationType Rate;

		#region Allors
		[Id("1da8c258-fb73-4cce-88f3-3c39d21a7996")]
		[AssociationId("191fa580-ad8b-4151-90e1-58c4c512ab68")]
		[RoleId("c3519e5a-981d-453f-ab7b-edf9f29556cc")]
		#endregion
		[Indexed]
		[Type(typeof(RevenueValueBreakClass))]
		[Plural("RevenueValueBreaks")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType RevenueValueBreak;

		#region Allors
		[Id("3230c33b-42ac-4eb4-b0c9-9791cc5604d7")]
		[AssociationId("0d248d95-468e-4a19-9e84-45065dfc0006")]
		[RoleId("8e87149c-0c44-4e02-b784-eeef963a4333")]
		#endregion
		[Indexed]
		[Type(typeof(PartyClassificationInterface))]
		[Plural("PartyClassifications")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PartyClassification;

		#region Allors
		[Id("50d6ddf3-47d9-4de1-954e-a4fae881edd0")]
		[AssociationId("ebe67168-92de-45f8-96b0-b373c47e7ff3")]
		[RoleId("24ff9932-4fb4-4eab-b490-c397602f5820")]
		#endregion
		[Indexed]
		[Type(typeof(OrderQuantityBreakClass))]
		[Plural("OrderQuantityBreaks")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType OrderQuantityBreak;

		#region Allors
		[Id("52883992-8e1b-472a-9cc7-67d4824a2cd4")]
		[AssociationId("5a14b240-2eb3-4051-b9cb-8eefb0fd1722")]
		[RoleId("ac8e7099-f0bd-492f-bbd9-5890060c56eb")]
		#endregion
		[Indexed]
		[Type(typeof(PackageQuantityBreakClass))]
		[Plural("PackageQuantityBreaks")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PackageQuantityBreak;

		#region Allors
		[Id("55c43896-ba79-4752-8fd4-7fd8501d64b6")]
		[AssociationId("38475cd5-ee15-4507-a810-f77ef0fb5cab")]
		[RoleId("9e905572-d1bf-435b-8238-3a00da09f243")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("Products")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Product;

		#region Allors
		[Id("5b91ebce-6ebe-4d5f-a8bc-22cd7e7d688a")]
		[AssociationId("de72aa4b-17f1-4037-99e5-95f30c1f8f90")]
		[RoleId("78d118ed-dcb7-4b8a-84ae-d1a9fdf11643")]
		#endregion
		[Indexed]
		[Type(typeof(RevenueQuantityBreakClass))]
		[Plural("RevenueQuantityBreaks")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType RevenueQuantityBreak;

		#region Allors
		[Id("5cab507a-bf96-40c9-89cd-86d59ad07c33")]
		[AssociationId("bbeb2003-75ee-415e-a841-bab8af7153bc")]
		[RoleId("6f1ba75a-c271-45d7-bebb-601889291784")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("SpecifiedFors")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SpecifiedFor;

		#region Allors
		[Id("6c0744ee-b730-490d-bb0c-b6be95211371")]
		[AssociationId("1eb6f686-586e-47cd-b23a-bcbc75430e7c")]
		[RoleId("54726272-92ba-494c-9c08-d19e3248e2f3")]
		#endregion
		[Indexed]
		[Type(typeof(ProductFeatureInterface))]
		[Plural("ProductFeatures")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductFeature;

		#region Allors
		[Id("80379d5a-1831-4eed-abd3-a9574e3edd1d")]
		[AssociationId("d1a93eac-7143-43fc-897e-be6b198e69ec")]
		[RoleId("cf38c778-74c9-4da2-8619-f3c87cee5f19")]
		#endregion
		[Indexed]
		[Type(typeof(AgreementPricingProgramClass))]
		[Plural("AgreementPricingPrograms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType AgreementPricingProgram;

		#region Allors
		[Id("8c32a2ca-a0c7-4c92-9b65-91d8b5ccee94")]
		[AssociationId("70ef8bda-aefc-458f-8f6e-5c0088bbad6b")]
		[RoleId("090325e4-2ada-4978-afcd-04db2793c02a")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("c2b2b046-9e62-4065-8f2d-10624f7565aa")]
		[AssociationId("6e44c9e4-cf29-4303-8217-e01b5ce9dcb7")]
		[RoleId("6fbee15a-bcba-46b6-9ea0-ff8c9a3256c4")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("cb552b8b-f251-4c57-8cc7-8cc299631022")]
		[AssociationId("087b6d0d-399d-408e-af06-84bd70a0eff6")]
		[RoleId("e968a3a1-7394-465a-829a-f8f33ea3fc4c")]
		#endregion
		[Indexed]
		[Type(typeof(OrderKindClass))]
		[Plural("OrderKinds")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType OrderKind;

		#region Allors
		[Id("dc1cf3af-2f22-43e6-863d-346e91aa2240")]
		[AssociationId("7a5ec695-a56a-40dc-8898-667276078d3d")]
		[RoleId("6dfffe42-670a-4076-b240-9e8ffb43f243")]
		#endregion
		[Indexed]
		[Type(typeof(OrderValueClass))]
		[Plural("OrderValues")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType OrderValue;

		#region Allors
		[Id("dc5ad82b-c18d-4971-9689-e81475ed6a54")]
		[AssociationId("40276e1c-d9eb-4ef8-b828-387f67f1a337")]
		[RoleId("9698ebe8-509d-493c-9d73-77028f60f2c7")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Prices")]
		public RelationType Price;

		#region Allors
		[Id("de59dbb7-996a-45be-ae2a-a7b5a0ff3d94")]
		[AssociationId("47ad080f-0e8f-4529-8470-ea4cd26d424d")]
		[RoleId("62d6d8df-de31-4995-a3ba-af9e5c06215c")]
		#endregion
		[Indexed]
		[Type(typeof(ProductCategoryClass))]
		[Plural("ProductCategories")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductCategory;

		#region Allors
		[Id("f8976686-bd76-4435-8ed8-f5e2490eeb94")]
		[AssociationId("38d58e93-0b17-4170-8f41-dbfb97bb11da")]
		[RoleId("8f9625b2-d7fd-482d-8880-386dbeb74773")]
		#endregion
		[Indexed]
		[Type(typeof(SalesChannelClass))]
		[Plural("SalesChannels")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesChannel;



		public static PriceComponentInterface Instance {get; internal set;}

		internal PriceComponentInterface() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
            this.SpecifiedFor.RoleType.IsRequired = true;
        }
    }
}