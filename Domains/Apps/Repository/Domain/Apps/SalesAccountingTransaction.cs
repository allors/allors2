namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("0aecacff-23d0-48ff-8934-a4e5f711c729")]
    #endregion
    public partial class SalesAccountingTransaction : ExternalAccountingTransaction 
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
        [Id("9b376e18-7cf8-43f7-ac89-ef4b32a1c8fd")]
        [AssociationId("ee71978e-2085-48d2-81ad-571cfcec8264")]
        [RoleId("3fe3be8d-563d-4455-8f1a-7771ff97005f")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]

        public Invoice Invoice { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion

    }
}