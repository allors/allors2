namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("4a70cbb3-6e23-4118-a07d-d611de9297de")]
    #endregion
    public partial class SerialisedInventoryItem : InventoryItem, SerialisedInventoryItemVersioned
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public Guid UniqueId { get; set; }
        public string SerialNumber { get; set; }
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

        #endregion

        #region Allors
        [Id("e0fe2033-85a9-428d-9918-f543fbcf3ed7")]
        [AssociationId("49e8ccb2-8a3f-4846-8067-9f68d005e44f")]
        [RoleId("9d19f214-3ed9-4e2d-a924-2d513ca01934")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        public SerialisedInventoryItemObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("14266ECC-B4FF-4365-9087-0F67946246D2")]
        [AssociationId("3B2D539D-3886-4614-B4D5-170F5A4D77DD")]
        [RoleId("FCDED27A-83F2-4D97-A74A-49ED05F5C212")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToMany)]
        public SerialisedInventoryItemVersion[] AllVersions { get; set; }

        #region Allors
        [Id("235F117A-3288-4729-8348-92BCEBCDB3B6")]
        [AssociationId("63D481C8-C311-4789-9145-A0AA9F8648FD")]
        [RoleId("F5D5D294-C53E-4174-BE73-687400481205")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        public SerialisedInventoryItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("15DE96E6-4406-4D6F-80DA-581D96A91706")]
        [AssociationId("8798C7DF-D871-4834-B366-E2306E627D1F")]
        [RoleId("84F259F6-90B6-4C55-887C-7E6D97066A2A")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        public SerialisedInventoryItemVersion PreviousVersion { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion
   }
}