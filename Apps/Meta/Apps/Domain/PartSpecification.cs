namespace Allors.Meta
{
    public partial class PartSpecificationInterface
    {
        #region Allors
        [Id("6CDDFE33-E27E-40A3-AA1A-AB6C33732F42")]
        #endregion
        public MethodType Approve;

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
            this.CurrentObjectState.RoleType.IsRequired = true;
        }
    }
}