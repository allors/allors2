namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("0b9034b1-288a-48a7-9d46-3ca6dcb7ca3f")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("AccountingTransactionNumbers")]
	public partial class AccountingTransactionNumberClass : Class
	{
		#region Allors
		[Id("1a7eda6e-7b1c-4faf-8635-05bc233c5dd8")]
		[AssociationId("ad6df638-67d2-4d41-9695-01c6adf3f251")]
		[RoleId("e2178198-6bbd-4caa-9402-40137b2bd529")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Numbers")]
		public RelationType Number;

		#region Allors
		[Id("e01605b0-ba04-4775-ad15-cac1281cec9e")]
		[AssociationId("75523554-b713-433b-8916-c70278649b52")]
		[RoleId("dcaeed3f-1bbe-40a0-aacc-9c26db3f984f")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Years")]
		public RelationType Year;

		#region Allors
		[Id("f3bcec3b-b08e-4eab-812e-bb5b31fe6a4d")]
		[AssociationId("e447318b-ab6a-4259-8fb0-fb0ae81b15f7")]
		[RoleId("018e866e-3593-45d2-a10e-45bd79a0faeb")]
		#endregion
		[Indexed]
		[Type(typeof(AccountingTransactionTypeClass))]
		[Plural("AccountingTransactionTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType AccountingTransactionType;



		public static AccountingTransactionNumberClass Instance {get; internal set;}

		internal AccountingTransactionNumberClass() : base(MetaPopulation.Instance)
        {
        }
	}
}