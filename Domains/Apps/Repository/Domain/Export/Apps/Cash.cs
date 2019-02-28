namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("87fbf592-45a1-4ef2-85ca-f47d4c51abca")]
    #endregion
    public partial class Cash : PaymentMethod 
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

        #endregion

        #region Allors
        [Id("39c8beda-d284-442b-886a-6d6b2fb51cc8")]
        [AssociationId("f90e529a-8303-4a66-9622-144cfaed3bf3")]
        [RoleId("90ee494e-2194-4972-bdde-7a3a30aff736")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Person PersonResponsible { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion

    }
}