namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f2d5bb8b-b50f-45e5-accb-c752a4445ad2")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("CustomerShipmentObjectStates")]
	public partial class CustomerShipmentObjectStateClass : Class
	{

		public static CustomerShipmentObjectStateClass Instance {get; internal set;}

		internal CustomerShipmentObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}