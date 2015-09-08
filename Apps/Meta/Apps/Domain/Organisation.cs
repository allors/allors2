namespace Allors.Meta
{
	#region Allors
	[Id("3a5dcec7-308f-48c7-afee-35d38415aa0b")]
	#endregion
	[Inherit(typeof(PartyInterface))]
	public partial class OrganisationClass : Class
	{
		#region Allors
		[Id("1c8bf2e3-6794-47c8-990c-f124d47653fb")]
		[AssociationId("d60f70d2-a17e-47d9-bccc-7971f5ef776d")]
		[RoleId("d0f185d6-1ae2-40bf-a95e-6fde7ae10fa9")]
		#endregion
		[Indexed]
		[Type(typeof(LegalFormClass))]
		[Plural("LegalForms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType LegalForm;

		#region Allors
		[Id("2cc74901-cda5-4185-bcd8-d51c745a8437")]
		[AssociationId("896a4589-4caf-4cd2-8365-c4200b12f519")]
		[RoleId("baa30557-79ff-406d-b374-9d32519b2de7")]
		#endregion
		[Indexed]
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("4cc8bc02-8305-4bd3-b0c7-e9b3ecaf4bd2")]
		[AssociationId("c2be4896-2eae-40fa-9300-b548741407f2")]
		[RoleId("a26de636-8efa-4df4-b56d-225ac25f31a8")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(UserGroupClass))]
		[Plural("CustomerContactUserGroups")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CustomerContactUserGroup;

		#region Allors
		[Id("786a74b0-015a-47db-8d3a-c790b326cc7d")]
		[AssociationId("6f7363d4-46c5-4bcb-b19c-314733af9e9e")]
		[RoleId("1c339b5d-6f97-41bd-952a-3706d383c3d8")]
		#endregion
		[Indexed]
		[Type(typeof(ImageClass))]
		[Plural("LogoImages")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType LogoImage;

		#region Allors
		[Id("78837f12-05d3-49f1-a607-43e96120bcf0")]
		[AssociationId("0df49189-f6a1-49cf-97c5-ab40e3087b6e")]
		[RoleId("d03e4b6a-6741-4290-a590-18e32b4a6e43")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(UserGroupClass))]
		[Plural("PartnerContactGroups")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType PartnerContactUserGroup;

		#region Allors
		[Id("813633df-c6cb-44a6-9fdf-579aa8180ebd")]
		[AssociationId("4e4c1ca5-43e1-4567-8f1e-636197ca72b7")]
		[RoleId("e5c40212-c5c5-44a1-8f18-f5d3dbeec9ca")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("TaxNumbers")]
		public RelationType TaxNumber;

		#region Allors
		[Id("a5318bd4-da7d-48bd-9d41-00c3261caa09")]
		[AssociationId("baae72eb-acf3-4dce-b480-fce90e124de3")]
		[RoleId("6458017c-a4bf-4815-a486-66d654f3801a")]
		#endregion
		[Indexed]
		[Type(typeof(IndustryClassificationClass))]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType IndustryClassification;

		#region Allors
		[Id("af80efaf-7ef1-4625-9717-564eef0504c4")]
		[AssociationId("ff2bb57b-4aaf-4c61-b282-6ce0852e8546")]
		[RoleId("844af39b-fae2-4d94-9e67-ff6d97152736")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(UserGroupClass))]
		[Plural("SupplierContactUserGroups")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType SupplierContactUserGroup;

		#region Allors
		[Id("d0ac426e-4775-4f2f-8055-08cb84e8e9bd")]
		[AssociationId("e8677033-8927-4a52-b210-9a98558625ba")]
		[RoleId("8e2fd09a-eda9-47e2-8908-2527e947ffd1")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(OrganisationClassificationInterface))]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType OrganisationClassification;

		public static OrganisationClass Instance {get; internal set;}

		internal OrganisationClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[LocalisedInterface.Instance.Locale.RoleType].IsRequiredOverride = true;
        }
    }
}