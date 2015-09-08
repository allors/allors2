namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("72237d95-e9c0-42c1-afe3-ec34f2e6cbfb")]
	#endregion
	[Inherit(typeof(AgreementItemInterface))]

	[Plural("AgreementPricingPrograms")]
	public partial class AgreementPricingProgramClass : Class
	{

		public static AgreementPricingProgramClass Instance {get; internal set;}

		internal AgreementPricingProgramClass() : base(MetaPopulation.Instance)
        {
        }
	}
}