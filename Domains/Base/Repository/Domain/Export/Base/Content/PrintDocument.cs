namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("6161594B-8ACF-4DFA-AE6D-A9BC96040714")]
    #endregion
    public partial class PrintDocument : AccessControlledObject, Deletable
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("4C5C2727-908C-4FB2-9EB5-DA31837422FC")]
        [AssociationId("0E33FC4F-B3D8-4EA8-AD03-E477FA1AD1E8")]
        [RoleId("6FEA8BBD-9D58-4CE7-B5BA-B9235FA9194C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public Media Media { get; set; }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
            throw new System.NotImplementedException();
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }
        #endregion
    }
}