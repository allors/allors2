namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("0b476761-ad10-4e00-88bb-0e44b4574990")]
	#endregion
	[Inherit(typeof(FixedAssetInterface))]

	[Plural("Vehicles")]
	public partial class VehicleClass : Class
	{

		public static VehicleClass Instance {get; internal set;}

		internal VehicleClass() : base(MetaPopulation.Instance)
        {
        }
	}
}