namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("dfe47c36-58b5-4438-b674-cc2e861922d6")]
	#endregion
	[Inherit(typeof(WorkEffortInterface))]

	[Plural("Programs")]
	public partial class ProgramClass : Class
	{

		public static ProgramClass Instance {get; internal set;}

		internal ProgramClass() : base(MetaPopulation.Instance)
        {
        }
	}
}