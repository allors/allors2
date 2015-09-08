namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("81c9eefa-9b8b-40c0-9f1e-e6ecc2fef119")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("SalesInvoiceTypes")]
	public partial class SalesInvoiceTypeClass : Class
	{

		public static SalesInvoiceTypeClass Instance {get; internal set;}

		internal SalesInvoiceTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}