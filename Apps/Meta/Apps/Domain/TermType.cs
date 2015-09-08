namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1468c86a-4ac4-4c64-a93b-1b0c5f4b41bc")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("TermTypes")]
	public partial class TermTypeClass : Class
	{

		public static TermTypeClass Instance {get; internal set;}

		internal TermTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}