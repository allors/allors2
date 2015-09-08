namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c37f7876-51af-4748-b083-4a6e42e99597")]
	#endregion
	[Inherit(typeof(CityBoundInterface))]
	[Inherit(typeof(GeographicBoundaryInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CountryBoundInterface))]

	[Plural("States")]
	public partial class StateClass : Class
	{
		#region Allors
		[Id("35ee6ba1-e75f-43f4-b33e-593748b5e359")]
		[AssociationId("040f516a-f173-44ba-b12c-a768e3216aec")]
		[RoleId("250129ac-caf9-486a-ae89-f47634738376")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;



		public static StateClass Instance {get; internal set;}

		internal StateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}