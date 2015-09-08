namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("622a0738-338e-454e-a8ca-4a8fa3e9d9a4")]
	#endregion
	[Inherit(typeof(EngagementItemInterface))]

	[Plural("StandardServiceOrderItems")]
	public partial class StandardServiceOrderItemClass : Class
	{

		public static StandardServiceOrderItemClass Instance {get; internal set;}

		internal StandardServiceOrderItemClass() : base(MetaPopulation.Instance)
        {
        }
	}
}