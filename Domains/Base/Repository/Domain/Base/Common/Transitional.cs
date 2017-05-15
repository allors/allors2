namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("ab2179ad-9eac-4b61-8d84-81cd777c4926")]
    #endregion
    public partial interface Transitional : AccessControlledObject 
    {
        #region Allors
        [Id("6e27b0e8-2ac1-4ec8-90fe-6a38b7a2f690")]
        [AssociationId("52f880b3-09af-4842-bb44-e86cfd937e14")]
        [RoleId("2ff12d5c-2042-4b55-ba4b-1b6389a066c6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        ObjectState PreviousObjectState { get; set; }

        #region Allors
        [Id("f0d9a21f-0570-4dca-9555-ccd8aabbb8d8")]
        [AssociationId("1e2268c1-badf-49bd-81fe-c6122cbd1f81")]
        [RoleId("d9c8465b-3f59-4985-9ff3-c04be6a242de")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        ObjectState LastObjectState { get; set; }
    }
}