namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2e245d61-7739-4dfe-b108-7c9f0f4aed17")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("VatRateUsages")]
	public partial class VatRateUsageClass : Class
	{

		public static VatRateUsageClass Instance {get; internal set;}

		internal VatRateUsageClass() : base(MetaPopulation.Instance)
        {
        }
	}
}