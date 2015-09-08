namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("785a36a9-4710-4f3f-bd26-dbaff5353535")]
	#endregion
	[Plural("AccountingTransactions")]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class AccountingTransactionInterface: Interface
	{
		#region Allors
		[Id("4e4cb94c-424c-4824-ad44-5bb1c7312a52")]
		[AssociationId("2ed212a9-6c8b-443f-a842-391aa0b6a265")]
		[RoleId("fc36b63a-7fae-414d-adb0-50a86d9fb238")]
		#endregion
		[Indexed]
		[Type(typeof(AccountingTransactionDetailClass))]
		[Plural("AccountingTransactionDetails")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType AccountingTransactionDetail;

		#region Allors
		[Id("657f2688-4af0-4580-add2-c8a30b32e016")]
		[AssociationId("e7a6ced6-1397-484a-b4c0-5bb7ebbaf9e0")]
		[RoleId("b4b2d735-b895-4112-8d6c-690b0d6f2cc1")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("77910a3f-3547-4d6b-92e0-f1fc136e22da")]
		[AssociationId("97cc6287-9dc0-404a-ad92-bfd2c3927d30")]
		[RoleId("83cfb29d-4311-4b16-9331-1c00d54b70c7")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("TransactionDates")]
		public RelationType TransactionDate;

		#region Allors
		[Id("a29cb739-8d2f-4e7d-a652-af8d2e190658")]
		[AssociationId("5f295cc2-a884-427b-8fb3-056af4f58b7b")]
		[RoleId("d6cc2527-7a3f-4bac-bf4b-991f484c51a7")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("DerivedTotalAmounts")]
		public RelationType DerivedTotalAmount;

		#region Allors
		[Id("a7fb7e5a-287a-41a1-b6b9-bd56601732f3")]
		[AssociationId("6f71e42e-1fa8-4905-93ef-ff5a417aff46")]
		[RoleId("d5d3a903-748a-4b0e-8da6-c23b304eb62c")]
		#endregion
		[Indexed]
		[Type(typeof(AccountingTransactionNumberClass))]
		[Plural("AccountingTransactionNumbers")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType AccountingTransactionNumber;

		#region Allors
		[Id("be061dda-bb8f-4bc1-b386-dc0c05dc6eaf")]
		[AssociationId("8943c9e2-3c6f-49c4-aa87-397af24e8073")]
		[RoleId("75b6517f-6e4a-4218-8ca1-de230c69a02e")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EntryDates")]
		public RelationType EntryDate;



		public static AccountingTransactionInterface Instance {get; internal set;}

		internal AccountingTransactionInterface() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.DerivedTotalAmount.RoleType.IsRequired = true;

            this.EntryDate.RoleType.IsRequired = true;
            this.TransactionDate.RoleType.IsRequired = true;
            this.Description.RoleType.IsRequired = true;
        }
    }
}