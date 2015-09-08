namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("62693ee8-1fd3-4b2b-85ce-8d88df3bba0c")]
	#endregion
	[Inherit(typeof(GeographicBoundaryCompositeInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Regions")]
	public partial class RegionClass : Class
	{
		#region Allors
		[Id("2b0f6297-9056-4c51-a898-e5bf09e67941")]
		[AssociationId("9e062953-c2a0-44da-b6bf-5669b11fe4ab")]
		[RoleId("e3e9d99b-7780-4528-91dd-d75298bf2437")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;



		public static RegionClass Instance {get; internal set;}

		internal RegionClass() : base(MetaPopulation.Instance)
        {
        }
	}
}