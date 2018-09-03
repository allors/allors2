namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a0cf565a-2dcf-4513-9110-8c34468d993f")]
    #endregion
    public partial class PurchaseReturn : Shipment, Versioned
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string HtmlContent { get; set; }

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
        #region PurchaseReturnState
        #region Allors
        [Id("DBC484A2-6EA0-47E9-8EAF-DFC5067CF34C")]
        [AssociationId("18AEA7F6-50FF-42FD-B7A1-1CFF26D74EDE")]
        [RoleId("50570016-7045-4B77-A97B-0DA372A68C7C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseReturnState PreviousPurchaseReturnState { get; set; }

        #region Allors
        [Id("589116AF-CE7E-4894-BD4D-8089FBBA7358")]
        [AssociationId("3ABA283D-9CD4-4D66-9CD3-F77ECCB20F0E")]
        [RoleId("D575BAEB-FE77-4544-91CD-C046A67452EC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseReturnState LastPurchaseReturnState { get; set; }

        #region Allors
        [Id("A65BBB49-2B63-4573-BBCC-5DDF13C86518")]
        [AssociationId("1D9C8895-23FC-4AC3-961D-CF53AEB7BE74")]
        [RoleId("97C8883C-E72A-4613-8DCB-4A6FE9C4E64C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseReturnState PurchaseReturnState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("0A657C34-9862-4129-B341-B3283F26905A")]
        [AssociationId("8B8F2E0D-1C1A-410F-AF09-68C9343427B2")]
        [RoleId("CAB7DA93-829B-4392-8A98-5FBBC6E16756")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseReturnVersion CurrentVersion { get; set; }

        #region Allors
        [Id("4F4AFCCC-4DFC-47B7-88C7-E7CEF1DB25DE")]
        [AssociationId("13AD8931-C8F8-4075-8772-901DCA7FB09F")]
        [RoleId("E7478EE8-5032-482F-900C-746F607A7306")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseReturnVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Invoice() { }
        #endregion
    }
}