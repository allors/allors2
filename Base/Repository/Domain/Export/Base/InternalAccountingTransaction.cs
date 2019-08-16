namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("5a783d98-845a-4784-9c92-5c75a4af3fb8")]
    #endregion
    public partial interface InternalAccountingTransaction : AccountingTransaction
    {
        #region Allors
        [Id("EF969E4C-ADD5-4A3D-A718-857CC99BBACA")]
        [AssociationId("96EC91E8-9035-48F5-A4A6-80269EE138A4")]
        [RoleId("6528D87E-DB0F-4EDE-95BB-54E5126C5F81")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        InternalOrganisation InternalOrganisation { get; set; }
    }
}