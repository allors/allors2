namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("90a8fa64-c9c7-4a7a-a543-d500668619eb")]
	#endregion
	[Inherit(typeof(WorkEffortInterface))]

	[Plural("Phases")]
	public partial class PhaseClass : Class
	{

		public static PhaseClass Instance {get; internal set;}

		internal PhaseClass() : base(MetaPopulation.Instance)
        {
        }
	}
}