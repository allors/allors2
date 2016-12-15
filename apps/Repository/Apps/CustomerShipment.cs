namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("9301efcb-2f08-4825-aa60-752c031e4697")]
    #endregion
    public partial class CustomerShipment : Deletable, Shipment 
    {
        #region inherited properties
        public ShipmentMethod ShipmentMethod { get; set; }

        public ContactMechanism BillToContactMechanism { get; set; }

        public ShipmentPackage[] ShipmentPackages { get; set; }

        public string ShipmentNumber { get; set; }

        public Document[] Documents { get; set; }

        public Party BillToParty { get; set; }

        public Party ShipToParty { get; set; }

        public ShipmentItem[] ShipmentItems { get; set; }

        public InternalOrganisation BillFromInternalOrganisation { get; set; }

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

        public string PrintContent { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        #endregion

        #region Allors
        [Id("15e8f37c-3963-490c-8f22-7fb1e40209df")]
        [AssociationId("30b4e232-dd11-4ee6-b1dd-ef1e05b54d92")]
        [RoleId("a282ae7a-2280-4ea8-a8c8-cf170f0714ac")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        public CustomerShipmentStatus CurrentShipmentStatus { get; set; }

        #region Allors
        [Id("4f7c79be-9f0d-4646-9488-dc86761866cd")]
        [AssociationId("06ff523b-b43d-424e-b54a-c184c5d3ac5f")]
        [RoleId("526cb9db-f5d7-42bc-a37d-c1ae680d1f92")]
        #endregion
        [Required]
        public bool ReleasedManually { get; set; }
        
        #region Allors
        [Id("7b1b6b60-9678-4a52-aee8-33bad04eeb40")]
        [AssociationId("8cf76b47-a09f-4112-8bec-733a30abc323")]
        [RoleId("6c812e1e-204b-4e85-8cfb-5dae89fb2bf2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        public CustomerShipmentObjectState CurrentObjectState { get; set; }
        
        #region Allors
        [Id("7b6a8a4f-574f-494f-b43b-7c5b7428d685")]
        [AssociationId("83787439-402b-4d57-8e70-aa157aa8d1fa")]
        [RoleId("0022a581-9823-4b8d-a3f5-ce068ab60fe8")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        public CustomerShipmentStatus[] ShipmentStatuses { get; set; }
        
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


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Delete(){}





        #endregion
    }
}