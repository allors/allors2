namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("edf1788a-0c75-4635-904d-db9f9a6a7399")]
	#endregion
	[Inherit(typeof(FinancialAccountTransactionInterface))]

	[Plural("Withdrawals")]
	public partial class WithdrawalClass : Class
	{
		#region Allors
		[Id("b97344ac-a848-4ee0-bdb5-a9d79bb785fc")]
		[AssociationId("265511f9-0f02-47c8-b7c4-392f09a69fa2")]
		[RoleId("c7dcd911-b352-4f0f-98fd-ea1c3d8d77d6")]
		#endregion
		[Indexed]
		[Type(typeof(DisbursementClass))]
		[Plural("Disbursements")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType Disbursement;



		public static WithdrawalClass Instance {get; internal set;}

		internal WithdrawalClass() : base(MetaPopulation.Instance)
        {
        }
	}
}