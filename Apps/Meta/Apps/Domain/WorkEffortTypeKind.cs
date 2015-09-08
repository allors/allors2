namespace Allors.Meta
{
	#region Allors
	[Id("8551adf6-5a97-41fe-aff8-6bec08b09d08")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]
	public partial class WorkEffortTypeKindClass : Class
	{
		public static WorkEffortTypeKindClass Instance {get; internal set;}

		internal WorkEffortTypeKindClass() : base(MetaPopulation.Instance)
        {
        }
	}
}