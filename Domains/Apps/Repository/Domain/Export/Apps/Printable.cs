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
        [Size(-1)]
        [Workspace]
        string HtmlContent { get; set; }

        #region Allors
        [Id("5B7BAB41-E61A-47DD-AAE6-03FC2EAD97CF")]
        [AssociationId("8F2D29FE-4480-40F4-AF83-5DC30CA626F1")]
        [RoleId("C3B5E2DC-97C3-4BBA-99B7-5F20273A65D0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        Media PdfContent { get; set; }
    }
}