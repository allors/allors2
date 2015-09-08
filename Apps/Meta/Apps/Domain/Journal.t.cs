namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d3446420-6d2a-4d18-a6eb-0405da9f7cc5")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Journals")]
	public partial class JournalClass : Class
	{
		#region Allors
		[Id("01abf1e4-c2f8-4d04-8046-f5ac5428ff11")]
		[AssociationId("1ee04497-3585-4910-83c3-1bcdbe3c3bd2")]
		[RoleId("82e88c20-35ac-4346-9881-157d305ed33b")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("UseAsDefaults")]
		public RelationType UseAsDefault;

		#region Allors
		[Id("04f786b4-66be-4616-9966-ac026384c0d3")]
		[AssociationId("a1ed1007-5f6a-4177-9847-45339c24331a")]
		[RoleId("0a653d75-829b-4612-b260-ec470eea0221")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationGlAccountClass))]
		[Plural("GlPaymentsInTransit")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType GlPaymentInTransit;

		#region Allors
		[Id("1ec79ec4-60a8-4fdc-b11e-8c25697cd457")]
		[AssociationId("cfda1134-8fdf-449c-99cb-1e9ad29448fc")]
		[RoleId("511439d3-de78-4b3b-8489-9fd661c41fdc")]
		#endregion
		[Indexed]
		[Type(typeof(JournalTypeClass))]
		[Plural("JournalTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType JournalType;

		#region Allors
		[Id("37493cfc-e817-4b89-b7cb-7d29f69cf41e")]
		[AssociationId("3a30aed2-9a0b-4eb2-948e-6ac033d2a5e0")]
		[RoleId("13871d2d-ae7a-48be-99da-79711be267f8")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("3a52aa7c-fa01-4845-866c-976e48ea2179")]
		[AssociationId("8ab91bc9-99da-4bb4-b249-866a54fb4117")]
		[RoleId("8bf5236d-6b99-4b86-b686-1cbf96bcde03")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("BlockUnpaidTransactionss")]
		public RelationType BlockUnpaidTransactions;

		#region Allors
		[Id("4f1b0471-67f9-4fa1-9b69-b5d9cbeda5e7")]
		[AssociationId("b18129bb-5cf8-4408-a4fb-b5782fe67684")]
		[RoleId("bf1656e9-0124-4403-855e-da94157e293d")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationGlAccountClass))]
		[Plural("ContraAccounts")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType ContraAccount;

		#region Allors
		[Id("76e76063-ddce-4a8a-98fb-884cd6179c45")]
		[AssociationId("1395e70f-cd1c-457e-bfd8-c7f31a32c8f4")]
		[RoleId("b6416161-0b77-4a30-b00e-59a0eaaa1e87")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("774f30df-26b4-41d5-9ecb-d1fd62244e1f")]
		[AssociationId("9a4078bb-350b-4fbb-8fc2-16e86928d32e")]
		[RoleId("dfbac079-a8f6-4576-8d54-39bf76553e0c")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(JournalTypeClass))]
		[Plural("PreviousJournalTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PreviousJournalType;

		#region Allors
		[Id("9aa48ebb-0ee0-4662-bfc5-f6b8ccb7a7c3")]
		[AssociationId("96d76f8b-5121-401f-bf2b-3f504494f4d7")]
		[RoleId("e4d8e7b3-48be-4a9a-848e-c35dd3889715")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(OrganisationGlAccountClass))]
		[Plural("PreviousContraAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PreviousContraAccount;

		#region Allors
		[Id("9b7c3687-b268-4c2b-8b04-c04a0c55d79f")]
		[AssociationId("47be858f-a462-4e23-acee-18aecdebc95e")]
		[RoleId("f1907d6b-5015-4583-8d63-5c74f5954b97")]
		#endregion
		[Indexed]
		[Type(typeof(JournalEntryClass))]
		[Plural("JournalEntries")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType JournalEntry;

		#region Allors
		[Id("dbdca15b-5337-44f1-b490-c69cb36df9c3")]
		[AssociationId("b4d7972b-4e79-4192-b862-c358ad10b48e")]
		[RoleId("1ba97bd5-9644-47c6-b5e0-52207322cc38")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("CloseWhenInBalances")]
		public RelationType CloseWhenInBalance;



		public static JournalClass Instance {get; internal set;}

		internal JournalClass() : base(MetaPopulation.Instance)
        {
        }
	}
}