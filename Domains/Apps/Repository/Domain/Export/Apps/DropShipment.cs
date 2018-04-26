namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a981c832-dd3a-4b97-9bc9-d2dd83872bf2")]
    #endregion
    public partial class DropShipment : Shipment, Versioned
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string HtmlContent { get; set; }

        public Media PdfContent { get; set; }

        public string Comment { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

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

        #region ObjectStates
        #region DropShipmentState
        #region Allors
        [Id("42DFF6EE-D2C5-4F4C-9141-CE616673AF79")]
        [AssociationId("4C674680-C49A-453E-8E39-9280BF7CD8C2")]
        [RoleId("47CA4444-E2D1-4D69-AFCB-7A533D1670CC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public DropShipmentState PreviousDropShipmentState { get; set; }

        #region Allors
        [Id("63FA97E4-5D5E-4FDF-A5CD-3E85891BF69E")]
        [AssociationId("F6829676-A383-49BD-AE3B-E8C7C893C7F2")]
        [RoleId("B3E67A51-D90B-4FE2-9C84-568BA0CC252F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public DropShipmentState LastDropShipmentState { get; set; }

        #region Allors
        [Id("68E02017-E090-4CA5-820F-02E99AC52A9F")]
        [AssociationId("59AB3925-33E9-446C-95DB-72BE123FE784")]
        [RoleId("CA14D7CF-64D6-4002-A214-E7EE8FC69DE6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public DropShipmentState DropShipmentState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("4073ADF9-15F7-43FA-95A0-A5338C438FBF")]
        [AssociationId("FE31357D-295F-4B9A-99D5-3B4848AFC1C5")]
        [RoleId("C149ED70-D5A7-4E55-86D6-0F7577483ABD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public DropShipmentVersion CurrentVersion { get; set; }

        #region Allors
        [Id("11ECBD90-95E7-43AE-863A-425B0DA7B66E")]
        [AssociationId("D5E74792-52A0-4875-9B32-587E65AF9B2D")]
        [RoleId("03E258CD-2211-487D-A4D6-D46C75B95C62")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public DropShipmentVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }





        #endregion
    }
}