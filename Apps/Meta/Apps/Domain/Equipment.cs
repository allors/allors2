namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("da852ff9-0c87-4fa6-a93a-90d97d28029c")]
	#endregion
	[Inherit(typeof(FixedAssetInterface))]

	[Plural("Equipments")]
	public partial class EquipmentClass : Class
	{

		public static EquipmentClass Instance {get; internal set;}

		internal EquipmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}