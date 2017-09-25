namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a0cf565a-2dcf-4513-9110-8c34468d993f")]
    #endregion
    public partial class PurchaseReturn : Shipment 
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
        [Id("01d0a8b8-0361-440f-8d96-967578262318")]
        [AssociationId("9a79ad26-180a-45fd-8b50-ca8c641e9f77")]
        [RoleId("e44876b6-c198-493a-8efc-4a4d09bd2b00")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]

        public PurchaseReturnStatus CurrentShipmentStatus { get; set; }
        #region Allors
        [Id("91b10295-d8d6-4240-914c-9ee8a6c21b96")]
        [AssociationId("47441947-8d72-4730-ab25-077dc80b4ca1")]
        [RoleId("ba9b3f52-0a0e-46ec-b3fb-d9330ebd5269")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]

        public PurchaseReturnObjectState CurrentObjectState { get; set; }
        #region Allors
        [Id("a5f3cf87-1730-4841-9df4-2638b10f3222")]
        [AssociationId("b1cb7246-2417-4618-bb03-decb38a0fc9f")]
        [RoleId("5ede1e3f-bed7-4603-adfe-f576e23a2e2f")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]

        public PurchaseReturnStatus[] ShipmentStatuses { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion

    }
}