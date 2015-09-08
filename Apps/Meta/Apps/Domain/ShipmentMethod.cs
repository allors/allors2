namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3a3e6acf-48f4-4a33-848c-0c77cb18693a")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("ShipmentMethods")]
	public partial class ShipmentMethodClass : Class
	{

		public static ShipmentMethodClass Instance {get; internal set;}

		internal ShipmentMethodClass() : base(MetaPopulation.Instance)
        {
        }
	}
}