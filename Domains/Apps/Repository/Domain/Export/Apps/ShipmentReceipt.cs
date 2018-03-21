namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("48d14522-5fa8-44a8-ba4c-e2ddfc18e069")]
    #endregion
    public partial class ShipmentReceipt : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("0c4eee66-ff66-49fa-9a06-4ce3848a6d3c")]
        [AssociationId("d67a1bb9-802a-47a9-97bd-28809cd5c85a")]
        [RoleId("89d49ef1-a3b6-4404-97d9-024c66e0a1f6")]
        #endregion
        [Size(256)]

        public string ItemDescription { get; set; }
        #region Allors
        [Id("2bbc4476-7a06-4c36-9985-68a60b72eacd")]
        [AssociationId("c8ca8009-f3e9-4154-a94a-9e60f6165f3a")]
        [RoleId("5e776569-8dd4-4dd2-993b-5bbccc15ca58")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public NonSerialisedInventoryItem InventoryItem { get; set; }
        #region Allors
        [Id("87f84720-1233-4779-be9d-4b0a12ba19cd")]
        [AssociationId("77a773f7-e649-4dd1-9dd9-d7a5eb09ae95")]
        [RoleId("9cbd890b-c0b5-4a0c-a931-fc5601b5ef0d")]
        #endregion
        [Size(256)]

        public string RejectionReason { get; set; }
        #region Allors
        [Id("9a76f8ba-ae96-4040-81ce-59330392e77a")]
        [AssociationId("ca64ae22-fc6c-4747-a04b-ac77911c0c5e")]
        [RoleId("1d77d632-e552-4745-a5d6-ebefc3f0ec06")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public OrderItem OrderItem { get; set; }
        #region Allors
        [Id("9a9cce59-f45c-4da0-adb6-9583a1694921")]
        [AssociationId("8cd7d5ad-ca46-4fb2-9df0-edd213680dd6")]
        [RoleId("1f523e25-d883-4207-8550-d8d2c95c2ac6")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityRejected { get; set; }
        #region Allors
        [Id("ccd41d3d-2be8-47ca-8217-4e2aa1d1c03b")]
        [AssociationId("e823098b-5333-4466-b845-fe4a4f1b09f5")]
        [RoleId("7ec7aeb3-abdf-4bdc-bee2-535d8a722a6b")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]

        public ShipmentItem ShipmentItem { get; set; }
        #region Allors
        [Id("ecdd6b27-3bcf-4f61-8e21-f829503aeeb0")]
        [AssociationId("b326cf9d-8770-4686-a7f5-2061d1683bb4")]
        [RoleId("82ef73a5-8d4e-44a0-a551-b0c1dee958ca")]
        #endregion
        [Required]

        public DateTime ReceivedDateTime { get; set; }
        #region Allors
        [Id("f057b89e-3688-4172-9efa-102298c7e0e4")]
        [AssociationId("5d2edcc9-1f42-44b1-8680-fd2cd02761a0")]
        [RoleId("b6f78ebd-95a9-4649-b19f-bf965dc60150")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityAccepted { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}