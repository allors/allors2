namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d3c5a482-e17a-4e37-84eb-55a035e80f2f")]
	#endregion
	[Plural("ProductFeatures")]
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class ProductFeatureInterface: Interface
	{
		#region Allors
		[Id("4a8c511a-8146-4d6d-bc35-d8d6b8f1786d")]
		[AssociationId("31ff19c6-9916-4f4c-8d67-30649d3a07ea")]
		[RoleId("8e35afcf-c606-4099-ba83-87c6b6fc37e1")]
		#endregion
		[Indexed]
		[Type(typeof(EstimatedProductCostInterface))]
		[Plural("EstimatedProductCosts")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType EstimatedProductCost;

		#region Allors
		[Id("8ac8ab84-f78f-4232-a4f7-390f55019663")]
		[AssociationId("6c65b1ad-91e4-4aae-a78e-d6d142bb98e5")]
		[RoleId("3115f401-fc58-48da-a769-afecafbeb729")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PriceComponentInterface))]
		[Plural("BasePrices")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType BasePrice;

		#region Allors
		[Id("b75855b8-c921-4d60-8ea0-650a0f574f7f")]
		[AssociationId("dd0b49c7-56f4-43ac-a470-ddc191d1c279")]
		[RoleId("64bfaf6d-aaac-42ec-ac37-22a1d674611f")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("badde93b-4691-435e-9ba3-e52435e9f574")]
		[AssociationId("c49ca161-d15b-4ba9-b42f-06144e8ca9f6")]
		[RoleId("a3a58a70-3bab-4c8a-ac47-8beaca3b46d2")]
		#endregion
		[Indexed]
		[Type(typeof(ProductFeatureInterface))]
		[Plural("DependentFeatures")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType DependentFeature;

		#region Allors
		[Id("ce228118-f5b2-49bb-b0cd-7e0ca8e10315")]
		[AssociationId("d90b3906-a48d-473c-85a7-baae359d58a7")]
		[RoleId("0305f707-683e-4fcd-94a0-7c0b3a2b27e4")]
		#endregion
		[Indexed]
		[Type(typeof(ProductFeatureInterface))]
		[Plural("IncompatibleFeatures")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType IncompatibleFeature;

		#region Allors
		[Id("efe16e22-edfb-40b1-83c0-110f874c285a")]
		[AssociationId("3c78c391-cf55-40ce-9d11-a0600787ed82")]
		[RoleId("6c3e8238-f1dd-4461-a73b-0927cd26db29")]
		#endregion
		[Indexed]
		[Type(typeof(VatRateClass))]
		[Plural("VatRates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType VatRate;



		public static ProductFeatureInterface Instance {get; internal set;}

		internal ProductFeatureInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}