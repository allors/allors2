namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("b2cf9a3d-f156-4da7-87bf-ecdeaa13e326")]
	#endregion
	[Inherit(typeof(WorkEffortInterface))]

	[Plural("Tasks")]
	public partial class TaskClass : Class
	{

		public static TaskClass Instance {get; internal set;}

		internal TaskClass() : base(MetaPopulation.Instance)
        {
        }
	}
}