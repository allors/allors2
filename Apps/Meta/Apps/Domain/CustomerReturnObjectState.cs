namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("671951f1-78fd-4b05-ac15-eafb2a35a6f8")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("CustomerReturnObjectStates")]
	public partial class CustomerReturnObjectStateClass : Class
	{

		public static CustomerReturnObjectStateClass Instance {get; internal set;}

		internal CustomerReturnObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}