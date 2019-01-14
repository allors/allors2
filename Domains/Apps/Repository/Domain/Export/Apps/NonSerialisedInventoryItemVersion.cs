namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("21C27A88-F99A-4871-B9D3-00C78F648574")]
    #endregion
    public partial class NonSerialisedInventoryItemVersion : InventoryItemVersion
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Part Part { get; set; }

        public string Name { get; set; }

        public Lot Lot { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public Facility Facility { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        #region Allors
        [Id("B3800237-4D03-4228-955D-7E573CEE47FA")]
        [AssociationId("5B906A1D-B127-4064-A944-401AFC9F86D7")]
        [RoleId("29FD8D29-6C87-4141-BE92-10723C0C49AE")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        public NonSerialisedInventoryItemState NonSerialisedInventoryItemState { get; set; }

        #region Allors
        [Id("E4CE5244-A60F-401B-836D-2D9EB5B9CDCA")]
        [AssociationId("EAC03D23-41B7-4EE1-AB3B-FE80AB4AE0A6")]
        [RoleId("4A520C92-095D-45EB-B72B-8CC721B5DF61")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityCommittedOut { get; set; }

        #region Allors
        [Id("D5DE8B3A-7526-419A-825F-B6BCF7847CC3")]
        [AssociationId("CA41E57D-BB4A-4CD2-A9B5-4F081339132A")]
        [RoleId("0EE40C3B-FB3F-41FF-9DE3-B9CA49E82D07")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityOnHand { get; set; }

        #region Allors
        [Id("01860D37-7452-440C-85C6-15C5178732B7")]
        [AssociationId("4ED29A37-C30D-45CC-945B-DF88488FDFB6")]
        [RoleId("5EE0DC1F-AD4D-4022-92B7-396274A40256")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal PreviousQuantityOnHand { get; set; }

        #region Allors
        [Id("BAE31508-C3D6-4A8B-A98D-BF1E2045A438")]
        [AssociationId("09C19EE6-DC24-4AA9-A3D8-D1C170CF31E6")]
        [RoleId("FFF2D824-35CF-48DC-889B-74F097EB66B1")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal AvailableToPromise { get; set; }

        #region Allors
        [Id("65A91257-858B-406F-9E27-ABEA58B360B8")]
        [AssociationId("9BB46D90-2238-4E22-A18F-24699B184E56")]
        [RoleId("DB9F489D-A247-4E6F-81FF-5AD0D89EA054")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityExpectedIn { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }
        #endregion
    }
}