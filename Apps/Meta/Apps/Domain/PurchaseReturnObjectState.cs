namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("23162c0f-f5ec-45a5-a948-13a3355d99f2")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("PurchaseReturnObjectStates")]
	public partial class PurchaseReturnObjectStateClass : Class
	{

		public static PurchaseReturnObjectStateClass Instance {get; internal set;}

		internal PurchaseReturnObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}