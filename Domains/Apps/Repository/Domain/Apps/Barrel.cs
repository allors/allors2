namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("4cd7ab57-544c-4900-a854-4aa9c5284b81")]
    #endregion
    public partial class Barrel : Container 
    {
        #region inherited properties
        public Facility Facility { get; set; }

        public string ContainerDescription { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion
        
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion
    }
}