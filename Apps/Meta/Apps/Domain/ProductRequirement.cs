namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("00cba2fb-feb8-4566-8898-3bde8820211f")]
	#endregion
	[Inherit(typeof(RequirementInterface))]

	[Plural("ProductRequirements")]
	public partial class ProductRequirementClass : Class
	{
		#region Allors
		[Id("48ce0470-5738-4d9b-ab23-ea244e90091d")]
		[AssociationId("af379058-8ac3-4d0e-8eb4-715fcdda5e44")]
		[RoleId("9237556e-d3c2-4404-a39c-11660471d23d")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("Products")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Product;

		#region Allors
		[Id("a72274b6-2767-4cb9-8f3d-dc1e367c6f1b")]
		[AssociationId("e991712f-bbed-4cb9-98ef-e7ff2506fc11")]
		[RoleId("57ed8d56-0c40-47a8-9e49-be7a35294800")]
		#endregion
		[Indexed]
		[Type(typeof(DesiredProductFeatureClass))]
		[Plural("DesiredProductFeatures")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType DesiredProductFeature;



		public static ProductRequirementClass Instance {get; internal set;}

		internal ProductRequirementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}