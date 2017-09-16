namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("14660A5A-254F-4F7F-AD7B-E3C97745B0AE")]
    #endregion
    public partial interface IPurchaseOrder : IOrder
    {
        #region Allors
        [Id("15ea478f-b71d-412f-8ee4-abe554b9a7d8")]
        [AssociationId("e48c8211-2539-41ba-9250-27a08799b31b")]
        [RoleId("6ef2d258-4291-4a9f-b7f0-9f154b789775")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        PurchaseOrderItem[] PurchaseOrderItems { get; set; }

        #region Allors
        [Id("1638a432-3a4f-4cca-906e-660b9164838b")]
        [AssociationId("04f4151a-1adf-426a-9fb1-a0f8cc782b0e")]
        [RoleId("20131db5-50af-42a8-9ac8-fd250c1aa8b6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        Party PreviousTakenViaSupplier { get; set; }

        #region Allors
        [Id("36607a9e-d411-4726-a63c-7622b928bfe8")]
        [AssociationId("a8573588-3898-4422-92a2-056448200216")]
        [RoleId("31a6a1a2-92ee-4ffd-9eb8-d69e8f2183fd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        Party TakenViaSupplier { get; set; }

        #region Allors
        [Id("3b1f04bd-c9ad-4fca-981c-2ca243fdc292")]
        [AssociationId("e50e5e9b-f312-4520-81d1-1dbd7f856d0f")]
        [RoleId("64e08e15-4d4d-465b-9d9e-c9e7c971a56d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        PurchaseOrderObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("4830cfc5-0375-4996-8cd8-27e36c102b65")]
        [AssociationId("efa439f8-787e-43d7-bd1b-400cba7e3a62")]
        [RoleId("583bfc51-0bb7-4ea5-914c-33a5c2d64196")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        ContactMechanism TakenViaContactMechanism { get; set; }

        #region Allors
        [Id("7eceb1b6-1395-4655-a558-6d72ad4b380e")]
        [AssociationId("b6e1159c-fcb7-47f1-822b-4ab75e5dac14")]
        [RoleId("ab3ee3c7-dc02-4acf-a34e-6b25783e11fc")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        ContactMechanism BillToContactMechanism { get; set; }

        #region Allors
        [Id("b390a733-e322-4ada-9ead-75a8c9976337")]
        [AssociationId("6082b0af-f5ed-493c-bb2b-ad4764053819")]
        [RoleId("c725b348-df8f-4a64-adc2-c3d8b3b986b5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        InternalOrganisation ShipToBuyer { get; set; }

        #region Allors
        [Id("ccf88515-6441-4d0f-a2e7-8f5ed7c0533e")]
        [AssociationId("ce230886-53a7-4360-b545-a20d3cf47f1f")]
        [RoleId("2f7e7d1b-6a61-41a6-a05f-375e8a5feeb2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        Facility Facility { get; set; }

        #region Allors
        [Id("d74bd1fd-f243-4b5d-8061-1eafe7c25beb")]
        [AssociationId("5465663b-6757-4b1d-9f91-233bfd86bc5d")]
        [RoleId("35c28c9f-852a-4ebb-bc2b-1dce9e3812fa")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("f05e0ba5-4321-4d88-8f2c-8994de5b44b7")]
        [AssociationId("38d76559-6a9c-48c7-bde5-1a2e685b9a40")]
        [RoleId("a0b2ec91-5b7e-4abb-91fb-91836cb88490")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        InternalOrganisation BillToPurchaser { get; set; }
    }
}