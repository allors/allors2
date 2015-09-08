namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("21f09e4c-7b3f-4152-8822-8c485011759c")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("SalesOrderItemObjectStates")]
	public partial class SalesOrderItemObjectStateClass : Class
	{

		public static SalesOrderItemObjectStateClass Instance {get; internal set;}

		internal SalesOrderItemObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}