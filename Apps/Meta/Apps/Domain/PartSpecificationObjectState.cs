namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("17b5b8ec-cb0e-4d81-b5e5-1a99a5afff2e")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("PartSpecificationObjectStates")]
	public partial class PartSpecificationObjectStateClass : Class
	{

		public static PartSpecificationObjectStateClass Instance {get; internal set;}

		internal PartSpecificationObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}