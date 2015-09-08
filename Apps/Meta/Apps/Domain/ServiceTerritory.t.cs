namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("987f8328-2bfa-47cd-9521-8b7bda78f90a")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(GeographicBoundaryCompositeInterface))]

	[Plural("ServiceTerritories")]
	public partial class ServiceTerritoryClass : Class
	{
		#region Allors
		[Id("a268313d-db1e-44e1-9fb1-7135d1157083")]
		[AssociationId("348c497e-7907-4409-b7b1-d77ebfd46258")]
		[RoleId("a23c1a3d-2a76-46b3-a26c-d5c5a66ebe0a")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;



		public static ServiceTerritoryClass Instance {get; internal set;}

		internal ServiceTerritoryClass() : base(MetaPopulation.Instance)
        {
        }
	}
}