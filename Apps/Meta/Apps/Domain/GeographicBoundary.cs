namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3453c2e1-77a4-4fe8-b663-02bac689883a")]
	#endregion
	[Plural("GeographicBoundaries")]
	[Inherit(typeof(GeoLocatableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class GeographicBoundaryInterface: Interface
	{
		#region Allors
		[Id("28e43fe9-cdf1-4671-af95-ead40ecbef15")]
		[AssociationId("97f83f4c-d7ea-4928-b0a2-7e001a66b7d2")]
		[RoleId("940ce144-a48d-4128-b110-ffcc4d578295")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(10)]
		[Plural("Abbreviations")]
		public RelationType Abbreviation;



		public static GeographicBoundaryInterface Instance {get; internal set;}

		internal GeographicBoundaryInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}