namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("21840af7-e7e7-4e8d-a720-3ea7ee5d2bfd")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("PurchaseShipmentObjectStates")]
	public partial class PurchaseShipmentObjectStateClass : Class
	{

		public static PurchaseShipmentObjectStateClass Instance {get; internal set;}

		internal PurchaseShipmentObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}