namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("bfdd33dc-5701-41ec-a768-f745155663d3")]
	#endregion
	[Plural("CityBounds")]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class CityBoundInterface: Interface
	{
		#region Allors
		[Id("7723a00d-8764-40e2-99a8-a790401689b5")]
		[AssociationId("bb222d51-4e32-4182-8c45-8ce6db2f2cea")]
		[RoleId("4aa9efbb-fc9d-44f3-b713-3b1493637467")]
		#endregion
		[Indexed]
		[Type(typeof(CityClass))]
		[Plural("Cities")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType City;



		public static CityBoundInterface Instance {get; internal set;}

		internal CityBoundInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}