namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("fb3dd618-eeb5-4ef6-87ca-abfe91dc603f")]
	#endregion
	[Inherit(typeof(OrderAdjustmentInterface))]

	[Plural("Fees")]
	public partial class FeeClass : Class
	{

		public static FeeClass Instance {get; internal set;}

		internal FeeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}