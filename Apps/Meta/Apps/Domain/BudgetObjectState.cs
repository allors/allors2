namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f8ae512e-bca5-498b-860b-11c06ab04d72")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("BudgetObjectStates")]
	public partial class BudgetObjectStateClass : Class
	{

		public static BudgetObjectStateClass Instance {get; internal set;}

		internal BudgetObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}