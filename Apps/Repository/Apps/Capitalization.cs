namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("a0a753be-15ca-49e2-8f5f-f956fa132f49")]
    #endregion
    public partial class Capitalization : InternalAccountingTransaction 
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


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion

    }
}