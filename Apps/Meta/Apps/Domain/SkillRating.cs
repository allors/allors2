namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2b44d390-bdd5-43aa-91c4-25b1966c46fb")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("SkillRatings")]
	public partial class SkillRatingClass : Class
	{

		public static SkillRatingClass Instance {get; internal set;}

		internal SkillRatingClass() : base(MetaPopulation.Instance)
        {
        }
	}
}