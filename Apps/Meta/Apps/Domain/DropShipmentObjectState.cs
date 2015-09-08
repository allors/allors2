namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("89d2037a-4bc2-4929-b333-5358ac4a14e5")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("DropShipmentObjectStates")]
	public partial class DropShipmentObjectStateClass : Class
	{

		public static DropShipmentObjectStateClass Instance {get; internal set;}

		internal DropShipmentObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}