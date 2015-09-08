namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("caa4814f-85a2-46a8-97a7-82220f8270cb")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("Priorities")]
	public partial class PriorityClass : Class
	{

		public static PriorityClass Instance {get; internal set;}

		internal PriorityClass() : base(MetaPopulation.Instance)
        {
        }
	}
}