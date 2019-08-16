namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("2bf859c6-de64-476f-a437-5eb57a778262")]
    #endregion
    public partial class PurchaseShipment : Shipment, Versioned
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public PrintDocument PrintDocument { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public ShipmentState PreviousShipmentState { get; set; }
        public ShipmentState LastShipmentState { get; set; }
        public ShipmentState ShipmentState { get; set; }
        public ShipmentMethod ShipmentMethod { get; set; }

        public ShipmentPackage[] ShipmentPackages { get; set; }

        public string ShipmentNumber { get; set; }

        public Document[] Documents { get; set; }

        public Person ShipFromContactPerson { get; set; }
        public Facility ShipFromFacility { get; set; }

        public Party ShipToParty { get; set; }

        public ShipmentItem[] ShipmentItems { get; set; }

        public PostalAddress ShipToAddress { get; set; }
        public Person ShipToContactPerson { get; set; }
        public Facility ShipToFacility { get; set; }

        public decimal EstimatedShipCost { get; set; }

        public DateTime EstimatedShipDate { get; set; }

        public DateTime LatestCancelDate { get; set; }

        public Carrier Carrier { get; set; }

        public DateTime EstimatedReadyDate { get; set; }

        public PostalAddress ShipFromAddress { get; set; }


        public string HandlingInstruction { get; set; }

        public Store Store { get; set; }

        public Party ShipFromParty { get; set; }

        public ShipmentRouteSegment[] ShipmentRouteSegments { get; set; }

        public DateTime EstimatedArrivalDate { get; set; }

        #endregion

        #region Versioning
        #region Allors
        [Id("828D99CE-40CC-47C1-95BD-CFF18DF096F3")]
        [AssociationId("D75E5851-7DFE-4F07-8D5F-B4ACC3B644F1")]
        [RoleId("18ACD0C0-F9B8-4FF7-9C98-EA1116B856F0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseShipmentVersion CurrentVersion { get; set; }

        #region Allors
        [Id("3F12F6E5-4080-48A7-A7EF-E5F2157DCB0C")]
        [AssociationId("EC80AE07-2302-4405-A593-8C6E2F190CB8")]
        [RoleId("94E4B4BB-200C-4B1B-9F29-F870DEF73634")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseShipmentVersion[] AllVersions { get; set; }
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

        public void Invoice() { }

        public void Print() { }
        #endregion
    }
}