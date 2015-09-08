namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3277910f-c4ee-40b6-8028-21f879e8da04")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("AccountingTransactionTypes")]
	public partial class AccountingTransactionTypeClass : Class
	{

		public static AccountingTransactionTypeClass Instance {get; internal set;}

		internal AccountingTransactionTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}