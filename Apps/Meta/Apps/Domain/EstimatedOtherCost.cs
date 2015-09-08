namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9b637b39-f61a-4985-bb1b-876ed769f448")]
	#endregion
	[Inherit(typeof(EstimatedProductCostInterface))]

	[Plural("EstimatedOtherCosts")]
	public partial class EstimatedOtherCostClass : Class
	{

		public static EstimatedOtherCostClass Instance {get; internal set;}

		internal EstimatedOtherCostClass() : base(MetaPopulation.Instance)
        {
        }
	}
}