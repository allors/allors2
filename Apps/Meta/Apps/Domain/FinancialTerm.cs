namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a73aa458-2293-4578-be67-ad32e36a4991")]
	#endregion
	[Inherit(typeof(AgreementTermInterface))]

	[Plural("FinancialTerms")]
	public partial class FinancialTermClass : Class
	{

		public static FinancialTermClass Instance {get; internal set;}

		internal FinancialTermClass() : base(MetaPopulation.Instance)
        {
        }
	}
}