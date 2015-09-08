namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("0aecacff-23d0-48ff-8934-a4e5f711c729")]
	#endregion
	[Inherit(typeof(ExternalAccountingTransactionInterface))]

	[Plural("SalesAccountingTransactions")]
	public partial class SalesAccountingTransactionClass : Class
	{
		#region Allors
		[Id("9b376e18-7cf8-43f7-ac89-ef4b32a1c8fd")]
		[AssociationId("ee71978e-2085-48d2-81ad-571cfcec8264")]
		[RoleId("3fe3be8d-563d-4455-8f1a-7771ff97005f")]
		#endregion
		[Indexed]
		[Type(typeof(InvoiceInterface))]
		[Plural("Invoices")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType Invoice;



		public static SalesAccountingTransactionClass Instance {get; internal set;}

		internal SalesAccountingTransactionClass() : base(MetaPopulation.Instance)
        {
        }
	}
}