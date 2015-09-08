namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c1b6fac9-8e69-4c07-8cec-e9b52c690e72")]
	#endregion
	[Inherit(typeof(EngagementItemInterface))]

	[Plural("GoodOrderItems")]
	public partial class GoodOrderItemClass : Class
	{
		#region Allors
		[Id("de65b7a6-b2b3-4d77-9cb4-94720adb43f0")]
		[AssociationId("3ed4dffc-09eb-4285-a31c-ba3af0666451")]
		[RoleId("2f1173ef-1723-4ee5-9ff3-a01b6216584a")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Prices")]
		public RelationType Price;

		#region Allors
		[Id("f7399ebd-64f0-4bfa-a063-e75389d6a7cc")]
		[AssociationId("30b12a84-e2cc-4d24-aca3-71568961f9ee")]
		[RoleId("bf1eeede-db39-4996-a2da-b3da503c2415")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Quantities")]
		public RelationType Quantity;



		public static GoodOrderItemClass Instance {get; internal set;}

		internal GoodOrderItemClass() : base(MetaPopulation.Instance)
        {
        }
	}
}