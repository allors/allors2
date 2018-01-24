namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("7dde949a-6f54-4ece-92b3-d269f50ef9d9")]
    #endregion
    public partial interface Order : Printable, Commentable, Localised, Auditable, Transitional
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
        [Derived]
        [Indexed]
        Currency Currency { get; set; }

        #region Allors
        [Id("1e94a270-3780-43fc-9b62-611f259b04fd")]
        [AssociationId("edabe0af-99a5-4a67-b6e1-66c341ad945d")]
        [RoleId("8138ed27-5520-458a-bc18-8d684daa4649")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePriceCustomerCurrency { get; set; }

        #region Allors
        [Id("24520951-4088-4c60-adaa-dd7bf00257de")]
        [AssociationId("6c4e690b-0f91-4fa6-bfa1-755048304dbb")]
        [RoleId("ca53ee57-213a-4449-8fc8-f04f2737a8a6")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalIncVatCustomerCurrency { get; set; }

        #region Allors
        [Id("35451f53-5a0e-443d-a9f7-36620a832b02")]
        [AssociationId("b93f7956-ffe3-4793-a5d5-ee60bf197e6e")]
        [RoleId("6b2890e7-f8e2-479d-8155-51adeade6799")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscountCustomerCurrency { get; set; }

        #region Allors
        [Id("45b3b293-b746-4d6d-9da7-e2378694f734")]
        [AssociationId("5e1ba42d-9325-45b5-9c41-cc9b12d0929a")]
        [RoleId("019d8b7a-79db-4100-a690-ae7587e30d8e")]
        #endregion
        [Workspace]
        [Size(256)]
        string CustomerReference { get; set; }

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
        [Id("73521788-7e0e-4ea2-9961-1a58f68cde5c")]
        [AssociationId("8e7ad6ef-7a40-472f-b7b9-f53a77e51548")]
        [RoleId("af64e731-adfb-414a-9520-51d4ea2c8f81")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        SalesTerm[] SalesTerms { get; set; }

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
        [Id("7f25a14f-c32f-4a86-ae2d-9f087f8b8214")]
        [AssociationId("5a227b86-4ea3-4ce9-89df-a672337ecd1d")]
        [RoleId("4ba525e4-7e31-4b9e-9fc4-8a26e9e352a1")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVatCustomerCurrency { get; set; }

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
        [Size(256)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("9fd3ea50-280e-4d0d-a9db-450991248a53")]
        [AssociationId("5c3af601-e6f8-4adc-af83-88058d86975a")]
        [RoleId("7badb3f0-51a2-4e9b-bdc7-3d3bfbf7865f")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandlingCustomerCurrency { get; set; }

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
        [Id("addf2b1e-a7c1-4ba8-94f0-13c99d2b8f63")]
        [AssociationId("c9ccf0b5-b3a2-4a46-a035-4567215ce48a")]
        [RoleId("2ad9cab5-25ea-49d2-9f70-145de25b2170")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        DiscountAdjustment DiscountAdjustment { get; set; }

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
        [Id("c2c10a98-2518-4243-8d58-4bee6316abc5")]
        [AssociationId("31d26b09-fde7-49b0-b800-718614f216d9")]
        [RoleId("61b0d93a-a36d-470f-b25d-089a7b209457")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurchargeCustomerCurrency { get; set; }

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
        [Id("c80447e8-4fc8-4491-8857-3129a007a267")]
        [AssociationId("a8210460-e40e-4fab-8adb-bb8d57af0ad6")]
        [RoleId("21b64676-ffa0-4e8c-9850-89096f7f5eff")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalFeeCustomerCurrency { get; set; }

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
        [Id("d5d2ec87-064b-4743-9a5e-55b68a84caf6")]
        [AssociationId("fec8b5fd-bf0f-4579-9af0-5a590b2b5b94")]
        [RoleId("1fba3917-63b1-472e-96bf-08fc5068a7b6")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }

        #region Allors
        [Id("e039e94d-db89-4a17-a692-e82fdb53bfea")]
        [AssociationId("f1eed6f2-fb70-4fd8-8e7a-0962759b00a7")]
        [RoleId("e5e2710b-f662-4a50-8203-d0d7c0789e3e")]
        #endregion
        [Workspace]
        [Required]
        DateTime OrderDate { get; set; }

        #region Allors
        [Id("e39720f8-2e56-4231-9ce0-fb8b8e0efd7e")]
        [AssociationId("b58cd94b-59c1-46ca-8c8e-93b3a45ff5dc")]
        [RoleId("62ca6d9d-6d19-4197-847b-7909faf28295")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVatCustomerCurrency { get; set; }

        #region Allors
        [Id("f38b3c7d-ac20-49be-a115-d7e83557f49a")]
        [AssociationId("f4ff4e74-0bff-4a2a-b4bd-3a08310c6ce2")]
        [RoleId("6d52e55f-2adb-4ec6-8b13-e8611dfcd38a")]
        #endregion
        [Workspace]
        DateTime DeliveryDate { get; set; }

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
        [Id("fc6cb229-6c94-4c80-a4a6-697d1d752997")]
        [AssociationId("2481df7c-624c-44ce-a201-7d7c4d339780")]
        [RoleId("f2540864-d417-482b-a7bf-0f01c1f185eb")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        SurchargeAdjustment SurchargeAdjustment { get; set; }

        #region Allors
        [Id("4754BBBA-2B13-45F5-AE5F-374F45E3E121")]
        [AssociationId("4B1089A9-C54E-4C9A-B36A-C417430CAF62")]
        [RoleId("336994F5-8C5B-4CD7-886E-C758C7ABCAC6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Person ContactPerson { get; set; }

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
        [Id("213642B9-E825-4BDA-B424-F798FA4E19A9")]
        #endregion
        [Workspace]
        void Confirm();

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
    }
}