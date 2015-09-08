using System.ComponentModel.DataAnnotations;

namespace Allors.Meta
{
	using System;

	public partial class WorkEffortInterface
	{
        #region Allors
        [Id("A8D6C356-6AB3-47EA-A0F7-25FBFB711A81")]
        #endregion
        public MethodType Confirm;

        #region Allors
        [Id("860F33C9-7CD9-427D-9FFD-93B1274C9EB2")]
        #endregion
        public MethodType Finish;

        #region Allors
        [Id("0A66E9CA-89A8-4D5A-B63F-E061CDBC0A2E")]
        #endregion
        public MethodType Cancel;

        #region Allors
        [Id("A1189C0F-8E2E-41B7-B61E-36525B3895B5")]
        #endregion
        public MethodType Reopen;

	    internal override void AppsExtend()
	    {
            this.Description.RoleType.IsRequired = true;
            this.CurrentObjectState.RoleType.IsRequired = true;
            this.ActualStart.RoleType.DataTypeAttribute = new DataTypeAttribute(DataType.DateTime);
            this.ActualCompletion.RoleType.DataTypeAttribute = new DataTypeAttribute(DataType.DateTime);
            this.ScheduledStart.RoleType.DataTypeAttribute = new DataTypeAttribute(DataType.DateTime);
            this.ScheduledCompletion.RoleType.DataTypeAttribute = new DataTypeAttribute(DataType.DateTime);
        }
	}
}