namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3b7ac95a-fdab-488d-b599-17ef9fcf33b0")]
	#endregion
	[Plural("GeographicBoundariesComposites")]
	[Inherit(typeof(GeographicBoundaryInterface))]

  	public partial class GeographicBoundaryCompositeInterface: Interface
	{
		#region Allors
		[Id("77d5f129-6096-45da-8b9f-39ef19276f1d")]
		[AssociationId("7484e00e-de39-4fbe-981a-aff3e693cf89")]
		[RoleId("03ef822a-e2d3-43ba-9051-2c663593fb31")]
		#endregion
		[Indexed]
		[Type(typeof(GeographicBoundaryInterface))]
		[Plural("Associations")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType Association;



		public static GeographicBoundaryCompositeInterface Instance {get; internal set;}

		internal GeographicBoundaryCompositeInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}