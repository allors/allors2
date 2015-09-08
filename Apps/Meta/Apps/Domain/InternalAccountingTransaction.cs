namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5a783d98-845a-4784-9c92-5c75a4af3fb8")]
	#endregion
	[Plural("InternalAccountingTransactions")]
	[Inherit(typeof(AccountingTransactionInterface))]

  	public partial class InternalAccountingTransactionInterface: Interface
	{
		#region Allors
		[Id("96a1901c-a17a-43d7-8d84-76e1586787f2")]
		[AssociationId("5f58fb32-15d9-47e0-9ace-9eb4c1cd2eda")]
		[RoleId("03f1ae5e-0644-47c7-b31e-345e92085a9c")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;



		public static InternalAccountingTransactionInterface Instance {get; internal set;}

		internal InternalAccountingTransactionInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}