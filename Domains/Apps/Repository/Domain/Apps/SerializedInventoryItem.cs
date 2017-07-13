namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("4a70cbb3-6e23-4118-a07d-d611de9297de")]
    #endregion
    public partial class SerializedInventoryItem : InventoryItem 
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
        [Id("a07e8bbb-7bf3-42e1-bcc2-d922a180f5e0")]
        [AssociationId("035a8f39-9b2f-403c-ae64-c43299d59ac2")]
        [RoleId("e53e4d41-6518-4008-a419-522145e712af")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]

        public SerializedInventoryItemStatus[] InventoryItemStatuses { get; set; }
        #region Allors
        [Id("de9caf09-6ae7-412e-b9bc-19ece66724da")]
        [AssociationId("ba630eb8-3087-43c6-9082-650094a0226e")]
        [RoleId("c0ada954-d86e-46c3-9a99-09209fb812a5")]
        #endregion
        [Required]
        [Unique]
        [Size(256)]

        public string SerialNumber { get; set; }
        #region Allors
        [Id("e0fe2033-85a9-428d-9918-f543fbcf3ed7")]
        [AssociationId("49e8ccb2-8a3f-4846-8067-9f68d005e44f")]
        [RoleId("9d19f214-3ed9-4e2d-a924-2d513ca01934")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]

        public SerializedInventoryItemObjectState CurrentObjectState { get; set; }
        #region Allors
        [Id("fdc2607c-1081-4836-8aa5-1efb96e38da4")]
        [AssociationId("dc285060-57aa-4941-9335-c1b6e273f162")]
        [RoleId("82b912e8-34f9-4a11-a33b-4fdeb7e54ffc")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]

        public SerializedInventoryItemStatus CurrentInventoryItemStatus { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion

    }
}