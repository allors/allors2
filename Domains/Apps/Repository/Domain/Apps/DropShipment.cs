namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("a981c832-dd3a-4b97-9bc9-d2dd83872bf2")]
    #endregion
    public partial class DropShipment : Shipment 
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
        [Id("0984f98c-fc64-4c86-aeb6-1d804d1506db")]
        [AssociationId("f7de3d8b-e404-4652-8eb1-dc58f8307e14")]
        [RoleId("9ac05629-f7ae-422e-8131-78389ba7ecf9")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]

        public DropShipmentStatus[] ShipmentStatuses { get; set; }
        #region Allors
        [Id("44230591-89df-46ec-882c-09bbac7fd5d2")]
        [AssociationId("fa5c5391-6bf5-435c-ba35-08d5315216db")]
        [RoleId("a3b29fd7-cf97-4cbf-9329-681542e8de75")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]

        public DropShipmentStatus CurrentShipmentStatus { get; set; }
        #region Allors
        [Id("a7d6815b-9d6c-44c4-a80f-bc2fd8aa1ea7")]
        [AssociationId("14c83374-67ae-480b-a67d-597e8614480e")]
        [RoleId("9b4e523e-215a-4b2a-bd99-1540113e5fc3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]

        public DropShipmentObjectState CurrentObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion

    }
}