namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("1248e212-ca71-44aa-9e87-6e83dae9d4fd")]
    #endregion
    public partial class Four : Object, Shared 
    {
        #region inherited properties
        #endregion


        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}