namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("8bae9154-ec37-4139-b52c-6c3df860fb20")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]
	[Inherit(typeof(ProductFeatureInterface))]

	[Plural("Colours")]
	public partial class ColourClass : Class
	{

		public static ColourClass Instance {get; internal set;}

		internal ColourClass() : base(MetaPopulation.Instance)
        {
        }
	}
}