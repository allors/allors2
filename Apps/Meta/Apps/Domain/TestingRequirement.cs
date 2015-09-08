namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a06befc5-c347-4ffb-9391-2a099fca5145")]
	#endregion
	[Inherit(typeof(PartSpecificationInterface))]

	[Plural("TestingRequirements")]
	public partial class TestingRequirementClass : Class
	{

		public static TestingRequirementClass Instance {get; internal set;}

		internal TestingRequirementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}