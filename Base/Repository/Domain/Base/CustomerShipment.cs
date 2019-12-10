// <copyright file="CustomerShipment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("9301efcb-2f08-4825-aa60-752c031e4697")]
    #endregion
    public partial class CustomerShipment : Shipment, Versioned
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
        public Media[] ElectronicDocuments { get; set; }

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
        [Workspace]
        public void Hold() { }

        #region Allors
        [Id("CB18DE5F-0E69-43C5-8CDB-30BB9AE75FD6")]
        #endregion
        public void PutOnHold() { }

        #region Allors
        [Id("7464BD56-E36A-4938-886F-1D8C61A062E2")]
        #endregion
        [Workspace]
        public void Cancel() { }

        #region Allors
        [Id("113C76E1-25E7-4CD2-9D82-1DAE38441DE9")]
        #endregion
        [Workspace]
        public void Continue() { }

        #region Allors
        [Id("370CC546-6502-4416-9DFF-3A3D8510E5D4")]
        #endregion
        [Workspace]
        public void Pick() { }

        #region Allors
        [Id("CB596594-7253-4B2E-8A00-71C062147CD8")]
        #endregion
        [Workspace]
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

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Create() { }

        public void Invoice() { }

        public void Print() { }

        #endregion
    }
}
