namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("150d21f7-20dd-4951-848f-f74a69dadb5b")]
	#endregion
	[Inherit(typeof(AgreementTermInterface))]

	[Plural("Incentives")]
	public partial class IncentiveClass : Class
	{

		public static IncentiveClass Instance {get; internal set;}

		internal IncentiveClass() : base(MetaPopulation.Instance)
        {
        }
	}
}