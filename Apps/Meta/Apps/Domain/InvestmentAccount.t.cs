namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("8a06c50b-5951-465e-86b8-43e733f20b90")]
	#endregion
	[Inherit(typeof(FinancialAccountInterface))]

	[Plural("InvestmentAccounts")]
	public partial class InvestmentAccountClass : Class
	{
		#region Allors
		[Id("9eefdec1-48db-4f91-9eac-928b6a42d4e4")]
		[AssociationId("2759ed05-afa4-49ea-91d1-20b8d2ff527c")]
		[RoleId("1d337bb7-2b33-4c8a-aeb3-d37c3ea72690")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;



		public static InvestmentAccountClass Instance {get; internal set;}

		internal InvestmentAccountClass() : base(MetaPopulation.Instance)
        {
        }
	}
}