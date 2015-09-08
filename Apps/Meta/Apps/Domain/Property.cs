namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("dc54aafb-f0f2-4f72-8a81-d5b2fc792b86")]
	#endregion
	[Inherit(typeof(FixedAssetInterface))]

	[Plural("Properties")]
	public partial class PropertyClass : Class
	{

		public static PropertyClass Instance {get; internal set;}

		internal PropertyClass() : base(MetaPopulation.Instance)
        {
        }
	}
}