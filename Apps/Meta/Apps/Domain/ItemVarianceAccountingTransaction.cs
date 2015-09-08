namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4af573b7-a87f-400c-97e4-80bda17376e0")]
	#endregion
	[Inherit(typeof(AccountingTransactionInterface))]

	[Plural("ItemVarianceAccountingTransactions")]
	public partial class ItemVarianceAccountingTransactionClass : Class
	{

		public static ItemVarianceAccountingTransactionClass Instance {get; internal set;}

		internal ItemVarianceAccountingTransactionClass() : base(MetaPopulation.Instance)
        {
        }
	}
}