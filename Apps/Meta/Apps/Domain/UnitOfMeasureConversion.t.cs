namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2e216901-eab9-42e3-9e49-7fe8e88291d3")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("UnitOfMeasureConversions")]
	public partial class UnitOfMeasureConversionClass : Class
	{
		#region Allors
		[Id("3ae94702-ee60-4057-a649-f655ff4e2865")]
		[AssociationId("1ab7d188-af19-4742-a0e6-11043b666bd4")]
		[RoleId("5372ec1c-9b57-4ed5-b665-cdad8a13d933")]
		#endregion
		[Indexed]
		[Type(typeof(IUnitOfMeasureInterface))]
		[Plural("ToUnitOfMeasures")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ToUnitOfMeasure;

		#region Allors
		[Id("5d7ed801-4a2e-4abc-a32d-d869210132af")]
		[AssociationId("a3467a5f-8c7d-453a-9a33-18d742f20d06")]
		[RoleId("4b8a465d-9334-427f-b799-d08b7c84200a")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDates")]
		public RelationType StartDate;

		#region Allors
		[Id("835118da-148a-4c42-ab07-75b213a8e1f7")]
		[AssociationId("f9f78e34-6fe1-4863-b831-cabe46cbc764")]
		[RoleId("c06dd0a5-dabe-46fa-97f7-62f6f4b47983")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(9)]
		[Plural("ConversionFactors")]
		public RelationType ConversionFactor;



		public static UnitOfMeasureConversionClass Instance {get; internal set;}

		internal UnitOfMeasureConversionClass() : base(MetaPopulation.Instance)
        {
        }
	}
}