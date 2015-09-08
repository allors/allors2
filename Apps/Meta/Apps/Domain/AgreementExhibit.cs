namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2830c388-b002-44d6-91b6-b2b43fa778f3")]
	#endregion
	[Inherit(typeof(AgreementItemInterface))]

	[Plural("AgreementExhibits")]
	public partial class AgreementExhibitClass : Class
	{

		public static AgreementExhibitClass Instance {get; internal set;}

		internal AgreementExhibitClass() : base(MetaPopulation.Instance)
        {
        }
	}
}