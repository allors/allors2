namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d14fa0d2-8743-4d3c-8109-2ab9161cb310")]
	#endregion
	[Inherit(typeof(ProductFeatureInterface))]
	[Inherit(typeof(EnumerationInterface))]

	[Plural("ProductQualities")]
	public partial class ProductQualityClass : Class
	{

		public static ProductQualityClass Instance {get; internal set;}

		internal ProductQualityClass() : base(MetaPopulation.Instance)
        {
        }
	}
}