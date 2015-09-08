namespace Allors.Meta
{
	using System;

	[Inherit(typeof(GeographicBoundaryInterface))]
	[Inherit(typeof(CityBoundInterface))]
	public partial class CountryClass : Class
	{
		#region Allors
		[Id("13010743-231f-43a8-9539-b95b83ab15da")]
		[AssociationId("de4d0d90-e41b-4b7c-bcdc-23269020ab4e")]
		[RoleId("5e2328a6-5413-401f-8106-7b8b29907b06")]
		#endregion
		[Indexed]
		[Type(typeof(VatRateClass))]
		[Plural("VatRates")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType VatRate;

		#region Allors
		[Id("2ecb8cfb-011d-4c31-a9cd-ed5a13ae23a4")]
		[AssociationId("ebdfd8e3-9d24-4721-b72b-5a5e4327d62b")]
		[RoleId("45aa4f50-a23b-4ce6-872f-d72b648e4e90")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("IbanLengths")]
		public RelationType IbanLength;

		#region Allors
		[Id("6553ee71-66dd-45f2-9de9-5656b011d2fc")]
		[AssociationId("0a5662c3-1f60-41d5-a703-638480cb3c15")]
		[RoleId("a14f5154-bcf2-44f4-a49e-3c17aca71247")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("EuMemberStates")]
		public RelationType EuMemberState;

		#region Allors
		[Id("7f0adb03-db73-44f2-a4a2-ece00f4908a2")]
		[AssociationId("081e6909-c744-4795-b587-82bbf938b5fe")]
		[RoleId("38546e92-a238-4d72-a731-a3f91dbcc61f")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("TelephoneCodes")]
		public RelationType TelephoneCode;

		#region Allors
		[Id("a2aa65d7-e0ef-4f6f-a194-9aeb49a1d898")]
		[AssociationId("86d7d9a6-77fd-491b-b563-86b8d0c76ee4")]
		[RoleId("4f6f041b-a1ea-47bc-92e4-650bddaa46ed")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("IbanRegexes")]
		public RelationType IbanRegex;

		#region Allors
		[Id("b829da1c-2eb7-495b-a4a9-98e335cd87f9")]
		[AssociationId("a0377434-67ae-4ab4-90b3-99fb6bc2bf90")]
		[RoleId("8a306049-a4b9-4489-a2b8-d627fa6444c3")]
		#endregion
		[Indexed]
		[Type(typeof(VatFormClass))]
		[Plural("VatForms")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType VatForm;

		#region Allors
		[Id("c231ce68-bf03-4122-8699-c3c6473ab90a")]
		[AssociationId("153203db-be9a-4722-aab3-7163de779a2a")]
		[RoleId("e72228ee-ae28-406c-b7ee-a9be1a4d3286")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("UriExtension")]
		public RelationType UriExtension;

        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
    }
}