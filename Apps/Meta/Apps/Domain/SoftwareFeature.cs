namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("34047b37-545d-420f-ae79-2e05123cd623")]
	#endregion
	[Inherit(typeof(ProductFeatureInterface))]
	[Inherit(typeof(EnumerationInterface))]

	[Plural("SoftwareFeatures")]
	public partial class SoftwareFeatureClass : Class
	{

		public static SoftwareFeatureClass Instance {get; internal set;}

		internal SoftwareFeatureClass() : base(MetaPopulation.Instance)
        {
        }
	}
}