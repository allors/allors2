namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("cabc3b20-0456-47d9-a030-6df1d1f8ea9e")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("CostCenterSplitMethods")]
	public partial class CostCenterSplitMethodClass : Class
	{

		public static CostCenterSplitMethodClass Instance {get; internal set;}

		internal CostCenterSplitMethodClass() : base(MetaPopulation.Instance)
        {
        }
	}
}