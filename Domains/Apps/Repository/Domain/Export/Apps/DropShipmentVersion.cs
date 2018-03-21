namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("97E9E1C9-E35F-4139-8206-2C34021A0799")]
    #endregion
    public partial class DropShipmentVersion : ShipmentVersion
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

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
        #endregion

        #region Allors
        [Id("2045FACF-3F58-4A6F-94CB-CC8369619EBB")]
        [AssociationId("B6FE45C4-96E7-4047-A955-7F638345306D")]
        [RoleId("A5995342-202A-4EE2-A202-9964BF9BF520")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public DropShipmentState DropShipmentState { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion
    }
}