namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("ca008b8d-584e-4aa5-a759-895b634defc5")]
    #endregion
    public partial class OwnBankAccount : PaymentMethod, FinancialAccount
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
        [Id("d83ca1e3-4137-4e92-a61d-0b8a1b8f7085")]
        [AssociationId("8a492054-a6be-4824-a0d2-daeed69c091b")]
        [RoleId("c90ac5e5-2368-45d1-bc4a-0621c30f20e5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public BankAccount BankAccount { get; set; }


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