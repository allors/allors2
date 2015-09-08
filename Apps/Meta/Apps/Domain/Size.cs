namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("45f5a73c-34d8-4452-8f22-7a744bd6650b")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]
	[Inherit(typeof(ProductFeatureInterface))]

	[Plural("Sizes")]
	public partial class SizeClass : Class
	{

		public static SizeClass Instance {get; internal set;}

		internal SizeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}