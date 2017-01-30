namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("27b6630a-35d0-4352-9223-b5b6c8d7496b")]
    #endregion
    public partial class PickList : AccessControlledObject, Printable, Transitional, UniquelyIdentifiable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string PrintContent { get; set; }

        public Guid UniqueId { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        #endregion

        #region Allors
        [Id("0bdfcd8a-af37-41a7-be2c-db7848d4fd05")]
        [AssociationId("88919577-6835-4c84-9e3d-a1ec50fc5c2b")]
        [RoleId("6042abcd-a859-42bb-818d-9409f7b08d7a")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        public CustomerShipment CustomerShipmentCorrection { get; set; }
        
        #region Allors
        [Id("1176ffe1-efff-4c02-b4df-5bba9052f6da")]
        [AssociationId("dcb3602c-f60e-4798-b32d-2a69f9e1056b")]
        [RoleId("920c6a7e-b8b8-4155-9209-4c8ed24a023a")]
        #endregion
        [Required]
        public DateTime CreationDate { get; set; }
        
        #region Allors
        [Id("3bb68c85-4e2b-42b8-b5fb-18a66c58c283")]
        [AssociationId("11fddfe2-9b04-4b53-a4ff-6f571e73c32a")]
        [RoleId("a139b102-f8a9-43f1-9b14-d3c76f7be294")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        public PickListItem[] PickListItems { get; set; }
        
        #region Allors
        [Id("4231c38e-e54c-480d-9e0f-2fe8bd101da1")]
        [AssociationId("b4d28461-6b82-4843-90ee-a5c3c0cddfc0")]
        [RoleId("11fa5c06-67ce-44e0-b205-e60be00e9922")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        public PickListObjectState CurrentObjectState { get; set; }
        
        #region Allors
        [Id("62239709-cd1f-4582-99f7-8f18e875e241")]
        [AssociationId("61ae7eeb-259c-44bb-9de7-aff577a66669")]
        [RoleId("fe4d009e-1ea4-43d2-b4ce-96a1d9af5cf7")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        public PickListStatus CurrentPickListStatus { get; set; }
        
        #region Allors
        [Id("6572f862-31b2-4be9-b7dc-7fff5d21f620")]
        [AssociationId("2a502d47-1319-45a4-ad52-70dd41435732")]
        [RoleId("76ddffff-4968-4b4b-8b52-58a1a05a774d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Person Picker { get; set; }
        
        #region Allors
        [Id("7b5e6ef5-e5c0-4e7c-8955-b6c18f136fee")]
        [AssociationId("ede5efc3-a840-44b5-8389-611c05ae4df2")]
        [RoleId("ec323cf6-acad-4e8b-bb73-0323e9aee277")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        public PickListStatus[] PickListStatuses { get; set; }
        
        #region Allors
        [Id("ae75482e-2c41-46d4-ab73-f3aac368fd50")]
        [AssociationId("6b8acd68-6aba-4092-8c87-cdc62d9a4c82")]
        [RoleId("61785577-8ab7-457c-870f-69ecb7c41f8b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Party ShipToParty { get; set; }
        
        #region Allors
        [Id("e334e938-35e7-4217-91fa-efb231f71a37")]
        [AssociationId("0706d8f1-764d-4ab9-b63a-1b0213cc9dbd")]
        [RoleId("4c3d2de1-6735-40fc-bfe9-65a64aaf966c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Store Store { get; set; }
        
        #region Allors
        [Id("46CB3076-14AE-48C1-8C9F-F4EFB4B060EB")]
        #endregion
        public void Hold() { }
        
        #region Allors
        [Id("F3D35303-BA28-4CF0-B393-7E7D76F5B86D")]
        #endregion
        public void Continue() { }
        
        #region Allors
        [Id("8753A40E-FAA1-44E7-86B1-6CA6712989B5")]
        #endregion
        public void Cancel() { }
        
        #region Allors
        [Id("CA2ADD8E-C43E-4C95-A8A4-B279FEE4CB0A")]
        #endregion
        public void SetPicked() { }
        
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion
    }
}