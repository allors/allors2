namespace Allors.Repository.Domain
{
    #region Allors
    [Id("eb0ff756-3e3d-4cf9-8935-8802a73d2df2")]
    #endregion
    public partial interface AccessControlledObject :  Object 
    {
        #region Allors
        [Id("5c70ca14-4601-4c65-9b0d-cb189f90be27")]
        [AssociationId("267053f0-43b4-4cc7-a0e2-103992b2d0c5")]
        [RoleId("867765fa-49dc-462f-b430-3c0e264c5283")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        Permission[] DeniedPermissions { get; set; }

        #region Allors
        [Id("b816fccd-08e0-46e0-a49c-7213c3604416")]
        [AssociationId("1739db0d-fe6b-42e1-a6a5-286536ff4f56")]
        [RoleId("9f722315-385a-42ab-b84e-83063b0e5b0d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        SecurityToken[] SecurityTokens { get; set; }
    }
}