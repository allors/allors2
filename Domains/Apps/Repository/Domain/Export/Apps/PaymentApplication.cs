namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("6fef08f0-d4cb-42f4-a10f-fb31787f65c3")]
    #endregion
    public partial class PaymentApplication : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("1147413e-9b57-45b3-a15c-44923e83001a")]
        [AssociationId("fba37a60-bd3c-4218-a0b8-64d3f60a7057")]
        [RoleId("23382708-15b9-4d96-8c87-599c80fd2f74")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal AmountApplied { get; set; }
        
        #region Allors
        [Id("b5f00552-5976-4368-9f38-dc4734b1c4af")]
        [AssociationId("c51f9be5-aee5-43db-b986-78e076ded8bf")]
        [RoleId("df69429e-b21d-4b28-83b3-17f365ac444d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public InvoiceItem InvoiceItem { get; set; }

        #region Allors
        [Id("d2a02ce6-569d-41ae-b54d-4a2347b84835")]
        [AssociationId("6a5043e4-be26-42fb-80c5-f60ac6af0284")]
        [RoleId("8dc0f6e8-a4fd-44d0-93f5-963c3265033b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Invoice Invoice { get; set; }
        
        #region Allors
        [Id("deb07a2f-6344-4888-bd1a-97413e82700a")]
        [AssociationId("1c722ac2-b579-4707-8e27-0b0a23510293")]
        [RoleId("345cb457-a401-4590-b21c-de3a213a5626")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public BillingAccount BillingAccount { get; set; }
        
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}