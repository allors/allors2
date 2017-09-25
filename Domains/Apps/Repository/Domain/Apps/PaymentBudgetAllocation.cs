namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("2e588028-5de2-411c-ab43-b406ca735d5b")]
    #endregion
    public partial class PaymentBudgetAllocation : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("28f23032-b81c-4dbb-aa6c-24740ae3bb26")]
        [AssociationId("451ff770-7f33-4251-b78b-907ad95a9c38")]
        [RoleId("1b82d64e-aeff-4150-b388-1144bef8b2ee")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Payment Payment { get; set; }
        #region Allors
        [Id("3245613c-71d3-4a5e-a687-6f5ac306d9df")]
        [AssociationId("3cf6b4ce-56df-44c9-9348-3a419a226edc")]
        [RoleId("e672774d-e11d-46df-9387-ef9f52315148")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public BudgetItem BudgetItem { get; set; }
        #region Allors
        [Id("a982dfa7-4c81-41d6-93ec-ea380a526ad0")]
        [AssociationId("cb680a9c-ba32-4ceb-b9fb-127041e509e5")]
        [RoleId("69f570b8-7f72-4ef5-934a-f7e7d0b0465d")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}