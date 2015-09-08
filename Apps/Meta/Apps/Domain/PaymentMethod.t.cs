namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f34d5b9b-b940-4885-9744-754dd0eae08d")]
	#endregion
	[Plural("PaymentMethods")]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]

  	public partial class PaymentMethodInterface: Interface
	{
		#region Allors
		[Id("0b16fdbc-c535-45a5-8be9-7b1d2c12337a")]
		[AssociationId("0d9ba18d-46fa-4a98-aa6a-37261f2f11a8")]
		[RoleId("7af97652-a1cf-49f8-a33c-33c45dcadd4e")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("BalanceLimits")]
		public RelationType BalanceLimit;

		#region Allors
		[Id("2e5e9d24-4697-4811-8636-1ebf9d86b9c2")]
		[AssociationId("d479d315-4478-4b97-98c4-bfe964ca9921")]
		[RoleId("bdbca6c9-6a5e-4700-a987-cd62db5b831a")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("CurrentBalances")]
		public RelationType CurrentBalance;

		#region Allors
		[Id("36559f29-1182-42d1-831d-587103456ce6")]
		[AssociationId("ce33ccae-be2c-4081-abd5-be803bdbc1a4")]
		[RoleId("0c76bfba-2ef1-46fb-bb1f-b49b57f792c0")]
		#endregion
		[Indexed]
		[Type(typeof(JournalClass))]
		[Plural("Journals")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Journal;

		#region Allors
		[Id("386c301e-8f0f-48fc-8bec-10ac0df6be9d")]
		[AssociationId("168be3f3-97ef-490d-ab65-29c7928310cc")]
		[RoleId("555b2755-24a1-4238-b390-f77e0fd205ac")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("59da5fc4-e861-4c7d-aa96-c15cebbb63f2")]
		[AssociationId("7e050127-bbea-490a-ac78-354c37daa799")]
		[RoleId("e48560a6-b94d-459b-98d1-4bf429816798")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationGlAccountClass))]
		[Plural("GlsPaymentInTransit")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType GlPaymentInTransit;

		#region Allors
		[Id("6e61f71f-77a1-4795-b876-ba5d74ebdc3e")]
		[AssociationId("ce1e09f8-1260-462d-be94-726a8716f6d8")]
		[RoleId("108077d8-ca1c-48c1-8154-f52f7807eb5b")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("RemarksPlural")]
		public RelationType Remarks;

		#region Allors
		[Id("8b11feda-09c8-4f8d-a21d-dddd87531d5b")]
		[AssociationId("d7361d9b-b76c-4a22-a385-487219d861d5")]
		[RoleId("2c58e2a1-c7bb-481e-a828-c5bfa0eaec49")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationGlAccountClass))]
		[Plural("GeneralLedgerAccounts")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType GeneralLedgerAccount;

		#region Allors
		[Id("a937fc55-d737-444b-93b0-525994e09f6a")]
		[AssociationId("c33d3bc7-13ff-4d83-be7b-e9fbd7c21d55")]
		[RoleId("2b00353d-bc87-4aa1-b260-3650f93320ff")]
		#endregion
		[Indexed]
		[Type(typeof(SupplierRelationshipClass))]
		[Plural("Creditors")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Creditor;

		#region Allors
		[Id("c32243ac-8810-478b-b0f4-11a1fe4773bd")]
		[AssociationId("433b6034-88a1-4355-81cd-dbd92ef6f7da")]
		[RoleId("238a0b8f-882a-47ea-96cc-ff19126974c1")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("IsActives")]
		public RelationType IsActive;



		public static PaymentMethodInterface Instance {get; internal set;}

		internal PaymentMethodInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}