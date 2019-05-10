namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("EB27ECDA-EE0D-4BC5-8FB1-88CF8501D7B0")]
    #endregion
    public partial class CustomerShipmentVersion : ShipmentVersion
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
        [Id("578B9E5B-5ACA-4E0A-9037-80F90A527AE2")]
        [AssociationId("756A3E0B-9CCE-434E-B111-D22C7A6FF311")]
        [RoleId("9FE6F69E-91B2-4F6B-A597-DFF5180AB213")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public CustomerShipmentState CustomerShipmentState { get; set; }

        #region Allors
        [Id("BC2DC6F6-143E-42DA-BFA8-B65A213D61AB")]
        [AssociationId("EB9B4CC4-4DC7-4559-B3F6-29B892B2E4FA")]
        [RoleId("3403649C-3E49-4BB7-91B2-AB2B9C40C7CC")]
        #endregion
        [Required]
        [Workspace]
        public bool ReleasedManually { get; set; }

        #region Allors
        [Id("C9CF7242-4C5E-4948-94F9-6AF30DE2B78B")]
        [AssociationId("3B7B6281-FCC2-4A43-A5D4-7F93A79ECDF7")]
        [RoleId("F26B111F-17EA-4075-A268-808F58CC548F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PaymentMethod PaymentMethod { get; set; }

        #region Allors
        [Id("02055EF3-C530-404C-A814-930E325F4763")]
        [AssociationId("C89EDB5E-5FE8-4147-B27D-C01656C22C4E")]
        [RoleId("53E5E292-76E9-4DE5-8CC3-4940099FC873")]
        #endregion
        [Required]
        [Workspace]
        public bool WithoutCharges { get; set; }

        #region Allors
        [Id("61370BA4-FC62-4B1C-A846-1E0DB65E8713")]
        [AssociationId("BF15E913-DDC8-477E-9700-35A8A71D2BDE")]
        [RoleId("BDB7EAF7-067B-47E7-B421-D2829018E97D")]
        #endregion
        [Required]
        [Workspace]
        public bool HeldManually { get; set; }

        #region Allors
        [Id("96CAFF72-6886-4EA5-A574-1ABA52E8F39A")]
        [AssociationId("CD2A0ED9-7395-477C-8260-DFD23452E0D1")]
        [RoleId("63210761-6DFE-41F0-90CA-27573733B259")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal ShipmentValue { get; set; }
        
        #region inherited methods
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}