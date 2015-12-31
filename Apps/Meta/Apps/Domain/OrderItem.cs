namespace Allors.Meta
{
    public partial class OrderItemInterface
    {
        #region Allors
        [Id("AC6B2E9E-DC3B-4FA5-80B2-EA13C0461F5F")]
        #endregion
        public MethodType Cancel;

        #region Allors
        [Id("A1E84095-C5A3-4E4E-B449-FC400A3E0D06")]
        #endregion
        public MethodType Reject;

        #region Allors
        [Id("D0FDE3AB-EEC4-46C6-A545-30C4EB57B9D9")]
        #endregion
        public MethodType Confirm;

        #region Allors
        [Id("D3953352-DB9E-4A59-8504-A0C400DC515E")]
        #endregion
        public MethodType Approve;

        #region Allors
        [Id("C1517567-1708-47E6-8298-9D9B157E45FF")]
        #endregion
        public MethodType Finish;

        #region Allors
        [Id("3962ED58-44BD-4A79-8F0C-6A98ED88BD44")]
        #endregion
        public MethodType Delete;

        internal override void AppsExtend()
        {
            this.PreviousQuantity.RoleType.IsRequired = true;
            this.TotalOrderAdjustmentCustomerCurrency.RoleType.IsRequired = true;
            this.TotalOrderAdjustment.RoleType.IsRequired = true;
            this.QuantityOrdered.RoleType.IsRequired = true;
        }
    }
}