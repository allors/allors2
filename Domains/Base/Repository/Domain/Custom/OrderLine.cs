namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region
    [Id("721008C3-C87C-40AB-966B-094E1271ED5F")]
    #endregion
    public partial class OrderLine : AccessControlledObject, Versioned
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion
        
        #region Allors
        [Id("7022167A-046E-45B3-A14E-AE0290C0F1D6")]
        [AssociationId("39DC23C3-9F36-48AE-94E6-8401FBAF8A4F")]
        [RoleId("7B145C97-85EB-4A51-ACEF-90B9A629EE31")]
        #endregion
        public decimal Amount { get; set; }

        #region Versioning
        #region Allors
        [Id("55F3D531-C58D-4FA7-B745-9E38D8CEC4C6")]
        [AssociationId("8B5CE991-9CC0-4419-B5A7-E2803F888A8E")]
        [RoleId("7663B87D-F17D-4822-A358-546124622073")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public OrderLineVersion CurrentVersion { get; set; }

        #region Allors
        [Id("CFC88B59-87A1-4F9E-ABBE-168694AB6CB5")]
        [AssociationId("2EA46390-F69F-436D-BCCC-84BEF6CD5997")]
        [RoleId("03585BB0-E87E-474F-8A76-0644D5C858F4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public OrderLineVersion[] AllVersions { get; set; }
        #endregion
        
        #region inherited methods

        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        #endregion
    }
}