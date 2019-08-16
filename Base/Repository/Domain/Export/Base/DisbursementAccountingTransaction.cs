namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a3a5aeea-3c8b-43ab-94f1-49a1bd2d7254")]
    #endregion
    public partial class DisbursementAccountingTransaction : ExternalAccountingTransaction
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
        [Id("62c15bc4-42fd-45b8-ae5d-5cdf92c0b414")]
        [AssociationId("920ffcd4-6085-4d22-8d81-caf2dde21e70")]
        [RoleId("33b6e056-e1e2-4173-97db-485593bf9e36")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]

        public Disbursement Disbursement { get; set; }


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
