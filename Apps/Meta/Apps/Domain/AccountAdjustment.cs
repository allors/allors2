namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4211ece6-a127-4359-9fa4-6537943a37a5")]
	#endregion
	[Inherit(typeof(FinancialAccountTransactionInterface))]

	[Plural("AccountAdjustments")]
	public partial class AccountAdjustmentClass : Class
	{

		public static AccountAdjustmentClass Instance {get; internal set;}

		internal AccountAdjustmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}