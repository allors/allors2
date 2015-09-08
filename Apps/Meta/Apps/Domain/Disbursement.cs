namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d152e0a4-c76f-4945-8c0f-ad1e5f70ad07")]
	#endregion
	[Inherit(typeof(PaymentInterface))]

	[Plural("Disbursements")]
	public partial class DisbursementClass : Class
	{

		public static DisbursementClass Instance {get; internal set;}

		internal DisbursementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}