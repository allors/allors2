namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("62ea5285-b9d8-4a41-9c14-79c712fd3bf4")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(GeographicBoundaryCompositeInterface))]

	[Plural("SalesTerritories")]
	public partial class SalesTerritoryClass : Class
	{
		#region Allors
		[Id("d904af24-887c-40b0-a5d0-7dce40ec4db3")]
		[AssociationId("0e172e31-8896-42c9-b1f2-2ff8bc1065c1")]
		[RoleId("bcf4d240-258b-43f3-ac94-4314685019ea")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;



		public static SalesTerritoryClass Instance {get; internal set;}

		internal SalesTerritoryClass() : base(MetaPopulation.Instance)
        {
        }
	}
}