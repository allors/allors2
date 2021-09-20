// <copyright file="Quote.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("066bf242-2710-4a68-8ff6-ce4d7d88a04a")]
    #endregion
    public partial interface Quote : Transitional, WorkItem, Printable, Auditable, Commentable, Deletable, Localised
    {
        #region ObjectStates
        #region QuoteState
        #region Allors
        [Id("B1792FCE-33EF-4A03-BCB7-92E839A55B2C")]
        [AssociationId("2FCD7B16-863B-4A41-9C2C-B7E45E74799A")]
        [RoleId("3E5206B4-40F2-4939-BA95-2D8D089CFDF5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        QuoteState PreviousQuoteState { get; set; }

        #region Allors
        [Id("C1B9AD76-9773-4A52-AADB-ED3E7222C89B")]
        [AssociationId("9945B2C0-06CC-4410-8861-05A54DDB8728")]
        [RoleId("9BEA275E-F1A0-4023-877E-EF67A150C3DF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        QuoteState LastQuoteState { get; set; }

        #region Allors
        [Id("2A4AADE6-B3F0-436B-BA9E-5D0ECB958077")]
        [AssociationId("62A27DA3-372A-4296-B7C6-0BD482A7CB31")]
        [RoleId("84F136C7-B146-4D20-A4D2-797740E60291")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        QuoteState QuoteState { get; set; }
        #endregion
        #endregion

        #region Allors
        [Id("AFB30FBE-9E93-4EBD-B8D3-5C5B231D70E1")]
        [AssociationId("520CF705-3E9C-4845-816C-F8DD17A4534B")]
        [RoleId("7B78FA1B-EAEA-419D-87E8-F0A746DBEA18")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        InternalOrganisation Issuer { get; set; }

        #region Allors
        [Id("BA16DE57-19A1-40BC-AF3C-99690EB5ECAB")]
        [AssociationId("B1D552EE-4967-4651-95C9-C9A9A846DE35")]
        [RoleId("E6732FB3-5BD9-4AB1-B5B9-394D643A47E0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Currency AssignedCurrency { get; set; }

        #region Allors
        [Id("0e251edc-35aa-499e-89f3-acc28ab82e6e")]
        [AssociationId("5d775782-55c2-420f-abd9-b0eafdb7bf90")]
        [RoleId("2f1464be-7427-42e2-aad5-b19a65b724bd")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        Currency DerivedCurrency { get; set; }

        #region Allors
        [Id("3B913CC6-C627-4F16-ACF5-98EC97CE5FDA")]
        [AssociationId("7CD50B18-3C4A-4A81-B6AA-9CE8BC43C0DA")]
        [RoleId("3A53DEDC-0877-44FA-89E3-1212EAB4FF36")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("D566DD5B-BF58-45A6-A68F-FD7D2652FB4D")]
        [AssociationId("8F0A31C1-1B2A-4531-84BA-7636F4D0B9DF")]
        [RoleId("00EF6AB3-E075-45F5-A839-96576E3546AB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        DateTime RequiredResponseDate { get; set; }

        #region Allors
        [Id("b5ecffab-0f27-4311-9f66-197f0cdc147f")]
        [AssociationId("37904354-0db4-43b2-8885-4e9e57b915f4")]
        [RoleId("f57f743f-5c90-4348-b9a0-52dc6b2116ae")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Derived]
        [Indexed]
        QuoteItem[] ValidQuoteItems { get; set; }

        #region Allors
        [Id("033df6dd-fdf7-44e4-84ca-5c7e100cb3f5")]
        [AssociationId("4b19f443-0d27-447d-8186-e5361a094460")]
        [RoleId("fa17ef86-c074-414e-b223-b62522d68280")]
        #endregion
        [Workspace]
        DateTime ValidFromDate { get; set; }

        #region Allors
        [Id("05e3454a-0a7a-488d-b4b1-f0fd41392ddf")]
        [AssociationId("ca3f0d26-9ead-4691-8f7f-f79272065251")]
        [RoleId("92e46228-ad44-4b9b-b727-23159a59bca3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        QuoteTerm[] QuoteTerms { get; set; }

        #region Allors
        [Id("2140e106-2ef3-427a-be94-458c2b8e154d")]
        [AssociationId("9d81ada4-a4f3-44bb-9098-bc1a3e61de19")]
        [RoleId("60581583-2536-4b09-acae-f0f877169dae")]
        #endregion
        [Workspace]
        DateTime ValidThroughDate { get; set; }

        #region Allors
        [Id("3da51ccc-24b9-4b03-9218-7da06492224d")]
        [AssociationId("602c70c9-ddc4-4cf5-a79f-0abcc0beba15")]
        [RoleId("d4d93ad0-c59d-40e7-a82c-4fb1e54a85f2")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("9119c598-cd98-43da-bfdf-1e6573112c9e")]
        [AssociationId("d48cd46d-889b-4e2d-a6d6-ee26f30fb3e5")]
        [RoleId("56f5d5ee-1ab5-48f2-a413-7b80dd2c283e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        Party Receiver { get; set; }

        #region Allors
        [Id("A1C248DF-7F2A-4622-9052-9106C67B1D71")]
        [AssociationId("ED908492-D8C8-40E8-853D-6F9602B1D646")]
        [RoleId("69D3F384-64C8-4D6C-AEA7-CCDA7CB51DCF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        ContactMechanism FullfillContactMechanism { get; set; }

        #region Allors
        [Id("37D046B8-3804-4912-9B53-C98D66A67BC0")]
        [AssociationId("0608C530-D264-46A8-B298-367CCC0FEF76")]
        [RoleId("F7A1DEFC-271D-43BD-B05F-2D9441C7EE3A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        VatRegime AssignedVatRegime { get; set; }

        #region Allors
        [Id("164735dd-c7a4-4e8a-8e17-9be8bd9a29e7")]
        [AssociationId("8a26230c-dae9-4260-9bc8-74ba902f32bf")]
        [RoleId("1217c356-d75f-4881-9598-59f295be0733")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        VatRegime DerivedVatRegime { get; set; }

        #region Allors
        [Id("9810d19b-6728-4e8b-91c0-050a9d363b1e")]
        [AssociationId("c2972a9a-4d61-44aa-ade4-270da3cda9a3")]
        [RoleId("2f6ff1a9-4f09-4ef6-a2d1-723b8c6a6770")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        VatRate DerivedVatRate{ get; set; }

        #region Allors
        [Id("0B00B80D-1A5C-4CB0-A50A-B6E552A1AF6F")]
        [AssociationId("FC12589D-86F4-40A9-BBA6-8F0A21F42BC7")]
        [RoleId("C550C3E6-22EA-41B4-9F11-E45CC4BFBFE9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        VatClause AssignedVatClause { get; set; }

        #region Allors
        [Id("3B47127B-7C65-4891-8D24-4622D7573EC5")]
        [AssociationId("6C19844B-6CB5-4260-93D3-EBFD9C3FAB28")]
        [RoleId("B65F6838-2DF5-4EA8-AF99-5B983216B76C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        VatClause DerivedVatClause { get; set; }

        #region Allors
        [Id("07efb261-438e-4f03-831f-fbc11ab944f2")]
        [AssociationId("07a3df9c-279e-4b2c-9960-fbf1bad3d37a")]
        [RoleId("3d23035a-6021-4f1f-a620-71f50fcf6b8e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRegime AssignedIrpfRegime { get; set; }

        #region Allors
        [Id("d1366534-36fa-4b64-9488-c5da5d083dfb")]
        [AssociationId("3bec23a9-523a-465e-a6e2-7d992bdd2a71")]
        [RoleId("659a2362-e2d6-481b-8919-5ffb7b66b4e3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        IrpfRegime DerivedIrpfRegime { get; set; }

        #region Allors
        [Id("f0fe835b-bf4a-4e17-b492-c7691ee26405")]
        [AssociationId("5b76fe3d-366c-4663-b740-24abe0c2abf7")]
        [RoleId("75e99c53-d6cc-42a0-870f-94da7d827cf4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        IrpfRate DerivedIrpfRate { get; set; }

        #region Allors
        [Id("37726ca4-936c-4dcc-b07a-a20c483f5890")]
        [AssociationId("ac2f42a0-55bf-45d0-b128-ad65b21562ac")]
        [RoleId("888ce960-67cd-4b58-8afc-a33b8d050199")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        public Locale DerivedLocale { get; set; }

        #region Allors
        [Id("7f8b987f-85fd-44f9-8218-1f4d136e4d1d")]
        [AssociationId("78d8d249-89e2-41e7-a7f0-61f2157d960c")]
        [RoleId("1afcd3d5-c707-4773-aa92-2953bb5debde")]
        #endregion
        [Required]
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIrpf { get; set; }

        #region Allors
        [Id("E38FDC05-A2BB-4E37-9B92-7976AEB5AD4E")]
        [AssociationId("CA767808-44B0-45ED-9B6E-23D671BA0A3B")]
        [RoleId("EBD390F4-6BC0-4497-A5E3-E67C46055CE2")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVat { get; set; }

        #region Allors
        [Id("2BB55996-6992-4960-ADD3-7B2AB846DBC3")]
        [AssociationId("7EB76ED7-4D4F-4AE0-9D1C-B7B5B6A8809B")]
        [RoleId("107337AA-EC3B-4A53-BDBE-CC9156C81D66")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVat { get; set; }

        #region Allors
        [Id("2163DB4B-684D-45BF-B56A-99D5311DDC52")]
        [AssociationId("C0A4674C-7860-4C64-81D2-49C930D04B6C")]
        [RoleId("5D293D35-7700-4DE0-9905-C5E309B689DE")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalIncVat { get; set; }

        #region Allors
        [Id("DB9E318C-FE6E-4A84-89DA-AD02EE9C3266")]
        [AssociationId("A35B6FE6-E152-43A2-9F81-944C870C115D")]
        [RoleId("69245985-8496-4703-9467-511974A69834")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurcharge { get; set; }

        #region Allors
        [Id("81140E8D-B94B-47AD-B478-E3AD91C1E66E")]
        [AssociationId("1192E38B-0350-42FB-A48D-098A79004AD4")]
        [RoleId("836B6F9C-BAC3-42FD-8F46-8CAFEAEB0EF5")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscount { get; set; }

        #region Allors
        [Id("3FD4A223-4EDB-44B7-8AE4-CC9B0AA9FEEE")]
        [AssociationId("379DDEBA-50D1-4F55-9CF5-C1FDC4E1D25F")]
        [RoleId("D396C4CB-3CDF-492D-8B3A-E26DADA94EAB")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandling { get; set; }

        #region Allors
        [Id("7B8FB84F-02FD-4616-BD09-2D7E421FBB5B")]
        [AssociationId("2BFBD0D1-83B9-4784-B2A3-D82938CF0A57")]
        [RoleId("D5373E0F-23EC-4E1B-8ED2-BC945B14FE84")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalFee { get; set; }

        #region Allors
        [Id("0441b70f-597c-4e71-8480-bcbe6b520152")]
        [AssociationId("921ebd78-9fdb-4c50-bf2e-8b45ed165924")]
        [RoleId("697da26c-6d80-4c00-a9fe-a818d47ad6bc")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExtraCharge { get; set; }

        #region Allors
        [Id("B02CBBD4-0D39-4851-80FA-E0727CF38353")]
        [AssociationId("E04679D7-7DA8-4073-AAC5-F6B17E8FCD47")]
        [RoleId("E3802964-CC33-474E-AC9C-F7950EE36EF7")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePrice { get; set; }

        #region Allors
        [Id("706BEBDC-C2E3-4534-B94C-B6A8D3222BA1")]
        [AssociationId("A0748124-471E-4277-B802-B63DA32559AC")]
        [RoleId("4902C7D6-58C2-40C6-BF3C-7ABD926C9269")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalListPrice { get; set; }

        #region Allors
        [Id("a59e8de9-9caa-41ea-942a-229d18339e7c")]
        [AssociationId("45d79194-32c3-44d0-8045-9edcf3c1dae2")]
        [RoleId("40609787-1acf-42d8-9771-0b9335c31efc")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal GrandTotal { get; set; }

        #region Allors
        [Id("1d41aa3d-3db8-444b-923e-e1d3057c4d31")]
        [AssociationId("29ee427c-431f-4013-af63-117a8121a7fb")]
        [RoleId("f2817865-2d65-4eab-851e-7ded90d019ef")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Indexed]
        OrderAdjustment[] OrderAdjustments { get; set; }

        #region Allors
        [Id("d7dc81e8-76e7-4c68-9843-a2aaf8293510")]
        [AssociationId("6fbc80d1-e72b-4484-a9b1-e606f15d2435")]
        [RoleId("219cb27f-20b5-48b3-9d89-4b119798b092")]
        #endregion
        [Workspace]
        [Required]
        DateTime IssueDate { get; set; }

        #region Allors
        [Id("e250154a-77c5-4a0b-ae3d-28668a9037d1")]
        [AssociationId("b5ba8cfd-2b16-4a50-89cd-46927d59b97a")]
        [RoleId("f5b6881b-c4d5-42e3-a024-0ae4564cb970")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        QuoteItem[] QuoteItems { get; set; }

        #region Allors
        [Id("e76cbd73-78b7-4ef8-a24c-9ac0db152f7f")]
        [AssociationId("057ad29f-c245-44b2-8a95-71bd6607830b")]
        [RoleId("218e3a6e-b530-41f7-a60e-7587f8072c8c")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        string QuoteNumber { get; set; }

        #region Allors
        [Id("94DE208B-5FF9-45F5-BD35-5BB7D7B33FB7")]
        [AssociationId("10F24038-D3E1-40DC-9D4F-27E738BE7F3D")]
        [RoleId("3D269537-523B-489A-A7DA-008DC8585F60")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        Request Request { get; set; }

        #region Allors
        [Id("2D6804B9-A745-497A-9F43-FADE6B1B76AB")]
        [AssociationId("542367E1-1CAB-4DB6-9FB3-ECAD8930601F")]
        [RoleId("EDF15A48-DF46-411C-B73A-9B6E9C184932")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Person ContactPerson { get; set; }

        #region Allors
        [Id("c44822c3-beb9-4eab-bfe0-f8f270460f26")]
        [AssociationId("05820858-8fc3-437a-b5f1-2b8e48353fea")]
        [RoleId("d13d6305-62c6-4426-8386-f26372dcc814")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        int SortableQuoteNumber { get; set; }

        #region Allors
        [Id("bd1de829-0619-4e3c-a104-89bcb5ab1f4d")]
        [AssociationId("ed6127c6-09cc-4f0e-ab19-c446488e604c")]
        [RoleId("5fdaef00-71e4-4e56-9b8f-17ea51621fdc")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public Media[] ElectronicDocuments { get; set; }

        #region Allors
        [Id("2e44c8f9-5da2-4027-b896-ae155b17be09")]
        [AssociationId("5464c4a1-8c72-4a45-808a-973336e576c2")]
        [RoleId("5fa91fc1-9850-4be4-aa3b-e5b648751cf1")]
        #endregion
        [Required]
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIrpfInPreferredCurrency { get; set; }

        #region Allors
        [Id("7aa1987c-cce6-47cb-b5e4-c72970afe038")]
        [AssociationId("b6b41a23-be2f-46af-b8f2-4f86c3350052")]
        [RoleId("72730da9-de19-45ba-8945-4c9538fc4920")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("5c712db9-55ca-4bb7-8734-421db98ac642")]
        [AssociationId("008c9da9-0054-4558-8c4f-14f26fd863d0")]
        [RoleId("d6f70c41-7f9f-4d41-8bc5-ae6010b85717")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("1aa160a0-c5b6-48eb-88f3-a5ed92022329")]
        [AssociationId("4af3052e-5205-4fa5-8dd3-7e12403ea82f")]
        [RoleId("aecd9043-c49b-4ba3-9644-ed09d3cc7253")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalIncVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("9f1e78d8-2fd0-46a2-8ccb-5b87e4f4b459")]
        [AssociationId("04d46c59-8d71-4d17-a9c4-1aa2cc893863")]
        [RoleId("0e9fd468-614d-4d72-b9f0-bcb56f6cd0d5")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurchargeInPreferredCurrency { get; set; }

        #region Allors
        [Id("3796228a-9816-4dd2-a330-ce6e9a62b16b")]
        [AssociationId("e28af182-660d-4882-908c-074a81aa5d7e")]
        [RoleId("937e45c9-f1b1-4070-b1d8-895fa9c34ea9")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscountInPreferredCurrency { get; set; }

        #region Allors
        [Id("984fb92c-be38-492d-866f-d2fa7ae2e366")]
        [AssociationId("1e5b18a5-b764-4610-bd76-5f5db3310522")]
        [RoleId("61b4fa56-eefe-4891-a967-945ad807a8c8")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandlingInPreferredCurrency { get; set; }

        #region Allors
        [Id("16a1a43e-0c42-45e8-94a6-ee8aa4d3dee4")]
        [AssociationId("99675829-d07a-4754-88b9-a832a3f7dafb")]
        [RoleId("00504ed4-0c7f-44e8-ab2b-418fc30eb59a")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalFeeInPreferredCurrency { get; set; }

        #region Allors
        [Id("bb5aabb9-80c4-478e-ac66-a60626e3c5a6")]
        [AssociationId("5f9a5562-006b-41be-9dbf-b38f19bcf186")]
        [RoleId("22062da3-b2a0-4bc3-bce0-2a6a98a73230")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExtraChargeInPreferredCurrency { get; set; }

        #region Allors
        [Id("a971dca8-e852-403d-b901-dea36a7898b5")]
        [AssociationId("6a7398ad-e420-40ad-ac81-546c15c7a3ab")]
        [RoleId("4cf9e6b3-658b-4286-a62e-fe1b08e9ad0b")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePriceInPreferredCurrency { get; set; }

        #region Allors
        [Id("d972f3b8-c11a-4213-a0b9-fc91d9ddd6f1")]
        [AssociationId("ad3840ed-258d-4ca7-a61e-ecdba2ab4233")]
        [RoleId("eecbcdc1-ff75-4cc2-b5dd-0fb1877c037d")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalListPriceInPreferredCurrency { get; set; }

        #region Allors
        [Id("36d27068-bc8a-49f7-ac97-a934e3de4617")]
        [AssociationId("d2082010-4f57-4f4e-9ab8-437a282ece37")]
        [RoleId("65086600-c70c-4247-b24d-a012dab31094")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal GrandTotalInPreferredCurrency { get; set; }

        #region Allors

        [Id("8C858157-B9BC-4E2C-97BC-646066532854")]

        #endregion
        [Workspace]
        void Create();

        #region Allors
        [Id("3df1ddb1-cb93-4da7-a5d1-fa22a164c2e2")]
        #endregion
        [Workspace]
        public void SetReadyForProcessing() { }

        #region Allors

        [Id("70F1138B-1383-4AA1-A08E-6C99F71F3F07")]

        #endregion
        [Workspace]
        void Reopen();

        #region Allors
        [Id("519F70DC-0C4C-43E7-8929-378D8871CD84")]
        #endregion
        [Workspace]
        void Approve();

        #region Allors
        [Id("506ED1BA-5F88-487E-B126-470FE1FD7791")]
        #endregion
        [Workspace]
        void Send();

        #region Allors
        [Id("90c9c005-3842-44f5-877c-f601523a888f")]
        #endregion
        [Workspace]
        void Accept();

        #region Allors
        [Id("6b5e540d-96a8-48cf-a888-7e7f6b844d28")]
        #endregion
        [Workspace]
        void Revise();

        #region Allors
        [Id("39694549-7173-4904-8AE0-DA7390F595A5")]
        #endregion
        [Workspace]
        void Reject();

        #region Allors
        [Id("712D9F73-0D39-4F25-9CD3-6D8BE6F8AEC8")]
        #endregion
        [Workspace]
        void Cancel();
    }
}
