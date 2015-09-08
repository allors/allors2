namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("364071a2-bcda-4bdc-b0f9-0e56d28604d6")]
	#endregion
	[Inherit(typeof(PartInterface))]

	[Plural("FinishedGoods")]
	public partial class FinishedGoodClass : Class
	{

		public static FinishedGoodClass Instance {get; internal set;}

		internal FinishedGoodClass() : base(MetaPopulation.Instance)
        {
        }
	}
}