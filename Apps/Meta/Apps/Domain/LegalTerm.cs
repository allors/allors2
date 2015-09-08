namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("14a2576c-3ea7-4016-aba2-44172fb7a952")]
	#endregion
	[Inherit(typeof(AgreementTermInterface))]

	[Plural("LegalTerms")]
	public partial class LegalTermClass : Class
	{

		public static LegalTermClass Instance {get; internal set;}

		internal LegalTermClass() : base(MetaPopulation.Instance)
        {
        }
	}
}