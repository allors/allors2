namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("98fc5441-2037-4134-b143-a9797af9d7f1")]
	#endregion
	[Inherit(typeof(ServiceInterface))]

	[Plural("DeliverableBasedServices")]
	public partial class DeliverableBasedServiceClass : Class
	{

		public static DeliverableBasedServiceClass Instance {get; internal set;}

		internal DeliverableBasedServiceClass() : base(MetaPopulation.Instance)
        {
        }
	}
}