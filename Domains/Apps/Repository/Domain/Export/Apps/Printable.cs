namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("61207a42-3199-4249-baa4-9dd11dc0f5b1")]
    #endregion
    public partial interface Printable : Object
    {
        #region Allors
        [Id("079C31BA-0D20-4CD7-921C-A1829E226970")]
        [AssociationId("C98431FE-98EA-44EB-97C4-8D5F2C147424")]
        [RoleId("B3ECE72C-D62C-4F24-805A-34D7FF21DE4F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Media PrintDocument { get; set; }
    }
}