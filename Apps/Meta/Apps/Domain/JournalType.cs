namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("7b23440c-d26b-42f5-a94b-e26872e63e7d")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("JournalTypes")]
	public partial class JournalTypeClass : Class
	{

		public static JournalTypeClass Instance {get; internal set;}

		internal JournalTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}