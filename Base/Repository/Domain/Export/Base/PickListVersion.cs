namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("15B7482D-17F1-4184-9C57-222D41215553")]
    #endregion
    public partial class PickListVersion : Version
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        #region Allors
        [Id("4231c38e-e54c-480d-9e0f-2fe8bd101da1")]
        [AssociationId("b4d28461-6b82-4843-90ee-a5c3c0cddfc0")]
        [RoleId("11fa5c06-67ce-44e0-b205-e60be00e9922")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        [Workspace]
        public PickListState PickListState { get; set; }

        #region Allors
        [Id("9A818C4C-D7FD-48B8-A207-E169006BCED8")]
        [AssociationId("29C88DF6-0E07-4004-A313-40E9753E90C0")]
        [RoleId("53C7E7B1-F247-48F9-9689-B72B3DBD54A4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public CustomerShipment CustomerShipmentCorrection { get; set; }

        #region Allors
        [Id("C793C53B-23DB-44E1-8BAB-B62E3C65FD5F")]
        [AssociationId("28A963BF-2339-4832-AB6D-4DE6102C8957")]
        [RoleId("DBF878A2-F7BB-47BC-B78B-051CBC83D120")]
        #endregion
        [Required]
        [Workspace]
        public DateTime CreationDate { get; set; }

        #region Allors
        [Id("18C30DFC-988C-49E3-BC47-1EF70E54E004")]
        [AssociationId("F312EC2B-1EA3-4A2A-ADAD-138DCDC12369")]
        [RoleId("6BEA18E6-36C8-4BCA-BC52-0E861CAF7179")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PickListItem[] PickListItems { get; set; }

        #region Allors
        [Id("30DE831B-2D14-4E6C-9629-EEA958FDA6DD")]
        [AssociationId("01E1D3DC-A02D-4E5D-87CF-044BC91C381C")]
        [RoleId("545FF59B-3AE5-4A15-96C9-8A949B02E3FB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Person Picker { get; set; }

        #region Allors
        [Id("854D73F1-4D46-4B6D-9084-7D9A5497CA1A")]
        [AssociationId("37F6A8C4-E2F8-486F-B689-AC58BC5A8A31")]
        [RoleId("2C20AE9F-67C8-4E97-A10F-DA492CA9B0F3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ShipToParty { get; set; }

        #region Allors
        [Id("A0EC7FD3-298F-4453-A611-549125C2B646")]
        [AssociationId("7984ADB4-4366-40F1-B7CC-CB65566B8574")]
        [RoleId("C4B9F421-42AC-4E3E-9B31-2E70EC40737D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Store Store { get; set; }

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