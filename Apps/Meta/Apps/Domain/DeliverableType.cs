namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("b1208ddd-9c28-46c3-8d05-2ea1ee29945d")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("DeliverableTypes")]
	public partial class DeliverableTypeClass : Class
	{

		public static DeliverableTypeClass Instance {get; internal set;}

		internal DeliverableTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}