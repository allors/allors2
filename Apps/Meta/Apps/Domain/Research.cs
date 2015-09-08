namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d1d8f99e-430d-4104-a2db-777a0f6292e3")]
	#endregion
	[Inherit(typeof(WorkEffortInterface))]

	[Plural("Researches")]
	public partial class ResearchClass : Class
	{

		public static ResearchClass Instance {get; internal set;}

		internal ResearchClass() : base(MetaPopulation.Instance)
        {
        }
	}
}