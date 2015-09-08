namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5c5c17d1-2132-403b-8819-e3c1aa7bd6a9")]
	#endregion
	[Inherit(typeof(DocumentInterface))]

	[Plural("BillOfLadings")]
	public partial class BillOfLadingClass : Class
	{

		public static BillOfLadingClass Instance {get; internal set;}

		internal BillOfLadingClass() : base(MetaPopulation.Instance)
        {
        }
	}
}