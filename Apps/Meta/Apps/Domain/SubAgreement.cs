namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("11e8fae8-3270-4789-a4eb-ca89cddd2859")]
	#endregion
	[Inherit(typeof(AgreementItemInterface))]

	[Plural("SubAgreements")]
	public partial class SubAgreementClass : Class
	{

		public static SubAgreementClass Instance {get; internal set;}

		internal SubAgreementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}