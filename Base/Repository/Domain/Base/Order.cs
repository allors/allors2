// <copyright file="Order.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("7dde949a-6f54-4ece-92b3-d269f50ef9d9")]
    #endregion
    public partial interface Order : Printable, Commentable, Localised, Auditable, Transitional, Deletable
    {
        #region Allors
        [Id("962215D2-4461-4BD3-9A98-F1A085B2343F")]
        [AssociationId("18EC98EE-5D4A-4EF4-9CD1-D6351BE0FD63")]
        [RoleId("4C5B1B31-EC17-4E95-B18A-5A8429105CA1")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("2DD5B3C2-1C24-4AFA-A5E0-930BB943E93E")]
        [AssociationId("3DC26CC0-1B4F-48B6-B3E9-5EBA97DDBB8A")]
        [RoleId("DDD72ADC-23B3-446E-B6AF-F495A64979F5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        Currency AssignedCurrency { get; set; }

        #region Allors
        [Id("9262e86c-177f-46ac-92f1-5937a5f67c2c")]
        [AssociationId("0fd385ca-9574-466a-b577-4aac2f3ed9fb")]
        [RoleId("6fab8da2-9ec6-4d5c-9a4b-07f0d3fa6a3a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Derived]
        [Indexed]
        Currency DerivedCurrency { get; set; }

        #region Allors
        [Id("817e6ddd-ad38-4294-a482-4797fd8eb5cb")]
        [AssociationId("744fcd09-38c3-423f-9a5e-ad76435bb5e4")]
        [RoleId("da47c923-3082-428d-8cdd-e01eead03924")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Derived]
        [Indexed]
        public Locale DerivedLocale { get; set; }

        #region Allors
        [Id("45b3b293-b746-4d6d-9da7-e2378694f734")]
        [AssociationId("5e1ba42d-9325-45b5-9c41-cc9b12d0929a")]
        [RoleId("019d8b7a-79db-4100-a690-ae7587e30d8e")]
        #endregion
        [Workspace]
        [Size(256)]
        string CustomerReference { get; set; }

        #region Allors
        [Id("6509263c-a11e-4554-b13d-4fa075fa8ed9")]
        [AssociationId("21bd72d4-b309-452c-a73c-49c7b926aca7")]
        [RoleId("2ba85811-2046-407d-a3a9-a53e05afe3ed")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVat { get; set; }

        #region Allors
        [Id("7374e62f-0f0b-49de-8c70-9ef224a706b1")]
        [AssociationId("cc79b674-ec5d-48d7-b296-c172f372b2b4")]
        [RoleId("29126171-cae2-4b50-89c2-7df91ab71444")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVat { get; set; }

        #region Allors
        [Id("7d23c0ec-57c9-4129-b7d1-ea4ec1ab83dd")]
        [AssociationId("cab49d9b-ea70-4284-85f2-70a45497933d")]
        [RoleId("38e534ed-4b3b-4855-8a7f-5c55e0a1827f")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIrpf { get; set; }

        #region Allors
        [Id("ba6e8dd3-ad74-4ead-96df-d9ba2e067bfc")]
        [AssociationId("a75c25e4-c88d-4d2b-981a-c5b561264e87")]
        [RoleId("77ce4873-d2fa-4f0b-ba8d-0403886d613c")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalIncVat { get; set; }

        #region Allors
        [Id("751cb60a-b8ba-473a-ab95-0909bd2bc61c")]
        [AssociationId("1fb281bd-40cd-45f4-bf37-b7b15ec646d7")]
        [RoleId("24a7b556-4674-42ed-97d9-0e0c466f5fd0")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurcharge { get; set; }

        #region Allors
        [Id("8592f390-a9fb-4275-93c2-b7e73afa2307")]
        [AssociationId("703bf4a7-8949-46ea-b7d3-092ab62c9bdd")]
        [RoleId("1420978d-4e64-434e-926e-e26bbce2dd1f")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscount { get; set; }

        #region Allors
        [Id("d0730f9e-3217-45b3-a5f8-6ae3a5174050")]
        [AssociationId("e575035f-9953-499b-b657-2cde6dd53349")]
        [RoleId("49aaea6a-b3b6-484f-8c49-f61e51a6b71a")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandling { get; set; }

        #region Allors
        [Id("faa16c88-2ca0-4eea-847e-793ab84d7dea")]
        [AssociationId("457ee3dc-c239-4053-a769-f7b50a10879c")]
        [RoleId("e2b42530-507e-4463-8c41-d8ff3886c5cf")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalFee { get; set; }

        #region Allors
        [Id("c0443691-9d8e-4192-a51e-09abb1dbbf24")]
        [AssociationId("a4512098-b115-4d39-854d-f1a073972f87")]
        [RoleId("e2a99ab6-8f9f-40ce-be2f-bd313802adc2")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExtraCharge { get; set; }

        #region Allors
        [Id("f636599a-9c61-4952-abcf-963e6f6bdcd8")]
        [AssociationId("94feb243-bb62-4bf4-9947-76d54df2f13c")]
        [RoleId("5955d3b6-b5cd-4878-a71f-070ce9a343cf")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePrice { get; set; }

        #region Allors
        [Id("41c58b71-f1f7-49c1-b852-8281bb8c8969")]
        [AssociationId("9742ffef-97ac-4fcc-b7b6-4e10279d47e7")]
        [RoleId("fe183a38-6fa8-47dc-bff6-c47180fcd8e9")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal GrandTotal { get; set; }

        #region Allors
        [Id("af6aaba3-20df-48b0-95ea-08aaad9b1183")]
        [AssociationId("8f4eac17-bb85-46ef-9bc0-8b6a4a4413a0")]
        [RoleId("8037ef85-18bd-4911-81af-3044c081be63")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Indexed]
        OrderAdjustment[] OrderAdjustments { get; set; }

        #region Allors
        [Id("73521788-7e0e-4ea2-9961-1a58f68cde5c")]
        [AssociationId("8e7ad6ef-7a40-472f-b7b9-f53a77e51548")]
        [RoleId("af64e731-adfb-414a-9520-51d4ea2c8f81")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        SalesTerm[] SalesTerms { get; set; }

        #region Allors
        [Id("7c04f907-4254-4b59-861a-7b545c12b3d3")]
        [AssociationId("6e8ff513-f6e2-411f-b679-1eda15e0f577")]
        [RoleId("795117b2-8b5a-4562-9acc-d77d2f93256a")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Derived]
        [Indexed]
        OrderItem[] ValidOrderItems { get; set; }

        #region Allors
        [Id("7db0e5f7-8a23-4be8-beba-8ddfd1972856")]
        [AssociationId("084ad016-6eaf-4cc9-aedd-80a4ba161067")]
        [RoleId("4acb8c07-e132-4b35-8e0c-416cdf4da35b")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Size(256)]
        string OrderNumber { get; set; }

        #region Allors
        [Id("8c972fae-b3ba-4e88-b769-d59c14325b00")]
        [AssociationId("a42b384a-3ec0-4c79-af3b-cccc510c019f")]
        [RoleId("d16485cb-983f-4526-b695-01d0d09f3742")]
        #endregion
        [Workspace]
        [Size(-1)]
        string Message { get; set; }

        #region Allors
        [Id("7E66C6E0-F4BD-4085-AD5E-7012B576AFC2")]
        [AssociationId("2BDED8C8-0A07-40B5-8F8A-7618E1D48E60")]
        [RoleId("27B5B028-4D3A-493C-9501-6BF29978C35D")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("a5875c41-9f08-49d0-9961-19a656c7e0cc")]
        [AssociationId("c6604ee5-e9f2-4b5a-9f08-fbfa1d126402")]
        [RoleId("6a783dbf-0f8d-4249-8e1e-6c0c2a61a97e")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        DateTime EntryDate { get; set; }

        #region Allors
        [Id("b205a525-fc61-436d-a66a-1a18bcfb5aff")]
        [AssociationId("142ba77b-066d-4514-a663-1859be50e29e")]
        [RoleId("fc9546af-4e2d-485f-806f-cbfce23a7314")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        OrderKind OrderKind { get; set; }

        #region Allors
        [Id("c6f86f31-d254-4001-94fa-273d041df31a")]
        [AssociationId("0716922b-d051-459c-83c9-4390fa7723d0")]
        [RoleId("58d1bcc3-332a-4830-b346-702efedaa010")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        VatRegime AssignedVatRegime { get; set; }

        #region Allors
        [Id("6145aff7-c0dd-4441-927b-f35857a8f225")]
        [AssociationId("5060aaa4-f021-496b-ab97-dc0624e58637")]
        [RoleId("5a82b8ab-bbc0-4209-869c-11e958b2b3d6")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        VatRegime DerivedVatRegime { get; set; }

        #region Allors
        [Id("3182b8cf-477c-47fc-84c5-93ea78edcc7d")]
        [AssociationId("548ce982-3c9a-448b-8faa-67345dcf81c3")]
        [RoleId("7e41aa08-1bb5-41df-a34a-e7a4e0a3f66e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRegime AssignedIrpfRegime { get; set; }

        #region Allors
        [Id("3193d806-ec3e-4abd-bab6-2b2c10c43c69")]
        [AssociationId("4bcb7929-f3f9-4412-9575-456ee24c65f6")]
        [RoleId("2eb9ec27-6c6d-4ef0-96ad-3430419dc0d0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        IrpfRegime DerivedIrpfRegime { get; set; }

        #region Allors
        [Id("e039e94d-db89-4a17-a692-e82fdb53bfea")]
        [AssociationId("f1eed6f2-fb70-4fd8-8e7a-0962759b00a7")]
        [RoleId("e5e2710b-f662-4a50-8203-d0d7c0789e3e")]
        #endregion
        [Workspace]
        [Required]
        DateTime OrderDate { get; set; }

        #region Allors
        [Id("f38b3c7d-ac20-49be-a115-d7e83557f49a")]
        [AssociationId("f4ff4e74-0bff-4a2a-b4bd-3a08310c6ce2")]
        [RoleId("6d52e55f-2adb-4ec6-8b13-e8611dfcd38a")]
        #endregion
        [Workspace]
        DateTime DeliveryDate { get; set; }

        #region Allors
        [Id("6f0c38b9-8f08-4eb4-8b99-9596121a75a1")]
        [AssociationId("a03d06f2-c395-46c3-b906-0831c2353ebe")]
        [RoleId("42cfb074-3217-46c5-a718-a67d5404c573")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        int SortableOrderNumber { get; set; }

        #region Allors
        [Id("7623fb2c-af18-437b-b746-ec5d05c696b2")]
        [AssociationId("b924e2e2-7b08-4231-93cb-97eff284ce37")]
        [RoleId("4d35f810-1f12-40da-9840-c3283eca4c5c")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public Media[] ElectronicDocuments { get; set; }

        #region Allors

        [Id("14B59435-4304-4070-AA25-EFDAB6431E73")]

        #endregion
        [Workspace]
        void Create();

        #region Allors
        [Id("116D62FC-04E5-407C-B044-7092454C8806")]
        #endregion
        [Workspace]
        void Approve();

        #region Allors
        [Id("8e77c337-e0ef-4524-b657-f904baaa8762")]
        #endregion
        [Workspace]
        void Revise();

        #region Allors
        [Id("F735D397-B989-41E8-A042-5C9EAEB41C32")]
        #endregion
        [Workspace]
        void Reject();

        #region Allors
        [Id("6ECEF1FD-19A6-44E0-97B9-1D0F879074B4")]
        #endregion
        [Workspace]
        void Hold();

        #region Allors
        [Id("4F5D213B-C6FC-424A-B8FE-4493B1D4E7B3")]
        #endregion
        [Workspace]
        void Continue();

        #region Allors
        [Id("6167FF6D-DED4-45BC-B4C4-5955B4727200")]
        #endregion
        [Workspace]
        void Cancel();

        #region Allors
        [Id("80BF3BC5-25D5-4CF6-A7E9-E01F34AFF9EA")]
        #endregion
        [Workspace]
        void Complete();

        #region Allors

        [Id("794F36F3-04A0-41E9-8AE1-AD48C006CE6B")]

        #endregion
        [Workspace]
        void Invoice();

        #region Allors
        [Id("468AA6DB-A42B-4389-AF15-70CA3265FC5E")]
        #endregion
        [Workspace]
        void Reopen();
    }
}
