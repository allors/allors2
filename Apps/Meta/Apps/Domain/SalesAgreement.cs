namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("7d620a47-475b-40de-a4a7-8be7994df18e")]
	#endregion
	[Inherit(typeof(AgreementInterface))]

	[Plural("SalesAgreements")]
	public partial class SalesAgreementClass : Class
	{

		public static SalesAgreementClass Instance {get; internal set;}

		internal SalesAgreementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}