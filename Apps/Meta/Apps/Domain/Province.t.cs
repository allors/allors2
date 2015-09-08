namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ada24931-020a-48e8-8f8d-18ddb8f46cf7")]
	#endregion
	[Inherit(typeof(CityBoundInterface))]
	[Inherit(typeof(GeographicBoundaryInterface))]
	[Inherit(typeof(CountryBoundInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Provinces")]
	public partial class ProvinceClass : Class
	{
		#region Allors
		[Id("e04bddba-a014-4793-8787-d9cb83ba7d60")]
		[AssociationId("da01d60d-4b8a-4472-9a6a-c21af0963a0b")]
		[RoleId("211c25b7-ecc2-4bdb-a73c-f37090eb165c")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;



		public static ProvinceClass Instance {get; internal set;}

		internal ProvinceClass() : base(MetaPopulation.Instance)
        {
        }
	}
}