namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("55DE0F4F-2ABD-4943-8319-39DC5D51B0D7")]
    #endregion
    public partial interface ProductIdentification : AccessControlledObject, Deletable
    {
        #region Allors
        [Id("80CE30EE-71CF-4E74-8D40-C0BCD9239A9C")]
        [AssociationId("B2DF68FF-69A7-497E-AE31-644D897C4D69")]
        [RoleId("8994D941-E67F-4B5B-B86F-C9E651CF6571")]
        #endregion
        [Required]
        [Workspace]
        string Identification { get; set; }

        #region Allors
        [Id("2B6445D3-EE83-4C9C-BEAC-867A3E823B9B")]
        [AssociationId("D409517B-9D4E-4185-A58F-10618854FC9C")]
        [RoleId("98A39FCA-EA32-4E93-81F3-A708EAFB63CF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        ProductIdentificationType ProductIdentificationType { get; set; }
   }
}