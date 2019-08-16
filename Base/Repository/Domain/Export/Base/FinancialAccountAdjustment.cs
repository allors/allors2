namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("12ba6843-bae1-41d1-9ef2-c19d74b0a365")]
    #endregion
    public partial class FinancialAccountAdjustment : FinancialAccountTransaction
    {
        #region inherited properties
        public string Description { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime TransactionDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region inherited methods

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
