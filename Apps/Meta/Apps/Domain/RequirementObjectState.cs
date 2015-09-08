namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("b1ee7191-544e-4cee-bbb1-d64364eb7137")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("RequirementObjectStates")]
	public partial class RequirementObjectStateClass : Class
	{

		public static RequirementObjectStateClass Instance {get; internal set;}

		internal RequirementObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}