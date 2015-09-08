namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5500cb42-1aae-4816-9bc1-d63ff273f144")]
	#endregion
	[Plural("FinancialAccountTransactions")]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class FinancialAccountTransactionInterface: Interface
	{
		#region Allors
		[Id("04411b65-a0a1-4e2c-9d10-a0ecfcf6c3d2")]
		[AssociationId("340a61a7-3458-47ea-b41d-4c559fd8b1d2")]
		[RoleId("1c6950b1-b5dc-4204-878a-f10029dcc4ab")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptons")]
		public RelationType Description;

		#region Allors
		[Id("07b3745c-581c-476b-a4a9-beacaa3bd700")]
		[AssociationId("7878206b-b4f9-4ddd-b69e-a041402844dd")]
		[RoleId("2e77d783-9cda-41e6-be8b-1bf96520a385")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EntryDates")]
		public RelationType EntryDate;

		#region Allors
		[Id("8f777804-597a-4604-a553-251e2e9d6502")]
		[AssociationId("f74151d5-ad2e-4418-b3a1-3772afbdaf52")]
		[RoleId("3135d67e-7290-4eb2-aec8-e783d9325a02")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("TransactionDates")]
		public RelationType TransactionDate;



		public static FinancialAccountTransactionInterface Instance {get; internal set;}

		internal FinancialAccountTransactionInterface() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.TransactionDate.RoleType.IsRequired = true;
        }
    }
}