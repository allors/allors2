namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("5b294591-e20a-4bad-940a-27ae7b2f8770")]
    #endregion
    public partial class NonSerializedInventoryItem : InventoryItem 
    {
        #region inherited properties
        public InventoryItemVariance[] InventoryItemVariances { get; set; }

        public Part Part { get; set; }

        public Container Container { get; set; }

        public string Name { get; set; }

        public Lot Lot { get; set; }

        public string Sku { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public ProductCategory[] DerivedProductCategories { get; set; }

        public Good Good { get; set; }

        public Facility Facility { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("0958b237-ba88-48d3-b662-90328801b197")]
        [AssociationId("72957576-5146-4578-8526-8b7a50025526")]
        [RoleId("ebd546cd-7341-496d-86ca-27a1b8fc253e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]

        public NonSerializedInventoryItemObjectState CurrentObjectState { get; set; }
        #region Allors
        [Id("2959a4d0-5945-4231-8a12-a2d1bdb9be04")]
        [AssociationId("d48f3a6f-915f-42fe-a508-8cddc3cf3fbc")]
        [RoleId("bd3e6dd7-c339-4ac4-bdce-31526ed7fa1a")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityCommittedOut { get; set; }
        #region Allors
        [Id("2d07e267-a0dc-452d-8166-a376ee38700d")]
        [AssociationId("87520701-7447-46b2-8bff-c8a4e23092ae")]
        [RoleId("29532211-2b5b-4e8a-a27e-c7a1afb68370")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]

        public NonSerializedInventoryItemStatus[] NonSerializedInventoryItemStatuses { get; set; }
        #region Allors
        [Id("981acef5-652b-41c1-88f2-e06052bab7e3")]
        [AssociationId("3772d6b0-c994-4240-b8de-054b2c72b25f")]
        [RoleId("25a16b8b-3f26-4cf3-8452-c7933d54af2a")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]

        public NonSerializedInventoryItemStatus CurrentInventoryItemStatus { get; set; }
        #region Allors
        [Id("a6b78e16-6aef-4478-b426-9429c1a01059")]
        [AssociationId("9bcc50ce-a070-4cdd-802f-4296908b75f7")]
        [RoleId("a44947f1-b7e2-4f0c-97d6-2fd32ecae097")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityOnHand { get; set; }
        #region Allors
        [Id("ba5e2476-abdd-4d61-8a14-5d99a36c4544")]
        [AssociationId("f1e3216e-1af7-4354-b8ac-258bfa9222ac")]
        [RoleId("4d41e84c-ee79-4ce2-874e-a000e30c1120")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal PreviousQuantityOnHand { get; set; }
        #region Allors
        [Id("dfbd2b04-306c-415c-af67-895810b01044")]
        [AssociationId("c1ec09e8-2c1e-4e4a-9496-8c081dee23d9")]
        [RoleId("9a56d091-f6a8-4db1-bd65-10d84eaaaa05")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal AvailableToPromise { get; set; }
        #region Allors
        [Id("eb32d183-9c7b-47a7-ab38-e4966d745161")]
        [AssociationId("a7512a69-d27e-47dc-9da5-8713489cc2e5")]
        [RoleId("9aaf1a36-04b9-4cc5-9a22-691b3b3c4633")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityExpectedIn { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion

    }
}