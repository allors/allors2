namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1b2113f6-2c00-4ea7-9408-72bae667eaa3")]
	#endregion
	[Inherit(typeof(AgreementInterface))]

	[Plural("SubContractorAgreements")]
	public partial class SubContractorAgreementClass : Class
	{

		public static SubContractorAgreementClass Instance {get; internal set;}

		internal SubContractorAgreementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}