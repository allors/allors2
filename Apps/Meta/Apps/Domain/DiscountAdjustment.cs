namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("0346a1e2-03c7-4f1e-94ae-35fdf64143a9")]
	#endregion
	[Inherit(typeof(OrderAdjustmentInterface))]

	[Plural("DiscountAdjustments")]
	public partial class DiscountAdjustmentClass : Class
	{

		public static DiscountAdjustmentClass Instance {get; internal set;}

		internal DiscountAdjustmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}