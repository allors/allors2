namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a3fe34f9-7dfb-46fe-98ec-ed9a7d14ac19")]
	#endregion
	[Inherit(typeof(ExternalAccountingTransactionInterface))]

	[Plural("Obligations")]
	public partial class ObligationClass : Class
	{

		public static ObligationClass Instance {get; internal set;}

		internal ObligationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}