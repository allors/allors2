namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("7118e029-a8b3-415b-b9e9-d48ba4ea2823")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CityBoundInterface))]
	[Inherit(typeof(GeographicBoundaryInterface))]
	[Inherit(typeof(CountryBoundInterface))]

	[Plural("Territories")]
	public partial class TerritoryClass : Class
	{
		#region Allors
		[Id("9e3780d3-887f-458c-937c-379b22205e2f")]
		[AssociationId("241f1107-e802-4c2f-b0e5-80f42b3f916b")]
		[RoleId("3b19bc32-8d8e-404c-80c5-9671408a630e")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;



		public static TerritoryClass Instance {get; internal set;}

		internal TerritoryClass() : base(MetaPopulation.Instance)
        {
        }
	}
}