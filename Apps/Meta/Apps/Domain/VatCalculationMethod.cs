namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3b73eea7-6455-4fe5-87c0-99c852f57e6b")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("VatCalculationMethods")]
	public partial class VatCalculationMethodClass : Class
	{

		public static VatCalculationMethodClass Instance {get; internal set;}

		internal VatCalculationMethodClass() : base(MetaPopulation.Instance)
        {
        }
	}
}