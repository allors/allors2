namespace Allors.Repository
{
    using Attributes;

    #region

    [Id("62173428-589F-43FA-8FA5-5579F084B8E3")]

    #endregion
    public partial class Middle : DerivationCounted
    {
        #region inherited properties
        public int DerivationCount { get; set; }

        #endregion
        
        #region Allors
        [Id("27D0ABFD-EBA0-46FE-812C-C67D8E3D12D0")]
        [AssociationId("07B1D5C3-5027-44C2-B2BA-6AC8631CF238")]
        [RoleId("90CFF1BA-25CC-48A2-B554-46EB5152E644")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        public Right Right { get; set; }

        #region Allors
        [Id("4616201B-7C52-4C5D-B390-4D9C0A8CADAD")]
        [AssociationId("A82C0D55-DE39-46F5-824C-59FF7A9BA48A")]
        [RoleId("97B9CB7A-EA14-4869-B7BB-4893911F89D8")]
        #endregion
        [Required]
        public int Counter { get; set; }

        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnInit()
        {
            
        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        #endregion
    }
}