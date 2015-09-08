namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("7fd1760c-ee1f-4d04-8a93-dfebc82757c1")]
	#endregion
	[Inherit(typeof(InternalAccountingTransactionInterface))]

	[Plural("Amortizations")]
	public partial class AmortizationClass : Class
	{

		public static AmortizationClass Instance {get; internal set;}

		internal AmortizationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}