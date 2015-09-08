namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f7d24734-88d3-47e7-b718-8815914c9ad4")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("WorkEffortObjectStates")]
	public partial class WorkEffortObjectStateClass : Class
	{

		public static WorkEffortObjectStateClass Instance {get; internal set;}

		internal WorkEffortObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}