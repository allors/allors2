namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("1a5195d6-8fff-4590-afe1-3f50c4fa0c67")]
    #endregion
    public partial class ReceiptAccountingTransaction : ExternalAccountingTransaction 
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

        #region Allors
        [Id("a69440e0-34f3-42ce-9f21-38ea22c5762e")]
        [AssociationId("0d841c4a-1f7b-443d-95a6-29a1205f203c")]
        [RoleId("52a40ec0-a108-491a-9f41-94885fcb09b5")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]

        public Receipt Receipt { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion

    }
}