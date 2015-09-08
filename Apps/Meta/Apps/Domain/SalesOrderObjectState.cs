namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("8c993e3f-59a0-42f0-a0ef-d49f9beb0af6")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("SalesOrderObjectStates")]
	public partial class SalesOrderObjectStateClass : Class
	{

		public static SalesOrderObjectStateClass Instance {get; internal set;}

		internal SalesOrderObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}