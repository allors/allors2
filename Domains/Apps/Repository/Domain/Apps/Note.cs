namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("587e017d-eb9a-412c-bd21-8ff91c42765b")]
    #endregion
    public partial class Note : ExternalAccountingTransaction 
    {
        #region inherited properties
        public Party FromParty { get; set; }

        public Party ToParty { get; set; }

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