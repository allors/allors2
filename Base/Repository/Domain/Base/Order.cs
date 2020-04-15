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
        Currency Currency { get; set; }

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
        [Id("d5d2ec87-064b-4743-9a5e-55b68a84caf6")]
        [AssociationId("fec8b5fd-bf0f-4579-9af0-5a590b2b5b94")]
        [RoleId("1fba3917-63b1-472e-96bf-08fc5068a7b6")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }

        #region Allors
        [Id("addf2b1e-a7c1-4ba8-94f0-13c99d2b8f63")]
        [AssociationId("c9ccf0b5-b3a2-4a46-a035-4567215ce48a")]
        [RoleId("2ad9cab5-25ea-49d2-9f70-145de25b2170")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        DiscountAdjustment DiscountAdjustment { get; set; }

        #region Allors
        [Id("fc6cb229-6c94-4c80-a4a6-697d1d752997")]
        [AssociationId("2481df7c-624c-44ce-a201-7d7c4d339780")]
        [RoleId("f2540864-d417-482b-a7bf-0f01c1f185eb")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        SurchargeAdjustment SurchargeAdjustment { get; set; }

        #region Allors
        [Id("615a233a-659a-44cc-b056-fe02643cbeed")]
        [AssociationId("847041e8-d780-4640-803d-927b23f7932f")]
        [RoleId("18ff7372-059c-4717-9d9f-a3a20ea5a7ba")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        Fee Fee { get; set; }

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
        VatRegime VatRegime { get; set; }

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
