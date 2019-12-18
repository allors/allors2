// <copyright file="Shipment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("9c6f4ad8-5a4e-4b6e-96b7-876f7aabcffb")]
    #endregion
    public partial interface Shipment : Printable, Commentable, Auditable, Transitional, Deletable
    {
        #region ObjectStates
        #region ShipmentState
        #region Allors
        [Id("DBC484A2-6EA0-47E9-8EAF-DFC5067CF34C")]
        [AssociationId("18AEA7F6-50FF-42FD-B7A1-1CFF26D74EDE")]
        [RoleId("50570016-7045-4B77-A97B-0DA372A68C7C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        ShipmentState PreviousShipmentState { get; set; }

        #region Allors
        [Id("589116AF-CE7E-4894-BD4D-8089FBBA7358")]
        [AssociationId("3ABA283D-9CD4-4D66-9CD3-F77ECCB20F0E")]
        [RoleId("D575BAEB-FE77-4544-91CD-C046A67452EC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        ShipmentState LastShipmentState { get; set; }

        #region Allors
        [Id("A65BBB49-2B63-4573-BBCC-5DDF13C86518")]
        [AssociationId("1D9C8895-23FC-4AC3-961D-CF53AEB7BE74")]
        [RoleId("97C8883C-E72A-4613-8DCB-4A6FE9C4E64C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        ShipmentState ShipmentState { get; set; }
        #endregion
        #endregion

        #region Allors
        [Id("05221b28-9c80-4d3b-933f-12a8a17bc261")]
        [AssociationId("c59ef057-da9a-433f-90d3-5ff657aa1e48")]
        [RoleId("6fe551cd-0808-466b-9ec9-833098ebad79")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        ShipmentMethod ShipmentMethod { get; set; }

        #region Allors
        [Id("17234c66-6b61-4ac9-a23b-4388e19f4888")]
        [AssociationId("bc2164ec-5d7e-4dff-8db6-4d1eeab970e6")]
        [RoleId("f939af72-bcb4-44bc-b47d-758c27304a7d")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        string ShipmentNumber { get; set; }

        #region Allors
        [Id("f1e92d31-db63-419c-8ed7-49f5db66c63d")]
        [AssociationId("fffbc8b5-a541-402d-8df6-3134cc52b306")]
        [RoleId("566b9c3a-3fec-455f-a40d-b23338d3508c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party ShipFromParty { get; set; }

        #region Allors
        [Id("c8b0eff8-4dff-449c-9d44-a7235cd24928")]
        [AssociationId("556c0ae6-045e-4f35-8f63-ffb41f57dc44")]
        [RoleId("5c34f5ee-5f25-42dd-97a8-7aa3aeb9973e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        PostalAddress ShipFromAddress { get; set; }

        #region Allors
        [Id("AC3FC462-A86D-481D-A5A9-F4B397E8B774")]
        [AssociationId("1CCF8ABC-122D-4F98-8724-DA79285D3395")]
        [RoleId("C2698CA2-68A6-481E-85E0-55ECB4F638BC")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Person ShipFromContactPerson { get; set; }

        #region Allors
        [Id("40277d59-6ab8-40b0-acee-c95ba759e2c8")]
        [AssociationId("d7feb989-dd2d-4619-b079-8a059129f8ed")]
        [RoleId("068d5263-18d7-40e4-80c1-4f9a8e88d10a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Facility ShipFromFacility { get; set; }

        #region Allors
        [Id("5891b368-89cd-4a0e-aaef-439f442909c8")]
        [AssociationId("5fef9e9f-bd3d-454a-8aa1-10b262a34a4b")]
        [RoleId("dd5e8d80-0395-413d-addb-ca66f36c50e8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party ShipToParty { get; set; }

        #region Allors
        [Id("7e1325e0-a072-46da-adb5-b997dde9980a")]
        [AssociationId("f73c3f6d-cc9c-4bda-a4c6-ef4f406a491d")]
        [RoleId("14f6385d-4e20-4ffe-89e7-f7a261eda78e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("D0A863A1-5A97-42B2-AE35-CFD964B6DE4F")]
        [AssociationId("3B8C4840-87A9-4D00-957E-1D7373CAF0F4")]
        [RoleId("4C907D61-581D-422D-923D-2DBD40221204")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Person ShipToContactPerson { get; set; }

        #region Allors
        [Id("E709E429-F7FA-4EAD-8AB4-3E029F64AE6B")]
        [AssociationId("B1020C97-98AC-4E2D-8596-466C2438454D")]
        [RoleId("D67888F9-9F9D-4B00-916C-8FC50E1E4756")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Facility ShipToFacility { get; set; }

        #region Allors
        [Id("6a568bea-6718-414a-b822-d8304502be7b")]
        [AssociationId("499bb422-b2f0-48cf-bf09-0544e768b5de")]
        [RoleId("b8724e90-9888-4f81-b70d-1eceb93af3d3")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        ShipmentItem[] ShipmentItems { get; set; }

        #region Allors
        [Id("894ecdf3-1322-4513-bf94-63882c5c29bf")]
        [AssociationId("da1adb58-e2be-4018-97b0-fb2ef107a661")]
        [RoleId("7e28940e-6039-4698-b1f5-b31769aa7bbb")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal EstimatedShipCost { get; set; }

        #region Allors
        [Id("97788e21-ec31-4fb2-9ef7-0b7b5a7367a1")]
        [AssociationId("227f8e47-58af-44be-bcaf-0da60e2c13d4")]
        [RoleId("338e2be0-6eb5-42ad-b51c-83dd9b7f0194")]
        #endregion
        [Workspace]
        DateTime EstimatedShipDate { get; set; }

        #region Allors
        [Id("a74391e5-bd03-4247-93b8-e7081d939823")]
        [AssociationId("41060c75-fb34-4391-96f3-d0d267344ba3")]
        [RoleId("eb3f084c-9d59-4fff-9fc3-186d7b9a19b3")]
        #endregion
        [Workspace]
        DateTime LatestCancelDate { get; set; }

        #region Allors
        [Id("b37c7c90-0287-4f12-b000-025e2505499c")]
        [AssociationId("13e8d5af-43ff-431b-85d8-5e7706dc2f75")]
        [RoleId("81367cbd-4713-46bd-8f4d-0df30c3daf96")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Carrier Carrier { get; set; }

        #region Allors
        [Id("b69c6812-bdc4-4a06-a782-fa8ff4a71aca")]
        [AssociationId("988cafce-2323-4c0d-b1cd-026045764ba4")]
        [RoleId("cd02effa-d176-4f6e-8407-ec12d23b9f2a")]
        #endregion
        [Workspace]
        DateTime EstimatedReadyDate { get; set; }

        #region Allors
        [Id("ee49c6ca-bb03-40d3-97f1-004cc5a31132")]
        [AssociationId("167b541c-d2dd-4d9b-9fe1-6cd8d1a5f727")]
        [RoleId("39a0ed41-436e-44bd-afc7-5d848397433b")]
        #endregion
        [Size(-1)]
        [Workspace]
        string HandlingInstruction { get; set; }

        #region Allors
        [Id("f1059139-6664-43d5-801f-79a4cc4288a6")]
        [AssociationId("92807e93-ed03-4dbc-9296-c508c879705b")]
        [RoleId("3f2699b9-9652-4af4-98d7-2ff803677692")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Store Store { get; set; }

        #region Allors
        [Id("165b529f-df1c-45b6-bbed-d19ffcb375f2")]
        [AssociationId("c71e40be-1f55-483d-9bfa-0d2dfb26c7d9")]
        [RoleId("18a5331e-120e-45e6-8ef4-35a1f48237e0")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        ShipmentPackage[] ShipmentPackages { get; set; }

        #region Allors
        [Id("18808545-f941-4c5a-8809-0f1fb0cca2d8")]
        [AssociationId("44940303-b210-42bd-8791-906004294261")]
        [RoleId("a65dbc06-f659-4e34-bf2d-af4b4717972e")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        Document[] Documents { get; set; }

        #region Allors
        [Id("A6444620-783D-40E1-A908-001B41A5F097")]
        [AssociationId("14F70A94-D5B2-4B21-A9F5-4E697989E5BB")]
        [RoleId("8B9F2B4F-4DCC-4223-9EE7-949F86BD8A78")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Media[] ElectronicDocuments { get; set; }

        #region Allors
        [Id("f403ab39-cc81-4e09-8794-a45db9ef178f")]
        [AssociationId("78c8d202-0277-4c3a-9e24-74041cc56872")]
        [RoleId("8086c3d5-1577-4c32-bf73-abe72aac725c")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        ShipmentRouteSegment[] ShipmentRouteSegments { get; set; }

        #region Allors
        [Id("fdac3beb-edf8-4d1b-80d4-21b643ef43ce")]
        [AssociationId("63d8adfc-6afb-499f-bd27-2f1d3f78bee6")]
        [RoleId("8f56ce24-500e-4db9-abce-c7a301c38fe6")]
        #endregion
        [Workspace]
        DateTime EstimatedArrivalDate { get; set; }

        #region Allors

        [Id("11D44169-2D96-4310-AD6C-59417D8CA0C2")]

        #endregion
        [Workspace]
        void Create();

        #region Allors
        [Id("F6B4B2D0-A896-480E-A441-F15AB11A3CC9")]
        #endregion
        [Workspace]
        void Invoice();
    }
}
