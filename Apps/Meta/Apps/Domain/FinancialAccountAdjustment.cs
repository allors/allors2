namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("12ba6843-bae1-41d1-9ef2-c19d74b0a365")]
	#endregion
	[Inherit(typeof(FinancialAccountTransactionInterface))]

	[Plural("FinancialAccountAdjustments")]
	public partial class FinancialAccountAdjustmentClass : Class
	{

		public static FinancialAccountAdjustmentClass Instance {get; internal set;}

		internal FinancialAccountAdjustmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}