namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c758f77e-e3b3-4517-831a-af1bf0e1dceb")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("VatRatePurchaseKinds")]
	public partial class VatRatePurchaseKindClass : Class
	{

		public static VatRatePurchaseKindClass Instance {get; internal set;}

		internal VatRatePurchaseKindClass() : base(MetaPopulation.Instance)
        {
        }
	}
}