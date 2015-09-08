namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("616a603d-d441-4408-8c43-179a1502dc64")]
	#endregion
	[Inherit(typeof(FacilityInterface))]

	[Plural("Plants")]
	public partial class PlantClass : Class
	{

		public static PlantClass Instance {get; internal set;}

		internal PlantClass() : base(MetaPopulation.Instance)
        {
        }
	}
}