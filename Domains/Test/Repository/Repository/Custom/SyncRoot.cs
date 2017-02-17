namespace Allors.Repository.Domain
{
    #region Allors
    [Id("2A863DCF-C6FE-4838-8D3A-1212A2076D70")]
    #endregion
    public partial interface SyncRoot : Object 
    {
        #region Allors
        [Id("615C6C58-513A-456F-A0CE-E472D173DCB0")]
        [AssociationId("089D3E5D-6A3C-4B94-9162-65DEE526AA1F")]
        [RoleId("D1C3F53B-9E51-4574-A180-B3A927E41A0B")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Synced]
        #endregion
        SyncDepth1 SyncDepth1 { get; set; }
    }
}