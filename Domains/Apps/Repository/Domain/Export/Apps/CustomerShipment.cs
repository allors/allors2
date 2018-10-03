namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("9301efcb-2f08-4825-aa60-752c031e4697")]
    #endregion
    public partial class CustomerShipment : Shipment, Versioned
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Media PrintDocument { get; set; }

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
        #region CustomerShipmentState
        #region Allors
        [Id("E74B56E8-6B32-4735-A4C5-426D07C8D5A2")]
        [AssociationId("EE5972B5-8970-4AD0-8A7D-2DEFC8608AA5")]
        [RoleId("09B0FF60-C763-468F-B7F0-C91C908CA7E1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public CustomerShipmentState PreviousCustomerShipmentState { get; set; }

        #region Allors
        [Id("A5E93268-7855-42DA-88F1-F0D79386EABF")]
        [AssociationId("A995EF2B-0C57-4FC6-8961-F74F2554810E")]
        [RoleId("7BD0AAED-13D3-44E2-A6BE-7ADCBC48C5E2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public CustomerShipmentState LastCustomerShipmentState { get; set; }

        #region Allors
        [Id("0788B9D5-566E-4E0F-8E72-054334B1713D")]
        [AssociationId("5FA2E880-6A51-40F8-BEF1-6371349DEEEB")]
        [RoleId("3F2A8980-093A-4EF3-8FF1-26D035E6C3C9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public CustomerShipmentState CustomerShipmentState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("99FBDEE8-FC43-453F-A9DA-77700CF693D2")]
        [AssociationId("4AAAEE18-5414-40E8-A951-465D540D22BE")]
        [RoleId("33DA6EAE-28F5-48D7-9285-35576852E463")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public CustomerShipmentVersion CurrentVersion { get; set; }

        #region Allors
        [Id("10587B6E-C296-411E-90BD-C45CDE0C0B1E")]
        [AssociationId("289838E4-0A5A-4A47-B0EF-3D1C5C361068")]
        [RoleId("B7597125-9E62-4792-9A4B-305A110D3E78")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public CustomerShipmentVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("4f7c79be-9f0d-4646-9488-dc86761866cd")]
        [AssociationId("06ff523b-b43d-424e-b54a-c184c5d3ac5f")]
        [RoleId("526cb9db-f5d7-42bc-a37d-c1ae680d1f92")]
        #endregion
        [Required]
        public bool ReleasedManually { get; set; }

        #region Allors
        [Id("897bcb4f-fa89-4d9b-8666-49bb061a69ae")]
        [AssociationId("d2945852-755a-45ef-b6dc-914767d3d2e5")]
        [RoleId("a3ab7835-d97e-4221-831d-0ba1ffe3c9d0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public PaymentMethod PaymentMethod { get; set; }

        #region Allors
        [Id("a754a290-571f-4c25-bd1c-d96a9765eec6")]
        [AssociationId("6d117db4-ef4d-483a-a68d-75c69e325bba")]
        [RoleId("66a18574-7b90-4e36-9d5d-a4f31bc6eba1")]
        #endregion
        [Required]
        public bool WithoutCharges { get; set; }

        #region Allors
        [Id("b94fa6e5-cfdf-4545-8eb3-43d03aceffc5")]
        [AssociationId("2d9a286e-95d5-4adb-ab29-7a9d95f83146")]
        [RoleId("33382f4f-5ebc-4589-b906-a8a2a3be28d2")]
        #endregion
        [Required]
        public bool HeldManually { get; set; }

        #region Allors
        [Id("f0fe5bc1-74d1-4fee-8039-b6952edecc92")]
        [AssociationId("c11d0979-373c-4c27-94d2-4d7350afc1c4")]
        [RoleId("2348278f-bf03-4133-b34c-2da5955a0a41")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal ShipmentValue { get; set; }

        #region Allors
        [Id("5FD4DD2D-51CC-46CD-B1C3-138CE68A9580")]
        #endregion
        public void Hold() { }

        #region Allors
        [Id("CB18DE5F-0E69-43C5-8CDB-30BB9AE75FD6")]
        #endregion
        public void PutOnHold() { }

        #region Allors
        [Id("7464BD56-E36A-4938-886F-1D8C61A062E2")]
        #endregion
        public void Cancel() { }

        #region Allors
        [Id("113C76E1-25E7-4CD2-9D82-1DAE38441DE9")]
        #endregion
        public void Continue() { }

        #region Allors
        [Id("CB596594-7253-4B2E-8A00-71C062147CD8")]
        #endregion
        public void Ship() { }

        #region Allors
        [Id("5723BE02-D661-4CEB-875E-A064D657B128")]
        #endregion
        public void ProcessOnContinue() { }

        #region Allors
        [Id("06AA18AA-03CC-4924-8FEC-A71E9A2F16C5")]
        #endregion
        public void SetPicked() { }

        #region Allors
        [Id("5F981009-A1F8-4DE2-930B-B1914BCFAD2B")]
        #endregion
        public void SetPacked() { }

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