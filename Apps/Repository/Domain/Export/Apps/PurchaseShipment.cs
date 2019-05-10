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
        #region PurchaseShipmentState
        #region Allors
        [Id("BD0653C6-B660-4C56-8E11-BA9F7B45EF0C")]
        [AssociationId("21312E30-ECAF-44A2-85FA-203A15896481")]
        [RoleId("80BCEA84-B30D-4A93-92CC-FCA4129C1462")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseShipmentState PreviousPurchaseShipmentState { get; set; }

        #region Allors
        [Id("20AD82AC-8275-419D-94C6-EF140BD12A15")]
        [AssociationId("78483FCF-7E8E-4BEC-B34B-74841F22243D")]
        [RoleId("4DD8B780-DDDE-4C32-9350-BACED5AA574D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseShipmentState LastPurchaseShipmentState { get; set; }

        #region Allors
        [Id("6B5F23D4-40FB-4F6E-BBDB-F3E4B15DB8E3")]
        [AssociationId("3AA4C6E1-5945-4390-B6FF-8305572DD527")]
        [RoleId("AA1E7849-0B5E-4EC8-A0AA-83D4FFA75B18")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseShipmentState PurchaseShipmentState { get; set; }
        #endregion
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

        #region Allors
        [Id("56ED269D-B023-47D5-B138-079999969CED")]
        [AssociationId("403D0108-D2AF-48F8-BEC3-4FCCAA513046")]
        [RoleId("4613570B-6A9B-4798-A3D9-77550E024A16")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InternalOrganisation Receiver { get; set; }

        #region Allors
        [Id("40277d59-6ab8-40b0-acee-c95ba759e2c8")]
        [AssociationId("d7feb989-dd2d-4619-b079-8a059129f8ed")]
        [RoleId("068d5263-18d7-40e4-80c1-4f9a8e88d10a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Facility Facility { get; set; }

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