// <copyright file="Invoice.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("a6f4eedb-b0b5-491d-bcc0-09d2bc109e86")]
    #endregion
    public partial interface Invoice : Commentable, Printable, Auditable, Transitional, Deletable
    {
        #region Allors
        [Id("8EBB1372-CA22-4639-85FC-D1C14AB0F500")]
        [AssociationId("D594FF30-C48F-4E93-9158-EF5906251CD3")]
        [RoleId("C67F31B3-A9D2-44EA-8795-7A76D5DC7F30")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("1c535b3f-bb97-43a8-bd29-29c4dc267814")]
        [AssociationId("d3155310-1267-4780-b69d-4dd47ef15e73")]
        [RoleId("2603b50f-78b9-429c-be30-38949bdec59a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        Currency AssignedCurrency { get; set; }

        #region Allors
        [Id("e4102661-d3dd-4b88-adb4-8d0358fc19c4")]
        [AssociationId("d151c7e2-e583-4218-aca9-3056abadf719")]
        [RoleId("f97944ca-2e05-44f6-b5f3-46d873fee370")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        [Indexed]
        Currency DerivedCurrency { get; set; }

        #region Allors
        [Id("2d82521d-30bd-4185-84c7-4dfe08b5ddef")]
        [AssociationId("aa6230a9-7a9e-4d42-a14a-49b1c3b382ab")]
        [RoleId("5c1fbd73-39e2-4a4a-b58b-2e6c7a110755")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("4b2eedbb-ec59-4e18-949f-f467e41f6401")]
        [AssociationId("b41474a8-482f-458f-b70d-b11e97129ea0")]
        [RoleId("5bab4dea-3566-4421-96c5-27b774b6542a")]
        #endregion
        [Size(256)]
        [Workspace]
        string CustomerReference { get; set; }

        #region Allors
        [Id("4d3f69a0-6e9d-4ba3-acd8-e5dab2a7f401")]
        [AssociationId("4ac19707-3c95-4b7d-b281-2f9d86c3eeb9")]
        [RoleId("15779f7b-07ce-4373-a9cd-1ee5690ddbfc")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal AmountPaid { get; set; }

        #region Allors
        [Id("6b474ddd-c2fd-4db1-bf18-44c86a309d53")]
        [AssociationId("01576aed-1f77-47db-bf04-40aa5dcae63a")]
        [RoleId("f0bd433a-f5cc-4a6d-be5f-8f09594aa566")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalDiscount { get; set; }

        #region Allors
        [Id("6ea961d5-89fc-4526-922a-80538ecb5654")]
        [AssociationId("66c5cfdd-6af4-4d75-b826-843be3b01bca")]
        [RoleId("f560bb3d-f855-4ec3-a5e7-4bd6c4da2595")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        BillingAccount BillingAccount { get; set; }

        #region Allors
        [Id("7b6ab1ed-845d-4671-bda2-43ad2327ea53")]
        [AssociationId("d0994e3f-4741-4f9e-9f4f-8923ed3afdf3")]
        [RoleId("4b4902c0-780d-4de2-97ff-54a5f3bdc521")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIncVat { get; set; }

        #region Allors
        [Id("7e8de8bd-f1c0-4fa5-a629-34d9d5f71b85")]
        [AssociationId("483b0b71-a4a8-4606-a432-d98d8bd262a2")]
        [RoleId("32e8201c-9e71-48e7-ae20-972e14ea4aeb")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalSurcharge { get; set; }

        #region Allors
        [Id("7fda150d-44c8-45a9-8048-dfe38d936c3e")]
        [AssociationId("e2199200-562f-474d-a822-094fba167dc6")]
        [RoleId("09cba9f7-d85f-4c54-a857-c28f22f0eaae")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalBasePrice { get; set; }

        #region Allors
        [Id("7a783e3c-9197-4d1a-8291-f95a3b3a799d")]
        [AssociationId("03f9fd09-c4c9-4d52-b15e-e83ceb490019")]
        [RoleId("d5738ae0-13ed-4466-8fb1-f6ade4fba672")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal GrandTotal { get; set; }

        #region Allors
        [Id("82541f62-bf0e-4e33-9971-15a5a4fa4469")]
        [AssociationId("b3579af4-1c8e-46c5-bc1c-a9d7711a4a48")]
        [RoleId("d54fdbf9-c580-4a49-b058-28aab77d81e0")]
        #endregion
        [Required]
        [Workspace]
        DateTime InvoiceDate { get; set; }

        #region Allors
        [Id("8798a760-de3d-4210-bd22-165582728f36")]
        [AssociationId("d0d6a00a-2d79-4798-b51f-7e6dfb8551d5")]
        [RoleId("c1f88c71-2415-4928-ae3b-16c7f85af30c")]
        #endregion
        [Derived]
        [Required]
        [Workspace]
        DateTime EntryDate { get; set; }

        #region Allors
        [Id("94029787-f838-47bb-9617-807a8514a350")]
        [AssociationId("92badbf6-7d16-46b2-b214-1ea26855970d")]
        [RoleId("2dc528f7-451e-4570-922b-649d9448ed11")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalShippingAndHandling { get; set; }

        #region Allors
        [Id("9eec85a4-e41a-4ca2-82fa-2dc0aa45c9d5")]
        [AssociationId("26c9285b-4c0e-443e-914b-ceb95d37a8fe")]
        [RoleId("4d9bb0e9-23b1-429e-bf61-2fa3b9afb2b8")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExVat { get; set; }

        #region Allors
        [Id("5e4bc0b7-8d9a-45ea-a5e7-8c608a286fdf")]
        [AssociationId("99c6caaf-3b1e-4735-8695-a5df344f546e")]
        [RoleId("10930c0b-ce15-47d2-bfba-fdff48103275")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Indexed]
        OrderAdjustment[] OrderAdjustments { get; set; }

        #region Allors
        [Id("9ff2d65b-0478-41cc-b70b-0df90cdbe190")]
        [AssociationId("38654202-df58-4f2a-9c8d-094fb511a19a")]
        [RoleId("a12bdf85-5c6d-43e4-92b2-8f2fefc03e3e")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        SalesTerm[] SalesTerms { get; set; }

        #region Allors
        [Id("ab342937-1e58-4cd7-99b5-c8a5e7afe317")]
        [AssociationId("0cd0981d-d26b-42e4-a50d-9747a1171b12")]
        [RoleId("431bbc5d-4de6-4cee-aa2d-f1f5c6e7e745")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        string InvoiceNumber { get; set; }

        #region Allors
        [Id("b298c12c-620b-4cf2-b47e-df17afc65552")]
        [AssociationId("4eff42a0-dfe5-440c-a2d2-7612ece8ff11")]
        [RoleId("92365fb1-d257-4fbd-81e4-097ef6d2405e")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Message { get; set; }

        #region Allors
        [Id("c2ecfd15-7662-45b4-99bd-9093ca108d23")]
        [AssociationId("32efeb84-a275-4b14-ba1f-aa99ba1bc776")]
        [RoleId("4e4351e1-7174-4337-b448-bd3f79e3aaa4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        VatRegime AssignedVatRegime { get; set; }

        #region Allors
        [Id("5e46c4ad-90da-4360-8775-05b0d9dd93b3")]
        [AssociationId("85e84089-a09b-48d5-b95d-4a75a286b2f6")]
        [RoleId("3c7e5797-d3f6-4406-8889-8750d5dcfaa1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        VatRegime DerivedVatRegime { get; set; }

        #region Allors
        [Id("16acde85-c301-4202-98cf-82b8cbd1d9e3")]
        [AssociationId("e3cb65d4-1a4b-4f06-a164-cfa486457e12")]
        [RoleId("a40ce229-f0fe-46a6-bcc4-0fdb7946a2a5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        VatRate DerivedVatRate { get; set; }

        #region Allors
        [Id("9a12fc10-722b-42c4-aa68-cec89aeb5c12")]
        [AssociationId("5fd3a989-2e6a-4d38-8289-dd63f5de1ebc")]
        [RoleId("c77df0d9-7303-4d5d-ae4b-3f096f45a865")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRegime AssignedIrpfRegime { get; set; }

        #region Allors
        [Id("84384c93-d213-4abf-bdd3-7d214cea729e")]
        [AssociationId("5ad1cd5d-558e-4117-814d-dc8c11c72f18")]
        [RoleId("a9b81ef4-513a-4316-8106-1f41730e1fea")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        IrpfRegime DerivedIrpfRegime { get; set; }

        #region Allors
        [Id("b24b9ab0-880b-42e5-856f-faa805b596aa")]
        [AssociationId("20699d61-24b8-4524-a661-62f4d8fc4c3c")]
        [RoleId("8f297851-6da8-47ca-92b7-112c4d04cf6e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        IrpfRate DerivedIrpfRate { get; set; }

        #region Allors
        [Id("c7350047-9282-41c8-8d82-4e1f86369e9c")]
        [AssociationId("0468ccd7-0e03-4bff-8812-ee1f979a6a3f")]
        [RoleId("09a4e368-3d7e-4dd5-8708-fa9ff5bddc4b")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalVat { get; set; }

        #region Allors
        [Id("2bba9da0-c6d6-4af8-9f93-3bf8c7a46a98")]
        [AssociationId("88695ee4-c648-46e8-9cf9-0c3b1d0cd803")]
        [RoleId("fbdb6a04-68c2-4a98-8401-8d1bc8007d29")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIrpf { get; set; }

        #region Allors
        [Id("fa826458-5423-43dd-b02f-fe2673a2d0f3")]
        [AssociationId("ac559656-d5c1-4325-a267-9775136a25af")]
        [RoleId("837d36ee-f23f-45bc-87a9-9760d08f29c4")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalFee { get; set; }

        #region Allors
        [Id("636a3b83-7157-42a4-bc24-db5419ccb3b7")]
        [AssociationId("06ba4fd1-96af-448a-85dc-3392b89755b2")]
        [RoleId("53485360-3319-456b-84a3-9b759b5c2117")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExtraCharge { get; set; }

        #region Allors
        [Id("BBA2D4EA-D31F-4C68-8935-2AC3CC1A267D")]
        [AssociationId("CE243EC9-8607-4B06-8749-5BC779BD12DF")]
        [RoleId("997B1E33-048F-4358-8495-E495653706F6")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Derived]
        [Indexed]
        InvoiceItem[] ValidInvoiceItems { get; set; }

        #region Allors
        [Id("1ef6f9b0-c541-4a8d-9ce2-fb6a330244e9")]
        [AssociationId("194041ad-93b6-4eb2-802d-2e5d111c3177")]
        [RoleId("f34e32a1-606c-4272-bdf8-b0545ba0c34b")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        int SortableInvoiceNumber { get; set; }

        #region Allors
        [Id("eaa66e01-f597-4c71-86e5-d78652fe926b")]
        [AssociationId("43978013-8e98-46e5-941c-ba19ba4fa6a5")]
        [RoleId("fddb51c0-e427-4575-b9d3-24f6fd3f4e06")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public Media[] ElectronicDocuments { get; set; }

        #region Allors
        [Id("893734a4-13e6-4740-8c3b-29e28c0137b0")]
        [AssociationId("5eb6e446-ce61-42a5-ad21-d70df8c2374a")]
        [RoleId("8ac13198-dbd3-49bd-8595-e26aaa1fd61b")]
        #endregion
        [Required]
        [Workspace]
        public Guid DerivationTrigger { get; set; }

        #region Allors
        [Id("3d3bc83a-dc13-44ab-b10e-cad3602316b2")]
        [AssociationId("6e98fd50-3325-486b-8b16-5b77fb5745fd")]
        [RoleId("72452148-9460-4333-8f4c-4acb67d8bad3")]
        #endregion
        [Required]
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIrpfInPreferredCurrency { get; set; }

        #region Allors
        [Id("5c22850a-6f1c-4140-9600-aa6632a1d8fa")]
        [AssociationId("2e9adcc3-041d-41ed-b1aa-155aa855b5fd")]
        [RoleId("69ab1a81-6308-4dd0-a9b0-2eb0ed16da5a")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("82ee09e6-c606-48e0-bcf8-732638b9aff9")]
        [AssociationId("9ec8271d-2237-4c20-95c2-0630f4f03c37")]
        [RoleId("cdc5536e-edc5-4e9e-886f-1a2dc0ff628c")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("9549673f-334d-4d8c-b426-c9a008edec97")]
        [AssociationId("62d9c769-bea7-4c1c-83b5-b656abff6ffc")]
        [RoleId("b25d04c9-d6df-4b48-ae18-2048f6e9a54d")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalIncVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("541097a6-1d7a-4a2b-b628-a860b795a462")]
        [AssociationId("57e04875-b185-4519-8542-49f030c1e412")]
        [RoleId("61a96741-25b5-4eca-a2c6-70b94f52a68a")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurchargeInPreferredCurrency { get; set; }

        #region Allors
        [Id("1f1739bd-fa2b-4b3f-bace-63e627a0161b")]
        [AssociationId("6d77a8cd-3095-4611-9ab2-6556e69f480b")]
        [RoleId("1100e4aa-28d3-4a8c-803d-bc953a2ec96e")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscountInPreferredCurrency { get; set; }

        #region Allors
        [Id("14725742-647b-414d-88b9-1ddf19cf0007")]
        [AssociationId("9bfeeb10-d9ab-499d-b35d-c4f33f28075c")]
        [RoleId("1346bc4b-dc61-4d31-8f7e-67fca75aa04c")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandlingInPreferredCurrency { get; set; }

        #region Allors
        [Id("5731f3a4-35b0-43c6-a74a-6b7ea2abdd6e")]
        [AssociationId("9eed8126-d43c-40d5-ba0d-129586c8690a")]
        [RoleId("0bbeeedc-739e-49d3-af22-28ed3fa7ca72")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalFeeInPreferredCurrency { get; set; }

        #region Allors
        [Id("0658f560-7ce5-498e-b470-b9ef24f228e5")]
        [AssociationId("c9c0d5fb-99f6-415e-a2e4-96ecdb52b586")]
        [RoleId("2c1863eb-d2f2-4482-b56c-3d8da7f8dbda")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExtraChargeInPreferredCurrency { get; set; }

        #region Allors
        [Id("49b5d6bc-abde-45fc-b49d-c4ed8e71e13e")]
        [AssociationId("35375be7-7f31-4588-b1df-59554d20664a")]
        [RoleId("6b9bc3a6-f62e-4eb9-b01f-1e4c91733cef")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePriceInPreferredCurrency { get; set; }

        #region Allors
        [Id("f58dfcea-aaf3-4b6a-a29e-0a7132d18611")]
        [AssociationId("f47bf6cb-7516-4c86-b91c-75384b72b3c1")]
        [RoleId("3310a54d-98bf-4c1d-ab18-66c7cf516369")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalListPriceInPreferredCurrency { get; set; }

        #region Allors
        [Id("85448a6b-1a4f-4634-9352-8dbd5c011f20")]
        [AssociationId("88924191-b16f-4be4-a7db-ff9f879ddb61")]
        [RoleId("5a0835c0-0d50-4b3a-b47a-6086e649f04c")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal GrandTotalInPreferredCurrency { get; set; }

        #region Allors

        [Id("B9226E72-AD90-4195-9DC7-64A26D12E6A3")]

        #endregion
        [Workspace]
        void Create();

        #region Allors

        [Id("832244fc-9ac7-4d45-b154-5b49136d97af")]

        #endregion
        [Workspace]
        void Revise();
    }
}
