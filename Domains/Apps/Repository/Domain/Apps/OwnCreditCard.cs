namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("23848955-69ae-40ce-b973-0d416ae80c78")]
    #endregion
    public partial class OwnCreditCard : PaymentMethod, FinancialAccount 
    {
        #region inherited properties
        public decimal BalanceLimit { get; set; }

        public decimal CurrentBalance { get; set; }

        public Journal Journal { get; set; }

        public string Description { get; set; }

        public OrganisationGlAccount GlPaymentInTransit { get; set; }

        public string Remarks { get; set; }

        public OrganisationGlAccount GeneralLedgerAccount { get; set; }

        public bool IsActive { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public FinancialAccountTransaction[] FinancialAccountTransactions { get; set; }

        #endregion

        #region Allors
        [Id("7ca9a38c-4318-4bb6-8bc6-50e5dfe9c701")]
        [AssociationId("3dc97f13-b6b7-47eb-ae6c-b57b45a2f129")]
        [RoleId("0bfa9940-e320-4e52-903a-b6765830069a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Person Owner { get; set; }
        #region Allors
        [Id("e2514c8b-5980-4e58-a75f-20890ed79516")]
        [AssociationId("2f572644-647a-4d4e-b085-400ba3a88f7a")]
        [RoleId("81d792be-5f29-415e-8290-66b98a95e9e3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public CreditCard CreditCard { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion

    }
}