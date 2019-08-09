namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("FBC46730-B636-4333-989C-53D5F76A32A0")]
    #endregion
    [Synced]
    public partial interface SyncDepthI1 : Object, DerivationCounted
    {
        #region Allors
        [Id("BC3991AE-475D-4CA2-A8E1-6DF5CCC65CE0")]
        [AssociationId("F16FD291-8211-4FC8-98A5-2915951538CC")]
        [RoleId("3A73B3DA-F8BE-4971-969B-1A4A64A94FE2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Synced]
        SyncDepth2 SyncDepth2 { get; set; }

        #region Allors
        [Id("DCD1D766-99F4-4DD4-A8F8-F1BEBBB2BBB5")]
        [AssociationId("DB982324-FF36-4CD8-9399-974D61453ED0")]
        [RoleId("2FD1620E-6CC7-436E-9726-7F7B91E93E89")]
        #endregion
        [Required]
        int Value { get; set; }
    }
}