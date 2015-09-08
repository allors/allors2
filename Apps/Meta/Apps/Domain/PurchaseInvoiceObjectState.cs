namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6c485526-bf9e-42e0-b47e-84552a72589a")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("PurchaseInvoiceObjectStates")]
	public partial class PurchaseInvoiceObjectStateClass : Class
	{

		public static PurchaseInvoiceObjectStateClass Instance {get; internal set;}

		internal PurchaseInvoiceObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}