namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("26f60d84-0659-4874-9c00-d6f3db11f073")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("SalesInvoiceItemTypes")]
	public partial class SalesInvoiceItemTypeClass : Class
	{

		public static SalesInvoiceItemTypeClass Instance {get; internal set;}

		internal SalesInvoiceItemTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}