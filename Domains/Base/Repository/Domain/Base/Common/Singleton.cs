namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("313b97a5-328c-4600-9dd2-b5bc146fb13b")]
    #endregion
    public partial class Singleton : AccessControlledObject
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("9c1634ab-be99-4504-8690-ed4b39fec5bc")]
        [AssociationId("45a4205d-7c02-40d4-8d97-6d7d59e05def")]
        [RoleId("1e051b37-cf30-43ed-a623-dd2928d6d0a3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public Locale DefaultLocale { get; set; }

        #region Allors
        [Id("9e5a3413-ed33-474f-adf2-149ad5a80719")]
        [AssociationId("33d5d8b9-3472-48d8-ab1a-83d00d9cb691")]
        [RoleId("e75a8956-4d02-49ba-b0cf-747b7a9f350d")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public Locale[] Locales { get; set; }

        #region Allors
        [Id("f16652b0-b712-43d7-8d4e-34a22487514d")]
        [AssociationId("c92466b5-55ba-496a-8880-2821f32f8f8e")]
        [RoleId("3a12d798-40c3-40e0-ba9f-9d01b1e39e89")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        public User Guest { get; set; }

        #region Allors
        [Id("6A6E0852-C984-47B8-939D-8E0B0B042B9D")]
        [AssociationId("E783AFBE-EF70-4AC1-8C0A-5DFE6FEDFBE0")]
        [RoleId("BCF431F6-10CD-4F33-873D-0B2F1A1EA09D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public SecurityToken InitialSecurityToken { get; set; }

        #region Allors
        [Id("f579494b-e550-4be6-9d93-84618ac78704")]
        [AssociationId("33f17e75-99cc-417e-99f3-c29080f08f0a")]
        [RoleId("ca9e3469-583c-4950-ba2c-1bc3a0fc3e96")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public SecurityToken DefaultSecurityToken { get; set; }

        #region Allors
        [Id("4D17A849-9AC9-4A5D-9F2A-EA0152061A15")]
        [AssociationId("6854E369-3026-47B1-AF0C-142A5C6FCA8E")]
        [RoleId("2C8B5D6D-0AF1-479D-B916-29F080856BD6")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        public AccessControl CreatorsAccessControl { get; set; }

        #region Allors
        [Id("f7e50cac-ab57-4ebe-b765-d63804924c48")]
        [AssociationId("cb47a309-ed8f-47d1-879f-478e63b350d8")]
        [RoleId("c955b6ef-57b7-404f-bba5-fa7aebf706f6")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        public AccessControl GuestAccessControl { get; set; }

        #region Allors
        [Id("829aa4a4-8232-4625-8cab-db7dc96da53f")]
        [AssociationId("56f18f8b-380b-4236-9a85-ed989c1a6e44")]
        [RoleId("a3b765ed-bbf6-4bc4-9551-6338705ef03e")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        public AccessControl AdministratorsAccessControl { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}