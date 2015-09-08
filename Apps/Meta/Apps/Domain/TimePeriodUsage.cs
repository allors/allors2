namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f7e69670-1824-44ea-b2cd-fdef02ef84a7")]
	#endregion
	[Inherit(typeof(DeploymentUsageInterface))]

	[Plural("TimePeriodUsages")]
	public partial class TimePeriodUsageClass : Class
	{

		public static TimePeriodUsageClass Instance {get; internal set;}

		internal TimePeriodUsageClass() : base(MetaPopulation.Instance)
        {
        }
	}
}