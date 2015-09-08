namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a7d98869-b51e-45b4-9403-06094bb61fcf")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("PurchaseInvoiceItemObjectStates")]
	public partial class PurchaseInvoiceItemObjectStateClass : Class
	{

		public static PurchaseInvoiceItemObjectStateClass Instance {get; internal set;}

		internal PurchaseInvoiceItemObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}