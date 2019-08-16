namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("7fd1760c-ee1f-4d04-8a93-dfebc82757c1")]
    #endregion
    public partial class Amortization : InternalAccountingTransaction
    {
        #region inherited properties

        public InternalOrganisation InternalOrganisation { get; set; }

        public AccountingTransactionDetail[] AccountingTransactionDetails { get; set; }

        public string Description { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal DerivedTotalAmount { get; set; }

        public AccountingTransactionNumber AccountingTransactionNumber { get; set; }

        public DateTime EntryDate { get; set; }

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
