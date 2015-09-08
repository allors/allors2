namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("37b665a5-9f73-4002-b7d2-7ed6987fe09a")]
	#endregion
	[Inherit(typeof(PartSpecificationInterface))]

	[Plural("PerformanceSpecifications")]
	public partial class PerformanceSpecificationClass : Class
	{

		public static PerformanceSpecificationClass Instance {get; internal set;}

		internal PerformanceSpecificationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}