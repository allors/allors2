namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5d4beea4-f480-460e-92ee-3e8d532ac7f9")]
	#endregion
	[Inherit(typeof(InventoryItemConfigurationInterface))]

	[Plural("ServiceConfigurations")]
	public partial class ServiceConfigurationClass : Class
	{

		public static ServiceConfigurationClass Instance {get; internal set;}

		internal ServiceConfigurationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}