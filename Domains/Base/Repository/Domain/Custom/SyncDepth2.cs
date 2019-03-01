namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("B9996F8F-12FB-4E42-8B7F-907433A622B2")]
    #endregion
    [Synced]
    public partial class SyncDepth2 : Object, DerivationCounted
    {
        #region inherited properties
        public int DerivationCount { get; set; }
        #endregion

        #region inherited methods
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void OnPreFinalize()
        {
            
        }

        public void OnFinalize()
        {
            
        }

        public void OnPostFinalize()
        {
            
        }

        #endregion

        #region Allors
        [Id("C6254113-00CB-475E-AE15-B45FC3E623BC")]
        [AssociationId("91C77FB8-FDB9-4E7A-8BAD-A0747BA1A480")]
        [RoleId("748C78D8-F48F-4615-B0ED-3F1AA2193D06")]
        #endregion
        [Required]
        public int Value { get; set; }
    }
}