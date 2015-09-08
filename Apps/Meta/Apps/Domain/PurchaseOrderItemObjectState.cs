namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ad76acee-eccc-42ce-9897-8c3f0252caf4")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("PurchaseOrderItemObjectStates")]
	public partial class PurchaseOrderItemObjectStateClass : Class
	{

		public static PurchaseOrderItemObjectStateClass Instance {get; internal set;}

		internal PurchaseOrderItemObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}