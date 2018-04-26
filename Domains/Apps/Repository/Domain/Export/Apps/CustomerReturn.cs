namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("7dd7114a-9e74-45d5-b904-415514af5628")]
    #endregion
    public partial class CustomerReturn : Shipment, Versioned
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
        #region CustomerReturnState
        #region Allors
        [Id("25DDE41F-7BBF-421A-8A3D-A1F7841DD59F")]
        [AssociationId("27ACF0AC-57D4-4ADF-909B-843678D75E24")]
        [RoleId("F3AB7B32-C2BF-4C3C-9AE5-9A192BE98537")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public CustomerReturnState PreviousCustomerReturnState { get; set; }

        #region Allors
        [Id("15026226-C170-40E9-A22A-DFD1C19AD9A0")]
        [AssociationId("77F515B2-CED2-43C3-B088-FADEF32792D3")]
        [RoleId("FA466EA5-DD8B-45C0-9792-1AEB134159B5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public CustomerReturnState LastCustomerReturnState { get; set; }

        #region Allors
        [Id("458732E6-EC51-4785-80A4-FD2B61E5CE4E")]
        [AssociationId("59462854-630E-4DAC-B7B7-425479C6E9D0")]
        [RoleId("509EB4E6-0473-48B6-AC58-B5BFD0A68019")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public CustomerReturnState CustomerReturnState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("3D250413-30C6-4BD9-A37E-7D5409F5CC96")]
        [AssociationId("F5816872-8DEE-4C16-AE29-343A76207F4E")]
        [RoleId("A9E3BD6E-5C05-4861-ABB1-35277857E6A1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public CustomerReturnVersion CurrentVersion { get; set; }

        #region Allors
        [Id("3E7D72CF-D5C1-43ED-883C-F408A06AB5A6")]
        [AssociationId("04BD14E3-C480-402F-A4B7-D9C9918E6000")]
        [RoleId("C118441D-364C-4B7B-B697-5AFC0564DC93")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public CustomerReturnVersion[] AllVersions { get; set; }
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