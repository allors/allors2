namespace Allors.Meta
{
    public partial class RequirementInterface
    {
        #region Allors
        [Id("B96906C0-83CB-48D5-A67C-8E3E05073B14")]
        #endregion
        public MethodType Reopen;

        #region Allors
        [Id("442F8C38-F7FC-44E4-8E67-00B8A535BD32")]
        #endregion
        public MethodType Cancel;
        
        #region Allors
        [Id("375A74B7-CBCE-4300-B9E3-96591A071CDF")]
        #endregion
        public MethodType Hold;
        
        #region Allors
        [Id("2F892E5F-ABB0-419F-BD6E-F593E07FC40E")]
        #endregion
        public MethodType Close;

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
            this.CurrentObjectState.RoleType.IsRequired = true;
        }
    }
}