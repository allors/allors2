namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("273e69b7-6cda-44d4-b1d6-605b32a6a70d")]
	#endregion
	[Inherit(typeof(ProductFeatureInterface))]
	[Inherit(typeof(EnumerationInterface))]

	[Plural("Models")]
	public partial class ModelClass : Class
	{

		public static ModelClass Instance {get; internal set;}

		internal ModelClass() : base(MetaPopulation.Instance)
        {
        }
	}
}