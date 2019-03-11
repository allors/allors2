namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("4a70cbb3-6e23-4118-a07d-d611de9297de")]
    #endregion
    public partial class SerialisedInventoryItem : InventoryItem
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public Permission[] DeniedPermissions { get; set; }
        
        public SecurityToken[] SecurityTokens { get; set; }
        
        public Guid UniqueId { get; set; }
        
        public Part Part { get; set; }
        
        public string Name { get; set; }
        
        public Lot Lot { get; set; }
        
        public UnitOfMeasure UnitOfMeasure { get; set; }
        
        public Facility Facility { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region SerialisedInventoryItemState
        #region Allors
        [Id("CCB71B4F-1A3F-4D08-B3E4-380FB2D513FF")]
        [AssociationId("D35F6D66-DAA2-4044-B4E9-FBCFBC7D2CD9")]
        [RoleId("35C5FABD-1F83-4D6C-8268-F027CC9F7B51")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SerialisedInventoryItemState PreviousSerialisedInventoryItemState { get; set; }

        #region Allors
        [Id("72A268C1-4A32-48C1-BB2D-837AC1DF361E")]
        [AssociationId("0ED35F86-9400-4F89-8F9D-A8D6A7408A78")]
        [RoleId("DF809B37-E9DA-463C-B532-02E44BC0394F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SerialisedInventoryItemState LastSerialisedInventoryItemState { get; set; }

        #region Allors
        [Id("7E757767-61AC-49E9-97CF-DE929C015D5B")]
        [AssociationId("60B25B4C-B160-498C-A3CF-EBB057EACACC")]
        [RoleId("87B18D10-A205-40E7-8403-733791AF3FD9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedInventoryItemState SerialisedInventoryItemState { get; set; }
        #endregion

        #region Versioning
        #region Allors
        [Id("235F117A-3288-4729-8348-92BCEBCDB3B6")]
        [AssociationId("63D481C8-C311-4789-9145-A0AA9F8648FD")]
        [RoleId("F5D5D294-C53E-4174-BE73-687400481205")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SerialisedInventoryItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("14266ECC-B4FF-4365-9087-0F67946246D2")]
        [AssociationId("3B2D539D-3886-4614-B4D5-170F5A4D77DD")]
        [RoleId("FCDED27A-83F2-4D97-A74A-49ED05F5C212")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SerialisedInventoryItemVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("B0753263-46F0-409E-8445-2B7E261DD7F6")]
        [AssociationId("A69A08A0-23E2-4E75-AD09-14BA88F480C1")]
        [RoleId("FCB6A221-E020-4757-8DF2-3ABC7114309D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public SerialisedItem SerialisedItem { get; set; }

        #region Allors
        [Id("C3C73F0D-2B82-460E-9C58-272AB7CFE8E4")]
        [AssociationId("13DC2350-3609-4B34-8327-39B652F790E1")]
        [RoleId("AE7A4060-D7E7-4EF3-8853-4971354919CB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedInventoryItemState InventoryItemState { get; set; }

        #region Allors
        [Id("2BE471C7-7C0C-436C-B55A-14930D2A8A5C")]
        [AssociationId("3327F140-B539-4053-B295-0B581914CBD1")]
        [RoleId("A3898BB3-461A-45E6-9E4D-02E51476D73F")]
        #endregion
        [Derived]
        [Required]
        [Workspace]
        public int Quantity { get; set; }

        #region inherited methods
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Delete() { }
        #endregion
    }
}