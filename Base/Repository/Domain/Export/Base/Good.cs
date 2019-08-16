namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("7D9E6DE4-E73D-42BE-B658-729309129A53")]
    #endregion
    public partial interface Good : Product
    {
        #region Allors
        [Id("4e8eceff-aec2-44f8-9820-4e417ed904c1")]
        [AssociationId("30f4ec83-5854-4a53-a594-ba1247d02b2f")]
        [RoleId("80361383-e1fc-4256-9b69-7cd43469d0de")]
        #endregion
        [Size(256)]
        [Workspace]
        string BarCode { get; set; }

        #region Allors
        [Id("acbe2dc6-63ad-4910-9752-4cab83e24afb")]
        [AssociationId("70d193cf-8985-4c25-84a5-31f4e2fd2a34")]
        [RoleId("73361510-c5a2-4c4f-afe5-94d2b9eaeea3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Product[] ProductSubstitutions { get; set; }

        #region Allors
        [Id("e1ee15a9-f173-4d81-a11d-82abff076fb4")]
        [AssociationId("20928aed-02cc-4ea1-9640-cd31cb54ba13")]
        [RoleId("e1c65763-9c2d-4111-bca1-638a69490e99")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Product[] ProductIncompatibilities { get; set; }
    }
}
