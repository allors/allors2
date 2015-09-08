namespace Allors.Meta
{
	public partial class RequirementInterface
	{
        #region Allors
        [Id("B96906C0-83CB-48D5-A67C-8E3E05073B14")]
        #endregion
        public MethodType Reopen;

	    internal override void AppsExtend()
        {
	        this.Description.RoleType.IsRequired = true;
            this.CurrentObjectState.RoleType.IsRequired = true;
        }
	}
}