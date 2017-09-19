namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("f3ef0124-e867-4da2-9323-80fbe1f214c2")]
    #endregion
    public partial interface OrderItem : AccessControlledObject, Commentable, Transitional, Priceable
    {
        #region Allors
        [Id("7D6B04D2-062C-45B8-96AB-DC41A3DECAF8")]
        [AssociationId("4ED896CE-3278-49B2-A3A7-A25AAB4FB072")]
        [RoleId("0A0AE42C-2676-4BA3-BA86-9357E1388C02")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("30493d04-3298-4888-8ee4-b8995d9cd5a1")]
        [AssociationId("0ab1707d-be04-49c2-a6b1-b6a17eb0a195")]
        [RoleId("95bd36e9-a956-46d2-b2b5-7d7d0f73c411")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        BudgetItem BudgetItem { get; set; }

        #region Allors
        [Id("38cd5e9e-7305-4c56-bff7-13918bd9f059")]
        [AssociationId("d21a1eff-5920-4dbb-9fcb-8f99ea1187f9")]
        [RoleId("a6e6c1d9-0009-4d5a-bebd-6e62c1d71a5a")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal PreviousQuantity { get; set; }

        #region Allors
        [Id("454f28cf-bf52-4465-83e4-e871ec36c491")]
        [AssociationId("5abe5891-40b6-4b87-a587-e6a2c7658c64")]
        [RoleId("c6d6c5dd-9239-45ae-970c-3716443bed29")]
        #endregion
        [Workspace]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal QuantityOrdered { get; set; }

        #region Allors
        [Id("6da42dec-ba03-4615-badb-9113a82ff2f7")]
        [AssociationId("f8b1946c-f4d3-4c9b-89c3-371b8ce1e329")]
        [RoleId("29fa13ea-307f-49ed-86ad-ff8321911013")]
        #endregion
        [Workspace]
        [Size(256)]
        string Description { get; set; }

        #region Allors
        [Id("70f92965-d99a-4a6a-bc27-029eec7b5c2d")]
        [AssociationId("93ffaaa6-7401-4b7f-a297-081a98bee032")]
        [RoleId("233fd990-758e-4f1d-87bd-ae3de8d9486b")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        PurchaseOrder CorrespondingPurchaseOrder { get; set; }

        #region Allors
        [Id("84faada9-1bdc-4c08-8892-760eb0cee2ba")]
        [AssociationId("3166f432-b675-474a-9a1f-7e558cc1dc58")]
        [RoleId("a2a2b2e8-0477-4b79-8602-7ca37fb17372")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalOrderAdjustmentCustomerCurrency { get; set; }

        #region Allors
        [Id("8f06f480-ff7e-4e34-bb7e-6f1271dcc551")]
        [AssociationId("dcdcdd88-63b5-4680-80a7-e915abe1cc98")]
        [RoleId("8b0d5be1-a4ed-49de-8913-bdafc5da57ae")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalOrderAdjustment { get; set; }

        #region Allors
        [Id("9674f349-3fcc-495c-b7eb-27b5b580597c")]
        [AssociationId("d7c3f753-9db0-4bb5-9b5d-4adbc695e320")]
        [RoleId("34e4011b-c124-4d53-ab30-26734e8ba04c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        QuoteItem QuoteItem { get; set; }

        #region Allors
        [Id("9dc87cdb-a35f-4a48-9f99-bf0fe07cad5c")]
        [AssociationId("b6f17e6b-f61a-4155-8e4c-79ebec1a01d4")]
        [RoleId("9ec4f475-ecb7-4d57-a642-043b0a703094")]
        #endregion
        [Workspace]
        DateTime AssignedDeliveryDate { get; set; }

        #region Allors
        [Id("a1769a74-d832-4ade-be59-a98b17033ca1")]
        [AssociationId("72f9c5a1-a66a-4181-b683-c0546f7cb95d")]
        [RoleId("279735e0-974a-46b3-b460-2bd528895f5a")]
        #endregion
        [Derived]
        [Workspace]
        DateTime DeliveryDate { get; set; }

        #region Allors
        [Id("b82c7b21-5ade-40b6-ba5d-62b6384eaaec")]
        [AssociationId("0f950d26-6a3f-4140-9273-7fe886f06582")]
        [RoleId("4aefa88c-6ce9-441e-a8a7-e65b2271c3b9")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Indexed]
        OrderTerm[] OrderTerms { get; set; }

        #region Allors
        [Id("ce398ebb-3b1e-476e-afd5-d32518542b70")]
        [AssociationId("d7f29cb6-bae1-41ce-bc67-c37c38f0ba73")]
        [RoleId("49a61617-586a-4b7b-bcc0-e2cf1f4cdee4")]
        #endregion
        [Workspace]
        [Size(-1)]
        string ShippingInstruction { get; set; }

        #region Allors
        [Id("dadeac55-1586-47ce-9983-2113179e275d")]
        [AssociationId("f6bdee3b-d274-4bd6-841e-7dc3d373083f")]
        [RoleId("6a038221-f3ec-4fd0-a235-7f6205404113")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Indexed]
        OrderItem[] Associations { get; set; }

        #region Allors
        [Id("feeed27a-c421-476c-b233-02d2fb9db76d")]
        [AssociationId("d1458a15-e035-4b07-a6b8-5a9af704a4ac")]
        [RoleId("34e046c2-881c-43e7-8c67-c14c595ac074")]
        #endregion
        [Workspace]
        [Size(-1)]
        string Message { get; set; }


        #region Allors
        [Id("5368A2C3-9ADF-46A3-9AC0-9C4A03DEAF9A")]
        #endregion
        [Workspace]
        void Cancel();

        #region Allors
        [Id("29D93AE6-FD73-408F-A8F0-CD05D96CF102")]
        #endregion
        [Workspace]
        void Reject();

        #region Allors
        [Id("9C496948-13BA-41C6-B8CB-60323AF3B3E9")]
        #endregion
        [Workspace]
        void Confirm();

        #region Allors
        [Id("DA334EDA-0CD3-4AB4-89C5-41C69D596C7C")]
        #endregion
        [Workspace]
        void Approve();

        #region Allors
        [Id("77ACDB9B-9A55-44C6-AAC4-99ACE0924EDB")]
        #endregion
        [Workspace]
        void Finish();

        #region Allors
        [Id("CBA21197-F595-4526-8DB3-43382CCD08E4")]
        #endregion
        [Workspace]
        void Delete();
    }
}