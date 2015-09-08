namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4117ba43-c7fd-4ba5-965e-50e2ce5b5058")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("DunningTypes")]
	public partial class DunningTypeClass : Class
	{

		public static DunningTypeClass Instance {get; internal set;}

		internal DunningTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}