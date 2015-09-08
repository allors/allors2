namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5bdc88b6-c45f-4835-aa50-26405f1314e3")]
	#endregion
	[Inherit(typeof(ExternalAccountingTransactionInterface))]

	[Plural("CreditLines")]
	public partial class CreditLineClass : Class
	{

		public static CreditLineClass Instance {get; internal set;}

		internal CreditLineClass() : base(MetaPopulation.Instance)
        {
        }
	}
}