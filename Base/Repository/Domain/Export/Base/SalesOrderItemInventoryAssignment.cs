namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("7975E28B-4707-44E2-8B56-0EAA8D7F9EFD")]
    #endregion
    public partial class SalesOrderItemInventoryAssignment : Versioned
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Allors
        [Id("BD8AB8B1-2913-413A-B7B1-CFEF5279330A")]
        [AssociationId("D084D31F-C790-435B-9E55-A26077BD69F6")]
        [RoleId("DF90990C-77C3-4E5D-949B-419824BB9FFF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public SalesOrderItem SalesOrderItem { get; set; }

        #region Allors
        [Id("BDE5FF54-505B-4241-88A8-334999E43C0B")]
        [AssociationId("8D8A2866-A248-4E1B-8DBD-F02B7BDB4B43")]
        [RoleId("61617EC6-FD7E-4EFC-9CB1-928A9953F0A4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InventoryItem InventoryItem { get; set; }

        #region Allors
        [Id("B5997790-CAC7-464A-91AD-AC01E6F1D57F")]
        [AssociationId("7C78AF6A-D78F-401D-9E02-36DEFDD34167")]
        [RoleId("16543ABC-223E-4CC4-9395-713533E46924")]
        #endregion
        [Required]
        [Workspace]
        public decimal Quantity { get; set; }

        #region Allors
        [Id("063A2F35-2CC0-4ACA-935D-31BBEDC0A2C6")]
        [AssociationId("5C0543A5-3894-4921-AD19-10065F56824E")]
        [RoleId("5447B345-8A1D-42B5-A5D9-0C22F8E5ACF5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Workspace]
        public InventoryItemTransaction[] InventoryItemTransactions { get; set; }

        #region Versioning
        #region Allors
        [Id("E3E0F9C1-7B16-4C9C-B1A0-57F7A7CA50AF")]
        [AssociationId("97B415F1-2FCE-4FB5-9B89-A8BE5F940E66")]
        [RoleId("C5A18B75-823E-405C-884C-7CD73EEEBF3C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderItemInventoryAssignmentVersion CurrentVersion { get; set; }

        #region Allors
        [Id("FE0F0C2F-1198-46E8-9423-09A736BDF24A")]
        [AssociationId("6511DD82-6FE1-4299-B195-E500F2094755")]
        [RoleId("4B7828B7-DED8-4317-9332-AD1A598B4DEB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderItemInventoryAssignmentVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}