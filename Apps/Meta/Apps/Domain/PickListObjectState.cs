namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f7108ec0-3203-4e62-b323-2e3a6a527d66")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("PickListObjectStates")]
	public partial class PickListObjectStateClass : Class
	{

		public static PickListObjectStateClass Instance {get; internal set;}

		internal PickListObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}