namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a3ca36e6-960d-4e3a-96d0-6ca1d71d05d7")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("OrderValues")]
	public partial class OrderValueClass : Class
	{
		#region Allors
		[Id("077a33bc-a822-4a23-918c-7fcaacdc61d1")]
		[AssociationId("f38c5851-7187-4f53-8eaf-85edee7e733d")]
		[RoleId("7ee1e68b-5bb5-4e72-b63d-83132346a503")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("ThroughAmounts")]
		public RelationType ThroughAmount;

		#region Allors
		[Id("b25816e8-4b0c-4857-907f-7a391df2c55e")]
		[AssociationId("017aab24-a93c-4654-bc89-96e075d13c08")]
		[RoleId("eedd52f8-0713-428e-b10a-a7da99b967aa")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("FromAmounts")]
		public RelationType FromAmount;



		public static OrderValueClass Instance {get; internal set;}

		internal OrderValueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}