namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2a84fcce-91f6-4d8b-9840-2ddd5f4b3dac")]
	#endregion
	[Inherit(typeof(EstimatedProductCostInterface))]

	[Plural("EstimatedLaborCosts")]
	public partial class EstimatedLaborCostClass : Class
	{

		public static EstimatedLaborCostClass Instance {get; internal set;}

		internal EstimatedLaborCostClass() : base(MetaPopulation.Instance)
        {
        }
	}
}