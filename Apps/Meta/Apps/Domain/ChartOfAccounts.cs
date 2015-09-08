namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6cf4845d-65a0-4957-95e9-f2b5327d6515")]
	#endregion
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("ChartsOfAccounts")]
	public partial class ChartOfAccountsClass : Class
	{
		#region Allors
		[Id("65f44f44-a613-4cbf-a924-1098c9876f20")]
		[AssociationId("d4bd5e5f-e973-489c-879d-31b0023de770")]
		[RoleId("0f6c3b14-f165-41df-aa8d-f49f53c53e05")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("71d503fb-ebb9-45b3-af62-1b233677adce")]
		[AssociationId("ca0820dd-e0b2-4714-8e2f-f3613dcdbd15")]
		[RoleId("d855adc2-f70e-48d3-a185-957bf27d3d58")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountClass))]
		[Plural("GeneralLedgerAccounts")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType GeneralLedgerAccount;



		public static ChartOfAccountsClass Instance {get; internal set;}

		internal ChartOfAccountsClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }

    }
}