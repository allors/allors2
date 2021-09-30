// <copyright file="Store.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("d8611e48-b0ba-4037-a992-09e3e26c6d5d")]
    #endregion
    public partial class Store : UniquelyIdentifiable, Object
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("A89BD354-9747-44AF-99E4-054F1CC42D9C")]
        [AssociationId("7E117430-B45D-4BEE-9DA3-564C89256892")]
        [RoleId("C76710A7-0A8A-4FAF-BF41-53CD7D50430B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InternalOrganisation InternalOrganisation { get; set; }

        #region Allors
        [Id("5A052D80-CD0D-4C95-9281-4DD59A3BE26B")]
        [AssociationId("AF4D8425-2521-460F-B4D2-F3DE128CBF58")]
        [RoleId("7458412B-92C3-46A0-9887-4F144455F822")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public Catalogue[] Catalogues { get; set; }

        #region Allors
        [Id("0a0ad3b1-afa2-4c78-8414-e657fabebb3e")]
        [AssociationId("c460c53b-1460-4bc2-8390-98e9c1492b71")]
        [RoleId("06502486-41cb-4840-856e-7d44c0038375")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal ShipmentThreshold { get; set; }

        #region Allors
        [Id("3e378f04-0d14-4b03-b8e2-b58da3039184")]
        [AssociationId("b4f8b63a-d4c6-4a40-a603-84c4225f02ed")]
        [RoleId("3a00ec26-a46e-4262-aed0-56cb25abf2b1")]
        #endregion
        [Size(256)]
        [Workspace]
        public string SalesInvoiceNumberPrefix { get; set; }

        #region Allors
        [Id("8a3d0121-e5f9-4bc9-a829-340e1b4b5402")]
        [AssociationId("d92f46c9-07aa-4be9-b0ab-36d66b24ae5e")]
        [RoleId("7d03bd42-e0fa-4053-bf9c-c45b06fcff97")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        public Counter SalesInvoiceNumberCounter { get; set; }

        #region Allors
        [Id("22CB75F8-CDF0-4D20-ABF5-79B43ADA30FD")]
        [AssociationId("DCD856F9-7C0C-4BEF-8529-AE59BB962F2E")]
        [RoleId("92DD02F8-3532-405C-9E08-07227F59140D")]
        #endregion
        [Size(256)]
        [Workspace]
        public string CreditNoteNumberPrefix { get; set; }

        #region Allors
        [Id("8193E342-DB5B-4001-8321-3013CEB469EB")]
        [AssociationId("D7ED58EC-4FBF-44A6-B828-0188D07A3617")]
        [RoleId("C5822581-BA32-4BD6-BDB9-6ABC0C261713")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        public Counter CreditNoteNumberCounter { get; set; }

        #region Allors
        [Id("e00e948e-6fc3-43fd-a49b-008fc6d6133f")]
        [AssociationId("3f5cbcd9-c36b-4792-b1fc-15cf533ba6f3")]
        [RoleId("a7e750a0-2e69-485d-8208-ab04682b6efd")]
        #endregion
        [Size(256)]
        [Workspace]
        public string SalesOrderNumberPrefix { get; set; }

        #region Allors
        [Id("124a58f1-f7a3-43d1-8f4d-0a068b7a2659")]
        [AssociationId("87a6b9dd-5c38-4b79-a0e4-75e6777f5207")]
        [RoleId("5725c16f-b079-4435-a347-660fe9de7223")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]

        public Counter SalesOrderNumberCounter { get; set; }

        #region Allors
        [Id("3a837bae-993a-4765-8d4f-b690bf65dc79")]
        [AssociationId("0304eacc-65bc-475d-9a82-00b0cdb233ad")]
        [RoleId("21c8c056-2997-4f75-82db-597e258dceb6")]
        #endregion
        [Size(256)]
        [Workspace]
        public string OutgoingShipmentNumberPrefix { get; set; }

        #region Allors
        [Id("dfc3f6be-0a95-49e0-8742-3901dbab5185")]
        [AssociationId("92072a30-fdb0-42c2-b4b6-f1fa53e83e7e")]
        [RoleId("9fc009b2-0c9c-446f-ba4f-39144cb6c90d")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        public Counter OutgoingShipmentNumberCounter { get; set; }

        #region Allors
        [Id("3CBCF813-7FD4-4C69-98C8-CFC260234477")]
        [AssociationId("92F3D9D3-39C9-4482-B66C-4D328F61B47B")]
        [RoleId("A79C364B-9CA8-4659-9ADE-FBBDECA0E2AB")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        public Counter SalesInvoiceTemporaryCounter { get; set; }

        #region Allors
        [Id("4927a65d-a9d3-4fad-afce-1ec8679d3a55")]
        [AssociationId("e2dc511c-86b0-46fe-b5cf-680dfe012f47")]
        [RoleId("f18df944-920c-474a-ac8e-2e10b460c522")]
        #endregion
        [Required]
        [Workspace]
        public int PaymentGracePeriod { get; set; }

        #region Allors
        [Id("4a647ddb-9a17-4544-8cae-6204140c413a")]
        [AssociationId("d657040d-138b-4f6f-9dc7-547448a1fd11")]
        [RoleId("93cab392-5d64-4270-9e5d-ac3a62dcde4d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Media LogoImage { get; set; }

        #region Allors
        [Id("555c3b9a-7556-4fdf-a431-6d18a6ae7cbd")]
        [AssociationId("b3625e45-4568-4030-85cc-565f77ccc1a1")]
        [RoleId("5bda243d-b0c9-47bd-9d33-9fd3723512b9")]
        #endregion
        [Required]
        [Workspace]
        public int PaymentNetDays { get; set; }

        #region Allors
        [Id("63d433b9-8cb3-428b-b516-be25f1895673")]
        [AssociationId("273cfb27-2698-469b-91ab-24901a4df9fd")]
        [RoleId("67f48e4c-6e56-47c8-a87f-47160453ece6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Facility DefaultFacility { get; set; }

        #region Allors
        [Id("6e4b701a-2540-4cec-8413-50bfb69d3a7c")]
        [AssociationId("2a1d8fe1-51af-4747-b6e4-7c2532e5fa8c")]
        [RoleId("2bcd6952-16c1-4153-a23c-6e58fae6a49c")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("79244ed7-6388-48ca-86db-7b57a64fe680")]
        [AssociationId("5d145726-fa9e-46f9-b389-7f8380e0088c")]
        [RoleId("1df01d8b-9d72-495c-baae-0f5ea4c9e76c")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal CreditLimit { get; set; }

        #region Allors
        [Id("7c9cda07-5920-4037-b934-5b74355c4b85")]
        [AssociationId("0da06b1f-12ce-43d1-9e21-82e506ce7750")]
        [RoleId("f1c26a78-d986-4fcc-ac55-3658783790ef")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public ShipmentMethod DefaultShipmentMethod { get; set; }

        #region Allors
        [Id("80670a7a-1be8-4407-917e-fa359e632519")]
        [AssociationId("dcd8b2e0-7490-40e4-ae4b-d1b0c0be0527")]
        [RoleId("72993b04-4c10-4467-9d99-064e1b39f9e2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Carrier DefaultCarrier { get; set; }

        #region Allors
        [Id("954d4e3c-f188-45f4-98b8-ece14ac7dabd")]
        [AssociationId("a08d71c8-6aa1-4ae3-bccc-a8078bd51071")]
        [RoleId("92f5fc7a-eabe-4710-b1fe-aa35e7fd1606")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal OrderThreshold { get; set; }

        #region Allors
        [Id("9a0dfe33-016a-4b41-979c-d17a6f87d2d2")]
        [AssociationId("a6741bcc-527d-4cbe-bd1e-fc881fd30951")]
        [RoleId("611edb41-213f-4ade-8ea5-a512b99ee9b6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public PaymentMethod DefaultCollectionMethod { get; set; }

        #region Allors
        [Id("bc11d48f-bcab-4880-afe8-0a52d3c11e44")]
        [AssociationId("d44420aa-80fc-4d55-8032-18b1b1c63d69")]
        [RoleId("6a37b722-41b4-411e-bc84-18918990ad14")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        public FiscalYearStoreSequenceNumbers[] FiscalYearsStoreSequenceNumbers { get; set; }

        #region Allors
        [Id("ca82d0f8-f886-4936-80f5-a7dbb7c550b5")]
        [AssociationId("94284261-b6db-4d74-ae74-955f6481375f")]
        [RoleId("49940864-e0db-4d0b-a607-b1725e6f45c9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public PaymentMethod[] CollectionMethods { get; set; }

        #region Allors
        [Id("85279191-9836-444B-A5CB-742A488D0467")]
        [AssociationId("E8884F9B-6D08-4092-9C0C-D98EFCA9D438")]
        [RoleId("0299E2AD-F239-41E7-9546-2FB014D98A96")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public BillingProcess BillingProcess { get; set; }

        #region Allors
        [Id("ECA0A308-BB12-419C-8E10-67BDCC7D37E6")]
        [AssociationId("219BBCE1-809C-44D9-AE2B-61C09A29A7E7")]
        [RoleId("6E13FD5D-D633-4278-AC18-4E4EE0A52A01")]
        #endregion
        [Required]
        [Workspace]
        public bool IsImmediatelyPicked { get; set; }

        #region Allors
        [Id("7A014C3C-F593-4528-AE32-EE4BE55D76A4")]
        [AssociationId("13DE0B1A-3CD0-4DC9-963C-AB529DA3B26E")]
        [RoleId("F1DDAC44-8A25-4E4B-8F49-B0F2288EB1E9")]
        #endregion
        [Required]
        [Workspace]
        public bool IsImmediatelyPacked { get; set; }

        #region Allors
        [Id("1136BB3C-905C-411B-AFED-FBE04BE132BD")]
        [AssociationId("7008A454-EE6C-402E-BAD0-0D06F76BA294")]
        [RoleId("B427AF19-EBE2-430D-B852-2CF56079F269")]
        #endregion
        [Required]
        [Workspace]
        public bool IsAutomaticallyShipped { get; set; }

        #region Allors
        [Id("8D639C6A-B8C1-4FFA-867F-95B75B4A6807")]
        [AssociationId("F950FDCE-A7A1-4E10-84BF-6BE4CC3AC9AA")]
        [RoleId("EDC6EF18-9CB4-4EAD-9FFD-AEEE13508924")]
        #endregion
        [Required]
        [Workspace]
        public bool AutoGenerateCustomerShipment { get; set; }

        #region Allors
        [Id("B3E6A681-E883-4FD5-82E4-F5A94F3F5148")]
        [AssociationId("6238AAB0-7DF1-4C41-9B71-28AFBD8C8F37")]
        [RoleId("99374B40-A004-4150-B63A-948C4C100A91")]
        #endregion
        [Required]
        [Workspace]
        public bool AutoGenerateShipmentPackage { get; set; }

        #region Allors
        [Id("CE31A755-7053-4A27-A0AE-7C38AFA03E2F")]
        [AssociationId("AA22F139-B7D6-4F86-ABCD-7A41ED5366DA")]
        [RoleId("217207B1-5FF6-4DD2-83AF-3E728ADE0BF9")]
        #endregion
        [Required]
        [Workspace]
        public bool UseCreditNoteSequence { get; set; }

        #region Allors
        [Id("3CECAB0B-A766-474E-9DB1-D6B93E02FF41")]
        [AssociationId("D3D23B7D-9C30-479F-95FA-CE4BA56C90F3")]
        [RoleId("B674A945-19BD-42B9-B925-9569D0B058D1")]
        #endregion
        [Required]
        [Workspace]
        public bool SerialisedInventoryItemStore { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

    }
}
