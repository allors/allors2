namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("fdbea721-61f8-4e75-b1dd-e3880636ee78")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]
	[Inherit(typeof(ProductFeatureInterface))]

	[Plural("ServiceFeatures")]
	public partial class ServiceFeatureClass : Class
	{

		public static ServiceFeatureClass Instance {get; internal set;}

		internal ServiceFeatureClass() : base(MetaPopulation.Instance)
        {
        }
	}
}