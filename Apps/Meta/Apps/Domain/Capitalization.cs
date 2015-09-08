namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a0a753be-15ca-49e2-8f5f-f956fa132f49")]
	#endregion
	[Inherit(typeof(InternalAccountingTransactionInterface))]

	[Plural("Capitalizations")]
	public partial class CapitalizationClass : Class
	{

		public static CapitalizationClass Instance {get; internal set;}

		internal CapitalizationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}