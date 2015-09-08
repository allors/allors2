namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("e7625d17-2485-4894-ba1a-c565b8c6052c")]
	#endregion
	[Inherit(typeof(OrderAdjustmentInterface))]

	[Plural("ShippingAndHandlingCharges")]
	public partial class ShippingAndHandlingChargeClass : Class
	{

		public static ShippingAndHandlingChargeClass Instance {get; internal set;}

		internal ShippingAndHandlingChargeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}