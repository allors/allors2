namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("339a58af-4939-4eee-8028-0fd18119ec34")]
	#endregion
	[Inherit(typeof(WorkEffortInterface))]

	[Plural("Activities")]
	public partial class ActivityClass : Class
	{

		public static ActivityClass Instance {get; internal set;}

		internal ActivityClass() : base(MetaPopulation.Instance)
        {
        }
	}
}