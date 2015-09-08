namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("cb6a8e8a-04a6-437b-b952-f502cca2a2db")]
	#endregion
	[Inherit(typeof(EstimatedProductCostInterface))]

	[Plural("EstimatedMaterialCosts")]
	public partial class EstimatedMaterialCostClass : Class
	{

		public static EstimatedMaterialCostClass Instance {get; internal set;}

		internal EstimatedMaterialCostClass() : base(MetaPopulation.Instance)
        {
        }
	}
}