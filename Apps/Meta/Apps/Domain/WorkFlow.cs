namespace Allors.Meta
{
	#region Allors
	[Id("b2a169ce-4e2a-48fc-aa39-dfc783ecb401")]
	#endregion
	[Inherit(typeof(WorkEffortInterface))]
	public partial class WorkFlowClass : Class
	{
		public static WorkFlowClass Instance {get; internal set;}

		internal WorkFlowClass() : base(MetaPopulation.Instance)
        {
        }
	}
}