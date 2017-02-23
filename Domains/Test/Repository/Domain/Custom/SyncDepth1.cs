namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("FBC46730-B636-4333-989C-53D5F76A32A0")]
    #endregion
    public partial interface SyncDepth1 : Object 
    {
        #region Allors
        [Id("BC3991AE-475D-4CA2-A8E1-6DF5CCC65CE0")]
        [AssociationId("F16FD291-8211-4FC8-98A5-2915951538CC")]
        [RoleId("3A73B3DA-F8BE-4971-969B-1A4A64A94FE2")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Synced]
        #endregion
        SyncDepth2 SyncDepth2 { get; set; }
    }
}