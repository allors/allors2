namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("7dd7114a-9e74-45d5-b904-415514af5628")]
    #endregion
    public partial class CustomerReturn : Shipment 
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
        [Id("29a65898-2f91-4163-a5ed-ccb8cd5b17cb")]
        [AssociationId("145f4e1b-b26d-4e44-8cc9-3afb537c58b2")]
        [RoleId("71416b87-4614-4d87-886c-4fe2eb936f40")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]

        public CustomerReturnStatus CurrentShipmentStatus { get; set; }
        #region Allors
        [Id("e7586be1-f751-4ac6-940b-a65b50834619")]
        [AssociationId("ca71aca7-fa06-44d1-830a-3eaf5e2355a2")]
        [RoleId("3fb0c486-6e24-4f53-b7cf-f98596402d55")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]

        public CustomerReturnStatus[] ShipmentStatuses { get; set; }
        #region Allors
        [Id("fe3fd846-2d69-4d62-941b-dabc40a15e1f")]
        [AssociationId("82695003-f47f-4324-9a7c-d89949981354")]
        [RoleId("b2d65c28-fbff-430b-a7ca-39201ce655ad")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]

        public CustomerReturnObjectState CurrentObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion

    }
}