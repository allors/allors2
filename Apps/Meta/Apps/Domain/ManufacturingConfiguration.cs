namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("b6c168d6-3d5c-4f5f-b6c6-d348600f1483")]
	#endregion
	[Inherit(typeof(InventoryItemConfigurationInterface))]

	[Plural("ManufacturingConfigurations")]
	public partial class ManufacturingConfigurationClass : Class
	{

		public static ManufacturingConfigurationClass Instance {get; internal set;}

		internal ManufacturingConfigurationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}