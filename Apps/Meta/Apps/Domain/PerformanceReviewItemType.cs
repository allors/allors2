namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("e80a9fe3-027b-4abd-acfb-99e3db9da70c")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("PerformanceReviewItemTypes")]
	public partial class PerformanceReviewItemTypeClass : Class
	{

		public static PerformanceReviewItemTypeClass Instance {get; internal set;}

		internal PerformanceReviewItemTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}