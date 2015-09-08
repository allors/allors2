namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("52ee223f-14e7-46e7-8e24-c6fdf19fa5d1")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("CostOfGoodsSoldMethods")]
	public partial class CostOfGoodsSoldMethodClass : Class
	{

		public static CostOfGoodsSoldMethodClass Instance {get; internal set;}

		internal CostOfGoodsSoldMethodClass() : base(MetaPopulation.Instance)
        {
        }
	}
}