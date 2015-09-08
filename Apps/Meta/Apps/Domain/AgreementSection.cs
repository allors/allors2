namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("e31d6dd2-b5b2-4fd8-949f-0df688ed2e9b")]
	#endregion
	[Inherit(typeof(AgreementItemInterface))]

	[Plural("AgreementSections")]
	public partial class AgreementSectionClass : Class
	{

		public static AgreementSectionClass Instance {get; internal set;}

		internal AgreementSectionClass() : base(MetaPopulation.Instance)
        {
        }
	}
}