namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("93960be2-f676-4e7f-9efb-f99c92303059")]
	#endregion
	[Plural("GeoLocatables")]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]

  	public partial class GeoLocatableInterface: Interface
	{
		#region Allors
		[Id("b0aba482-63eb-4482-a232-3863f089f4d9")]
		[AssociationId("340069b9-a00b-420d-8f8d-52e627729db3")]
		[RoleId("bab847eb-ff35-49dd-ae44-ccf4e1ee6743")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(8)]
		[Scale(6)]
		[Plural("Latitudes")]
		public RelationType Latitude;

		#region Allors
		[Id("c51b6be6-5678-4664-b2c9-874cc46deb2e")]
		[AssociationId("0d7f48c7-84e5-4ea6-8242-22e4cb35e8cd")]
		[RoleId("66d37e99-b7aa-42c7-8b03-0d4bee43a1e7")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(9)]
		[Scale(6)]
		[Plural("Longitudes")]
		public RelationType Longitude;



		public static GeoLocatableInterface Instance {get; internal set;}

		internal GeoLocatableInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}