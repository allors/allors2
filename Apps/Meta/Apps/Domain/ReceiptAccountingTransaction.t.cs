namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1a5195d6-8fff-4590-afe1-3f50c4fa0c67")]
	#endregion
	[Inherit(typeof(ExternalAccountingTransactionInterface))]

	[Plural("ReceiptAccountingTransactions")]
	public partial class ReceiptAccountingTransactionClass : Class
	{
		#region Allors
		[Id("a69440e0-34f3-42ce-9f21-38ea22c5762e")]
		[AssociationId("0d841c4a-1f7b-443d-95a6-29a1205f203c")]
		[RoleId("52a40ec0-a108-491a-9f41-94885fcb09b5")]
		#endregion
		[Indexed]
		[Type(typeof(ReceiptClass))]
		[Plural("Receipts")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType Receipt;



		public static ReceiptAccountingTransactionClass Instance {get; internal set;}

		internal ReceiptAccountingTransactionClass() : base(MetaPopulation.Instance)
        {
        }
	}
}