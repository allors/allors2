namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("db24e487-daf7-4625-9073-8fd083f653dc")]
	#endregion
	[Inherit(typeof(RequirementInterface))]

	[Plural("CustomerRequirements")]
	public partial class CustomerRequirementClass : Class
	{

		public static CustomerRequirementClass Instance {get; internal set;}

		internal CustomerRequirementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}