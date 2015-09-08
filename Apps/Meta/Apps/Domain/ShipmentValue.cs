namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("20ef8456-83f2-4722-b8a8-1d8ab3129843")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("ShipmentValues")]
	public partial class ShipmentValueClass : Class
	{
		#region Allors
		[Id("a87bb4e3-ca1c-4887-9305-19febfc531fd")]
		[AssociationId("dd6f8067-ae66-41d1-b211-9d9b68459bcc")]
		[RoleId("48754c2d-fc0c-47ac-af53-bc4b2f9adc20")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("ThroughAmounts")]
		public RelationType ThroughAmount;

		#region Allors
		[Id("d637b8ab-c6ac-4855-81db-f0a1f1584219")]
		[AssociationId("af4f35a2-6f4b-4e99-9d5d-271eafc5db17")]
		[RoleId("3f7500ff-afc7-4dc8-a454-e35838380a0c")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("FromAmounts")]
		public RelationType FromAmount;



		public static ShipmentValueClass Instance {get; internal set;}

		internal ShipmentValueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}