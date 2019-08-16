namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("4EA6AD12-C1FB-4661-B4F7-72B81435DD70")]
    #endregion
    public partial class SyncDepthC1 : SyncDepthI1
    {
        #region inherited properties
        public int DerivationCount { get; set; }

        public SyncDepth2 SyncDepth2 { get; set; }

        public int Value { get; set; }

        #endregion

        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}