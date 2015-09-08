namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a4092f59-2baf-4041-83e6-5d50c8338a5c")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("SalesInvoiceObjectStates")]
	public partial class SalesInvoiceObjectStateClass : Class
	{

		public static SalesInvoiceObjectStateClass Instance {get; internal set;}

		internal SalesInvoiceObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}