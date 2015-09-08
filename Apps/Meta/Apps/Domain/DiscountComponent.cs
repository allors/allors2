namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c0b927c4-7197-4295-8edf-057b6b4b3a6a")]
	#endregion
	[Inherit(typeof(PriceComponentInterface))]

	[Plural("DiscountComponents")]
	public partial class DiscountComponentClass : Class
	{
		#region Allors
		[Id("1101cd39-852b-4eac-8649-de1a3f080703")]
		[AssociationId("ff284a40-cfa1-4b5b-90ec-c42b4dc35ef5")]
		[RoleId("88c08616-c1e6-4c53-b1e8-74fa33bc310d")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Percentages")]
		public RelationType Percentage;



		public static DiscountComponentClass Instance {get; internal set;}

		internal DiscountComponentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}