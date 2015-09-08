namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a18de27f-54fe-4160-b149-475bebeaf716")]
	#endregion
	[Inherit(typeof(PriceComponentInterface))]

	[Plural("SurchargeComponents")]
	public partial class SurchargeComponentClass : Class
	{
		#region Allors
		[Id("0e9d10cd-6905-42ca-9db3-aed9b123eb2a")]
		[AssociationId("79b7473b-d65d-469b-9061-bb344da42c7e")]
		[RoleId("f5d669b0-89d3-4605-ae6e-dcee6c673c50")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Percentages")]
		public RelationType Percentage;



		public static SurchargeComponentClass Instance {get; internal set;}

		internal SurchargeComponentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}