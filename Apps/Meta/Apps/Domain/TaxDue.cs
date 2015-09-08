namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("57b74174-1418-4307-96f7-e579638d7dd9")]
	#endregion
	[Inherit(typeof(ExternalAccountingTransactionInterface))]

	[Plural("TaxDues")]
	public partial class TaxDueClass : Class
	{

		public static TaxDueClass Instance {get; internal set;}

		internal TaxDueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}