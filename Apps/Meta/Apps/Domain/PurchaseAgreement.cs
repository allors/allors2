namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1032dc2f-72b7-4ba2-b47d-ba14d52a18c9")]
	#endregion
	[Inherit(typeof(AgreementInterface))]

	[Plural("PurchaseAgreements")]
	public partial class PurchaseAgreementClass : Class
	{

		public static PurchaseAgreementClass Instance {get; internal set;}

		internal PurchaseAgreementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}