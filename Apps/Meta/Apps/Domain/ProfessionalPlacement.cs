namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("b83205c5-261f-4d9d-9789-55966ae8d61b")]
	#endregion
	[Inherit(typeof(EngagementItemInterface))]

	[Plural("ProfessionalPlacements")]
	public partial class ProfessionalPlacementClass : Class
	{

		public static ProfessionalPlacementClass Instance {get; internal set;}

		internal ProfessionalPlacementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}