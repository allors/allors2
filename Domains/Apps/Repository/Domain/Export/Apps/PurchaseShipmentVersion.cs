namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("BC4FAF1C-9ADC-4FAE-BCC5-818DA779CA6E")]
    #endregion
    public partial class PurchaseShipmentVersion : ShipmentVersion
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
        [Id("15CF6ACD-AF38-43C8-A64A-A7D6FAB3FCC0")]
        [AssociationId("E449A312-5963-454E-8107-0AE6B06AD566")]
        [RoleId("BEECCB94-F76C-4D07-94E4-C033172F9899")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseShipmentState PurchaseShipmentState { get; set; }

        #region Allors
        [Id("1BE298F2-F6A4-4F87-BEA5-9D01BB97B1B2")]
        [AssociationId("A6228FD8-6E88-415D-9659-7660D2A265E3")]
        [RoleId("B41C7693-05CE-4398-886B-07589F5A2D25")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public Facility Facility { get; set; }

        #region Allors
        [Id("E19B7B87-D820-4654-A712-84DEE73FD0E3")]
        [AssociationId("FA07F9AD-CA51-4DC3-A586-B793A8DA2C8F")]
        [RoleId("8B8DE300-F240-488A-AE7B-AAE6A21BE7E5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseOrder PurchaseOrder { get; set; }
        
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion
    }
}