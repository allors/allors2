namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("66e7dcf3-90bc-4ac6-988f-54015f5bef11")]
	#endregion
	[Inherit(typeof(DocumentInterface))]

	[Plural("PackagingSlips")]
	public partial class PackagingSlipClass : Class
	{

		public static PackagingSlipClass Instance {get; internal set;}

		internal PackagingSlipClass() : base(MetaPopulation.Instance)
        {
        }
	}
}