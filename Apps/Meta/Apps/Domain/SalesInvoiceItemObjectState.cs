namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4babdd0c-52dd-4fb8-bbf5-120aa58eff50")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("SalesInvoiceItemObjectStates")]
	public partial class SalesInvoiceItemObjectStateClass : Class
	{

		public static SalesInvoiceItemObjectStateClass Instance {get; internal set;}

		internal SalesInvoiceItemObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}