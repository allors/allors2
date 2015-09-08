namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("14f7d6d1-ade6-4a3a-a3ef-f614a375180e")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("PurchaseInvoiceItemTypes")]
	public partial class PurchaseInvoiceItemTypeClass : Class
	{

		public static PurchaseInvoiceItemTypeClass Instance {get; internal set;}

		internal PurchaseInvoiceItemTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}