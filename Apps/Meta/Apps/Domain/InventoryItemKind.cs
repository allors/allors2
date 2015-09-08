namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("759f97a9-3105-49b4-81a0-c94c3700397c")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("InventoryItemKinds")]
	public partial class InventoryItemKindClass : Class
	{

		public static InventoryItemKindClass Instance {get; internal set;}

		internal InventoryItemKindClass() : base(MetaPopulation.Instance)
        {
        }
	}
}