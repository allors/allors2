namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("ef5fb351-2f0f-454a-b7b2-104af42b2c72")]
    #endregion
    public partial class PayCheck : Payment 
    {
        #region inherited properties
        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime EffectiveDate { get; set; }

        public Party SendingParty { get; set; }

        public PaymentApplication[] PaymentApplications { get; set; }

        public string ReferenceNumber { get; set; }

        public Party ReceivingParty { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("59ddff84-5e67-4210-b721-955e08f8453e")]
        [AssociationId("5d445586-f239-4e3b-a3cb-368d46df306f")]
        [RoleId("9a3b62ee-6197-4670-ad8b-c01201ea2235")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public Deduction[] Deductions { get; set; }
        #region Allors
        [Id("5db6f5b5-e24e-44fd-bc41-4e0466e97906")]
        [AssociationId("53d7d8c9-7028-4ec8-82af-6373e21e3532")]
        [RoleId("c2e4cf65-7a57-4dcd-ab49-c6cbc6b9d9fb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Employment Employment { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion

    }
}