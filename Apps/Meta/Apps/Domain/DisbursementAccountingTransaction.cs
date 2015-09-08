namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a3a5aeea-3c8b-43ab-94f1-49a1bd2d7254")]
	#endregion
	[Inherit(typeof(ExternalAccountingTransactionInterface))]

	[Plural("DisbursementAccountingTransactions")]
	public partial class DisbursementAccountingTransactionClass : Class
	{
		#region Allors
		[Id("62c15bc4-42fd-45b8-ae5d-5cdf92c0b414")]
		[AssociationId("920ffcd4-6085-4d22-8d81-caf2dde21e70")]
		[RoleId("33b6e056-e1e2-4173-97db-485593bf9e36")]
		#endregion
		[Indexed]
		[Type(typeof(DisbursementClass))]
		[Plural("Disbursements")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType Disbursement;



		public static DisbursementAccountingTransactionClass Instance {get; internal set;}

		internal DisbursementAccountingTransactionClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Disbursement.RoleType.IsRequired = true;
        }
    }
}