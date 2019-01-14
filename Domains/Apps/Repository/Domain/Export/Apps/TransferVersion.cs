namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("827E2A7D-2FEF-4511-9EA4-9D2A3BF76153")]
    #endregion
    public partial class TransferVersion : ShipmentVersion 
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

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
        [Id("25E5B113-B5F6-4885-9535-FA34E4DB2D82")]
        [AssociationId("5F3593E5-1E0D-49DA-A270-2D5810980693")]
        [RoleId("13878D7F-CB57-4E42-96B7-6931AC46AE2B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public TransferState TransferState { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion
   }
}