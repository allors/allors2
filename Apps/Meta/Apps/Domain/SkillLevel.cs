namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("555882ea-d25a-4da2-a8ea-330469c8cd41")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("SkillLevels")]
	public partial class SkillLevelClass : Class
	{

		public static SkillLevelClass Instance {get; internal set;}

		internal SkillLevelClass() : base(MetaPopulation.Instance)
        {
        }
	}
}