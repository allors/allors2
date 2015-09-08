namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("52458d42-94ee-4757-bcfb-bc9c45ed6dc6")]
	#endregion
	[Inherit(typeof(FinancialAccountTransactionInterface))]

	[Plural("Deposits")]
	public partial class DepositClass : Class
	{
		#region Allors
		[Id("2a41dcff-72f9-4225-8a92-1955f10b8ae2")]
		[AssociationId("3a24349b-c31d-4ba1-bb95-616852f07c93")]
		[RoleId("04ff9dbf-60cd-4062-b61a-c26b78cf1c48")]
		#endregion
		[Indexed]
		[Type(typeof(ReceiptClass))]
		[Plural("Receipts")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType Receipt;



		public static DepositClass Instance {get; internal set;}

		internal DepositClass() : base(MetaPopulation.Instance)
        {
        }
	}
}