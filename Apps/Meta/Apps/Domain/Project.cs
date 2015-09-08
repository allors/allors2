namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2e2c567e-4c1f-4729-97a1-5ae203be936c")]
	#endregion
	[Inherit(typeof(WorkEffortInterface))]

	[Plural("Projects")]
	public partial class ProjectClass : Class
	{

		public static ProjectClass Instance {get; internal set;}

		internal ProjectClass() : base(MetaPopulation.Instance)
        {
        }
	}
}