namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("cd66a79f-c4b8-4c33-b6ec-1928809b6b88")]
    #endregion
    public partial class Transfer : Shipment 
    {
        #region inherited properties
        public ShipmentMethod ShipmentMethod { get; set; }

        public ContactMechanism BillToContactMechanism { get; set; }

        public ShipmentPackage[] ShipmentPackages { get; set; }

        public string ShipmentNumber { get; set; }

        public Document[] Documents { get; set; }

        public Party BillToParty { get; set; }

        public Party ShipToParty { get; set; }

        public ShipmentItem[] ShipmentItems { get; set; }

        public InternalOrganisation BillFromInternalOrganisation { get; set; }

        public ContactMechanism ReceiverContactMechanism { get; set; }

        public PostalAddress ShipToAddress { get; set; }

        public decimal EstimatedShipCost { get; set; }

        public DateTime EstimatedShipDate { get; set; }

        public DateTime LatestCancelDate { get; set; }

        public Carrier Carrier { get; set; }

        public ContactMechanism InquireAboutContactMechanism { get; set; }

        public DateTime EstimatedReadyDate { get; set; }

        public PostalAddress ShipFromAddress { get; set; }

        public ContactMechanism BillFromContactMechanism { get; set; }

        public string HandlingInstruction { get; set; }

        public Store Store { get; set; }

        public Party ShipFromParty { get; set; }

        public ShipmentRouteSegment[] ShipmentRouteSegments { get; set; }

        public DateTime EstimatedArrivalDate { get; set; }

        public string PrintContent { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        #endregion

        #region Allors
        [Id("01757aca-7f45-4061-8721-1fa3d8cca852")]
        [AssociationId("b0b86e04-cd64-4a19-94dd-86ba558b478b")]
        [RoleId("d775ad19-df10-4941-b384-d0de7c3ed943")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]

        public TransferObjectState CurrentObjectState { get; set; }
        #region Allors
        [Id("2fc36280-2378-4c2d-aab1-b2f038a5cfa5")]
        [AssociationId("731be3ab-46e5-4ff9-acc7-c7d106f32896")]
        [RoleId("ea288e25-6d3c-4138-86fc-4e0fb86a088e")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]

        public TransferStatus CurrentShipmentStatus { get; set; }
        #region Allors
        [Id("e415cf27-7ae7-48a7-a889-ad90a7384a68")]
        [AssociationId("b205e173-5355-4dcc-a615-521b46e3759a")]
        [RoleId("96976d0f-10b8-4c67-a9a1-9b87b64eb46c")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]

        public TransferStatus[] ShipmentStatuses { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion

    }
}