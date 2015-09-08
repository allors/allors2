namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("70468d86-b8a0-4aff-881e-fca2386f64da")]
	#endregion
	[Inherit(typeof(OrderAdjustmentInterface))]

	[Plural("SurchargeAdjustments")]
	public partial class SurchargeAdjustmentClass : Class
	{

		public static SurchargeAdjustmentClass Instance {get; internal set;}

		internal SurchargeAdjustmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}