namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d726e301-4e4a-4ccb-9a6e-bc6fc4a327ab")]
	#endregion
	[Inherit(typeof(AgreementInterface))]

	[Plural("ClientAgreements")]
	public partial class ClientAgreementClass : Class
	{

		public static ClientAgreementClass Instance {get; internal set;}

		internal ClientAgreementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}