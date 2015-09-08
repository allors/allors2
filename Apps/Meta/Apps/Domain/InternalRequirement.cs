namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("b46c9149-21ef-45a6-aef6-c6aa30389d7f")]
	#endregion
	[Inherit(typeof(RequirementInterface))]

	[Plural("InternalRequirements")]
	public partial class InternalRequirementClass : Class
	{

		public static InternalRequirementClass Instance {get; internal set;}

		internal InternalRequirementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}