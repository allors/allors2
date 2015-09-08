namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("18cd7011-e0ed-4f45-a6a8-c28fbf80d95a")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("PurchaseInvoiceTypes")]
	public partial class PurchaseInvoiceTypeClass : Class
	{

		public static PurchaseInvoiceTypeClass Instance {get; internal set;}

		internal PurchaseInvoiceTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}