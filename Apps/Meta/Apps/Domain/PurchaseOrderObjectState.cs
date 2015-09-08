namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("45e4f0da-9a6b-4077-bcc4-d49d9ec4cc97")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("PurchaseOrderObjectStates")]
	public partial class PurchaseOrderObjectStateClass : Class
	{

		public static PurchaseOrderObjectStateClass Instance {get; internal set;}

		internal PurchaseOrderObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}