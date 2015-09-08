namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("123bfcba-0548-4637-8dfc-267d6c0ac262")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("Skills")]
	public partial class SkillClass : Class
	{

		public static SkillClass Instance {get; internal set;}

		internal SkillClass() : base(MetaPopulation.Instance)
        {
        }
	}
}