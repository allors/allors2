namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ca008b8d-584e-4aa5-a759-895b634defc5")]
	#endregion
	[Inherit(typeof(PaymentMethodInterface))]
	[Inherit(typeof(FinancialAccountInterface))]

	[Plural("OwnBankAccounts")]
	public partial class OwnBankAccountClass : Class
	{
		#region Allors
		[Id("d83ca1e3-4137-4e92-a61d-0b8a1b8f7085")]
		[AssociationId("8a492054-a6be-4824-a0d2-daeed69c091b")]
		[RoleId("c90ac5e5-2368-45d1-bc4a-0621c30f20e5")]
		#endregion
		[Indexed]
		[Type(typeof(BankAccountClass))]
		[Plural("BankAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BankAccount;



		public static OwnBankAccountClass Instance {get; internal set;}

		internal OwnBankAccountClass() : base(MetaPopulation.Instance)
        {
        }
	}
}